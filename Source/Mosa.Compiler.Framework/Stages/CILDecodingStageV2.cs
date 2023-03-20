// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Represents the CIL decoding compilation stage.
/// </summary>
/// <remarks>
/// The CIL decoding stage takes a stream of bytes and decodes the instructions represented into an MSIL based intermediate
/// representation. The instructions are grouped into basic blocks.
/// </remarks>
public sealed class CILDecodingStageV2 : BaseMethodCompilerStage
{
	private enum StackType
	{ Int32, Int64, R4, R8, Object, ManagedPointer, ValueType };

	private enum ElementType
	{ I1, I2, I4, I8, U1, U2, U4, U8, R4, R8, I, Object, ManagedPointer };

	#region StackEntry Class

	private class StackEntry
	{
		public Operand Operand;
		public StackType StackType;
		public MosaType Type;   // for ValueTypes

		public StackEntry(StackType stackType, Operand operand)
		{
			StackType = stackType;
			Operand = operand;
		}

		public StackEntry(StackType stackType, Operand operand, MosaType type)
		{
			StackType = stackType;
			Operand = operand;
			Type = type;
		}

		public override string ToString()
		{
			return $"{StackType} {Operand}";
		}
	}

	#endregion StackEntry Class

	#region PrefixValues Class

	private class PrefixValues
	{
		public bool Unaligned { get; set; } = false; // ldind, stind, ldfld, stfld, ldobj, stobj, initblk, or cpblk

		public bool Volatile { get; set; } = false; // Ldsfld and Stsfld

		public bool Tailcall { get; set; } = false; // Call, Calli, or Callvirt

		public bool Constrained => ContrainedType != null;

		public bool Readonly { get; set; } = false; // ldelema

		public bool NoTypeCheck { get; set; } = false;

		public bool NoRangeCheck { get; set; } = false;

		public bool NoNullCheck { get; set; } = false;

		public bool Reset = false;

		public MosaType ContrainedType { get; set; }

		public void ResetAll()
		{
			Unaligned = false;
			Volatile = false;
			Tailcall = false;
			Readonly = false;
			NoTypeCheck = false;
			NoRangeCheck = false;
			NoNullCheck = false;
			ContrainedType = null;
		}
	}

	#endregion PrefixValues Class

	private readonly Dictionary<BasicBlock, StackEntry> ExceptionStackEntries = new Dictionary<BasicBlock, StackEntry>();

	private readonly Dictionary<BasicBlock, StackEntry[]> OutgoingStacks = new Dictionary<BasicBlock, StackEntry[]>();

	private Operand[] LocalStack;
	private StackType[] LocalStackType;
	private ElementType[] LocalElementType;

	private SortedList<int, int> Targets;

	private readonly Dictionary<MosaMethod, IntrinsicMethodDelegate> InstrinsicMap = new Dictionary<MosaMethod, IntrinsicMethodDelegate>();

	private TraceLog trace;

	protected override void Finish()
	{
		Targets = null;
		trace = null;

		//MethodCompiler.Stop();
	}

	protected override void Run()
	{
		if (!MethodCompiler.IsCILStream)
			return;

		trace = CreateTraceLog();

		var prologueBlock = BasicBlocks.CreatePrologueBlock();
		var startBlock = BasicBlocks.CreateStartBlock();

		var prologue = new Context(prologueBlock.First);
		prologue.AppendInstruction(IRInstruction.Prologue);
		prologue.AppendInstruction(IRInstruction.Jmp, startBlock);

		CollectTargets();

		CreateBasicBlocks();

		CreateHandlersBlocks();

		CreateLocalVariables();

		InitializeLocalVariables();

		CreateInstructions();

		var epilogueBlock = BasicBlocks.EpilogueBlock;

		if (epilogueBlock != null)
		{
			var epilogue = new Context(epilogueBlock.First);
			epilogue.AppendInstruction(IRInstruction.Epilogue);
		}

		MethodCompiler.ProtectedRegions = ProtectedRegion.CreateProtectedRegions(BasicBlocks, Method.ExceptionHandlers);

		InsertBlockProtectInstructions();

		ProtectedRegion.FinalizeAll(BasicBlocks, MethodCompiler.ProtectedRegions);
	}

	protected override void Setup()
	{
		Targets = new SortedList<int, int>();
	}

	private void AddTarget(int target)
	{
		if (!Targets.ContainsKey(target))
			Targets.Add(target, target);
	}

	private void CollectTargets()
	{
		var code = Method.Code;

		for (int index = 0; index < code.Count; index++)
		{
			var instruction = code[index];

			var opcode = (OpCode)instruction.OpCode;

			if (opcode == OpCode.Br || opcode == OpCode.Br_s)
			{
				AddTarget((int)instruction.Operand);
			}
			else if (IsBranch(opcode))
			{
				AddTarget((int)instruction.Operand);
				AddTarget(code[index + 1].Offset);
			}
			else if (opcode == OpCode.Switch)
			{
				foreach (var target in (int[])instruction.Operand)
				{
					AddTarget(target);
				}

				AddTarget(code[index + 1].Offset);
			}
			else if (opcode == OpCode.Leave || opcode == OpCode.Leave_s)
			{
				AddTarget((int)instruction.Operand);
			}
		}

		foreach (var clause in Method.ExceptionHandlers)
		{
			if (clause.TryStart != 0)
			{
				AddTarget(clause.TryStart);
			}

			if (clause.TryEnd != 0)
			{
				AddTarget(clause.TryEnd);
			}

			if (clause.HandlerStart != 0)
			{
				AddTarget(clause.HandlerStart);
			}

			if (clause.FilterStart != null)
			{
				AddTarget(clause.FilterStart.Value);
			}

			if (clause.ExceptionHandlerType == ExceptionHandlerType.Exception)
			{
				AddTarget(clause.HandlerStart);
			}

			if (clause.ExceptionHandlerType == ExceptionHandlerType.Filter)
			{
				AddTarget(clause.HandlerStart);
				AddTarget(clause.FilterStart.Value);
			}
		}
	}

	private void CreateBasicBlocks()
	{
		foreach (var target in Targets)
		{
			GetOrCreateBlock(target.Key);
		}
	}

	private void CreateHandlersBlocks()
	{
		foreach (var clause in Method.ExceptionHandlers)
		{
			if (clause.TryStart != 0)
			{
				GetOrCreateBlock(clause.TryStart);
			}

			if (clause.TryEnd != 0)
			{
				GetOrCreateBlock(clause.TryEnd);
			}

			if (clause.HandlerStart != 0)
			{
				var block = GetOrCreateBlock(clause.HandlerStart);
				BasicBlocks.AddHeadBlock(block);
				BasicBlocks.AddHandlerHeadBlock(block);
			}

			if (clause.FilterStart != null)
			{
				var block = GetOrCreateBlock(clause.FilterStart.Value);
				BasicBlocks.AddHeadBlock(block);
				BasicBlocks.AddHandlerHeadBlock(block);
			}

			if (clause.ExceptionHandlerType == ExceptionHandlerType.Exception)
			{
				var handler = GetOrCreateBlock(clause.HandlerStart);
				var context = new Context(handler);
				var exceptionObject = AllocateVirtualRegister(clause.Type);

				context.AppendInstruction(IRInstruction.ExceptionStart, exceptionObject);

				ExceptionStackEntries.Add(handler, new StackEntry(StackType.Object, exceptionObject));
			}

			if (clause.ExceptionHandlerType == ExceptionHandlerType.Filter)
			{
				{
					var handler = GetOrCreateBlock(clause.HandlerStart);
					var context = new Context(handler);
					var exceptionObject = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);

					context.AppendInstruction(IRInstruction.ExceptionStart, exceptionObject);

					ExceptionStackEntries.Add(handler, new StackEntry(StackType.Object, exceptionObject));
				}

				{
					var handler = GetOrCreateBlock(clause.FilterStart.Value);
					var context = new Context(handler);
					var exceptionObject = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);

					context.AppendInstruction(IRInstruction.FilterStart, exceptionObject);

					ExceptionStackEntries.Add(handler, new StackEntry(StackType.Object, exceptionObject));
				}
			}
		}
	}

	private void InsertBlockProtectInstructions()
	{
		foreach (var handler in Method.ExceptionHandlers)
		{
			var tryBlock = BasicBlocks.GetByLabel(handler.TryStart);

			var tryHandler = BasicBlocks.GetByLabel(handler.HandlerStart);

			var context = new Context(tryBlock);

			while (context.IsEmpty || context.Instruction == IRInstruction.TryStart)
			{
				context.GotoNext();
			}

			context.AppendInstruction(IRInstruction.TryStart, tryHandler);

			context = new Context(tryHandler);

			if (handler.ExceptionHandlerType == ExceptionHandlerType.Finally)
			{
				var exceptionObject = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);
				var finallyOperand = Is32BitPlatform ? AllocateVirtualRegister32() : AllocateVirtualRegister64();

				context.AppendInstruction2(IRInstruction.FinallyStart, exceptionObject, finallyOperand);
			}
		}
	}

	private void CreateInstructions()
	{
		var prefixValues = new PrefixValues();

		var block = BasicBlocks.GetByLabel(0);
		var stack = new Stack<StackEntry>();
		var context = new Context(block.AfterFirst);
		var endNode = block.First;

		var code = Method.Code;
		var totalCode = code.Count;

		trace?.Log($"Block => {block}");

		for (int index = 0; index < totalCode; index++)
		{
			var instruction = code[index];
			var opcode = (OpCode)instruction.OpCode;
			var label = instruction.Offset;

			if (block == null)
			{
				block = BasicBlocks.GetByLabel(label);
				stack = CreateIncomingStack(block);

				context.Node = block.AfterFirst;
				endNode = block.First;

				trace?.Log($"Block => {block}");
				trace?.Log($" Start Stack Size: {stack.Count}");
			}

			bool processed = Translate(stack, context, instruction, opcode, block, prefixValues, label);

			if (!processed)
			{
				throw new CompilerException($"Error: Unknown or unprocessable opcode: {opcode}");
			}

			if (prefixValues.Reset)
			{
				prefixValues.ResetAll();
			}

			UpdateLabel(context.Node, label, endNode);
			endNode = context.Node;

			var peekNextblock = (index + 1 == totalCode) ? null : BasicBlocks.GetByLabel(code[index + 1].Offset);

			if (peekNextblock != null || index + 1 == totalCode)
			{
				if (opcode != OpCode.Leave
					&& opcode != OpCode.Leave
					&& opcode != OpCode.Leave_s
					&& opcode != OpCode.Endfilter
					&& opcode != OpCode.Endfinally
					&& opcode != OpCode.Jmp
					&& opcode != OpCode.Br
					&& opcode != OpCode.Br_s
					&& opcode != OpCode.Ret
					&& opcode != OpCode.Throw
					&& opcode != OpCode.Brfalse
					&& opcode != OpCode.Brfalse_s
					&& opcode != OpCode.Brtrue
					&& opcode != OpCode.Brtrue_s
				   )
				{
					context.AppendInstruction(IRInstruction.Jmp, peekNextblock);
				}

				var arrayStack = stack.ToArray().Reverse().ToArray(); // FIXME

				OutgoingStacks.Add(block, arrayStack);

				trace?.Log($" End Stack Size: {stack.Count}");

				trace?.Log($"");

				stack = null;
				block = null;
			}
		}
	}

	private bool Translate(Stack<StackEntry> stack, Context context, MosaInstruction instruction, OpCode opcode, BasicBlock block, PrefixValues prefixValues, int label)
	{
		prefixValues.Reset = true;

		trace?.Log($"   {label:X5}: {opcode}");

		switch (opcode)
		{
			case OpCode.Add: return Add(context, stack);
			case OpCode.Add_ovf: return AddSigned(context, stack);
			case OpCode.Add_ovf_un: return AddUnsigned(context, stack);
			case OpCode.And: return And(context, stack);
			case OpCode.Arglist: return false;                                  // TODO: Not implemented in v1 either
			case OpCode.Beq: return Branch(context, stack, ConditionCode.Equal, instruction);
			case OpCode.Beq_s: return Branch(context, stack, ConditionCode.Equal, instruction);
			case OpCode.Bge: return Branch(context, stack, ConditionCode.GreaterOrEqual, instruction);
			case OpCode.Bge_s: return Branch(context, stack, ConditionCode.GreaterOrEqual, instruction);
			case OpCode.Bge_un: return Branch(context, stack, ConditionCode.UnsignedGreaterOrEqual, instruction);
			case OpCode.Bge_un_s: return Branch(context, stack, ConditionCode.UnsignedGreaterOrEqual, instruction);
			case OpCode.Bgt: return Branch(context, stack, ConditionCode.Greater, instruction);
			case OpCode.Bgt_s: return Branch(context, stack, ConditionCode.Greater, instruction);
			case OpCode.Bgt_un: return Branch(context, stack, ConditionCode.UnsignedGreater, instruction);
			case OpCode.Bgt_un_s: return Branch(context, stack, ConditionCode.UnsignedGreater, instruction);
			case OpCode.Ble: return Branch(context, stack, ConditionCode.LessOrEqual, instruction);
			case OpCode.Ble_s: return Branch(context, stack, ConditionCode.LessOrEqual, instruction);
			case OpCode.Ble_un: return Branch(context, stack, ConditionCode.UnsignedLessOrEqual, instruction);
			case OpCode.Ble_un_s: return Branch(context, stack, ConditionCode.UnsignedLessOrEqual, instruction);
			case OpCode.Blt: return Branch(context, stack, ConditionCode.Less, instruction);
			case OpCode.Blt_s: return Branch(context, stack, ConditionCode.Less, instruction);
			case OpCode.Blt_un: return Branch(context, stack, ConditionCode.UnsignedLess, instruction);
			case OpCode.Blt_un_s: return Branch(context, stack, ConditionCode.UnsignedLess, instruction);
			case OpCode.Bne_un: return Branch(context, stack, ConditionCode.NotEqual, instruction);
			case OpCode.Bne_un_s: return Branch(context, stack, ConditionCode.NotEqual, instruction);
			case OpCode.Box: return Box(context, stack, instruction);
			case OpCode.Br: return Branch(context, stack, instruction);
			case OpCode.Br_s: return Branch(context, stack, instruction);
			case OpCode.Break: return Break(context, stack);
			case OpCode.Brfalse: return Branch1(context, stack, ConditionCode.Equal, instruction);
			case OpCode.Brfalse_s: return Branch1(context, stack, ConditionCode.Equal, instruction);
			case OpCode.Brtrue: return Branch1(context, stack, ConditionCode.NotEqual, instruction);
			case OpCode.Brtrue_s: return Branch1(context, stack, ConditionCode.NotEqual, instruction);
			case OpCode.Call: return Call(context, stack, instruction);
			case OpCode.Calli: return false;                                    // TODO: Not implemented in v1 either
			case OpCode.Callvirt: return Callvirt(context, stack, instruction, prefixValues);
			case OpCode.Castclass: return Castclass(context, stack);
			case OpCode.Ceq: return Compare(context, stack, ConditionCode.Equal);
			case OpCode.Cgt: return Compare(context, stack, ConditionCode.Greater);
			case OpCode.Cgt_un: return Compare(context, stack, ConditionCode.UnsignedGreater);
			case OpCode.Ckfinite: return false;                                 // TODO: Not implemented in v1 either
			case OpCode.Clt: return Compare(context, stack, ConditionCode.Less);
			case OpCode.Clt_un: return Compare(context, stack, ConditionCode.UnsignedLess);
			case OpCode.Conv_i: return ConvertI(context, stack);
			case OpCode.Conv_i1: return ConvertI1(context, stack);
			case OpCode.Conv_i2: return ConvertI2(context, stack);
			case OpCode.Conv_i4: return ConvertI4(context, stack);
			case OpCode.Conv_i8: return ConvertI8(context, stack);
			case OpCode.Conv_ovf_i: return ConvertIWithOverflow(context, stack);
			case OpCode.Conv_ovf_i_un: return false;                            // TODO
			case OpCode.Conv_ovf_i1: return ConvertI1WithOverflow(context, stack);
			case OpCode.Conv_ovf_i1_un: return ConvertUToI1WithOverflow(context, stack);
			case OpCode.Conv_ovf_i2: return ConvertI2WithOverflow(context, stack);
			case OpCode.Conv_ovf_i2_un: return ConvertUToI2WithOverflow(context, stack);
			case OpCode.Conv_ovf_i4: return ConvertI4WithOverflow(context, stack);
			case OpCode.Conv_ovf_i4_un: return ConvertUToI4WithOverflow(context, stack);
			case OpCode.Conv_ovf_i8: return ConvertI8WithOverflow(context, stack);
			case OpCode.Conv_ovf_i8_un: return ConvertUToI8WithOverflow(context, stack);
			case OpCode.Conv_ovf_u: return ConvertUWithOverflow(context, stack);
			case OpCode.Conv_ovf_u_un: return false;                            // TODO
			case OpCode.Conv_ovf_u1: return ConvertU1WithOverflow(context, stack);
			case OpCode.Conv_ovf_u1_un: return ConvertUToU1WithOverflow(context, stack);
			case OpCode.Conv_ovf_u2: return ConvertU2WithOverflow(context, stack);
			case OpCode.Conv_ovf_u2_un: return ConvertUToU2WithOverflow(context, stack);
			case OpCode.Conv_ovf_u4: return ConvertU4WithOverflow(context, stack);
			case OpCode.Conv_ovf_u4_un: return ConvertUToU4WithOverflow(context, stack);
			case OpCode.Conv_ovf_u8: return ConvertU8WithOverflow(context, stack);
			case OpCode.Conv_ovf_u8_un: return ConvertUToU8WithOverflow(context, stack);
			case OpCode.Conv_r_un: return ConvertUToF(context, stack);
			case OpCode.Conv_r4: return ConvertR4(context, stack);
			case OpCode.Conv_r8: return ConvertR8(context, stack);
			case OpCode.Conv_u: return ConvertU(context, stack);
			case OpCode.Conv_u1: return ConvertU1(context, stack);
			case OpCode.Conv_u2: return ConvertU2(context, stack);
			case OpCode.Conv_u4: return ConvertU4(context, stack);
			case OpCode.Conv_u8: return ConvertU8(context, stack);
			case OpCode.Cpblk: return Cpblk(context, stack, instruction);
			case OpCode.Cpobj: return Cpobj(context, stack, instruction);
			case OpCode.Div: return Div(context, stack);
			case OpCode.Div_un: return DivUnsigned(context, stack);
			case OpCode.Dup: return Dup(context, stack);
			case OpCode.Endfilter: return false;                                // TODO: Not implemented in v1 either
			case OpCode.Endfinally: return Endfinally(context);
			case OpCode.Extop: return false;                                    // TODO: Not implemented in v1 either
			case OpCode.Initblk: return Initblk(context, stack);
			case OpCode.InitObj: return InitObj(context, stack, instruction);
			case OpCode.Isinst: return Isinst(context, stack, instruction);
			case OpCode.Jmp: return false;                                      // TODO: Not implemented in v1 either
			case OpCode.Ldarg: return Ldarg(context, stack, (int)instruction.Operand);
			case OpCode.Ldarg_0: return Ldarg(context, stack, 0);
			case OpCode.Ldarg_1: return Ldarg(context, stack, 1);
			case OpCode.Ldarg_2: return Ldarg(context, stack, 2);
			case OpCode.Ldarg_3: return Ldarg(context, stack, 3);
			case OpCode.Ldarg_s: return Ldarg(context, stack, (int)instruction.Operand);
			case OpCode.Ldarga: return Ldarga(context, stack, (int)instruction.Operand);
			case OpCode.Ldarga_s: return Ldarga(context, stack, (int)instruction.Operand);
			case OpCode.Ldc_i4: return Constant32(context, stack, (int)instruction.Operand);
			case OpCode.Ldc_i4_0: return Constant32(context, stack, 0);
			case OpCode.Ldc_i4_1: return Constant32(context, stack, 1);
			case OpCode.Ldc_i4_2: return Constant32(context, stack, 2);
			case OpCode.Ldc_i4_3: return Constant32(context, stack, 3);
			case OpCode.Ldc_i4_4: return Constant32(context, stack, 4);
			case OpCode.Ldc_i4_5: return Constant32(context, stack, 5);
			case OpCode.Ldc_i4_6: return Constant32(context, stack, 6);
			case OpCode.Ldc_i4_7: return Constant32(context, stack, 7);
			case OpCode.Ldc_i4_8: return Constant32(context, stack, 8);
			case OpCode.Ldc_i4_m1: return Constant32(context, stack, -1);
			case OpCode.Ldc_i4_s: return Constant32(context, stack, (sbyte)instruction.Operand);
			case OpCode.Ldc_i8: return Constant64(context, stack, (long)instruction.Operand);
			case OpCode.Ldc_r4: return ConstantR4(context, stack, (float)instruction.Operand);
			case OpCode.Ldc_r8: return ConstantR8(context, stack, (double)instruction.Operand);
			case OpCode.Ldelem: return Ldelem(context, stack, instruction);
			case OpCode.Ldelem_i: return Ldelem(context, stack, ElementType.I);
			case OpCode.Ldelem_i1: return Ldelem(context, stack, ElementType.I1);
			case OpCode.Ldelem_i2: return Ldelem(context, stack, ElementType.I2);
			case OpCode.Ldelem_i4: return Ldelem(context, stack, ElementType.I4);
			case OpCode.Ldelem_i8: return Ldelem(context, stack, ElementType.I8);
			case OpCode.Ldelem_r4: return Ldelem(context, stack, ElementType.R4);
			case OpCode.Ldelem_r8: return Ldelem(context, stack, ElementType.R8);
			case OpCode.Ldelem_ref: return Ldelem(context, stack, ElementType.Object);
			case OpCode.Ldelem_u1: return Ldelem(context, stack, ElementType.U1);
			case OpCode.Ldelem_u2: return Ldelem(context, stack, ElementType.U2);
			case OpCode.Ldelem_u4: return Ldelem(context, stack, ElementType.U4);
			case OpCode.Ldelema: return Ldelema(context, stack, instruction);
			case OpCode.Ldfld: return Ldfld(context, stack, instruction);
			case OpCode.Ldflda: return Ldflda(context, stack, instruction);
			case OpCode.Ldftn: return Ldftn(context, stack, instruction);
			case OpCode.Ldind_i: return Ldind(context, stack, ElementType.I);
			case OpCode.Ldind_i1: return Ldind(context, stack, ElementType.I1);
			case OpCode.Ldind_i2: return Ldind(context, stack, ElementType.I2);
			case OpCode.Ldind_i4: return Ldind(context, stack, ElementType.I4);
			case OpCode.Ldind_i8: return Ldind(context, stack, ElementType.I8);
			case OpCode.Ldind_r4: return Ldind(context, stack, ElementType.R4);
			case OpCode.Ldind_r8: return Ldind(context, stack, ElementType.R8);
			case OpCode.Ldind_ref: return Ldind(context, stack, ElementType.Object);
			case OpCode.Ldind_u1: return Ldind(context, stack, ElementType.U1);
			case OpCode.Ldind_u2: return Ldind(context, stack, ElementType.U2);
			case OpCode.Ldind_u4: return Ldind(context, stack, ElementType.U4);
			case OpCode.Ldlen: return Ldlen(context, stack);
			case OpCode.Ldloc: return Ldloc(context, stack, (int)instruction.Operand);
			case OpCode.Ldloc_0: return Ldloc(context, stack, 0);
			case OpCode.Ldloc_1: return Ldloc(context, stack, 1);
			case OpCode.Ldloc_2: return Ldloc(context, stack, 2);
			case OpCode.Ldloc_3: return Ldloc(context, stack, 3);
			case OpCode.Ldloc_s: return Ldloc(context, stack, (int)instruction.Operand);
			case OpCode.Ldloca: return Ldloca(context, stack, instruction);
			case OpCode.Ldloca_s: return Ldloca(context, stack, instruction);
			case OpCode.Ldnull: return Ldnull(context, stack);
			case OpCode.Ldobj: return Ldobj(context, stack, instruction);
			case OpCode.Ldsfld: return Ldsfld(context, stack, instruction);
			case OpCode.Ldsflda: return Ldsflda(context, stack, instruction);
			case OpCode.Ldstr: return Ldstr(context, stack, instruction);
			case OpCode.Ldtoken: return Ldtoken(context, stack, instruction);
			case OpCode.Ldvirtftn: return false;                                // TODO: Not implemented in v1 either
			case OpCode.Leave: return Leave(context, stack, instruction, block);
			case OpCode.Leave_s: return Leave(context, stack, instruction, block);
			case OpCode.Localalloc: return false;                               // TODO: Not implemented in v1 either
			case OpCode.Mkrefany: return false;                                 // TODO: Not implemented in v1 either
			case OpCode.Mul: return Mul(context, stack);
			case OpCode.Mul_ovf: return MulSigned(context, stack);
			case OpCode.Mul_ovf_un: return MulUnsigned(context, stack);
			case OpCode.Neg: return Neg(context, stack);
			case OpCode.Newarr: return Newarr(context, stack, instruction);
			case OpCode.Newobj: return Newobj(context, stack, instruction);
			case OpCode.Nop: return Nop(context);
			case OpCode.Not: return Not(context, stack);
			case OpCode.Or: return Or(context, stack);
			case OpCode.Pop: return Pop(context, stack);
			case OpCode.Constrained: return Constrained(prefixValues, instruction);
			case OpCode.No: /* TODO */ prefixValues.Reset = false; return true;
			case OpCode.ReadOnly: prefixValues.Readonly = true; prefixValues.Reset = false; return true;
			case OpCode.Tailcall: prefixValues.Tailcall = true; prefixValues.Reset = false; return true;
			case OpCode.Unaligned: prefixValues.Unaligned = true; prefixValues.Reset = false; return true;
			case OpCode.Volatile: prefixValues.Volatile = true; prefixValues.Reset = false; return true;
			case OpCode.Refanytype: return false;                               // TODO: Not implemented in v1 either
			case OpCode.Refanyval: return false;                                // TODO: Not implemented in v1 either
			case OpCode.Rem: return RemOperand(context, stack);
			case OpCode.Rem_un: return RemUnsigned(context, stack);
			case OpCode.Ret: return Ret(context, stack);
			case OpCode.Rethrow: return Rethrow(context, stack);
			case OpCode.Shl: return Shl(context, stack, instruction);
			case OpCode.Shr: return Shr(context, stack, instruction);
			case OpCode.Shr_un: return ShrU(context, stack, instruction);
			case OpCode.Sizeof: return Sizeof(context, stack, instruction);
			case OpCode.Starg: return StoreArgument(context, stack, (int)instruction.Operand);
			case OpCode.Starg_s: return StoreArgument(context, stack, (int)instruction.Operand);
			case OpCode.Stelem: return Stelem(context, stack, instruction);
			case OpCode.Stelem_i: return Stelem(context, stack, ElementType.I);
			case OpCode.Stelem_i1: return Stelem(context, stack, ElementType.I1);
			case OpCode.Stelem_i2: return Stelem(context, stack, ElementType.I2);
			case OpCode.Stelem_i4: return Stelem(context, stack, ElementType.I4);
			case OpCode.Stelem_i8: return Stelem(context, stack, ElementType.I8);
			case OpCode.Stelem_r4: return Stelem(context, stack, ElementType.R4);
			case OpCode.Stelem_r8: return Stelem(context, stack, ElementType.R8);
			case OpCode.Stelem_ref: return Stelem(context, stack, ElementType.Object);
			case OpCode.Stfld: return Stfld(context, stack, instruction);
			case OpCode.Stind_i: return Stind(context, stack, ElementType.I);
			case OpCode.Stind_i1: return Stind(context, stack, ElementType.I1);
			case OpCode.Stind_i2: return Stind(context, stack, ElementType.I2);
			case OpCode.Stind_i4: return Stind(context, stack, ElementType.I4);
			case OpCode.Stind_i8: return Stind(context, stack, ElementType.I8);
			case OpCode.Stind_r4: return Stind(context, stack, ElementType.R4);
			case OpCode.Stind_r8: return Stind(context, stack, ElementType.R8);
			case OpCode.Stind_ref: return Stind(context, stack, ElementType.Object);
			case OpCode.Stloc: return Stloc(context, stack, (int)instruction.Operand);
			case OpCode.Stloc_0: return Stloc(context, stack, 0);
			case OpCode.Stloc_1: return Stloc(context, stack, 1);
			case OpCode.Stloc_2: return Stloc(context, stack, 2);
			case OpCode.Stloc_3: return Stloc(context, stack, 3);
			case OpCode.Stloc_s: return Stloc(context, stack, (int)instruction.Operand);
			case OpCode.Stobj: return Stobj(context, stack, instruction);
			case OpCode.Stsfld: return Stsfld(context, stack, instruction);
			case OpCode.Sub: return Sub(context, stack);
			case OpCode.Sub_ovf: return SubSigned(context, stack);
			case OpCode.Sub_ovf_un: return SubUnsigned(context, stack);
			case OpCode.Switch: return Switch(context, stack, instruction);
			case OpCode.Throw: return Throw(context, stack);
			case OpCode.Unbox: return Unbox(context, stack, instruction);
			case OpCode.Unbox_any: return UnboxAny(context, stack, instruction);
			case OpCode.Xor: return Xor(context, stack);

			default: return false;
		}
	}

	private Stack<StackEntry> CreateIncomingStack(BasicBlock block)
	{
		if (block.IsHandlerHeadBlock && ExceptionStackEntries.ContainsKey(block))
		{
			var incomingStack1 = new Stack<StackEntry>(1);

			incomingStack1.Push(ExceptionStackEntries[block]);

			return incomingStack1;
		}

		if (block.PreviousBlocks.Count == 0)
		{
			return new Stack<StackEntry>();
		}

		if (block.PreviousBlocks.Count == 1)
		{
			var outgoingstack = new Stack<StackEntry>(1);

			foreach (var stackentry in OutgoingStacks[block.PreviousBlocks[0]])
			{
				outgoingstack.Push(stackentry);
			}

			return outgoingstack;
		}

		var total = OutgoingStacks[block.PreviousBlocks[0]].Length;
		var incomingStack = new Stack<StackEntry>(total);

		for (int index = 0; index < total; index++)
		{
			StackEntry first = null;
			var identifcal = true;

			foreach (var previousBlock in block.PreviousBlocks)
			{
				var outgoingstack = OutgoingStacks[previousBlock];
				var outgoing = outgoingstack[index];

				if (first == null)
				{
					first = outgoing;
				}
				else if (first != outgoing)
				{
					identifcal = false;
					break;
				}
			}

			if (identifcal)
			{
				incomingStack.Push(first);
				continue;
			}

			Operand destination;
			BaseInstruction instruction;

			if (first.StackType == StackType.ValueType)
			{
				destination = AddStackLocal(first.Type);
				instruction = IRInstruction.MoveCompound;
			}
			else
			{
				var firstType = GetType(first);
				destination = AllocateVirtualRegister(firstType);
				instruction = GetMoveInstruction(firstType);
			}

			foreach (var previousBlock in block.PreviousBlocks)
			{
				var source = OutgoingStacks[previousBlock][index].Operand;

				previousBlock.ContextBeforeBranch.AppendInstruction(instruction, destination, source);
			}

			var entry = new StackEntry(first.StackType, destination, first.Type);

			incomingStack.Push(entry);
		}

		return incomingStack;
	}

	private void CreateLocalVariables()
	{
		int count = Method.LocalVariables.Count;

		LocalStack = new Operand[count];

		if (count == 0)
			return;

		var arg = new bool[count];
		var argCount = 0;

		var code = Method.Code;

		for (int label = 0; label < code.Count; label++)
		{
			var instruction = code[label];

			var opcode = (OpCode)instruction.OpCode;

			if (opcode == OpCode.Ldloca || opcode == OpCode.Ldloca_s)
			{
				var index = (int)instruction.Operand;

				if (!arg[index])
				{
					arg[index] = true;
					argCount++;

					if (argCount == count)  // early out
						break;
				}
			}
		}

		LocalStackType = new StackType[count];
		LocalElementType = new ElementType[count];

		for (int index = 0; index < count; index++)
		{
			var type = Method.LocalVariables[index];

			var underlyingType = GetUnderlyingType(type.Type);
			var stackType = GetStackTypeDefaultValueType(underlyingType);

			var isPrimitive = IsPrimitive(underlyingType);

			if (stackType == StackType.ValueType || arg[index] || type.IsPinned)
			{
				LocalStack[index] = MethodCompiler.AddStackLocal(type.Type, type.IsPinned);
			}
			else
			{
				LocalStack[index] = AllocatedOperand(stackType, type.Type);
			}

			LocalStackType[index] = stackType;
			LocalElementType[index] = isPrimitive ? GetElementType(underlyingType) : Is32BitPlatform ? ElementType.I4 : ElementType.I8;
		}
	}

	private void InitializeLocalVariables()
	{
		var prologue = new Context(BasicBlocks.PrologueBlock.AfterFirst);

		for (int index = 0; index < LocalStack.Length; index++)
		{
			var local = LocalStack[index];
			var localstacktype = LocalStackType[index];

			if (!local.IsVirtualRegister)
				continue;

			switch (localstacktype)
			{
				case StackType.Object:
					prologue.AppendInstruction(IRInstruction.MoveObject, local, Operand.GetNull(local.Type));
					break;

				case StackType.Int32:
					prologue.AppendInstruction(IRInstruction.Move32, local, Constant32_0);
					break;

				case StackType.Int64:
					prologue.AppendInstruction(IRInstruction.Move64, local, Constant64_0);
					break;

				case StackType.R4:
					prologue.AppendInstruction(IRInstruction.MoveR4, local, ConstantR4_0);
					break;

				case StackType.R8:
					prologue.AppendInstruction(IRInstruction.MoveR8, local, ConstantR8_0);
					break;

				default:
					prologue.AppendInstruction(IRInstruction.Move32, local, Constant32_0);
					break;
			}
		}
	}

	private static void UpdateLabel(InstructionNode node, int label, InstructionNode endNode)
	{
		while (node != endNode)
		{
			node.Label = label;
			node = node.Previous;
		}
	}

	private StackEntry PopStack(Stack<StackEntry> stack)
	{
		var entry = stack.Pop();

		trace?.Log($"     Pop  #{stack.Count}: {entry}");

		return entry;
	}

	private void PushStack(Stack<StackEntry> stack, StackEntry entry)
	{
		trace?.Log($"     Push #{stack.Count}: {entry}");

		stack.Push(entry);
	}

	#region Helpers

	private List<Operand> GetOperandParameters(Stack<StackEntry> stack, int paramCount, bool hasThis)
	{
		var count = paramCount + (hasThis ? 1 : 0);

		var operands = new List<Operand>(count);

		for (int i = 0; i < count; i++)
		{
			var operand = PopStack(stack).Operand;
			operands.Add(operand);
		}

		operands.Reverse();

		return operands;
	}

	private static MosaType GetUnderlyingType(MosaType type)
	{
		return MosaTypeLayout.GetUnderlyingType(type);
	}

	private static bool IsBranch(OpCode opcode)
	{
		return opcode switch
		{
			OpCode.Beq => true,
			OpCode.Beq_s => true,
			OpCode.Bge => true,
			OpCode.Bge_s => true,
			OpCode.Bge_un => true,
			OpCode.Bge_un_s => true,
			OpCode.Bgt => true,
			OpCode.Bgt_s => true,
			OpCode.Bgt_un => true,
			OpCode.Bgt_un_s => true,
			OpCode.Ble => true,
			OpCode.Ble_s => true,
			OpCode.Ble_un => true,
			OpCode.Ble_un_s => true,
			OpCode.Blt => true,
			OpCode.Blt_s => true,
			OpCode.Blt_un => true,
			OpCode.Blt_un_s => true,
			OpCode.Bne_un => true,
			OpCode.Bne_un_s => true,
			OpCode.Brfalse_s => true,
			OpCode.Brtrue_s => true,
			OpCode.Brfalse => true,
			OpCode.Brtrue => true,
			_ => false,
		};
	}

	private static bool IsPrimitive(MosaType underlyingType)
	{
		return MosaTypeLayout.IsPrimitive(underlyingType);
	}

	private Operand AllocatedOperand(StackType stackType, MosaType type = null)
	{
		return stackType switch
		{
			StackType.Int32 => AllocateVirtualRegister32(),
			StackType.Int64 => AllocateVirtualRegister64(),
			StackType.R4 => AllocateVirtualRegisterR4(),
			StackType.R8 => AllocateVirtualRegisterR8(),
			StackType.Object => AllocateVirtualRegisterObject(),
			StackType.ManagedPointer => AllocateVirtualRegisterManagedPointer(),
			StackType.ValueType => MethodCompiler.AddStackLocal(type),
			_ => throw new CompilerException("Not implemented yet"),
		};
	}

	private StackEntry CreateStateEntry(MosaType type)
	{
		if (type == null)
			return null;

		var underlyingType = GetUnderlyingType(type);
		var stackType = GetStackTypeDefaultValueType(underlyingType);

		var operand = AllocatedOperand(stackType, type);

		return (stackType == StackType.ValueType) ? new StackEntry(stackType, operand, type) : new StackEntry(stackType, operand);
	}

	private string EmitString(string data, uint token)
	{
		string symbolName = $"$ldstr${Method.Module.Name}${token}";
		var linkerSymbol = Linker.DefineSymbol(symbolName, SectionKind.ROData, NativeAlignment, (uint)(ObjectHeaderSize + NativePointerSize + (data.Length * 2)));
		var writer = new BinaryWriter(linkerSymbol.Stream);

		Linker.Link(LinkType.AbsoluteAddress, PatchType.I32, linkerSymbol, ObjectHeaderSize - NativePointerSize, Metadata.TypeDefinition + "System.String", 0);

		writer.WriteZeroBytes(ObjectHeaderSize);
		writer.Write(data.Length, NativePointerSize);
		writer.Write(Encoding.Unicode.GetBytes(data));
		return symbolName;
	}

	private ElementType GetElementType(StackType stackType)
	{
		return stackType switch
		{
			StackType.Int32 => ElementType.I4,
			StackType.Int64 => ElementType.I8,
			StackType.Object => ElementType.Object,
			StackType.R4 => ElementType.R4,
			StackType.R8 => ElementType.R8,
			StackType.ManagedPointer => ElementType.ManagedPointer,
			_ => throw new CompilerException($"Unable to convert stacktype ({stackType}) to element type"),
		};
	}

	private ElementType GetElementType(MosaType type)
	{
		if (type.IsReferenceType)
			return ElementType.Object;
		else if (type.IsI1)
			return ElementType.I1;
		else if (type.IsI2)
			return ElementType.I2;
		else if (type.IsI4)
			return ElementType.I4;
		else if (type.IsI8)
			return ElementType.I8;
		else if (type.IsU1)
			return ElementType.U1;
		else if (type.IsU2)
			return ElementType.U2;
		else if (type.IsU4)
			return ElementType.U4;
		else if (type.IsU8)
			return ElementType.U8;
		else if (type.IsR8)
			return ElementType.R8;
		else if (type.IsR4)
			return ElementType.R4;
		else if (type.IsBoolean)
			return ElementType.U1;
		else if (type.IsChar)
			return ElementType.U2;
		else if (type.IsI)
			return Is32BitPlatform ? ElementType.I4 : ElementType.I8;
		else if (type.IsManagedPointer)
			return ElementType.ManagedPointer;
		else if (type.IsPointer)
			return Is32BitPlatform ? ElementType.I4 : ElementType.I8;

		// TODO --- enums?

		throw new CompilerException($"Cannot translate to Type {type} to ElementType");
	}

	private BasicBlock GetOrCreateBlock(int label)
	{
		var block = BasicBlocks.GetByLabel(label);

		block ??= BasicBlocks.CreateBlock(label, label);

		return block;
	}

	private Operand GetMethodTablePointer(MosaType runtimeType)
	{
		return Operand.CreateLabel(TypeSystem.BuiltIn.Pointer, Metadata.TypeDefinition + runtimeType.FullName);
	}

	private Operand GetRuntimeTypeHandle(MosaType runtimeType)
	{
		return Operand.CreateLabel(TypeSystem.GetTypeByName("System.RuntimeTypeHandle"), Metadata.TypeDefinition + runtimeType.FullName);
	}

	private uint GetSize(ElementType elementType)
	{
		return elementType switch
		{
			ElementType.I1 => 1,
			ElementType.I2 => 2,
			ElementType.I4 => 4,
			ElementType.I8 => 8,
			ElementType.U1 => 1,
			ElementType.U2 => 2,
			ElementType.U4 => 4,
			ElementType.U8 => 8,
			ElementType.R4 => 4,
			ElementType.R8 => 8,
			ElementType.Object => Is32BitPlatform ? 4 : 8u,
			ElementType.ManagedPointer => Is32BitPlatform ? 4 : 8u,
			_ => throw new CompilerException($"Cannot get size of {elementType}"),
		};
	}

	private StackType GetStackType(MosaType type)
	{
		if (type.IsReferenceType)
			return StackType.Object;
		else if (type.IsManagedPointer)
			return StackType.ManagedPointer;
		else if (type.IsI1 || type.IsI2 || type.IsI4 || type.IsU1 || type.IsU2 || type.IsU4 || type.IsChar || type.IsBoolean)
			return StackType.Int32;
		else if (type.IsI8 || type.IsU8)
			return StackType.Int64;
		else if (type.IsR8)
			return StackType.R8;
		else if (type.IsR4)
			return StackType.R4;
		else if (type.IsI)
			return Is32BitPlatform ? StackType.Int32 : StackType.Int64;
		else if (type.IsPointer)
			return Is32BitPlatform ? StackType.Int32 : StackType.Int64;
		else if (type.IsValueType)
			return StackType.ValueType;

		throw new CompilerException($"Cannot translate to stacktype {type}");
	}

	private StackType GetStackTypeDefaultValueType(MosaType type)
	{
		return (type == null) ? StackType.ValueType : GetStackType(type);
	}

	private StackType GetStackType(ElementType elementType)
	{
		return elementType switch
		{
			ElementType.I1 => StackType.Int32,
			ElementType.I2 => StackType.Int32,
			ElementType.I4 => StackType.Int32,
			ElementType.I8 => StackType.Int64,
			ElementType.U1 => StackType.Int32,
			ElementType.U2 => StackType.Int32,
			ElementType.U4 => StackType.Int32,
			ElementType.U8 => StackType.Int64,
			ElementType.R4 => StackType.R4,
			ElementType.R8 => StackType.R8,
			ElementType.Object => StackType.Object,
			ElementType.ManagedPointer => StackType.ManagedPointer,
			_ => throw new CompilerException($"Cannot translate to ElementType {elementType} to StackType"),
		};
	}

	private MosaType GetType(StackType stackType)
	{
		return stackType switch
		{
			StackType.Int32 => TypeSystem.BuiltIn.I4,
			StackType.Int64 => TypeSystem.BuiltIn.I8,
			StackType.R4 => TypeSystem.BuiltIn.R4,
			StackType.R8 => TypeSystem.BuiltIn.R8,
			StackType.Object => TypeSystem.BuiltIn.Object,
			StackType.ManagedPointer => TypeSystem.BuiltIn.Pointer,
			_ => null,
		};
	}

	private MosaType GetType(StackEntry stackEntry)
	{
		if (stackEntry.StackType == StackType.ValueType)
			return stackEntry.Type;

		return GetType(stackEntry.StackType);
	}

	#endregion Helpers

	#region Instruction Maps

	private BaseIRInstruction GetBoxInstruction(ElementType elementType)
	{
		return elementType switch
		{
			ElementType.R4 => IRInstruction.BoxR4,
			ElementType.R8 => IRInstruction.BoxR8,
			ElementType.U4 => IRInstruction.Box32,
			ElementType.I4 => IRInstruction.Box32,
			ElementType.U8 => IRInstruction.Box64,
			ElementType.I8 => IRInstruction.Box64,
			ElementType.I1 => IRInstruction.Box32,
			ElementType.U1 => IRInstruction.Box32,
			ElementType.I2 => IRInstruction.Box32,
			ElementType.U2 => IRInstruction.Box32,
			ElementType.I when Is32BitPlatform => IRInstruction.Box32,
			ElementType.I when Is64BitPlatform => IRInstruction.Box64,
			_ => throw new CompilerException($"Invalid ElementType = {elementType}"),
		};
	}

	private BaseInstruction GetLoadInstruction(ElementType elementType)
	{
		return elementType switch
		{
			ElementType.I1 => IRInstruction.LoadSignExtend8x32,
			ElementType.U1 => IRInstruction.LoadZeroExtend8x32,
			ElementType.I2 => IRInstruction.LoadSignExtend16x32,
			ElementType.U2 => IRInstruction.LoadZeroExtend16x32,
			ElementType.I4 => IRInstruction.Load32,
			ElementType.U4 => IRInstruction.Load32,
			ElementType.I8 => IRInstruction.Load64,
			ElementType.U8 => IRInstruction.Load64,
			ElementType.R4 => IRInstruction.LoadR4,
			ElementType.R8 => IRInstruction.LoadR8,
			ElementType.Object => IRInstruction.LoadObject,
			ElementType.I when Is32BitPlatform => IRInstruction.Load32,
			ElementType.I when Is64BitPlatform => IRInstruction.Load64,
			ElementType.ManagedPointer when Is32BitPlatform => IRInstruction.Load32,
			ElementType.ManagedPointer when Is64BitPlatform => IRInstruction.Load64,
			_ => throw new CompilerException($"Invalid ElementType = {elementType}"),
		};
	}

	private BaseInstruction GetLoadParamInstruction(ElementType elementType)
	{
		return elementType switch
		{
			ElementType.I1 => IRInstruction.LoadParamSignExtend8x32,
			ElementType.U1 => IRInstruction.LoadParamZeroExtend8x32,
			ElementType.I2 => IRInstruction.LoadParamSignExtend16x32,
			ElementType.U2 => IRInstruction.LoadParamZeroExtend16x32,
			ElementType.I4 => IRInstruction.LoadParam32,
			ElementType.U4 => IRInstruction.LoadParam32,
			ElementType.I8 => IRInstruction.LoadParam64,
			ElementType.U8 => IRInstruction.LoadParam64,
			ElementType.R4 => IRInstruction.LoadParamR4,
			ElementType.R8 => IRInstruction.LoadParamR8,
			ElementType.Object => IRInstruction.LoadParamObject,
			ElementType.I when Is32BitPlatform => IRInstruction.LoadParam32,
			ElementType.I when Is64BitPlatform => IRInstruction.LoadParam64,
			ElementType.ManagedPointer when Is32BitPlatform => IRInstruction.LoadParam32,
			ElementType.ManagedPointer when Is64BitPlatform => IRInstruction.LoadParam64,
			_ => throw new CompilerException($"Invalid ElementType = {elementType}"),
		};
	}

	private BaseInstruction GetMoveInstruction(ElementType elementType)
	{
		return elementType switch
		{
			ElementType.I1 => IRInstruction.Move32,
			ElementType.U1 => IRInstruction.Move32,
			ElementType.I2 => IRInstruction.Move32,
			ElementType.U2 => IRInstruction.Move32,
			ElementType.I4 => IRInstruction.Move32,
			ElementType.U4 => IRInstruction.Move32,
			ElementType.I8 => IRInstruction.Move64,
			ElementType.U8 => IRInstruction.Move64,
			ElementType.R4 => IRInstruction.MoveR4,
			ElementType.R8 => IRInstruction.MoveR8,
			ElementType.Object => IRInstruction.MoveObject,
			ElementType.I when Is32BitPlatform => IRInstruction.Move32,
			ElementType.I when Is64BitPlatform => IRInstruction.Move64,
			ElementType.ManagedPointer when Is32BitPlatform => IRInstruction.Move32,
			ElementType.ManagedPointer when Is64BitPlatform => IRInstruction.Move64,
			_ => throw new CompilerException($"Invalid ElementType = {elementType}"),
		};
	}

	private BaseInstruction GetStoreInstruction(ElementType elementType)
	{
		return elementType switch
		{
			ElementType.I1 => IRInstruction.Store8,
			ElementType.U1 => IRInstruction.Store8,
			ElementType.I2 => IRInstruction.Store16,
			ElementType.U2 => IRInstruction.Store16,
			ElementType.I4 => IRInstruction.Store32,
			ElementType.U4 => IRInstruction.Store32,
			ElementType.I8 => IRInstruction.Store64,
			ElementType.U8 => IRInstruction.Store64,
			ElementType.R4 => IRInstruction.StoreR4,
			ElementType.R8 => IRInstruction.StoreR8,
			ElementType.Object => IRInstruction.StoreObject,
			ElementType.I when Is32BitPlatform => IRInstruction.Store32,
			ElementType.I when Is64BitPlatform => IRInstruction.Store64,
			ElementType.ManagedPointer when Is32BitPlatform => IRInstruction.Store32,
			ElementType.ManagedPointer when Is64BitPlatform => IRInstruction.Store64,
			_ => throw new CompilerException($"Invalid ElementType = {elementType}"),
		};
	}

	private BaseInstruction GetStoreParamInstruction(ElementType elementType)
	{
		return elementType switch
		{
			ElementType.I1 => IRInstruction.StoreParam8,
			ElementType.U1 => IRInstruction.StoreParam8,
			ElementType.I2 => IRInstruction.StoreParam16,
			ElementType.U2 => IRInstruction.StoreParam16,
			ElementType.I4 => IRInstruction.StoreParam32,
			ElementType.U4 => IRInstruction.StoreParam32,
			ElementType.I8 => IRInstruction.StoreParam64,
			ElementType.U8 => IRInstruction.StoreParam64,
			ElementType.R4 => IRInstruction.StoreParamR4,
			ElementType.R8 => IRInstruction.StoreParamR8,
			ElementType.Object => IRInstruction.StoreParamObject,
			ElementType.I when Is32BitPlatform => IRInstruction.StoreParam32,
			ElementType.I when Is64BitPlatform => IRInstruction.StoreParam64,
			ElementType.ManagedPointer when Is32BitPlatform => IRInstruction.StoreParam32,
			ElementType.ManagedPointer when Is64BitPlatform => IRInstruction.StoreParam64,
			_ => throw new CompilerException($"Invalid ElementType = {elementType}"),
		};
	}

	#endregion Instruction Maps

	#region CIL Shortcuts

	private bool Constant32(Context context, Stack<StackEntry> stack, int value)
	{
		var result = AllocateVirtualRegister32();
		context.AppendInstruction(IRInstruction.Move32, result, CreateConstant32(value));
		PushStack(stack, new StackEntry(StackType.Int32, result));
		return true;
	}

	private bool Constant64(Context context, Stack<StackEntry> stack, long value)
	{
		var result = AllocateVirtualRegister64();
		context.AppendInstruction(IRInstruction.Move64, result, CreateConstant64(value));
		PushStack(stack, new StackEntry(StackType.Int64, result));
		return true;
	}

	private bool ConstantR4(Context context, Stack<StackEntry> stack, float value)
	{
		var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
		context.AppendInstruction(IRInstruction.MoveR4, result, CreateConstantR4(value));
		PushStack(stack, new StackEntry(StackType.R4, result));
		return true;
	}

	private bool ConstantR8(Context context, Stack<StackEntry> stack, double value)
	{
		var result = AllocateVirtualRegisterR8();
		context.AppendInstruction(IRInstruction.MoveR8, result, CreateConstantR8(value));
		PushStack(stack, new StackEntry(StackType.R8, result));
		return true;
	}

	private bool Ldnull(Context context, Stack<StackEntry> stack)
	{
		var result = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);
		context.AppendInstruction(IRInstruction.MoveObject, result, Operand.GetNullObject(TypeSystem));
		PushStack(stack, new StackEntry(StackType.Object, result));
		return true;
	}

	#endregion CIL Shortcuts

	#region CIL

	private bool Add(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.StackType)
		{
			case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
				{
					var result = AllocateVirtualRegisterR4();
					context.AppendInstruction(IRInstruction.AddR4, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.R4, result));
					return true;
				}

			case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
				{
					var result = AllocateVirtualRegisterR8();
					context.AppendInstruction(IRInstruction.AddR8, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.R8, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.Add32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.Add64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int64:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.Add64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int32:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.Add64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is32BitPlatform:
			case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is32BitPlatform:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.Add32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
			case StackType.Int64 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.Add64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					var v1 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.Add64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					var v1 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.Add64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			default: return false;
		}
	}

	private bool AddUnsigned(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.StackType)
		{
			case StackType.Int32 when entry2.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					var result2 = AllocateVirtualRegister32();
					context.AppendInstruction2(IRInstruction.AddCarryOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					context.AppendInstruction2(IRInstruction.AddCarryOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int64:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.AddCarryOut64, result, result2, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int32:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.AddCarryOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is32BitPlatform:
			case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is32BitPlatform:
				{
					var result = AllocateVirtualRegister32();
					var result2 = AllocateVirtualRegister32();
					context.AppendInstruction2(IRInstruction.AddCarryOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
			case StackType.Int64 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					context.AppendInstruction2(IRInstruction.AddCarryOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					var v1 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.AddCarryOut64, result, result2, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					var v1 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.AddCarryOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			default: return false;
		}
	}

	private bool AddSigned(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.StackType)
		{
			case StackType.Int32 when entry2.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					var result2 = AllocateVirtualRegister32();
					context.AppendInstruction2(IRInstruction.AddOverflowOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					context.AppendInstruction2(IRInstruction.AddOverflowOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int64:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.AddOverflowOut64, result, result2, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int32:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.AddOverflowOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is32BitPlatform:
			case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is32BitPlatform:
				{
					var result = AllocateVirtualRegister32();
					var result2 = AllocateVirtualRegister32();
					context.AppendInstruction2(IRInstruction.AddOverflowOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
			case StackType.Int64 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					context.AppendInstruction2(IRInstruction.AddOverflowOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					var v1 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.AddOverflowOut64, result, result2, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					var v1 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.AddOverflowOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			default: return false;
		}
	}

	private bool And(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.StackType)
		{
			case StackType.Int32 when entry2.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.And32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.And64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int64:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.And64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int32:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.And64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			default:
				return false;
		}
	}

	private bool Box(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry = PopStack(stack);
		var type = (MosaType)instruction.Operand;

		var result = AllocateVirtualRegisterObject();
		PushStack(stack, new StackEntry(StackType.Object, result));

		if (type.IsReferenceType)
		{
			context.AppendInstruction(MoveInstruction, result, entry.Operand);
			return true;
		}

		var methodTable = GetMethodTablePointer(type);
		var isPrimitive = IsPrimitive(type);

		if (isPrimitive)
		{
			var elementType = GetElementType(type);
			var boxInstruction = GetBoxInstruction(elementType);

			context.AppendInstruction(boxInstruction, result, methodTable, entry.Operand);
		}
		else
		{
			var typeSize = Alignment.AlignUp(TypeLayout.GetTypeSize(type), TypeLayout.NativePointerAlignment);

			//var address = AllocateVirtualRegisterManagedPointer();
			//context.AppendInstruction(IRInstruction.AddressOf, address, entry.Operand);

			context.AppendInstruction(IRInstruction.Box, result, methodTable, entry.Operand, CreateConstant32(typeSize));
		}
		return true;
	}

	private bool Break(Context context, Stack<StackEntry> stack)
	{
		return true;
	}

	private bool Branch(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var target = (int)instruction.Operand;
		var block = BasicBlocks.GetByLabel(target);

		context.AppendInstruction(IRInstruction.Jmp, block);

		return true;
	}

	private bool Branch(Context context, Stack<StackEntry> stack, ConditionCode conditionCode, MosaInstruction instruction)
	{
		var entry2 = PopStack(stack);
		var entry1 = PopStack(stack);

		var target = (int)instruction.Operand;
		var block = BasicBlocks.GetByLabel(target);

		switch (entry1.StackType)
		{
			case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.CompareR4, conditionCode, result, entry1.Operand, entry2.Operand);
					context.AppendInstruction(IRInstruction.Branch32, ConditionCode.NotEqual, null, result, Constant32_0, block);
					return true;
				}

			case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.CompareR8, conditionCode, result, entry1.Operand, entry2.Operand);
					context.AppendInstruction(IRInstruction.Branch32, ConditionCode.NotEqual, null, result, Constant32_0, block);
					return true;
				}

			case StackType.Object when entry2.StackType == StackType.Object:
				context.AppendInstruction(IRInstruction.BranchObject, conditionCode, null, entry1.Operand, entry2.Operand, block);
				return true;

			case StackType.Int32 when entry2.StackType == StackType.Int32:
				context.AppendInstruction(IRInstruction.Branch32, conditionCode, null, entry1.Operand, entry2.Operand, block);
				return true;

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				context.AppendInstruction(IRInstruction.Branch64, conditionCode, null, entry1.Operand, entry2.Operand, block);
				return true;

			default:

				// TODO: Managed Pointers

				return false;
		}
	}

	private bool Branch1(Context context, Stack<StackEntry> stack, ConditionCode conditionCode, MosaInstruction instruction)
	{
		var entry = PopStack(stack);

		var target = (int)instruction.Operand;
		var block = BasicBlocks.GetByLabel(target);
		var nextblock = BasicBlocks.GetByLabel(instruction.Next.Value);

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.Branch32, conditionCode, null, entry.Operand, Constant32_0, block);
				context.AppendInstruction(IRInstruction.Jmp, nextblock);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.Branch64, conditionCode, null, entry.Operand, Constant64_0, block);
				context.AppendInstruction(IRInstruction.Jmp, nextblock);
				return true;

			case StackType.Object:
				context.AppendInstruction(IRInstruction.BranchObject, conditionCode, null, entry.Operand, ConstantZero, block);
				context.AppendInstruction(IRInstruction.Jmp, nextblock);
				return true;

			default:
				return false;
		}
	}

	private bool Call(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var method = (MosaMethod)instruction.Operand;

		var operands = GetOperandParameters(stack, method.Signature.Parameters.Count, method.HasThis && !method.HasExplicitThis);

		Operand result = null;

		if (!method.Signature.ReturnType.IsVoid)
		{
			var resultStackType = CreateStateEntry(method.Signature.ReturnType);

			result = resultStackType.Operand;

			PushStack(stack, resultStackType);
		}

		var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

		context.AppendInstruction(IRInstruction.CallStatic, result, symbol, operands);
		context.InvokeMethod = method;

		if (ProcessExternalCall(context))
			return true;

		return true;
	}

	private bool Callvirt(Context context, Stack<StackEntry> stack, MosaInstruction instruction, PrefixValues prefixValues)
	{
		var method = (MosaMethod)instruction.Operand;

		var operands = GetOperandParameters(stack, method.Signature.Parameters.Count, method.HasThis && !method.HasExplicitThis);

		Operand result = null;

		if (!method.Signature.ReturnType.IsVoid)
		{
			var resultStackType = CreateStateEntry(method.Signature.ReturnType);

			result = resultStackType.Operand;

			PushStack(stack, resultStackType);
		}

		if (prefixValues.Constrained)
		{
			var type = prefixValues.ContrainedType;

			if (type.IsValueType)
			{
				method = GetMethodOrOverride(type, method);

				var symbol2 = Operand.CreateSymbolFromMethod(method, TypeSystem);
				context.AppendInstruction(IRInstruction.CallStatic, result, symbol2, operands);

				// PocessExternalCall(context))

				return true;
			}
		}

		var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

		if (method.IsVirtual)
		{
			if (method.DeclaringType.IsInterface)
			{
				context.AppendInstruction(IRInstruction.CallInterface, result, symbol, operands);
			}
			else
			{
				context.AppendInstruction(IRInstruction.CallVirtual, result, symbol, operands);
			}
		}
		else
		{
			context.AppendInstruction(IRInstruction.CallStatic, result, symbol, operands);
		}

		if (ProcessExternalCall(context))
			return true;

		return true;
	}

	private bool Castclass(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);

		var result = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);
		PushStack(stack, new StackEntry(StackType.Object, result));

		if (entry.StackType == StackType.Object)
		{
			// TODO: Do this right
			context.AppendInstruction(IRInstruction.MoveObject, result, entry.Operand);
			return true;
		}

		return false;
	}

	private bool Compare(Context context, Stack<StackEntry> stack, ConditionCode conditionCode)
	{
		var entry2 = PopStack(stack);
		var entry1 = PopStack(stack);

		var result = AllocateVirtualRegister32();
		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry1.StackType)
		{
			case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
				context.AppendInstruction(IRInstruction.CompareR4, conditionCode, result, entry1.Operand, entry2.Operand);
				return true;

			case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
				context.AppendInstruction(IRInstruction.CompareR8, conditionCode, result, entry1.Operand, entry2.Operand);
				return true;

			case StackType.Object when entry2.StackType == StackType.Object:
				context.AppendInstruction(IRInstruction.CompareObject, conditionCode, result, entry1.Operand, entry2.Operand);
				return true;

			case StackType.Int32 when entry2.StackType == StackType.Int32:
				context.AppendInstruction(IRInstruction.Compare32x32, conditionCode, result, entry1.Operand, entry2.Operand);
				return true;

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				context.AppendInstruction(IRInstruction.Compare64x32, conditionCode, result, entry1.Operand, entry2.Operand);
				return true;

			default:

				// TODO: Managed Pointers

				return false;
		}
	}

	private static bool Constrained(PrefixValues prefixValues, MosaInstruction instruction)
	{
		var type = (MosaType)instruction.Operand;

		prefixValues.ContrainedType = type;
		prefixValues.Reset = false;

		return true;
	}

	private bool ConvertI(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);

		if (Is32BitPlatform)
		{
			var result = AllocateVirtualRegister32();
			PushStack(stack, new StackEntry(StackType.Int32, result));

			switch (entry.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
					return true;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.Truncate64x32, result, entry.Operand);
					return true;

				case StackType.R4:
					context.AppendInstruction(IRInstruction.ConvertR4ToI32, result, entry.Operand);
					return true;

				case StackType.R8:
					context.AppendInstruction(IRInstruction.ConvertR8ToI32, result, entry.Operand);
					return true;
			}
		}
		else
		{
			var result = AllocateVirtualRegister64();
			PushStack(stack, new StackEntry(StackType.Int64, result));

			switch (entry.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry.Operand);
					return true;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.Move64, result, entry.Operand);
					return true;

				case StackType.R4:
					context.AppendInstruction(IRInstruction.ConvertR4ToI64, result, entry.Operand);
					return true;

				case StackType.R8:
					context.AppendInstruction(IRInstruction.ConvertR8ToI64, result, entry.Operand);
					return true;
			}
		}

		return false;
	}

	private bool ConvertI1(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				{
					context.AppendInstruction(IRInstruction.SignExtend8x32, result, entry.Operand);
					return true;
				}

			case StackType.Int64:
				{
					var v1 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.SignExtend8x32, result, v1);
					return true;
				}

			case StackType.R4:
				{
					var v1 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.ConvertR4ToI32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
					return true;
				}

			case StackType.R8:
				{
					var v1 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.ConvertR8ToI32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
					return true;
				}

			default:
				return false;
		}
	}

	private bool ConvertI2(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				{
					context.AppendInstruction(IRInstruction.SignExtend16x32, result, entry.Operand);
					return true;
				}

			case StackType.Int64:
				{
					var v1 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.SignExtend16x32, result, v1);
					return true;
				}

			case StackType.R4:
				{
					var v1 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.ConvertR4ToI32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));
					return true;
				}

			case StackType.R8:
				{
					var v1 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.ConvertR8ToI32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));
					return true;
				}

			default:
				return false;
		}
	}

	private bool ConvertI4(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.Truncate64x32, result, entry.Operand);
				return true;

			case StackType.R4:
				context.AppendInstruction(IRInstruction.ConvertR4ToI32, result, entry.Operand);
				return true;

			case StackType.R8:
				context.AppendInstruction(IRInstruction.ConvertR8ToI32, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertI8(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister64();

		PushStack(stack, new StackEntry(StackType.Int64, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.SignExtend32x64, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.Move64, result, entry.Operand);
				return true;

			case StackType.R4:
				context.AppendInstruction(IRInstruction.ConvertR4ToI64, result, entry.Operand);
				return true;

			case StackType.R8:
				context.AppendInstruction(IRInstruction.ConvertR8ToI64, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertR4(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);

		PushStack(stack, new StackEntry(StackType.R4, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.ConvertI32ToR4, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.ConvertI64ToR4, result, entry.Operand);
				return true;

			case StackType.R4:
				context.AppendInstruction(IRInstruction.MoveR4, result, entry.Operand);
				return true;

			case StackType.R8:
				context.AppendInstruction(IRInstruction.ConvertR8ToR4, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertR8(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegisterR8();

		PushStack(stack, new StackEntry(StackType.R8, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.ConvertI32ToR8, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.ConvertI64ToR8, result, entry.Operand);
				return true;

			case StackType.R4:
				context.AppendInstruction(IRInstruction.ConvertR4ToR8, result, entry.Operand);
				return true;

			case StackType.R8:
				context.AppendInstruction(IRInstruction.MoveR8, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertU(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);

		if (Is32BitPlatform)
		{
			var result = AllocateVirtualRegister32();
			PushStack(stack, new StackEntry(StackType.Int32, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);
					return true;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.Truncate64x32, result, entry1.Operand);
					return true;

				case StackType.R4:
					context.AppendInstruction(IRInstruction.ConvertR4ToU32, result, entry1.Operand);
					return true;

				case StackType.R8:
					context.AppendInstruction(IRInstruction.ConvertR8ToU32, result, entry1.Operand);
					return true;

				case StackType.ManagedPointer:
					context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);
					return true;
			}

			// TODO: Float
		}
		else
		{
			var result = AllocateVirtualRegister64();
			PushStack(stack, new StackEntry(StackType.Int64, result));

			switch (entry1.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry1.Operand);
					return true;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.Move64, result, entry1.Operand);
					return true;

				case StackType.R4:
					context.AppendInstruction(IRInstruction.ConvertR4ToU64, result, entry1.Operand);
					return true;

				case StackType.R8:
					context.AppendInstruction(IRInstruction.ConvertR8ToU64, result, entry1.Operand);
					return true;

				case StackType.ManagedPointer:
					context.AppendInstruction(IRInstruction.Move64, result, entry1.Operand);
					return true;
			}

			// TODO: Float
		}

		return false;
	}

	private bool ConvertU1(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.And32, result, entry.Operand, CreateConstant32(0xFF));
				return true;

			case StackType.Int64:
				{
					var v1 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
					return true;
				}

			case StackType.R4:
				{
					var v1 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.ConvertR4ToU32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
					return true;
				}

			case StackType.R8:
				{
					var v1 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.ConvertR8ToU32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
					return true;
				}

			default:
				return false;
		}
	}

	private bool ConvertU2(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.And32, result, entry.Operand, CreateConstant32(0xFFFF));
				return true;

			case StackType.Int64:
				{
					var v1 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));
					return true;
				}

			case StackType.R4:
				{
					var v1 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.ConvertR4ToU32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));

					return true;
				}

			case StackType.R8:
				{
					var v1 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.ConvertR8ToI32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));

					return true;
				}

			default:
				return false;
		}
	}

	private bool ConvertU4(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.Truncate64x32, result, entry.Operand);
				return true;

			case StackType.R4:
				context.AppendInstruction(IRInstruction.ConvertR4ToU32, result, entry.Operand);
				return true;

			case StackType.R8:
				context.AppendInstruction(IRInstruction.ConvertR8ToU32, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertU8(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister64();
		PushStack(stack, new StackEntry(StackType.Int64, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.Move64, result, entry.Operand);
				return true;

			case StackType.R4:
				context.AppendInstruction(IRInstruction.ConvertR4ToU64, result, entry.Operand);
				return true;

			case StackType.R8:
				context.AppendInstruction(IRInstruction.ConvertR8ToU64, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToF(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegisterR8();
		PushStack(stack, new StackEntry(StackType.R8, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.ConvertU32ToR8, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.ConvertU64ToR8, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToI1(Context context, Stack<StackEntry> stack)
	{
		// convert unsigned to an int8 (on the stack as int32)

		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				{
					var v1 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.And32, v1, entry.Operand, CreateConstant32(0xFF));
					context.AppendInstruction(IRInstruction.Or32, result, v1, CreateConstant32(~0xFF));
					return true;
				}

			case StackType.Int64:
				{
					var v1 = AllocateVirtualRegister32();
					var v2 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFF));
					context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(~0xFF));
					return true;
				}

			default:
				return false;
		}
	}

	private bool ConvertUToI2(Context context, Stack<StackEntry> stack)
	{
		// convert unsigned to an int8 (on the stack as int32)

		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				{
					var v1 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.And32, v1, entry.Operand, CreateConstant32(0xFFFF));
					context.AppendInstruction(IRInstruction.Or32, result, v1, CreateConstant32(~0xFFFF));
					return true;
				}

			case StackType.Int64:
				{
					var v1 = AllocateVirtualRegister32();
					var v2 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFFFF));
					context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(~0xFFFF));
					return true;
				}

			default:
				return false;
		}
	}

	private bool ConvertUToI4(Context context, Stack<StackEntry> stack)
	{
		// convert unsigned to an int4 (on the stack as int32)

		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.Truncate64x32, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToI8(Context context, Stack<StackEntry> stack)
	{
		// convert unsigned to an int64 (on the stack as int32)

		var entry = PopStack(stack);
		var result = AllocateVirtualRegister64();

		PushStack(stack, new StackEntry(StackType.Int64, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.SignExtend32x64, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.Move64, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToU1(Context context, Stack<StackEntry> stack)
	{
		// convert unsigned to an unsigned int8 (on the stack as int32)

		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.And32, result, entry.Operand, CreateConstant32(0xFF));
				return true;

			case StackType.Int64:
				var v1 = AllocateVirtualRegister32();
				context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
				context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToU2(Context context, Stack<StackEntry> stack)
	{
		// convert unsigned to an unsigned int8 (on the stack as int32)

		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.And32, result, entry.Operand, CreateConstant32(0xFFFF));
				return true;

			case StackType.Int64:
				var v1 = AllocateVirtualRegister32();
				context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
				context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToU4(Context context, Stack<StackEntry> stack)
	{
		// convert unsigned to an unsigned int4 (on the stack as int32)

		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.Truncate64x32, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToU8(Context context, Stack<StackEntry> stack)
	{
		// convert unsigned to an unsigned int64 (on the stack as int32)

		var entry = PopStack(stack);
		var result = AllocateVirtualRegister64();

		PushStack(stack, new StackEntry(StackType.Int64, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.Move64, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertIWithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);

		if (Is32BitPlatform)
		{
			var result = AllocateVirtualRegister32();
			PushStack(stack, new StackEntry(StackType.Int32, result));

			switch (entry.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
					return true;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.CheckedConversionI64ToI32, result, entry.Operand);
					return true;

				case StackType.R4:
					context.AppendInstruction(IRInstruction.CheckedConversionR4ToI32, result, entry.Operand);
					return true;

				case StackType.R8:
					context.AppendInstruction(IRInstruction.CheckedConversionR8ToI32, result, entry.Operand);
					return true;
			}
		}
		else
		{
			var result = AllocateVirtualRegister64();
			PushStack(stack, new StackEntry(StackType.Int64, result));

			switch (entry.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.SignExtend32x64, result, entry.Operand);
					return true;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.Move64, result, entry.Operand);
					return true;

				case StackType.R4:
					context.AppendInstruction(IRInstruction.CheckedConversionR4ToI64, result, entry.Operand);
					return true;

				case StackType.R8:
					context.AppendInstruction(IRInstruction.CheckedConversionR8ToI64, result, entry.Operand);
					return true;
			}
		}

		return false;
	}

	private bool ConvertUWithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);

		if (Is32BitPlatform)
		{
			var result = AllocateVirtualRegister32();
			PushStack(stack, new StackEntry(StackType.Int32, result));

			switch (entry.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.CheckedConversionU32ToI32, result, entry.Operand);
					return true;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.CheckedConversionU64ToI32, result, entry.Operand);
					return true;
			}
		}
		else
		{
			var result = AllocateVirtualRegister64();
			PushStack(stack, new StackEntry(StackType.Int64, result));

			switch (entry.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry.Operand);
					return true;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.CheckedConversionU64ToI64, result, entry.Operand);
					return true;
			}
		}

		return false;
	}

	private bool ConvertI1WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionI32ToI8, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionI64ToI8, result, entry.Operand);
				return true;

			case StackType.R4:
				context.AppendInstruction(IRInstruction.CheckedConversionR4ToI8, result, entry.Operand);
				return true;

			case StackType.R8:
				context.AppendInstruction(IRInstruction.CheckedConversionR8ToI8, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToI1WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionU32ToI8, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionU64ToI8, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertI2WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionI32ToI16, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionI64ToI16, result, entry.Operand);
				return true;

			case StackType.R4:
				context.AppendInstruction(IRInstruction.CheckedConversionR4ToI16, result, entry.Operand);
				return true;

			case StackType.R8:
				context.AppendInstruction(IRInstruction.CheckedConversionR8ToI16, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToI2WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionU32ToI16, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionU64ToI16, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertI4WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionI64ToI32, result, entry.Operand);
				return true;

			case StackType.R4:
				context.AppendInstruction(IRInstruction.CheckedConversionR4ToI32, result, entry.Operand);
				return true;

			case StackType.R8:
				context.AppendInstruction(IRInstruction.CheckedConversionR8ToI32, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToI4WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int64, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionU32ToI32, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionU64ToI32, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertI8WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister64();

		PushStack(stack, new StackEntry(StackType.Int64, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.SignExtend32x64, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.Move64, result, entry.Operand);
				return true;

			case StackType.R4:
				context.AppendInstruction(IRInstruction.CheckedConversionR4ToI64, result, entry.Operand);
				return true;

			case StackType.R8:
				context.AppendInstruction(IRInstruction.CheckedConversionR8ToI64, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToI8WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister64();

		PushStack(stack, new StackEntry(StackType.Int64, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionU64ToI64, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertU1WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionI32ToU8, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionI64ToU8, result, entry.Operand);
				return true;

			case StackType.R4:
				context.AppendInstruction(IRInstruction.CheckedConversionR4ToU8, result, entry.Operand);
				return true;

			case StackType.R8:
				context.AppendInstruction(IRInstruction.CheckedConversionR8ToU8, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToU1WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionU32ToU8, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionU64ToU8, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertU2WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionI32ToU16, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionI64ToU16, result, entry.Operand);
				return true;

			case StackType.R4:
				context.AppendInstruction(IRInstruction.CheckedConversionR4ToU16, result, entry.Operand);
				return true;

			case StackType.R8:
				context.AppendInstruction(IRInstruction.CheckedConversionR8ToU16, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToU2WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionU32ToU16, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionU64ToU16, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertU4WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionI32ToU32, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionI64ToU32, result, entry.Operand);
				return true;

			case StackType.R4:
				context.AppendInstruction(IRInstruction.CheckedConversionR4ToU32, result, entry.Operand);
				return true;

			case StackType.R8:
				context.AppendInstruction(IRInstruction.CheckedConversionR8ToU32, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToU4WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int64, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionU64ToU32, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertU8WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister64();

		PushStack(stack, new StackEntry(StackType.Int64, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionI32ToU64, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionI64ToU64, result, entry.Operand);
				return true;

			case StackType.R4:
				context.AppendInstruction(IRInstruction.CheckedConversionR4ToU64, result, entry.Operand);
				return true;

			case StackType.R8:
				context.AppendInstruction(IRInstruction.CheckedConversionR8ToU64, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToU8WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = AllocateVirtualRegister32();

		PushStack(stack, new StackEntry(StackType.Int32, result));

		switch (entry.StackType)
		{
			case StackType.Int32:
				context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry.Operand);
				return true;

			case StackType.Int64:
				context.AppendInstruction(IRInstruction.Move64, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool Cpblk(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var source = PopStack(stack);
		var destination = PopStack(stack);

		if ((source.StackType == StackType.Int32 || source.StackType == StackType.Int64) && (destination.StackType == StackType.Int32 || destination.StackType == StackType.Int64))
		{
			context.AppendInstruction(IRInstruction.MemoryCopy, null, source.Operand, destination.Operand);
			return true;
		}

		return false;
	}

	private bool Cpobj(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var source = PopStack(stack);
		var destination = PopStack(stack);

		// TODO

		return false;
	}

	private bool Div(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.StackType)
		{
			case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
				{
					var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
					context.AppendInstruction(IRInstruction.DivR4, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.R4, result));
					return true;
				}

			case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
				{
					var result = AllocateVirtualRegisterR8();
					context.AppendInstruction(IRInstruction.DivR8, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.R8, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.DivSigned32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.DivSigned64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int64:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.DivSigned64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int32:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.DivSigned64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			default:
				return false;
		}
	}

	private bool DivUnsigned(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.StackType)
		{
			case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
				{
					var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
					context.AppendInstruction(IRInstruction.DivR4, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.R4, result));
					return true;
				}

			case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
				{
					var result = AllocateVirtualRegisterR8();
					context.AppendInstruction(IRInstruction.DivR8, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.R8, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.DivUnsigned32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.DivUnsigned64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int64:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.DivUnsigned64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int32:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.DivUnsigned64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			default:
				return false;
		}
	}

	private bool Dup(Context context, Stack<StackEntry> stack)
	{
		var entry = stack.Peek();
		PushStack(stack, entry);
		return true;
	}

	private bool Endfinally(Context context)
	{
		context.AppendInstruction(IRInstruction.FinallyEnd);

		return true;
	}

	private bool Initblk(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);
		var entry3 = PopStack(stack);

		context.AppendInstruction(IRInstruction.MemorySet, null, entry1.Operand, entry2.Operand, entry3.Operand);

		return true;
	}

	private bool InitObj(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry = PopStack(stack);

		// Retrieve the type reference
		var type = (MosaType)instruction.Operand;

		// According to ECMA Spec, if the pointer element type is a reference type then
		// this instruction is the equivalent of ldnull followed by stind.ref

		if (type.IsReferenceType)
		{
			context.AppendInstruction(IRInstruction.StoreObject, null, entry.Operand, ConstantZero, Operand.GetNullObject(TypeSystem));
			context.MosaType = type;
		}
		else
		{
			var size = CreateConstant32(TypeLayout.GetTypeSize(type));
			context.AppendInstruction(IRInstruction.MemorySet, null, entry.Operand, ConstantZero, size);
		}

		return true;
	}

	private bool Isinst(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry = PopStack(stack);
		var type = (MosaType)instruction.Operand;

		var result = AllocateVirtualRegisterObject();

		// TODO: non-nullable ValueTypes

		if (!type.IsInterface)
		{
			context.AppendInstruction(IRInstruction.IsInstanceOfType, result, GetRuntimeTypeHandle(type), entry.Operand);
		}
		else
		{
			var slot = TypeLayout.GetInterfaceSlot(type);
			context.AppendInstruction(IRInstruction.IsInstanceOfInterfaceType, result, CreateConstant32(slot), entry.Operand);
		}

		PushStack(stack, new StackEntry(StackType.Object, result));

		return true;
	}

	private bool Ldarg(Context context, Stack<StackEntry> stack, int index)
	{
		var parameter = MethodCompiler.Parameters[index];
		var type = parameter.Type;
		var underlyingType = GetUnderlyingType(type);
		var isPrimitive = IsPrimitive(underlyingType);

		if (isPrimitive)
		{
			var elementType = GetElementType(underlyingType);
			var stacktype = GetStackType(elementType);
			var result = AllocatedOperand(stacktype);

			var loadInstruction = GetLoadParamInstruction(elementType);
			context.AppendInstruction(loadInstruction, result, parameter);

			PushStack(stack, new StackEntry(stacktype, result));
		}
		else
		{
			var stacktype = GetStackType(type);
			var result = AllocatedOperand(stacktype, type);

			context.AppendInstruction(IRInstruction.LoadParamCompound, result, parameter);
			context.MosaType = type;

			PushStack(stack, new StackEntry(StackType.ValueType, result, type));
		}

		return true;
	}

	private bool Ldarga(Context context, Stack<StackEntry> stack, int index)
	{
		var parameter = MethodCompiler.Parameters[index];
		var result = AllocateVirtualRegisterManagedPointer();

		context.AppendInstruction(IRInstruction.AddressOf, result, parameter);

		PushStack(stack, new StackEntry(StackType.ManagedPointer, result));

		return true;
	}

	private bool Ldelem(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		var index = entry1.Operand;
		var array = entry2.Operand;

		var type = (MosaType)instruction.Operand;

		var underlyingType = GetUnderlyingType(type);
		var isPrimitive = IsPrimitive(underlyingType);

		context.AppendInstruction(IRInstruction.CheckArrayBounds, null, array, index);

		var elementOffset = CalculateArrayElementOffset(context, type, index);
		var totalElementOffset = CalculateTotalArrayOffset(context, elementOffset);

		if (isPrimitive)
		{
			var stacktype = GetStackType(underlyingType);
			var result = AllocatedOperand(stacktype);
			var elementType = GetElementType(stacktype);
			var loadInstruction = GetLoadInstruction(elementType);

			context.AppendInstruction(loadInstruction, result, array, totalElementOffset);

			PushStack(stack, new StackEntry(stacktype, result));
		}
		else
		{
			var result = AllocatedOperand(StackType.ValueType, type);

			context.AppendInstruction(IRInstruction.LoadCompound, result, array, totalElementOffset);
			context.MosaType = type.ElementType;

			PushStack(stack, new StackEntry(StackType.ValueType, result, type));
		}

		return true;
	}

	private bool Ldelem(Context context, Stack<StackEntry> stack, ElementType elementType)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		var array = entry2.Operand;
		var index = entry1.Operand;

		//var underlyingType = GetUnderlyingType(type.ElementType);

		context.AppendInstruction(IRInstruction.CheckArrayBounds, null, array, index);

		var elementOffset = CalculateArrayElementOffset(context, GetSize(elementType), index);
		var totalElementOffset = CalculateTotalArrayOffset(context, elementOffset);

		var stacktype = GetStackType(elementType);
		var result = AllocatedOperand(stacktype);
		PushStack(stack, new StackEntry(stacktype, result));

		var loadInstruction = GetLoadInstruction(elementType);

		context.AppendInstruction(loadInstruction, result, array, totalElementOffset);

		return true;
	}

	private bool Ldelema(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		var index = entry1.Operand;
		var array = entry2.Operand;

		var type = (MosaType)instruction.Operand;

		//var underlyingType = GetUnderlyingType(type);

		var result = AllocatedOperand(StackType.ManagedPointer);

		// Array bounds check
		context.AppendInstruction(IRInstruction.CheckArrayBounds, null, array, index);

		var elementOffset = CalculateArrayElementOffset(context, type, index);
		var totalElementOffset = CalculateTotalArrayOffset(context, elementOffset);

		context.AppendInstruction(Is32BitPlatform ? (BaseIRInstruction)IRInstruction.Add32 : IRInstruction.Add64, result, array, totalElementOffset);

		PushStack(stack, new StackEntry(StackType.ManagedPointer, result));

		return true;
	}

	private bool Ldfld(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry = PopStack(stack);

		var field = (MosaField)instruction.Operand;
		var offset = TypeLayout.GetFieldOffset(field);
		var fieldType = field.FieldType;

		var fieldUnderlyingType = GetUnderlyingType(fieldType);
		var fieldStacktype = GetStackTypeDefaultValueType(fieldUnderlyingType);
		var isFieldPrimitive = IsPrimitive(fieldUnderlyingType);

		var result = AllocatedOperand(fieldStacktype, isFieldPrimitive ? fieldUnderlyingType : fieldType);

		PushStack(stack, new StackEntry(fieldStacktype, result));

		var operand = entry.Operand;

		if (entry.StackType == StackType.ValueType)
		{
			var address = AllocateVirtualRegisterManagedPointer();

			context.AppendInstruction(IRInstruction.AddressOf, address, operand);

			operand = address;
		}

		var classUnderlyingType = GetUnderlyingType(field.DeclaringType);
		var classStacktype = GetStackTypeDefaultValueType(classUnderlyingType);
		var isClassPrimitive = IsPrimitive(classUnderlyingType);

		bool isPointer = operand.IsManagedPointer || operand.Type == TypeSystem.BuiltIn.I || operand.Type == TypeSystem.BuiltIn.U;
		bool isMove = (MosaTypeLayout.IsUnderlyingPrimitive(operand.Type) && !result.IsOnStack && !operand.IsReferenceType && !isPointer);

		if (isFieldPrimitive && isClassPrimitive && field.DeclaringType.IsValueType && !isPointer)
		{
			//&& (entry.StackType is StackType.ManagedPointer or StackType.Int32 or StackType.Int64)
			Debug.Assert(offset == 0);

			var elementType = GetElementType(fieldStacktype);
			var moveInstruction = GetMoveInstruction(elementType);

			context.AppendInstruction(moveInstruction, result, entry.Operand);

			return true;
		}

		var fixedOffset = CreateConstant32(offset);

		if (fieldStacktype == StackType.ValueType)
		{
			if (isFieldPrimitive)
			{
				var elementType = GetElementType(fieldStacktype);
				var loadInstruction = GetLoadInstruction(elementType);

				context.AppendInstruction(loadInstruction, result, operand, fixedOffset);
			}
			else
			{
				context.AppendInstruction(IRInstruction.LoadCompound, result, operand, fixedOffset);
				context.MosaType = fieldType;
			}
		}
		else if (isFieldPrimitive)
		{
			var loadInstruction = GetLoadInstruction(fieldUnderlyingType);
			context.AppendInstruction(loadInstruction, result, operand, fixedOffset);
		}
		else
		{
			context.AppendInstruction(IRInstruction.LoadCompound, result, operand, fixedOffset);
			context.MosaType = fieldType;
		}
		return true;
	}

	private bool Ldtoken(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var move = GetMoveInstruction(ElementType.I);

		Operand source = null;

		if (instruction.Operand is MosaType)
		{
			var type = (MosaType)instruction.Operand;
			source = Operand.CreateUnmanagedSymbolPointer(Metadata.TypeDefinition + type.FullName, TypeSystem);
		}
		else if (instruction.Operand is MosaMethod)
		{
			var method = (MosaMethod)instruction.Operand;
			source = Operand.CreateUnmanagedSymbolPointer(Metadata.MethodDefinition + method.FullName, TypeSystem);
		}
		else if (instruction.Operand is MosaField)
		{
			var field = (MosaField)instruction.Operand;
			source = Operand.CreateUnmanagedSymbolPointer(Metadata.FieldDefinition + field.FullName, TypeSystem);
			MethodScanner.AccessedField(context.MosaField);
		}

		var runtimeFieldHandle = TypeSystem.GetTypeByName("System.RuntimeFieldHandle"); // FUTURE: cache this

		var result = AllocateVirtualRegister(runtimeFieldHandle);
		context.AppendInstruction(move, result, source);

		//PushStack(stack, new StackEntry(Is32BitPlatform ? StackType.Int32 : StackType.Int64, result));
		PushStack(stack, new StackEntry(StackType.ValueType, result));

		return true;
	}

	private bool Leave(Context context, Stack<StackEntry> stack, MosaInstruction instruction, BasicBlock currentBlock)
	{
		var leaveBlock = BasicBlocks.GetByLabel((int)instruction.Operand);

		// Traverse to the header block
		var headerBlock = TraverseBackToNativeBlock(currentBlock);

		// Find enclosing try or finally handler
		var exceptionHandler = FindImmediateExceptionHandler(headerBlock.Label);
		bool inTry = exceptionHandler.IsLabelWithinTry(headerBlock.Label);

		var endInstruction = inTry ? (BaseInstruction)IRInstruction.TryEnd : IRInstruction.ExceptionEnd;

		context.AppendInstruction(endInstruction, leaveBlock);  // added header block

		return true;
	}

	private bool Ldflda(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry = PopStack(stack);

		var field = (MosaField)instruction.Operand;

		MethodScanner.AccessedField(field);

		uint offset = TypeLayout.GetFieldOffset(field);
		var fieldPtr = field.FieldType.ToManagedPointer(); // FUTURE: AllocateVirtualRegisterManagedPointer();

		var result = AllocatedOperand(StackType.ManagedPointer, fieldPtr);

		if (offset == 0)
		{
			var move = GetMoveInstruction(ElementType.I);

			context.AppendInstruction(move, result, entry.Operand);
		}
		else
		{
			if (Is32BitPlatform)
				context.AppendInstruction(IRInstruction.Add32, result, entry.Operand, CreateConstant32(offset));
			else
				context.AppendInstruction(IRInstruction.Add64, result, entry.Operand, CreateConstant64(offset));
		}

		PushStack(stack, new StackEntry(StackType.ManagedPointer, result));

		return true;
	}

	private bool Ldftn(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var method = (MosaMethod)instruction.Operand;

		var functionPointer = TypeSystem.ToFnPtr(method.Signature);

		var stacktype = GetStackType(functionPointer);
		var result = AllocatedOperand(stacktype);

		var move = GetMoveInstruction(ElementType.I);

		context.AppendInstruction(move, result, Operand.CreateSymbolFromMethod(method, TypeSystem));

		PushStack(stack, new StackEntry(stacktype, result));

		MethodScanner.MethodInvoked(method, Method);

		var methodData = MethodCompiler.Compiler.GetMethodData(method);

		if (!methodData.IsReferenced)
		{
			methodData.IsReferenced = true;

			MethodScheduler.AddToRecompileQueue(methodData);
		}

		return true;
	}

	private bool Ldind(Context context, Stack<StackEntry> stack, ElementType elementType)
	{
		var entry = PopStack(stack);

		var stacktype = GetStackType(elementType);
		var result = AllocatedOperand(stacktype);

		PushStack(stack, new StackEntry(stacktype, result));

		var loadInstruction = GetLoadInstruction(elementType);
		context.AppendInstruction(loadInstruction, result, entry.Operand, ConstantZero);

		return true;
	}

	private bool Ldlen(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);

		if (entry.StackType == StackType.Object)
		{
			if (Is32BitPlatform)
			{
				var result = AllocateVirtualRegister32();
				context.AppendInstruction(IRInstruction.Load32, result, entry.Operand, ConstantZero);
				PushStack(stack, new StackEntry(StackType.Int32, result));
				return true;
			}
			else
			{
				var result = AllocateVirtualRegister64();
				context.AppendInstruction(IRInstruction.Load64, result, entry.Operand, ConstantZero);
				PushStack(stack, new StackEntry(StackType.Int64, result));
				return true;
			}
		}

		return false;
	}

	private bool Ldloc(Context context, Stack<StackEntry> stack, int index)
	{
		var stacktype = LocalStackType[index];
		var local = LocalStack[index];

		if (stacktype == StackType.ValueType)
		{
			var result2 = AddStackLocal(local.Type);
			context.AppendInstruction(IRInstruction.MoveCompound, result2, local);
			context.MosaType = local.Type;

			PushStack(stack, new StackEntry(stacktype, result2, local.Type));

			return true;
		}

		var resultType = GetType(stacktype);
		var result = AllocateVirtualRegister(resultType);

		PushStack(stack, new StackEntry(stacktype, result));

		var elementType = LocalElementType[index];

		if (local.IsVirtualRegister)
		{
			var move = GetMoveInstruction(elementType);

			context.AppendInstruction(move, result, local);
			return true;
		}
		else
		{
			var loadInstruction = GetLoadInstruction(elementType);

			context.AppendInstruction(loadInstruction, result, StackFrame, local);
			return true;
		}
	}

	private bool Ldloca(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var index = (int)instruction.Operand;

		var local = LocalStack[index];

		var result = AllocateVirtualRegisterManagedPointer();

		PushStack(stack, new StackEntry(StackType.ManagedPointer, result));

		context.AppendInstruction(IRInstruction.AddressOf, result, local);

		return true;
	}

	private bool Ldobj(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry = PopStack(stack);
		var address = entry.Operand;
		var type = (MosaType)instruction.Operand;

		var underlyingType = GetUnderlyingType(type);
		var isPrimitive = IsPrimitive(underlyingType);

		if (isPrimitive)
		{
			var elementType = GetElementType(underlyingType);
			var stacktype = GetStackType(elementType);
			var result = AllocatedOperand(stacktype);

			PushStack(stack, new StackEntry(stacktype, result));

			var loadInstruction = GetLoadInstruction(elementType);
			context.AppendInstruction(loadInstruction, result, address, ConstantZero);

			return Ldind(context, stack, elementType);
		}
		else
		{
			var result = MethodCompiler.AddStackLocal(type);
			context.AppendInstruction(IRInstruction.LoadCompound, result, address, ConstantZero);
			context.MosaType = type;
			PushStack(stack, new StackEntry(StackType.ValueType, result, type));
			return true;
		}
	}

	private bool Ldsfld(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var field = (MosaField)instruction.Operand;
		var type = field.FieldType;

		var underlyingType = GetUnderlyingType(type);
		var isPrimitive = IsPrimitive(underlyingType);

		var fieldOperand = Operand.CreateStaticField(field, TypeSystem);

		if (isPrimitive)
		{
			var elementType = GetElementType(underlyingType);
			var stacktype = GetStackType(elementType);
			var result = AllocatedOperand(stacktype);

			PushStack(stack, new StackEntry(stacktype, result));

			if (type.IsReferenceType)
			{
				var symbol = GetStaticSymbol(field);
				var staticReference = Operand.CreateLabel(TypeSystem.BuiltIn.Object, symbol.Name);

				context.AppendInstruction(IRInstruction.LoadObject, result, staticReference, ConstantZero);
			}
			else
			{
				var loadInstruction = GetLoadInstruction(elementType);

				context.AppendInstruction(loadInstruction, result, fieldOperand, ConstantZero);
				context.MosaType = type;
			}
		}
		else
		{
			var result = AllocateVirtualRegister(type);
			context.AppendInstruction(IRInstruction.LoadCompound, result, fieldOperand, ConstantZero);
			context.MosaType = type;
			PushStack(stack, new StackEntry(StackType.ValueType, result, type));
		}

		return true;
	}

	private bool Ldsflda(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var field = (MosaField)instruction.Operand;

		var result = AllocateVirtualRegisterManagedPointer();
		var fieldOperand = Operand.CreateStaticField(field, TypeSystem);

		context.AppendInstruction(IRInstruction.AddressOf, result, fieldOperand);

		PushStack(stack, new StackEntry(StackType.ManagedPointer, result));   // FIXME: transient pointer or unmanaged pointer

		MethodScanner.AccessedField(field);

		return true;
	}

	private bool Ldstr(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var result = AllocateVirtualRegister(TypeSystem.BuiltIn.String);

		var token = (uint)instruction.Operand;

		var stringdata = TypeSystem.LookupUserString(Method.Module, token);
		var symbolName = EmitString(stringdata, token);

		var symbol = Operand.CreateStringSymbol(TypeSystem.BuiltIn.String, symbolName, MethodCompiler.Compiler.ObjectHeaderSize, stringdata);

		context.AppendInstruction(IRInstruction.MoveObject, result, symbol);

		PushStack(stack, new StackEntry(StackType.Object, result));

		return true;
	}

	private bool Mul(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.StackType)
		{
			case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
				{
					var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
					context.AppendInstruction(IRInstruction.MulR4, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.R4, result));
					return true;
				}

			case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
				{
					var result = AllocateVirtualRegisterR8();
					context.AppendInstruction(IRInstruction.MulR8, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.R8, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.MulSigned32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.MulSigned64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int64:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.MulSigned64, result, entry2.Operand, v1);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int32:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.MulSigned64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			default: return false;
		}
	}

	private bool MulSigned(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.StackType)
		{
			case StackType.Int32 when entry2.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					var result2 = AllocateVirtualRegister32();
					context.AppendInstruction2(IRInstruction.MulOverflowOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister32();
					context.AppendInstruction2(IRInstruction.MulOverflowOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int64:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.MulOverflowOut64, result, result2, entry2.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int32:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.MulOverflowOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			default: return false;
		}
	}

	private bool MulUnsigned(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.StackType)
		{
			case StackType.Int32 when entry2.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					var result2 = AllocateVirtualRegister32();
					context.AppendInstruction2(IRInstruction.MulCarryOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister32();
					context.AppendInstruction2(IRInstruction.MulCarryOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int64:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.MulCarryOut64, result, result2, entry2.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int32:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.MulCarryOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			default: return false;
		}
	}

	private bool Neg(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);

		switch (entry.StackType)
		{
			case StackType.R4:
				{
					var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
					var zero = CreateConstantR4(0);
					context.AppendInstruction(IRInstruction.SubR4, result, zero, entry.Operand);
					PushStack(stack, new StackEntry(StackType.R4, result));
					return true;
				}

			case StackType.R8:
				{
					var result = AllocateVirtualRegisterR8();
					var zero = CreateConstantR8(0);
					context.AppendInstruction(IRInstruction.SubR8, result, zero, entry.Operand);
					PushStack(stack, new StackEntry(StackType.R8, result));
					return true;
				}

			case StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.Sub32, result, Constant32_0, entry.Operand);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.Sub64, result, Constant64_0, entry.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			default:
				return false;
		}
	}

	private bool Newarr(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var elements = PopStack(stack);

		var arrayType = (MosaType)instruction.Operand;

		var elementSize = GetTypeSize(arrayType.ElementType, false);
		var methodTable = GetMethodTablePointer(arrayType);
		var size = CreateConstant32(elementSize);
		var result = AllocateVirtualRegisterObject();

		context.AppendInstruction(IRInstruction.NewArray, result, methodTable, size, elements.Operand);
		context.MosaType = arrayType;

		PushStack(stack, new StackEntry(StackType.Object, result));

		return true;
	}

	private bool Newobj(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var method = (MosaMethod)instruction.Operand;
		var classType = method.DeclaringType;
		int paramCount = method.Signature.Parameters.Count;

		var underlyingType = GetUnderlyingType(classType);
		var stackType = GetStackTypeDefaultValueType(underlyingType);

		var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

		var operands = new List<Operand>();

		for (int i = 0; i < paramCount; i++)
		{
			var param = PopStack(stack);
			operands.Add(param.Operand);
		}

		operands.Reverse();

		// FUTURE: Replace with new stage
		if (ProcessInternalCall(method, context, operands, stack))
			return true;

		MethodScanner.TypeAllocated(classType, Method);

		if (stackType == StackType.Object)
		{
			var result = AllocateVirtualRegisterObject();

			var methodTable = GetMethodTablePointer(classType);
			var size = CreateConstant32(TypeLayout.GetTypeSize(classType));

			context.AppendInstruction(IRInstruction.NewObject, result, methodTable, size);

			operands.Insert(0, result);

			context.AppendInstruction(IRInstruction.CallStatic, null, symbol, operands);

			//context.InvokeMethod = method;  // optional??
			context.MosaType = classType;   // optional??

			PushStack(stack, new StackEntry(StackType.Object, result));

			return true;
		}
		else if (stackType == StackType.ValueType)
		{
			var newThisLocal = AddStackLocal(classType);
			var newThis = AllocateVirtualRegisterManagedPointer();

			context.AppendInstruction(IRInstruction.AddressOf, newThis, newThisLocal);

			operands.Insert(0, newThis);

			context.AppendInstruction(IRInstruction.CallStatic, null, symbol, operands);

			//context.InvokeMethod = method; // optional??

			PushStack(stack, new StackEntry(StackType.ValueType, newThisLocal));

			return true;
		}
		else if (stackType == StackType.Int32 || stackType == StackType.Int64)
		{
			//var result = stackType == StackType.Int32 ? AllocateVirtualRegister32() : AllocateVirtualRegister64();
			var result = AllocateVirtualRegister(GetType(stackType));

			var newThisLocal = AddStackLocal(classType);
			var newThis = AllocateVirtualRegisterManagedPointer();

			context.AppendInstruction(IRInstruction.AddressOf, newThis, newThisLocal);

			operands.Insert(0, newThis);

			context.AppendInstruction(IRInstruction.CallStatic, null, symbol, operands);
			//context.InvokeMethod = method;  // optional??

			var loadInstruction = GetLoadInstruction(newThisLocal.Type);
			context.AppendInstruction(loadInstruction, result, StackFrame, newThisLocal);

			PushStack(stack, new StackEntry(stackType, result));

			return true;
		}
		else if (stackType != StackType.ValueType)
		{
			return false;
		}

		return false;
	}

	private bool Nop(Context context)
	{
		context.AppendInstruction(IRInstruction.Nop);
		return true;
	}

	private bool Not(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);

		switch (entry.StackType)
		{
			case StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.Not32, result, entry.Operand);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.Not64, result, entry.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			default:
				return false;
		}
	}

	private bool Or(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.StackType)
		{
			case StackType.Int32 when entry2.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.Or32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.Or64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int64:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.Or64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int32:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.Or64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			default: return false;
		}
	}

	private bool Pop(Context context, Stack<StackEntry> stack)
	{
		PopStack(stack);

		return true;
	}

	private bool RemOperand(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.StackType)
		{
			case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
				{
					var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
					context.AppendInstruction(IRInstruction.RemR4, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.R4, result));
					return true;
				}

			case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
				{
					var result = AllocateVirtualRegisterR8();
					context.AppendInstruction(IRInstruction.RemR8, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.R8, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.RemSigned32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.RemSigned64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int64:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.RemSigned64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int32:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.RemSigned64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			default:
				return false;
		}
	}

	private bool RemUnsigned(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.StackType)
		{
			case StackType.R4 when entry2.StackType == StackType.R4:
				{
					var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
					context.AppendInstruction(IRInstruction.RemR4, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.R4, result));
					return true;
				}

			case StackType.R8 when entry2.StackType == StackType.R8:
				{
					var result = AllocateVirtualRegisterR8();
					context.AppendInstruction(IRInstruction.RemR8, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.R8, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.RemUnsigned32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.RemUnsigned64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int64:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.RemUnsigned64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int32:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.RemUnsigned64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			default: return false;
		}
	}

	private bool Ret(Context context, Stack<StackEntry> stack)
	{
		if (!Method.Signature.ReturnType.IsVoid)
		{
			var entry = PopStack(stack);

			switch (entry.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.SetReturn32, null, entry.Operand);
					break;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.SetReturn64, null, entry.Operand);
					break;

				case StackType.R4:
					context.AppendInstruction(IRInstruction.SetReturnR4, null, entry.Operand);
					break;

				case StackType.R8:
					context.AppendInstruction(IRInstruction.SetReturnR8, null, entry.Operand);
					break;

				case StackType.Object:
					context.AppendInstruction(IRInstruction.SetReturnObject, null, entry.Operand);
					break;

				case StackType.ValueType:
					context.AppendInstruction(IRInstruction.SetReturnCompound, null, entry.Operand);
					context.MosaType = entry.Type;
					break;

				case StackType.ManagedPointer when Is32BitPlatform:
					context.AppendInstruction(IRInstruction.SetReturn32, null, entry.Operand);
					break;

				case StackType.ManagedPointer when Is64BitPlatform:
					context.AppendInstruction(IRInstruction.SetReturn64, null, entry.Operand);
					break;

				default: return false;
			}
		}

		var block = GetOrCreateBlock(BasicBlock.EpilogueLabel);
		context.AppendInstruction(IRInstruction.Jmp, block);

		return true;
	}

	private bool Rethrow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);

		context.AppendInstruction(IRInstruction.Rethrow, entry.Operand);

		return true;
	}

	private bool Shl(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		var shift = entry1.Operand;
		var value = entry2.Operand;

		switch (entry1.StackType)
		{
			case StackType.Int32 when entry2.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.ShiftLeft32, result, value, shift);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ShiftLeft64, result, value, shift);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ShiftLeft64, result, value, shift);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ShiftLeft64, result, value, shift);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			default: return false;
		}
	}

	private bool Shr(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		var shiftAmount = entry1.Operand;
		var value = entry2.Operand;

		switch (entry2.StackType)
		{
			case StackType.Int32 when entry1.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.ArithShiftRight32, result, value, shiftAmount);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry1.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ArithShiftRight64, result, value, shiftAmount);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry1.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ArithShiftRight64, result, value, shiftAmount);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry1.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ArithShiftRight64, result, value, shiftAmount);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			default: return false;
		}
	}

	private bool ShrU(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		var shiftAmount = entry1.Operand;
		var value = entry2.Operand;

		switch (entry2.StackType)
		{
			case StackType.Int32 when entry1.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.ShiftRight32, result, value, shiftAmount);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry1.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ShiftRight32, result, value, shiftAmount);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry1.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.ShiftRight64, result, value, shiftAmount);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry1.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ShiftRight64, result, value, shiftAmount);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			default: return false;
		}
	}

	private bool Sizeof(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var type = (MosaType)instruction.Operand;
		var result = AllocateVirtualRegister32();

		var size = type.IsPointer ? NativePointerSize : MethodCompiler.TypeLayout.GetTypeSize(type);

		context.AppendInstruction(IRInstruction.Move32, result, CreateConstant32(size));

		PushStack(stack, new StackEntry(StackType.Int32, result));

		return true;
	}

	private bool Stelem(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);
		var entry3 = PopStack(stack);

		var array = entry3.Operand;
		var index = entry2.Operand;
		var value = entry1.Operand;

		var type = (MosaType)instruction.Operand;

		var underlyingType = GetUnderlyingType(type);
		var isPrimitive = IsPrimitive(underlyingType);

		context.AppendInstruction(IRInstruction.CheckArrayBounds, null, array, index);

		var elementOffset = CalculateArrayElementOffset(context, type, index);
		var totalElementOffset = CalculateTotalArrayOffset(context, elementOffset);

		if (isPrimitive)
		{
			var stacktype = GetStackType(underlyingType);
			var elementType = GetElementType(stacktype);
			var storeInstruction = GetStoreInstruction(elementType);

			context.AppendInstruction(storeInstruction, null, array, totalElementOffset, value);

			return true;
		}
		else
		{
			context.AppendInstruction(IRInstruction.StoreCompound, null, array, totalElementOffset, value);
			context.MosaType = type;

			return true;
		}
	}

	private bool Stelem(Context context, Stack<StackEntry> stack, ElementType elementType)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);
		var entry3 = PopStack(stack);

		var array = entry3.Operand;
		var index = entry2.Operand;
		var value = entry1.Operand;

		context.AppendInstruction(IRInstruction.CheckArrayBounds, null, array, index);

		var elementOffset = CalculateArrayElementOffset(context, GetSize(elementType), index);
		var totalElementOffset = CalculateTotalArrayOffset(context, elementOffset);

		var storeInstruction = GetStoreInstruction(elementType);

		context.AppendInstruction(storeInstruction, null, array, totalElementOffset, value);

		return true;
	}

	private bool Stfld(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		var field = (MosaField)instruction.Operand;
		var offset = TypeLayout.GetFieldOffset(field);
		var type = field.FieldType;

		var offsetOperand = CreateConstant32(offset);

		switch (entry1.StackType)
		{
			case StackType.Int32:
			case StackType.Int64:
			case StackType.R4:
			case StackType.R8:
			case StackType.ManagedPointer:
			case StackType.Object:
				{
					var underlyingType = GetUnderlyingType(type);
					var isPrimitive = IsPrimitive(underlyingType);

					if (isPrimitive)
					{
						var elementType = GetElementType(underlyingType);
						var storeInstruction = GetStoreInstruction(elementType);

						context.AppendInstruction(storeInstruction, null, entry2.Operand, offsetOperand, entry1.Operand);
						//context.MosaType = type;

						return true;
					}
					else
					{
						context.AppendInstruction(IRInstruction.StoreCompound, null, entry2.Operand, offsetOperand, entry1.Operand);
						context.MosaType = type;

						return true;
					}
				}
			case StackType.ValueType:
				{
					context.AppendInstruction(IRInstruction.StoreCompound, null, entry2.Operand, offsetOperand, entry1.Operand);
					context.MosaType = type;

					return true;
				}

			default: return false;
		}
	}

	private bool Stind(Context context, Stack<StackEntry> stack, ElementType elementType)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		var address = entry2.Operand;
		var value = entry1.Operand;

		var storeInstruction = GetStoreInstruction(elementType);
		context.AppendInstruction(storeInstruction, null, address, ConstantZero, value);

		return true;
	}

	private bool Stloc(Context context, Stack<StackEntry> stack, int index)
	{
		var entry = PopStack(stack);
		var source = entry.Operand;

		var stacktype = LocalStackType[index];
		var local = LocalStack[index];

		if (stacktype == StackType.ValueType)
		{
			context.AppendInstruction(IRInstruction.MoveCompound, local, source);
			context.MosaType = local.Type;
			return true;
		}

		var elementType = LocalElementType[index];

		if (local.IsVirtualRegister)
		{
			var storeInstruction = GetMoveInstruction(elementType);

			context.AppendInstruction(storeInstruction, local, source);
			return true;
		}
		else
		{
			var storeInstruction = GetStoreInstruction(elementType);

			context.AppendInstruction(storeInstruction, null, StackFrame, local, source);
			return true;
		}

		//if (local.IsVirtualRegister)
		//{
		//	switch (stacktype)
		//	{
		//		case StackType.Int32:
		//			context.AppendInstruction(IRInstruction.Move32, local, source);
		//			return true;

		//		case StackType.Int64:
		//			context.AppendInstruction(IRInstruction.Move64, local, source);
		//			return true;

		//		case StackType.Object:
		//			context.AppendInstruction(IRInstruction.MoveObject, local, source);
		//			return true;

		//		case StackType.R4:
		//			context.AppendInstruction(IRInstruction.MoveR4, local, source);
		//			return true;

		//		case StackType.R8:
		//			context.AppendInstruction(IRInstruction.MoveR8, local, source);
		//			return true;

		//		case StackType.ManagedPointer when Is32BitPlatform:
		//			context.AppendInstruction(IRInstruction.Move32, local, source);
		//			return true;

		//		case StackType.ManagedPointer when Is64BitPlatform:
		//			context.AppendInstruction(IRInstruction.Move64, local, source);
		//			return true;

		//		default: return false;
		//	}
		//}
		//else
		//{
		//	switch (stacktype)
		//	{
		//		case StackType.Int32:
		//			context.AppendInstruction(IRInstruction.Store32, null, StackFrame, local, source);
		//			return true;

		//		case StackType.Int64:
		//			context.AppendInstruction(IRInstruction.Store64, null, StackFrame, local, source);
		//			return true;

		//		case StackType.Object:
		//			context.AppendInstruction(IRInstruction.StoreObject, null, StackFrame, local, source);
		//			return true;

		//		case StackType.R4:
		//			context.AppendInstruction(IRInstruction.StoreR4, null, StackFrame, local, source);
		//			return true;

		//		case StackType.R8:
		//			context.AppendInstruction(IRInstruction.StoreR8, null, StackFrame, local, source);
		//			return true;

		//		case StackType.ManagedPointer when Is32BitPlatform:
		//			context.AppendInstruction(IRInstruction.Store32, null, StackFrame, local, source);
		//			return true;

		//		case StackType.ManagedPointer when Is64BitPlatform:
		//			context.AppendInstruction(IRInstruction.Store64, null, StackFrame, local, source);
		//			return true;

		//		default: return false;
		//	}
		//}

		//return false;
	}

	private bool Stobj(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		var address = entry2.Operand;
		var value = entry1.Operand;

		var type = (MosaType)instruction.Operand;
		var underlyingType = GetUnderlyingType(type);
		var isPrimitive = IsPrimitive(underlyingType);

		if (isPrimitive)
		{
			var stackType = GetStackType(underlyingType);
			var elementType = GetElementType(stackType);
			var storeInstruction = GetStoreInstruction(elementType);

			context.AppendInstruction(storeInstruction, null, address, ConstantZero, value);
			return true;
		}
		else
		{
			context.AppendInstruction(IRInstruction.StoreCompound, null, address, ConstantZero, value);
			return true;
		}
	}

	private bool Stsfld(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry = PopStack(stack);
		var source = entry.Operand;

		var field = (MosaField)instruction.Operand;
		var type = field.FieldType;

		var underlyingType = GetUnderlyingType(type);
		var isPrimitive = IsPrimitive(underlyingType);

		var fieldOperand = Operand.CreateStaticField(field, TypeSystem);

		if (isPrimitive)
		{
			var elementType = GetElementType(underlyingType);

			var storeInstruction = GetStoreInstruction(elementType);

			if (type.IsReferenceType)
			{
				var symbol = GetStaticSymbol(field);
				var staticReference = Operand.CreateLabel(TypeSystem.BuiltIn.Object, symbol.Name);

				context.AppendInstruction(IRInstruction.StoreObject, null, staticReference, ConstantZero, source);

				//context.MosaType = type;
			}
			else
			{
				context.AppendInstruction(storeInstruction, null, fieldOperand, ConstantZero, source);
				context.MosaType = type;
			}
		}
		else
		{
			context.AppendInstruction(IRInstruction.StoreCompound, null, fieldOperand, ConstantZero, source);
			context.MosaType = type;
		}

		return true;
	}

	private bool StoreArgument(Context context, Stack<StackEntry> stack, int index)
	{
		var entry = PopStack(stack);

		var value = entry.Operand;

		var parameter = MethodCompiler.Parameters[index];
		var type = parameter.Type;
		var underlyingType = GetUnderlyingType(type);
		var isPrimitive = IsPrimitive(underlyingType);

		if (isPrimitive)
		{
			var elementType = GetElementType(underlyingType);
			var storeInstruction = GetStoreParamInstruction(elementType);

			context.AppendInstruction(storeInstruction, null, parameter, value);
			return true;
		}
		else
		{
			context.AppendInstruction(IRInstruction.StoreParamCompound, null, parameter, value);
			context.MosaType = type;
			return true;
		}
	}

	private bool Sub(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.StackType)
		{
			case StackType.R4 when entry2.StackType == StackType.R4:
				{
					var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
					context.AppendInstruction(IRInstruction.SubR4, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.R4, result));
					return true;
				}

			case StackType.R8 when entry2.StackType == StackType.R8:
				{
					var result = AllocateVirtualRegisterR8();
					context.AppendInstruction(IRInstruction.SubR8, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.R8, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.Sub32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.Sub64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int64:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.Sub64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int32:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.Sub64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.ManagedPointer when entry2.StackType == StackType.ManagedPointer && Is32BitPlatform:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.Sub32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.ManagedPointer when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.Sub64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is32BitPlatform:
			case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is32BitPlatform:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.Sub32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.Sub64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					var v1 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.Sub64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					var v1 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.Sub64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			default: return false;
		}
	}

	private bool SubSigned(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.StackType)
		{
			case StackType.Int32 when entry2.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					var result2 = AllocateVirtualRegister32();
					context.AppendInstruction2(IRInstruction.SubOverflowOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					context.AppendInstruction2(IRInstruction.SubOverflowOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int64:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.SubOverflowOut64, result, result2, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int32:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.SubOverflowOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.ManagedPointer when entry2.StackType == StackType.ManagedPointer && Is32BitPlatform:
				{
					var result = AllocateVirtualRegister32();
					var result2 = AllocateVirtualRegister32();
					context.AppendInstruction2(IRInstruction.SubOverflowOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.ManagedPointer when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					context.AppendInstruction2(IRInstruction.SubOverflowOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is32BitPlatform:
			case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is32BitPlatform:
				{
					var result = AllocateVirtualRegister32();
					var result2 = AllocateVirtualRegister32();
					context.AppendInstruction2(IRInstruction.SubOverflowOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					context.AppendInstruction2(IRInstruction.SubOverflowOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					var v1 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.SubOverflowOut64, result, result2, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					var v1 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.SubOverflowOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			default: return false;
		}
	}

	private bool SubUnsigned(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.StackType)
		{
			case StackType.Int32 when entry2.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					var result2 = AllocateVirtualRegister32();
					context.AppendInstruction2(IRInstruction.SubCarryOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					context.AppendInstruction2(IRInstruction.SubCarryOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int64:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.SubCarryOut64, result, result2, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int32:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.SubCarryOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.ManagedPointer when entry2.StackType == StackType.ManagedPointer && Is32BitPlatform:
				{
					var result = AllocateVirtualRegister32();
					var result2 = AllocateVirtualRegister32();
					context.AppendInstruction2(IRInstruction.SubCarryOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.ManagedPointer when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					context.AppendInstruction2(IRInstruction.SubCarryOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is32BitPlatform:
			case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is32BitPlatform:
				{
					var result = AllocateVirtualRegister32();
					var result2 = AllocateVirtualRegister32();
					context.AppendInstruction2(IRInstruction.SubCarryOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					context.AppendInstruction2(IRInstruction.SubCarryOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					var v1 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.SubCarryOut64, result, result2, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is64BitPlatform:
				{
					var result = AllocateVirtualRegister64();
					var result2 = AllocateVirtualRegister64();
					var v1 = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.SubCarryOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(StackType.ManagedPointer, result));
					return true;
				}

			default: return false;
		}
	}

	private bool Throw(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);

		if (entry.StackType == StackType.Object)
		{
			context.AppendInstruction(IRInstruction.Throw, null, entry.Operand);
			stack.Clear();
			return true;
		}

		return false;
	}

	private bool Switch(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry = PopStack(stack);

		context.AppendInstruction(IRInstruction.Switch, null, entry.Operand);

		foreach (var target in (int[])instruction.Operand)
		{
			var block = BasicBlocks.GetByLabel(target);

			context.AddBranchTarget(block);
		}

		// REFERENCE: The last value is the fall thru - this is not implemented correctly in later stages (fixme)
		context.AddBranchTarget(BasicBlocks.GetByLabel(instruction.Next.Value));

		return true;
	}

	private bool Unbox(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry = PopStack(stack);
		var type = (MosaType)instruction.Operand;

		var underlyingType = GetUnderlyingType(type);
		var isPrimitive = IsPrimitive(underlyingType);

		if (isPrimitive)
		{
			var elementType = GetElementType(underlyingType);

			var moveInstruction = GetMoveInstruction(elementType);
			var stackType = GetStackType(elementType);
			var result = AllocatedOperand(stackType);

			context.AppendInstruction(moveInstruction, result, entry.Operand, ConstantZero); //CreateConstant32(8)

			PushStack(stack, new StackEntry(stackType, result));
		}
		else
		{
			var result = AllocatedOperand(StackType.ManagedPointer);
			PushStack(stack, new StackEntry(StackType.ManagedPointer, result));

			var v1 = AllocateVirtualRegisterManagedPointer();
			var loadInstruction = GetLoadInstruction(type);

			context.AppendInstruction(IRInstruction.Unbox, v1, entry.Operand);
			context.AppendInstruction(loadInstruction, result, v1, ConstantZero);
			context.MosaType = type;
		}

		return true;
	}

	private bool UnboxAny(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry = PopStack(stack);

		var type = (MosaType)instruction.Operand;

		// FUTURE: Check for valid cast
		var methodTable = GetMethodTablePointer(type);

		if (type.IsReferenceType)
		{
			// FUTURE: treat as castclass
			PushStack(stack, entry);
			return true;
		}

		var underlyingType = GetUnderlyingType(type);
		var isPrimitive = IsPrimitive(underlyingType);

		if (isPrimitive)
		{
			var elementType = GetElementType(underlyingType);

			var loadInstruction = GetLoadInstruction(elementType);
			var stackType = GetStackType(elementType);
			var result = AllocatedOperand(stackType);

			context.AppendInstruction(loadInstruction, result, entry.Operand, ConstantZero); //CreateConstant32(8)

			PushStack(stack, new StackEntry(stackType, result));
			return true;
		}
		else
		{
			var result = AllocatedOperand(StackType.ValueType, type);

			//var tmpLocal = AddStackLocal(type);
			//var typeSize = Alignment.AlignUp(TypeLayout.GetTypeSize(type), TypeLayout.NativePointerAlignment);

			//var adr = AllocateVirtualRegisterManagedPointer();
			//context.AppendInstruction(IRInstruction.AddressOf, adr, tmpLocal);
			//context.AppendInstruction(IRInstruction.UnboxAny, tmpLocal, entry.Operand, adr, CreateConstant32(typeSize));
			//context.AppendInstruction(IRInstruction.LoadCompound, result, tmpLocal, ConstantZero);

			//context.AppendInstruction(IRInstruction.LoadCompound, result, tmpLocal, ConstantZero);

			var adr = AllocateVirtualRegisterManagedPointer();
			context.AppendInstruction(IRInstruction.Unbox, adr, entry.Operand);
			context.AppendInstruction(IRInstruction.LoadCompound, result, adr, ConstantZero);

			PushStack(stack, new StackEntry(StackType.ValueType, result, type));
			return true;
		}
	}

	private bool Xor(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.StackType)
		{
			case StackType.Int32 when entry2.StackType == StackType.Int32:
				{
					var result = AllocateVirtualRegister32();
					context.AppendInstruction(IRInstruction.Xor32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int32, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int64:
				{
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.Xor64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int32 when entry2.StackType == StackType.Int64:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.Xor64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			case StackType.Int64 when entry2.StackType == StackType.Int32:
				{
					var v1 = AllocateVirtualRegister64();
					var result = AllocateVirtualRegister64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.Xor64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(StackType.Int64, result));
					return true;
				}

			default: return false;
		}
	}

	#endregion CIL

	public LinkerSymbol GetStaticSymbol(MosaField field)
	{
		return Linker.DefineSymbol($"{Metadata.StaticSymbolPrefix}{field.DeclaringType}+{field.Name}", SectionKind.BSS, Architecture.NativeAlignment, NativePointerSize);
	}

	private bool ProcessInternalCall(MosaMethod method, Context context, List<Operand> operands, Stack<StackEntry> stack)
	{
		if (!method.IsInternal || !method.IsConstructor)
			return false;

		if (method.DeclaringType.IsValueType)
			return false;

		var newmethod = method.DeclaringType.FindMethodByNameAndParameters("Ctor", method.Signature.Parameters);

		var result = AllocateVirtualRegisterObject();
		var symbol = Operand.CreateSymbolFromMethod(newmethod, TypeSystem);

		context.AppendInstruction(IRInstruction.CallStatic, result, symbol);
		context.AppendOperands(operands);

		PushStack(stack, new StackEntry(StackType.Object, result));

		MethodScanner.TypeAllocated(newmethod.DeclaringType, newmethod);

		return true;
	}

	/// <summary>Processes external method calls.</summary>
	/// <param name="context">The transformation context.</param>
	/// <returns>
	///   <c>true</c> if the method was replaced by an intrinsic; <c>false</c> otherwise.</returns>
	/// <remarks>This method checks if the call target has an Intrinsic-Attribute applied with
	/// the current architecture. If it has, the method call is replaced by the specified
	/// native instruction.</remarks>
	private bool ProcessExternalCall(Context context)
	{
		var method = context.Operand1.Method;

		var intrinsic = ResolveIntrinsicDelegateMethod(method);

		if (intrinsic == null)
			return false;

		if (method.IsExternal)
		{
			var operands = context.GetOperands();
			var result = context.Result;

			//operands.Insert(0, Operand.CreateSymbolFromMethod(method, TypeSystem));
			context.SetInstruction(IRInstruction.IntrinsicMethodCall, result, operands);

			return true;
		}
		else
		{
			context.RemoveOperand(0);

			intrinsic(context, MethodCompiler);

			return true;
		}
	}

	/// <summary>Processes external method calls.</summary>
	/// <param name="method"></param>
	/// <param name="context">The transformation context.</param>
	/// <param name="result"></param>
	/// <param name="operands"></param>
	/// <returns>
	///   <c>true</c> if the method was replaced by an intrinsic; <c>false</c> otherwise.</returns>
	/// <remarks>This method checks if the call target has an Intrinsic-Attribute applied with
	/// the current architecture. If it has, the method call is replaced by the specified
	/// native instruction.</remarks>
	private bool ProcessExternalCall(MosaMethod method, Context context, Operand result, List<Operand> operands)
	{
		var intrinsic = ResolveIntrinsicDelegateMethod(method);

		if (intrinsic == null)
			return false;

		if (method.IsExternal)
		{
			var operand1 = Operand.CreateSymbolFromMethod(context.InvokeMethod, TypeSystem);
			context.AppendInstruction(IRInstruction.IntrinsicMethodCall, result, operand1);
			context.AppendOperands(operands);
		}
		else
		{
			//intrinsic(context, result, operands, MethodCompiler); // future
		}

		return true;
	}

	private IntrinsicMethodDelegate ResolveIntrinsicDelegateMethod(MosaMethod method)
	{
		if (InstrinsicMap.TryGetValue(method, out var intrinsic))
		{
			return intrinsic;
		}

		if (method.IsExternal)
		{
			intrinsic = Architecture.GetInstrinsicMethod(method.ExternMethodModule);
		}
		else
		{
			var methodName = $"{method.DeclaringType.FullName}::{method.Name}";

			intrinsic = MethodCompiler.Compiler.GetInstrincMethod(methodName);
		}

		InstrinsicMap.Add(method, intrinsic);

		return intrinsic;
	}

	private MosaMethod GetMethodOrOverride(MosaType type, MosaMethod method)
	{
		var implMethod = type.FindMethodBySignature(method.Name, method.Signature);

		if (implMethod != null)
			return implMethod;

		return method;
	}

	#region Array Helpers

	/// <summary>Calculates the element offset for the specified index.</summary>
	/// <param name="context">The node.</param>
	/// <param name="elementType">The array type.</param>
	/// <param name="index">The index operand.</param>
	/// <returns>Element offset operand.</returns>
	private Operand CalculateArrayElementOffset(Context context, MosaType elementType, Operand index)
	{
		var size = GetTypeSize(elementType, false);

		return CalculateArrayElementOffset(context, size, index);
	}

	/// <summary>Calculates the element offset for the specified index.</summary>
	/// <param name="context">The node.</param>
	/// <param name="size">The element size.</param>
	/// <param name="index">The index operand.</param>
	/// <returns>Element offset operand.</returns>
	private Operand CalculateArrayElementOffset(Context context, uint size, Operand index)
	{
		if (Is32BitPlatform)
		{
			var elementOffset = AllocateVirtualRegister32();
			var elementSize = CreateConstant32(size);

			context.AppendInstruction(IRInstruction.MulUnsigned32, elementOffset, index, elementSize);

			return elementOffset;
		}
		else
		{
			var elementOffset = AllocateVirtualRegister64();
			var elementSize = CreateConstant64(size);

			context.AppendInstruction(IRInstruction.MulUnsigned64, elementOffset, index, elementSize);

			return elementOffset;
		}
	}

	/// <summary>Calculates the base of the array elements.</summary>
	/// <param name="context"></param>
	/// <param name="elementOffset">The array.</param>
	/// <returns>Base address for array elements.</returns>
	private Operand CalculateTotalArrayOffset(Context context, Operand elementOffset)
	{
		var fixedOffset = CreateConstant32(NativePointerSize);
		var arrayElement = Is32BitPlatform ? AllocateVirtualRegister32() : AllocateVirtualRegister64();

		if (Is32BitPlatform)
			context.AppendInstruction(IRInstruction.Add32, arrayElement, elementOffset, fixedOffset);
		else
			context.AppendInstruction(IRInstruction.Add64, arrayElement, elementOffset, fixedOffset);

		return arrayElement;
	}

	#endregion Array Helpers

	/// <summary>
	/// Visitation function for Conversion instruction from unsigned source.
	/// </summary>
	/// <param name="context">The context.</param>
	private void CheckedConversionUnsigned(Context context)
	{
		var result = context.Result;
		var source = context.Operand1;
		var type = context.MosaType;

		// First check to see if we have a matching checked conversion function

		var sourceTypeString = (source.Type.IsI4) ? "U4" :
			(source.Type.IsI8) ? "U8" :
			(source.Type.IsR4) ? "R4" :
			(source.Type.IsR8) ? "R8" :
			(source.Type.IsI) ? Is32BitPlatform ? "U4" : "U8" :
			(source.Type.IsPointer) ? Is32BitPlatform ? "U4" : "U8" :
			(!source.Type.IsValueType) ? Is32BitPlatform ? "U4" : "U8" :
			throw new CompilerException();

		var resultTypeString = type.IsU || type.IsI || type.IsPointer ? Is32BitPlatform ? "U4" : "U8" : type.TypeCode.ToString();

		var methodName = $"{sourceTypeString}To{resultTypeString}";
		var method = GetMethod("Mosa.Runtime.Math.CheckedConversion", methodName);

		Debug.Assert(method != null);

		var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

		context.SetInstruction(IRInstruction.CallStatic, result, symbol, source);

		MethodScanner.MethodInvoked(method, Method);
	}
}
