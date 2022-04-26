// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Represents the CIL decoding compilation stage.
	/// </summary>
	/// <remarks>
	/// The CIL decoding stage takes a stream of bytes and decodes the instructions represented into an MSIL based intermediate
	/// representation. The instructions are grouped into basic blocks.
	/// </remarks>
	public sealed class CILDecodingStageV2 : BaseMethodCompilerStage
	{
		#region Stack classes

		private enum StackType
		{ Int32, Int64, R4, R8, ManagedPointer, Object, ValueType };

		private enum ElementType
		{ I1, I2, I4, I8, U1, U2, U4, U8, R4, R8, I, Ref };

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
		}

		#endregion Stack classes

		private class PrefixValues
		{
			public bool Unaligned { get; set; } = false; // ldind, stind, ldfld, stfld, ldobj, stobj, initblk, or cpblk
			public bool Volatile { get; set; } = false; // Ldsfld and Stsfld
			public bool Tailcall { get; set; } = false; // Call, Calli, or Callvirt
			public bool Constrained { get; set; } = false; // callvirt
			public bool Readonly { get; set; } = false; // ldelema
			public bool NoTypeCheck { get; set; } = false;
			public bool NoRangeCheck { get; set; } = false;
			public bool NoNullCheck { get; set; } = false;

			public bool Reset = false;

			public void ResetAll()
			{
				Unaligned = false;
				Volatile = false;
				Tailcall = false;
				Constrained = false;
				Readonly = false;
				NoTypeCheck = false;
				NoRangeCheck = false;
				NoNullCheck = false;
			}
		}

		private readonly Dictionary<BasicBlock, Stack<StackEntry>> stacks = new Dictionary<BasicBlock, Stack<StackEntry>>();

		private Operand[] LocalStack;
		private StackType[] LocalStackType;

		private SortedList<int, int> Targets;

		protected override void Finish()
		{
			Targets = null;

			//MethodCompiler.Stop();
		}

		protected override void Run()
		{
			if (!MethodCompiler.IsCILStream)
				return;

			// Create the prologue block
			var prologue = CreateNewBlock(BasicBlock.PrologueLabel);
			BasicBlocks.AddHeadBlock(prologue);

			var jmpNode = new InstructionNode()
			{
				Label = BasicBlock.PrologueLabel,
				Block = prologue
			};
			prologue.First.Insert(jmpNode);

			SetInitalEvaluationStack(prologue);

			var startBlock = CreateNewBlock(0);

			jmpNode.SetInstruction(IRInstruction.Jmp, startBlock);

			CollectTargets();

			CreateBasicBlocks();

			CreateHandlersBlocks();

			CreateLocalVariables();

			InitializeLocalVariables();

			CreateInstructions();

			InsertBlockProtectInstructions();
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

					SetInitalEvaluationStack(handler, new StackEntry(StackType.Object, exceptionObject));
				}

				if (clause.ExceptionHandlerType == ExceptionHandlerType.Filter)
				{
					{
						var handler = GetOrCreateBlock(clause.HandlerStart);
						var context = new Context(handler);
						var exceptionObject = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);

						context.AppendInstruction(IRInstruction.ExceptionStart, exceptionObject);

						SetInitalEvaluationStack(handler, new StackEntry(StackType.Object, exceptionObject));
					}

					{
						var handler = GetOrCreateBlock(clause.FilterStart.Value);
						var context = new Context(handler);
						var exceptionObject = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);

						context.AppendInstruction(IRInstruction.FilterStart, exceptionObject);

						SetInitalEvaluationStack(handler, new StackEntry(StackType.Object, exceptionObject));
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
					var finallyOperand = Is32BitPlatform ? AllocateVirtualRegisterI32() : AllocateVirtualRegisterI64();

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

			for (int index = 0; index < totalCode; index++)
			{
				var instruction = code[index];
				var opcode = (OpCode)instruction.OpCode;
				var label = instruction.Offset;

				if (block == null)
				{
					block = BasicBlocks.GetByLabel(label);

					stack = GetEvaluationStack(block);

					context.Node = block.AfterFirst;
					endNode = block.First;
				}

				bool processed = Translate(stack, context, instruction, opcode, block, label, prefixValues);

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

				if (peekNextblock != null || index == totalCode - 1)
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

					foreach (var nextblock in block.NextBlocks)
					{
						SaveEvaluationStack(nextblock, stack);
					}

					stack = null;
					block = null;
				}
			}
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

			for (int index = 0; index < count; index++)
			{
				var type = Method.LocalVariables[index];
				var underlyingType = GetUnderlyingType(type.Type);

				var stackType = underlyingType != null ? GetStackType(underlyingType) : StackType.ValueType;
				LocalStackType[index] = stackType;

				//LocalStack[index] = MethodCompiler.AddStackLocal(type.Type, type.IsPinned);

				if (stackType == StackType.ValueType || arg[index] || type.IsPinned)
				{
					LocalStack[index] = MethodCompiler.AddStackLocal(type.Type, type.IsPinned);
				}
				else
				{
					LocalStack[index] = AllocatedOperand(stackType, type.Type);
				}
			}
		}

		private void InitializeLocalVariables()
		{
			var prologue = new Context(BasicBlocks.PrologueBlock.First);

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
						prologue.AppendInstruction(IRInstruction.Move32, local, ConstantZero64);
						break;

					case StackType.Int64:
						prologue.AppendInstruction(IRInstruction.Move64, local, ConstantZero64);
						break;

					case StackType.R4:
						prologue.AppendInstruction(IRInstruction.MoveR4, local, CreateConstantR4(0.0f));
						break;

					case StackType.R8:
						prologue.AppendInstruction(IRInstruction.MoveR8, local, CreateConstantR8(0.0d));
						break;

					default:
						prologue.AppendInstruction(IRInstruction.Move32, local, ConstantZero32);
						break;
				}
			}
		}

		private bool Translate(Stack<StackEntry> stack, Context context, MosaInstruction instruction, OpCode opcode, BasicBlock block, int label, PrefixValues prefixValues)
		{
			prefixValues.Reset = true;

			switch (opcode)
			{
				case OpCode.Add: return Add(context, stack);
				case OpCode.Add_ovf: return Add(context, stack);                    // TODO: implement overflow check
				case OpCode.Add_ovf_un: return Add(context, stack);                 // TODO: implement overflow check
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
				case OpCode.Callvirt: return Callvirt(context, stack, instruction);
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
				case OpCode.Conv_ovf_i: return ConvertI(context, stack);            // TODO: implement overflow check
				case OpCode.Conv_ovf_i_un: return false;                            // TODO
				case OpCode.Conv_ovf_i1: return ConvertI1(context, stack);          // TODO: implement overflow check
				case OpCode.Conv_ovf_i1_un: return ConvertUToI1(context, stack);    // TODO: implement overflow check
				case OpCode.Conv_ovf_i2: return ConvertI2(context, stack);          // TODO: implement overflow check
				case OpCode.Conv_ovf_i2_un: return ConvertUToI2(context, stack);    // TODO: implement overflow check
				case OpCode.Conv_ovf_i4: return ConvertI4(context, stack);          // TODO: implement overflow check
				case OpCode.Conv_ovf_i4_un: return ConvertUToI4(context, stack);    // TODO: implement overflow check
				case OpCode.Conv_ovf_i8: return ConvertI8(context, stack);          // TODO: implement overflow check
				case OpCode.Conv_ovf_i8_un: return ConvertUToI8(context, stack);    // TODO: implement overflow check
				case OpCode.Conv_ovf_u: return ConvertU(context, stack);            // TODO: implement overflow check
				case OpCode.Conv_ovf_u_un: return false;                            // TODO
				case OpCode.Conv_ovf_u1: return ConvertU1(context, stack);          // TODO: implement overflow check
				case OpCode.Conv_ovf_u1_un: return ConvertUToU1(context, stack);    // TODO: implement overflow check
				case OpCode.Conv_ovf_u2: return ConvertU2(context, stack);          // TODO: implement overflow check
				case OpCode.Conv_ovf_u2_un: return ConvertUToU2(context, stack);    // TODO: implement overflow check
				case OpCode.Conv_ovf_u4: return ConvertU4(context, stack);          // TODO: implement overflow check
				case OpCode.Conv_ovf_u4_un: return ConvertUToU4(context, stack);    // TODO: implement overflow check
				case OpCode.Conv_ovf_u8: return ConvertU8(context, stack);          // TODO: implement overflow check
				case OpCode.Conv_ovf_u8_un: return ConvertUToU8(context, stack);    // TODO: implement overflow check
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
				case OpCode.Ldelem_ref: return Ldelem(context, stack, ElementType.Ref);
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
				case OpCode.Ldind_ref: return Ldind(context, stack, ElementType.Ref);
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
				case OpCode.Mul_ovf: return Mul(context, stack);                    // TODO: implement overflow check
				case OpCode.Mul_ovf_un: return Mul(context, stack);                 // TODO: implement overflow check
				case OpCode.Neg: return Neg(context, stack);
				case OpCode.Newarr: return Newarr(context, stack, instruction);
				case OpCode.Newobj: return Newobj(context, stack, instruction);
				case OpCode.Nop: return Nop(context);
				case OpCode.Not: return Not(context, stack);
				case OpCode.Or: return Or(context, stack);
				case OpCode.Pop: return Pop(context, stack);
				case OpCode.Constrained: prefixValues.Constrained = true; prefixValues.Reset = false; return true;
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
				case OpCode.Stelem_ref: return Stelem(context, stack, ElementType.Ref);
				case OpCode.Stfld: return Stfld(context, stack, instruction);
				case OpCode.Stind_i: return Stind(context, stack, ElementType.I);
				case OpCode.Stind_i1: return Stind(context, stack, ElementType.I1);
				case OpCode.Stind_i2: return Stind(context, stack, ElementType.I2);
				case OpCode.Stind_i4: return Stind(context, stack, ElementType.I4);
				case OpCode.Stind_i8: return Stind(context, stack, ElementType.I8);
				case OpCode.Stind_r4: return Stind(context, stack, ElementType.R4);
				case OpCode.Stind_r8: return Stind(context, stack, ElementType.R8);
				case OpCode.Stind_ref: return Stind(context, stack, ElementType.Ref);
				case OpCode.Stloc: return Stloc(context, stack, (int)instruction.Operand);
				case OpCode.Stloc_0: return Stloc(context, stack, 0);
				case OpCode.Stloc_1: return Stloc(context, stack, 1);
				case OpCode.Stloc_2: return Stloc(context, stack, 2);
				case OpCode.Stloc_3: return Stloc(context, stack, 3);
				case OpCode.Stloc_s: return Stloc(context, stack, (int)instruction.Operand);
				case OpCode.Stobj: return Stobj(context, stack, instruction);
				case OpCode.Stsfld: return Stsfld(context, stack, instruction);
				case OpCode.Sub: return Sub(context, stack);
				case OpCode.Sub_ovf: return Sub(context, stack);                    // TODO: implement overflow check
				case OpCode.Sub_ovf_un: return Sub(context, stack);                 // TODO: implement overflow check
				case OpCode.Switch: return Switch(context, stack, instruction);
				case OpCode.Throw: return Throw(context, stack);
				case OpCode.Unbox: return Unbox(context, stack, instruction);
				case OpCode.Unbox_any: return UnboxAny(context, stack, instruction);
				case OpCode.Xor: return Xor(context, stack);

				default: return false;
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

		#region Helpers

		private static List<Operand> GetOperandParameters(Stack<StackEntry> stack, int paramCount, bool hasThis)
		{
			var operands = new List<Operand>();

			for (int i = 0; i < paramCount; i++)
			{
				var param = stack.Pop();
				operands.Add(param.Operand);
			}

			if (hasThis)
			{
				var thisOperand = stack.Pop().Operand;
				operands.Insert(0, thisOperand);
			}

			//operands.Reverse();
			return operands;
		}

		private static MosaType GetUnderlyingType(MosaType type)
		{
			return MosaTypeLayout.GetUnderlyingType(type);
		}

		private static bool IsBranch(OpCode opcode)
		{
			switch (opcode)
			{
				case OpCode.Beq: return true;
				case OpCode.Beq_s: return true;
				case OpCode.Bge: return true;
				case OpCode.Bge_s: return true;
				case OpCode.Bge_un: return true;
				case OpCode.Bge_un_s: return true;
				case OpCode.Bgt: return true;
				case OpCode.Bgt_s: return true;
				case OpCode.Bgt_un: return true;
				case OpCode.Bgt_un_s: return true;
				case OpCode.Ble: return true;
				case OpCode.Ble_s: return true;
				case OpCode.Ble_un: return true;
				case OpCode.Ble_un_s: return true;
				case OpCode.Blt: return true;
				case OpCode.Blt_s: return true;
				case OpCode.Blt_un: return true;
				case OpCode.Blt_un_s: return true;
				case OpCode.Bne_un: return true;
				case OpCode.Bne_un_s: return true;
				case OpCode.Brfalse_s: return true;
				case OpCode.Brtrue_s: return true;
				case OpCode.Brfalse: return true;
				case OpCode.Brtrue: return true;
				default: return false;
			}
		}

		private static bool IsCompoundType(MosaType underlyingType)
		{
			return !IsPrimitive(underlyingType);
		}

		private static bool IsPrimitive(MosaType underlyingType)
		{
			return MosaTypeLayout.IsPrimitive(underlyingType);
		}

		private Operand AllocatedOperand(StackType stackType, MosaType type = null)
		{
			switch (stackType)
			{
				case StackType.Int32: return AllocateVirtualRegisterI32();
				case StackType.Int64: return AllocateVirtualRegisterI64();
				case StackType.R4: return AllocateVirtualRegisterR4();
				case StackType.R8: return AllocateVirtualRegisterR8();
				case StackType.Object: return AllocateVirtualRegisterObject();
				case StackType.ManagedPointer: return AllocateVirtualRegisterManagedPointer();
				case StackType.ValueType: return MethodCompiler.AddStackLocal(type);
				default: throw new CompilerException("Not implemented yet");
			}
		}

		private StackEntry CreateStateEntry(MosaType type)
		{
			if (type == null)
				return null;

			var underlyingType = GetUnderlyingType(type);
			var stackType = underlyingType != null ? GetStackType(underlyingType) : StackType.ValueType;

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
			switch (stackType)
			{
				case StackType.Int32: return ElementType.I4;
				case StackType.Int64: return ElementType.I8;
				case StackType.Object: return ElementType.Ref;
				case StackType.R4: return ElementType.R4;
				case StackType.R8: return ElementType.R8;

				case StackType.ManagedPointer when Is32BitPlatform: return ElementType.I4;
				case StackType.ManagedPointer when Is64BitPlatform: return ElementType.I8;
			}

			throw new CompilerException($"Unable to convert stacktype ({stackType}) to element type");
		}

		private ElementType GetElementType(MosaType type)
		{
			if (type.IsReferenceType)
				return ElementType.Ref;
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
				return Is32BitPlatform ? ElementType.I4 : ElementType.I8;
			else if (type.IsPointer)
				return Is32BitPlatform ? ElementType.I4 : ElementType.I8;

			// TODO --- enums?

			throw new CompilerException($"Cannot translate to Type {type} to ElementType");
		}

		private Stack<StackEntry> GetEvaluationStack(BasicBlock block)
		{
			if (!stacks.TryGetValue(block, out Stack<StackEntry> stack))
			{
				stack = new Stack<StackEntry>();
			}

			var newstack = new Stack<StackEntry>(stack);

			return newstack;
		}

		private void SaveEvaluationStack(BasicBlock block, Stack<StackEntry> stack)
		{
			if (stacks.TryGetValue(block, out var foundstack))
			{
				// TODO: Check stack size
			}
			else
			{
				stacks.Add(block, stack);
			}
		}

		private void SetInitalEvaluationStack(BasicBlock block, StackEntry stackEntry = null)
		{
			var stack = new Stack<StackEntry>();

			stacks.Add(block, stack);

			if (stackEntry != null)
				stack.Push(stackEntry);
		}

		private BasicBlock GetOrCreateBlock(int label)
		{
			var block = BasicBlocks.GetByLabel(label);

			if (block == null)
			{
				block = CreateNewBlock(label, label);
			}

			return block;
		}

		private Operand GetMethodTablePointer(MosaType runtimeType)
		{
			return Operand.CreateLabel(TypeSystem.BuiltIn.Pointer, Metadata.TypeDefinition + runtimeType.FullName);
		}

		private Operand GetRuntimeTypeHandle(MosaType runtimeType)
		{
			return Operand.CreateLabel(TypeSystem.GetTypeByName("System", "RuntimeTypeHandle"), Metadata.TypeDefinition + runtimeType.FullName);
		}

		private uint GetSize(ElementType elementType)
		{
			switch (elementType)
			{
				case ElementType.I1: return 1;
				case ElementType.I2: return 2;
				case ElementType.I4: return 4;
				case ElementType.I8: return 8;
				case ElementType.U1: return 1;
				case ElementType.U2: return 2;
				case ElementType.U4: return 4;
				case ElementType.U8: return 8;
				case ElementType.R4: return 4;
				case ElementType.R8: return 8;
				case ElementType.Ref: return Is32BitPlatform ? 4 : 8u;
			}

			throw new CompilerException($"Cannot get size of {elementType}");
		}

		private StackType GetStackType(MosaType type)
		{
			if (type.IsReferenceType)
				return StackType.Object;
			else if (type.IsI1 || type.IsI2 || type.IsI4 || type.IsU1 || type.IsU2 || type.IsU4 || type.IsChar || type.IsBoolean)
				return StackType.Int32;
			else if (type.IsI8 || type.IsU8)
				return StackType.Int64;
			else if (type.IsR8)
				return StackType.R8;
			else if (type.IsR4)
				return StackType.R4;

			if (type.IsI)
				return Is32BitPlatform ? StackType.Int32 : StackType.Int64;

			if (type.IsPointer)
				return Is32BitPlatform ? StackType.Int32 : StackType.Int64;

			// TODO --- enums and other value types that fit into 32 or 64 bit register

			if (type.IsValueType)
				return StackType.ValueType;

			throw new CompilerException($"Cannot translate to stacktype {type}");
		}

		private StackType GetStackType(ElementType elementType)
		{
			switch (elementType)
			{
				case ElementType.I1: return StackType.Int32;
				case ElementType.I2: return StackType.Int32;
				case ElementType.I4: return StackType.Int32;
				case ElementType.I8: return StackType.Int64;
				case ElementType.U1: return StackType.Int32;
				case ElementType.U2: return StackType.Int32;
				case ElementType.U4: return StackType.Int32;
				case ElementType.U8: return StackType.Int64;
				case ElementType.Ref: return StackType.Object;
				case ElementType.R4: return StackType.R4;
				case ElementType.R8: return StackType.R8;
			}

			throw new CompilerException($"Cannot translate to ElementType {elementType} to StackType");
		}

		private MosaType GetType(StackType stackType)
		{
			switch (stackType)
			{
				case StackType.Int32: return TypeSystem.BuiltIn.I4;
				case StackType.Int64: return TypeSystem.BuiltIn.I8;
				case StackType.R4: return TypeSystem.BuiltIn.R4;
				case StackType.R8: return TypeSystem.BuiltIn.R8;
				case StackType.Object: return TypeSystem.BuiltIn.Object;
				default: return null;
			}
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
			switch (elementType)
			{
				case ElementType.R4: return IRInstruction.BoxR4;
				case ElementType.R8: return IRInstruction.BoxR8;
				case ElementType.U4: return IRInstruction.Box32;
				case ElementType.I4: return IRInstruction.Box32;
				case ElementType.U8: return IRInstruction.Box64;
				case ElementType.I8: return IRInstruction.Box64;

				case ElementType.I1: return IRInstruction.Box32;
				case ElementType.U1: return IRInstruction.Box32;
				case ElementType.I2: return IRInstruction.Box32;
				case ElementType.U2: return IRInstruction.Box32;

				case ElementType.I when Is32BitPlatform: return IRInstruction.Box32;
				case ElementType.I when Is64BitPlatform: return IRInstruction.Box64;
			}

			throw new InvalidOperationException();
		}

		private BaseInstruction GetLoadInstruction(ElementType elementType)
		{
			switch (elementType)
			{
				case ElementType.I1: return IRInstruction.LoadSignExtend8x32;
				case ElementType.U1: return IRInstruction.LoadZeroExtend8x32;
				case ElementType.I2: return IRInstruction.LoadSignExtend16x32;
				case ElementType.U2: return IRInstruction.LoadZeroExtend16x32;
				case ElementType.I4: return IRInstruction.Load32;
				case ElementType.U4: return IRInstruction.Load32;
				case ElementType.I8: return IRInstruction.Load64;
				case ElementType.U8: return IRInstruction.Load64;
				case ElementType.R4: return IRInstruction.LoadR4;
				case ElementType.R8: return IRInstruction.LoadR8;
				case ElementType.Ref: return IRInstruction.LoadObject;
				case ElementType.I when Is32BitPlatform: return IRInstruction.Load32;
				case ElementType.I when Is64BitPlatform: return IRInstruction.Load64;
			}

			throw new CompilerException($"Invalid ElementType {elementType}");
		}

		private BaseInstruction GetLoadParamInstruction(ElementType elementType)
		{
			switch (elementType)
			{
				case ElementType.I1: return IRInstruction.LoadParamSignExtend8x32;
				case ElementType.U1: return IRInstruction.LoadParamZeroExtend8x32;
				case ElementType.I2: return IRInstruction.LoadParamSignExtend16x32;
				case ElementType.U2: return IRInstruction.LoadParamZeroExtend16x32;
				case ElementType.I4: return IRInstruction.LoadParam32;
				case ElementType.U4: return IRInstruction.LoadParam32;
				case ElementType.I8: return IRInstruction.LoadParam64;
				case ElementType.U8: return IRInstruction.LoadParam64;
				case ElementType.R4: return IRInstruction.LoadParamR4;
				case ElementType.R8: return IRInstruction.LoadParamR8;
				case ElementType.Ref: return IRInstruction.LoadParamObject;
				case ElementType.I when Is32BitPlatform: return IRInstruction.LoadParam32;
				case ElementType.I when Is64BitPlatform: return IRInstruction.LoadParam64;
			}

			throw new CompilerException($"Invalid ElementType {elementType}");
		}

		private BaseInstruction GetMoveInstruction(ElementType elementType)
		{
			switch (elementType)
			{
				case ElementType.I1: return IRInstruction.Move32;
				case ElementType.U1: return IRInstruction.Move32;
				case ElementType.I2: return IRInstruction.Move32;
				case ElementType.U2: return IRInstruction.Move32;
				case ElementType.I4: return IRInstruction.Move32;
				case ElementType.U4: return IRInstruction.Move32;
				case ElementType.I8: return IRInstruction.Move64;
				case ElementType.U8: return IRInstruction.Move64;
				case ElementType.R4: return IRInstruction.MoveR4;
				case ElementType.R8: return IRInstruction.MoveR8;
				case ElementType.Ref: return IRInstruction.MoveObject;
				case ElementType.I when Is32BitPlatform: return IRInstruction.Move32;
				case ElementType.I when Is64BitPlatform: return IRInstruction.Move64;
			}

			throw new CompilerException($"Invalid ElementType {elementType}");
		}

		private BaseInstruction GetStoreInstruction(ElementType elementType)
		{
			switch (elementType)
			{
				case ElementType.I1: return IRInstruction.Store8;
				case ElementType.U1: return IRInstruction.Store8;
				case ElementType.I2: return IRInstruction.Store16;
				case ElementType.U2: return IRInstruction.Store16;
				case ElementType.I4: return IRInstruction.Store32;
				case ElementType.U4: return IRInstruction.Store32;
				case ElementType.I8: return IRInstruction.Store64;
				case ElementType.U8: return IRInstruction.Store64;
				case ElementType.R4: return IRInstruction.StoreR4;
				case ElementType.R8: return IRInstruction.StoreR8;
				case ElementType.Ref: return IRInstruction.StoreObject;
				case ElementType.I when Is32BitPlatform: return IRInstruction.Store32;
				case ElementType.I when Is64BitPlatform: return IRInstruction.Store64;
			}

			throw new CompilerException($"Invalid ElementType {elementType}");
		}

		private BaseInstruction GetStoreParamInstruction(ElementType elementType)
		{
			switch (elementType)
			{
				case ElementType.I1: return IRInstruction.StoreParam8;
				case ElementType.U1: return IRInstruction.StoreParam8;
				case ElementType.I2: return IRInstruction.StoreParam16;
				case ElementType.U2: return IRInstruction.StoreParam16;
				case ElementType.I4: return IRInstruction.StoreParam32;
				case ElementType.U4: return IRInstruction.StoreParam32;
				case ElementType.I8: return IRInstruction.StoreParam64;
				case ElementType.U8: return IRInstruction.StoreParam64;
				case ElementType.R4: return IRInstruction.StoreParamR4;
				case ElementType.R8: return IRInstruction.StoreParamR8;
				case ElementType.Ref: return IRInstruction.StoreParamObject;
				case ElementType.I when Is32BitPlatform: return IRInstruction.StoreParam32;
				case ElementType.I when Is64BitPlatform: return IRInstruction.StoreParam64;
			}

			throw new CompilerException($"Invalid ElementType {elementType}");
		}

		#endregion Instruction Maps

		#region CIL Shortcuts

		private bool Constant32(Context context, Stack<StackEntry> stack, int value)
		{
			var result = AllocateVirtualRegisterI32();
			context.AppendInstruction(IRInstruction.Move32, result, CreateConstant32(value));
			stack.Push(new StackEntry(StackType.Int32, result));
			return true;
		}

		private bool Constant64(Context context, Stack<StackEntry> stack, long value)
		{
			var result = AllocateVirtualRegisterI64();
			context.AppendInstruction(IRInstruction.Move64, result, CreateConstant64(value));
			stack.Push(new StackEntry(StackType.Int64, result));
			return true;
		}

		private bool ConstantR4(Context context, Stack<StackEntry> stack, float value)
		{
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
			context.AppendInstruction(IRInstruction.MoveR4, result, CreateConstantR4(value));
			stack.Push(new StackEntry(StackType.R4, result));
			return true;
		}

		private bool ConstantR8(Context context, Stack<StackEntry> stack, double value)
		{
			var result = AllocateVirtualRegisterR8();
			context.AppendInstruction(IRInstruction.MoveR8, result, CreateConstantR8(value));
			stack.Push(new StackEntry(StackType.R8, result));
			return true;
		}

		private bool Ldnull(Context context, Stack<StackEntry> stack)
		{
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);
			context.AppendInstruction(IRInstruction.MoveObject, result, Operand.GetNullObject(TypeSystem));
			stack.Push(new StackEntry(StackType.Object, result));
			return true;
		}

		#endregion CIL Shortcuts

		#region CIL

		private bool Add(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
					{
						var result = AllocateVirtualRegisterR4();
						context.AppendInstruction(IRInstruction.AddR4, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R4, result));
						return true;
					}

				case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
					{
						var result = AllocateVirtualRegisterR8();
						context.AppendInstruction(IRInstruction.AddR8, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R8, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Add32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Add64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.Add64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.Add64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is32BitPlatform:
				case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is32BitPlatform:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Add32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.ManagedPointer, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
				case StackType.Int64 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Add64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.ManagedPointer, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
					{
						var result = AllocateVirtualRegisterI64();
						var v1 = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.Add64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.ManagedPointer, result));
						return true;
					}

				case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is64BitPlatform:
					{
						var result = AllocateVirtualRegisterI64();
						var v1 = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.Add32, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.ManagedPointer, result));
						return true;
					}

				default: return false;
			}
		}

		private bool And(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.And32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.And64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.And64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.And64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Box(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var entry = stack.Pop();
			var type = (MosaType)instruction.Operand;

			var result = AllocateVirtualRegisterObject();
			stack.Push(new StackEntry(StackType.Object, result));

			if (type.IsReferenceType)
			{
				return true;
			}

			var methodTable = GetMethodTablePointer(type);
			var isPrimitive = IsPrimitive(type);

			if (isPrimitive)
			{
				var elementType = GetElementType(type);

				var boxInstruction = GetBoxInstruction(elementType);
				context.AppendInstruction(boxInstruction, result, methodTable, entry.Operand);
				return true;
			}
			else
			{
				var typeSize = Alignment.AlignUp(TypeLayout.GetTypeSize(type), TypeLayout.NativePointerAlignment);

				//var address = AllocateVirtualRegisterManagedPointer();
				//context.AppendInstruction(IRInstruction.AddressOf, address, entry.Operand);

				context.AppendInstruction(IRInstruction.Box, result, methodTable, entry.Operand, CreateConstant32(typeSize));
				return true;
			}
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
			var entry2 = stack.Pop();
			var entry1 = stack.Pop();

			var target = (int)instruction.Operand;
			var block = BasicBlocks.GetByLabel(target);
			var nextblock = BasicBlocks.GetByLabel(instruction.Next.Value);

			switch (entry1.StackType)
			{
				case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.CompareR4, result, entry1.Operand, entry2.Operand);
						context.AppendInstruction(IRInstruction.Branch32, ConditionCode.NotZero, null, result, ConstantZero32, block);
						context.AppendInstruction(IRInstruction.Jmp, nextblock);
						return true;
					}

				case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.CompareR8, conditionCode, result, entry1.Operand, entry2.Operand);
						context.AppendInstruction(IRInstruction.Branch32, ConditionCode.NotZero, null, result, ConstantZero32, block);
						context.AppendInstruction(IRInstruction.Jmp, nextblock);
						return true;
					}

				case StackType.Object when entry2.StackType == StackType.Object:
					context.AppendInstruction(IRInstruction.BranchObject, conditionCode, null, entry1.Operand, entry2.Operand, block);
					context.AppendInstruction(IRInstruction.Jmp, nextblock);
					return true;

				case StackType.Int32 when entry2.StackType == StackType.Int32:
					context.AppendInstruction(IRInstruction.Branch32, conditionCode, null, entry1.Operand, entry2.Operand, block);
					context.AppendInstruction(IRInstruction.Jmp, nextblock);
					return true;

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					context.AppendInstruction(IRInstruction.Branch64, conditionCode, null, entry1.Operand, entry2.Operand, block);
					context.AppendInstruction(IRInstruction.Jmp, nextblock);
					return true;

				default:

					// TODO: Managed Pointers

					return false;
			}
		}

		private bool Branch1(Context context, Stack<StackEntry> stack, ConditionCode conditionCode, MosaInstruction instruction)
		{
			var entry = stack.Pop();

			var target = (int)instruction.Operand;
			var block = BasicBlocks.GetByLabel(target);
			var nextblock = BasicBlocks.GetByLabel(instruction.Next.Value);

			switch (entry.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.Branch32, conditionCode, null, entry.Operand, ConstantZero32, block);
					context.AppendInstruction(IRInstruction.Jmp, nextblock);
					return true;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.Branch64, conditionCode, null, entry.Operand, ConstantZero64, block);
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

				stack.Push(resultStackType);
			}

			//if (ProcessExternalCall(context, method, operands))
			//	return true;

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			context.AppendInstruction(IRInstruction.CallStatic, result, symbol, operands);

			//context.InvokeMethod = method; // Optional?

			return true;
		}

		private bool Callvirt(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var method = (MosaMethod)instruction.Operand;

			// TODO: when type.IsValueType & ConstrainedPrefixInstruction

			var operands = GetOperandParameters(stack, method.Signature.Parameters.Count, method.HasThis && !method.HasExplicitThis);

			Operand result = null;

			if (!method.Signature.ReturnType.IsVoid)
			{
				var resultStackType = CreateStateEntry(method.Signature.ReturnType);

				result = resultStackType.Operand;

				stack.Push(resultStackType);
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

			//context.InvokeMethod = method; // Optional

			return true;
		}

		private bool Castclass(Context context, Stack<StackEntry> stack)
		{
			var entry = stack.Pop();

			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);
			stack.Push(new StackEntry(StackType.Object, result));

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
			var entry2 = stack.Pop();
			var entry1 = stack.Pop();

			var result = AllocateVirtualRegisterI32();
			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry1.StackType)
			{
				case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
					context.AppendInstruction(IRInstruction.CompareR4, result, entry1.Operand, entry2.Operand);
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

		private bool ConvertI(Context context, Stack<StackEntry> stack)
		{
			var entry = stack.Pop();

			if (Is32BitPlatform)
			{
				var result = AllocateVirtualRegisterI32();
				stack.Push(new StackEntry(StackType.Int32, result));

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
				var result = AllocateVirtualRegisterI64();
				stack.Push(new StackEntry(StackType.Int64, result));

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
			var entry = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry.StackType)
			{
				case StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.And32, v1, entry.Operand, CreateConstant32(0xFF));
						context.AppendInstruction(IRInstruction.Or32, result, v1, CreateConstant32(0xFFFFFF00));
						return true;
					}

				case StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						var v2 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
						context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFF));
						context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(0xFFFFFF00));
						return true;
					}

				case StackType.R4:
					{
						var v1 = AllocateVirtualRegisterI32();
						var v2 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ConvertR4ToI32, v1, entry.Operand);
						context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFF));
						context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(0xFFFFFF00));
						return true;
					}

				case StackType.R8:
					{
						var v1 = AllocateVirtualRegisterI32();
						var v2 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ConvertR8ToI32, v1, entry.Operand);
						context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFF));
						context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(0xFFFFFF00));
						return true;
					}

				default:
					return false;
			}
		}

		private bool ConvertI2(Context context, Stack<StackEntry> stack)
		{
			var entry = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry.StackType)
			{
				case StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.And32, v1, entry.Operand, CreateConstant32(0xFFFF));
						context.AppendInstruction(IRInstruction.Or32, result, v1, CreateConstant32(0xFFFF0000));
						return true;
					}

				case StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						var v2 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
						context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFFFF));
						context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(0xFFFF0000));
						return true;
					}

				case StackType.R4:
					{
						var v1 = AllocateVirtualRegisterI32();
						var v2 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ConvertR4ToI32, v1, entry.Operand);
						context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFFFF));
						context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(0xFFFF0000));
						return true;
					}

				case StackType.R8:
					{
						var v1 = AllocateVirtualRegisterI32();
						var v2 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ConvertR8ToI32, v1, entry.Operand);
						context.AppendInstruction(IRInstruction.And32, v2, v1, CreateConstant32(0xFFFF));
						context.AppendInstruction(IRInstruction.Or32, result, v2, CreateConstant32(0xFFFF0000));
						return true;
					}

				default:
					return false;
			}
		}

		private bool ConvertI4(Context context, Stack<StackEntry> stack)
		{
			var entry = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
					return true;

				case StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
						context.AppendInstruction(IRInstruction.Move32, result, v1);
						return true;
					}

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
			var entry = stack.Pop();
			var result = AllocateVirtualRegisterI64();

			stack.Push(new StackEntry(StackType.Int64, result));

			switch (entry.StackType)
			{
				case StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry.Operand);
						context.AppendInstruction(IRInstruction.Move32, result, v1);
						return true;
					}

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
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
			var entry = stack.Pop();
			var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
			stack.Push(new StackEntry(StackType.R4, result));

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
			var entry = stack.Pop();
			var result = AllocateVirtualRegisterR8();
			stack.Push(new StackEntry(StackType.R8, result));

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
			var entry1 = stack.Pop();

			if (Is32BitPlatform)
			{
				var result = AllocateVirtualRegisterI32();
				stack.Push(new StackEntry(StackType.Int32, result));

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
				var result = AllocateVirtualRegisterI64();
				stack.Push(new StackEntry(StackType.Int64, result));

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
			var entry = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.And32, result, entry.Operand, CreateConstant32(0xFF));
					return true;

				case StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
						context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
						return true;
					}

				case StackType.R4:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ConvertR4ToU32, v1, entry.Operand);
						context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
						return true;
					}

				case StackType.R8:
					{
						var v1 = AllocateVirtualRegisterI32();
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
			var entry = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.And32, result, entry.Operand, CreateConstant32(0xFFFF));
					return true;

				case StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
						context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));
						return true;
					}

				case StackType.R4:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ConvertR4ToU32, v1, entry.Operand);
						context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));

						return true;
					}

				case StackType.R8:
					{
						var v1 = AllocateVirtualRegisterI32();
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
			var entry = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
					return true;

				case StackType.Int64:
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry.Operand);
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
			var entry = stack.Pop();
			var result = AllocateVirtualRegisterI64();
			stack.Push(new StackEntry(StackType.Int64, result));

			if (entry.StackType != StackType.Int32)
			{
				if (entry.StackType == StackType.Int64)
				{
					context.AppendInstruction(IRInstruction.Move64, result, entry.Operand);
					return true;
				}

				if (entry.StackType == StackType.R4)
				{
					context.AppendInstruction(IRInstruction.ConvertR4ToU64, result, entry.Operand);
					return true;
				}

				if (entry.StackType == StackType.R8)
				{
					context.AppendInstruction(IRInstruction.ConvertR8ToU64, result, entry.Operand);
					return true;
				}

				return false;
			}
			var v1 = AllocateVirtualRegisterI64();
			context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry.Operand);
			context.AppendInstruction(IRInstruction.Move32, result, v1);
			return true;
		}

		private bool ConvertUToF(Context context, Stack<StackEntry> stack)
		{
			var entry = stack.Pop();
			var result = AllocateVirtualRegisterR8();
			stack.Push(new StackEntry(StackType.R8, result));

			if (entry.StackType != StackType.Int32)
			{
				if (entry.StackType == StackType.Int64)
				{
					context.AppendInstruction(IRInstruction.ConvertU64ToR8, result, entry.Operand);
					return true;
				}

				return false;
			}
			context.AppendInstruction(IRInstruction.ConvertU32ToR8, result, entry.Operand);
			return true;
		}

		private bool ConvertUToI1(Context context, Stack<StackEntry> stack)
		{
			// convert unsigned to an int8 (on the stack as int32)

			var entry = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry.StackType)
			{
				case StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.And32, v1, entry.Operand, CreateConstant32(0xFF));
						context.AppendInstruction(IRInstruction.Or32, result, v1, CreateConstant32(~0xFF));
						return true;
					}

				case StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						var v2 = AllocateVirtualRegisterI32();
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

			var entry = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry.StackType)
			{
				case StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.And32, v1, entry.Operand, CreateConstant32(0xFFFF));
						context.AppendInstruction(IRInstruction.Or32, result, v1, CreateConstant32(~0xFFFF));
						return true;
					}

				case StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						var v2 = AllocateVirtualRegisterI32();
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

			var entry = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

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

			var entry = stack.Pop();
			var result = AllocateVirtualRegisterI64();

			stack.Push(new StackEntry(StackType.Int64, result));

			switch (entry.StackType)
			{
				case StackType.Int32:
					context.AppendInstruction(IRInstruction.SignExtend32x64, result, entry.Operand, CreateConstant32(0xFFFF));
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

			var entry = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry.StackType)
			{
				case StackType.Int32:
					{
						context.AppendInstruction(IRInstruction.And32, result, entry.Operand, CreateConstant32(0xFF));
						return true;
					}

				case StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
						context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
						return true;
					}

				default:
					return false;
			}
		}

		private bool ConvertUToU2(Context context, Stack<StackEntry> stack)
		{
			// convert unsigned to an unsigned int8 (on the stack as int32)

			var entry = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

			switch (entry.StackType)
			{
				case StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.And32, result, entry.Operand, CreateConstant32(0xFFFF));
						return true;
					}

				case StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
						context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));
						return true;
					}

				default:
					return false;
			}
		}

		private bool ConvertUToU4(Context context, Stack<StackEntry> stack)
		{
			// convert unsigned to an unsigned int4 (on the stack as int32)

			var entry = stack.Pop();
			var result = AllocateVirtualRegisterI32();

			stack.Push(new StackEntry(StackType.Int32, result));

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

			var entry = stack.Pop();
			var result = AllocateVirtualRegisterI64();

			stack.Push(new StackEntry(StackType.Int64, result));

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
			var source = stack.Pop();
			var destination = stack.Pop();

			if ((source.StackType == StackType.Int32 || source.StackType == StackType.Int64) && (destination.StackType == StackType.Int32 || destination.StackType == StackType.Int64))
			{
				context.AppendInstruction(IRInstruction.MemoryCopy, null, source.Operand, destination.Operand);
				return true;
			}

			return false;
		}

		private bool Cpobj(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var source = stack.Pop();
			var destination = stack.Pop();

			// TODO

			return false;
		}

		private bool Div(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
					{
						var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
						context.AppendInstruction(IRInstruction.DivR4, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R4, result));
						return true;
					}

				case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
					{
						var result = AllocateVirtualRegisterR8();
						context.AppendInstruction(IRInstruction.DivR8, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R8, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.DivSigned32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.DivSigned64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.DivSigned64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.DivSigned64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool DivUnsigned(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
					{
						var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
						context.AppendInstruction(IRInstruction.DivR4, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R4, result));
						return true;
					}

				case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
					{
						var result = AllocateVirtualRegisterR8();
						context.AppendInstruction(IRInstruction.DivR8, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R8, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.DivUnsigned32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.DivUnsigned64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.DivUnsigned64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.DivUnsigned64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Dup(Context context, Stack<StackEntry> stack)
		{
			var entry = stack.Peek();
			stack.Push(entry);
			return true;
		}

		private bool Endfinally(Context context)
		{
			context.AppendInstruction(IRInstruction.FinallyEnd);

			return true;
		}

		private bool Initblk(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Peek();
			var entry2 = stack.Peek();
			var entry3 = stack.Peek();

			context.AppendInstruction(IRInstruction.MemorySet, null, entry1.Operand, entry2.Operand, entry3.Operand);

			return true;
		}

		private bool InitObj(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var entry = stack.Peek();

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
			var entry = stack.Pop();
			var type = (MosaType)instruction.Operand;

			var result = AllocateVirtualRegisterObject();

			// TODO: non-nullable ValueTypes

			if (!type.IsInterface)
			{
				context.AppendInstruction(IRInstruction.IsInstanceOfType, result, GetRuntimeTypeHandle(type), entry.Operand);
			}
			else
			{
				uint slot = TypeLayout.GetInterfaceSlot(type);
				context.AppendInstruction(IRInstruction.IsInstanceOfInterfaceType, result, CreateConstant32(slot), entry.Operand);
			}

			stack.Push(new StackEntry(StackType.Object, result));

			return true;
		}

		private bool Ldarg(Context context, Stack<StackEntry> stack, int index)
		{
			var parameter = MethodCompiler.Parameters[index];
			var type = parameter.Type;
			var underlyingType = GetUnderlyingType(type);
			var isCompound = IsCompoundType(underlyingType);

			if (isCompound)
			{
				//var elementType = GetElementType(underlyingType);
				var stacktype = GetStackType(type);
				var result = AllocatedOperand(stacktype, type);

				context.AppendInstruction(IRInstruction.LoadParamCompound, result, parameter);
				context.MosaType = type;
				stack.Push(new StackEntry(StackType.ValueType, result, type));
				return true;
			}
			else
			{
				var elementType = GetElementType(underlyingType);
				var stacktype = GetStackType(elementType);
				var result = AllocatedOperand(stacktype);

				var loadInstruction = GetLoadParamInstruction(elementType);
				context.AppendInstruction(loadInstruction, result, parameter);

				stack.Push(new StackEntry(stacktype, result));

				return true;
			}
		}

		private bool Ldarga(Context context, Stack<StackEntry> stack, int index)
		{
			var parameter = MethodCompiler.Parameters[index];

			var result = AllocateVirtualRegisterManagedPointer();

			context.AppendInstruction(IRInstruction.AddressOf, result, parameter);

			stack.Push(new StackEntry(StackType.ManagedPointer, result));

			return true;
		}

		private bool Ldelem(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			var index = entry1.Operand;
			var array = entry2.Operand;

			var type = (MosaType)instruction.Operand;

			//var underlyingType = GetUnderlyingType(type.ElementType);
			var isCompound = IsCompoundType(type);

			context.AppendInstruction(IRInstruction.CheckArrayBounds, null, array, index);

			var elementOffset = CalculateArrayElementOffset(context, type, index);
			var totalElementOffset = CalculateTotalArrayOffset(context, elementOffset);

			if (isCompound)
			{
				var result = AllocatedOperand(StackType.ValueType, type);

				context.AppendInstruction(IRInstruction.LoadCompound, result, array, totalElementOffset);
				context.MosaType = type.ElementType;

				stack.Push(new StackEntry(StackType.ValueType, result, type));

				return true;
			}
			else
			{
				var stacktype = GetStackType(type);
				var result = AllocatedOperand(stacktype);
				var elementType = GetElementType(stacktype);
				var loadInstruction = GetLoadInstruction(elementType);

				context.AppendInstruction(loadInstruction, result, array, totalElementOffset);

				stack.Push(new StackEntry(stacktype, result));

				return true;
			}
		}

		private bool Ldelem(Context context, Stack<StackEntry> stack, ElementType elementType)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			var index = entry1.Operand;
			var array = entry2.Operand;

			//var underlyingType = GetUnderlyingType(type.ElementType);

			context.AppendInstruction(IRInstruction.CheckArrayBounds, null, array, index);

			var elementOffset = CalculateArrayElementOffset(context, GetSize(elementType), index);
			var totalElementOffset = CalculateTotalArrayOffset(context, elementOffset);

			var stacktype = GetStackType(elementType);
			var result = AllocatedOperand(stacktype);
			stack.Push(new StackEntry(stacktype, result));

			var loadInstruction = GetLoadInstruction(elementType);

			context.AppendInstruction(loadInstruction, result, array, totalElementOffset);

			return true;
		}

		private bool Ldelema(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

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

			stack.Push(new StackEntry(StackType.ManagedPointer, result));

			return true;
		}

		private bool Ldfld(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var entry = stack.Pop();

			var field = (MosaField)instruction.Operand;
			uint offset = TypeLayout.GetFieldOffset(field);
			var type = field.FieldType;

			switch (entry.StackType)
			{
				case StackType.Int32:
				case StackType.Int64:
				case StackType.R4:
				case StackType.R8:
				case StackType.ManagedPointer:
				case StackType.Object:
					{
						var underlyingType = GetUnderlyingType(type);
						var isCompound = IsCompoundType(underlyingType);

						if (isCompound)
						{
							var result = AllocatedOperand(StackType.ValueType, type);

							context.AppendInstruction(IRInstruction.LoadCompound, result, entry.Operand, CreateConstant32(offset));
							context.MosaType = type;

							stack.Push(new StackEntry(StackType.ValueType, result, type));

							return true;
						}
						else
						{
							var stacktype = underlyingType != null ? GetStackType(underlyingType) : StackType.ValueType;
							var result = AllocatedOperand(stacktype);
							var elementType = GetElementType(stacktype);
							var loadInstruction = GetLoadInstruction(elementType);

							context.AppendInstruction(loadInstruction, result, entry.Operand, CreateConstant32(offset));

							stack.Push(new StackEntry(stacktype, result));

							return true;
						}
					}
				case StackType.ValueType:
					{
						var underlyingType = GetUnderlyingType(type);
						var isCompound = IsCompoundType(underlyingType);

						if (isCompound)
						{
							var result = AllocatedOperand(StackType.ValueType, type);

							context.AppendInstruction(IRInstruction.LoadCompound, result, entry.Operand, CreateConstant32(offset));
							context.MosaType = type;

							stack.Push(new StackEntry(StackType.ValueType, result, type));

							return true;
						}
						else
						{
							var stacktype = underlyingType != null ? GetStackType(underlyingType) : StackType.ValueType;
							var result = AllocatedOperand(stacktype);
							var elementType = GetElementType(stacktype);
							var loadInstruction = GetLoadInstruction(elementType);

							var address = AllocateVirtualRegisterManagedPointer();
							var fixedOffset = CreateConstant32(offset);

							context.AppendInstruction(IRInstruction.AddressOf, address, entry.Operand);
							context.AppendInstruction(loadInstruction, result, address, fixedOffset);

							stack.Push(new StackEntry(stacktype, result));

							return true;
						}
					}

				default: return false;
			}
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

			var runtimeFieldHandle = TypeSystem.GetTypeByName("System", "RuntimeFieldHandle"); // FUTURE: cache this

			var result = AllocateVirtualRegister(runtimeFieldHandle);
			context.AppendInstruction(move, result, source);

			//stack.Push(new StackEntry(Is32BitPlatform ? StackType.Int32 : StackType.Int64, result));
			stack.Push(new StackEntry(StackType.ValueType, result));

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
			var entry = stack.Pop();

			var field = (MosaField)instruction.Operand;

			MethodScanner.AccessedField(field);

			uint offset = TypeLayout.GetFieldOffset(field);

			var fieldPtr = field.FieldType.ToManagedPointer();

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

			stack.Push(new StackEntry(StackType.ManagedPointer, result));

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

			stack.Push(new StackEntry(stacktype, result));

			MethodScanner.MethodInvoked(method, Method);

			var methodData = MethodCompiler.Compiler.GetMethodData(method);

			methodData.HasMethodPointerReferenced = true;

			return true;
		}

		private bool Ldind(Context context, Stack<StackEntry> stack, ElementType elementType)
		{
			var entry = stack.Pop();

			var stacktype = GetStackType(elementType);
			var result = AllocatedOperand(stacktype);

			stack.Push(new StackEntry(stacktype, result));

			var loadInstruction = GetLoadInstruction(elementType);
			context.AppendInstruction(loadInstruction, result, entry.Operand, ConstantZero);

			return true;
		}

		private bool Ldlen(Context context, Stack<StackEntry> stack)
		{
			var entry = stack.Pop();

			if (entry.StackType == StackType.Object)
			{
				if (Is32BitPlatform)
				{
					var result = AllocateVirtualRegisterI32();
					context.AppendInstruction(IRInstruction.Load32, result, entry.Operand, ConstantZero);
					stack.Push(new StackEntry(StackType.Int32, result));
					return true;
				}
				else
				{
					var result = AllocateVirtualRegisterI64();
					context.AppendInstruction(IRInstruction.Load64, result, entry.Operand, ConstantZero);
					stack.Push(new StackEntry(StackType.Int64, result));
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
				//var result2 = AllocateVirtualRegister(local.Type);
				var result2 = AddStackLocal(local.Type);
				context.AppendInstruction(IRInstruction.MoveCompound, result2, local);
				context.MosaType = local.Type;

				stack.Push(new StackEntry(stacktype, result2, local.Type));

				return true;
			}

			var resultType = GetType(stacktype);
			var result = AllocateVirtualRegister(resultType);

			stack.Push(new StackEntry(stacktype, result));

			var elementType = GetElementType(stacktype);

			if (local.IsVirtualRegister)
			{
				var moveInstruction = GetMoveInstruction(elementType);

				context.AppendInstruction(moveInstruction, result, local);
				return true;
			}
			else
			{
				var loadInstruction = GetLoadParamInstruction(elementType);

				context.AppendInstruction(loadInstruction, result, local);
				return true;
			}
		}

		private bool Ldloca(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var index = (int)instruction.Operand;

			var local = LocalStack[index];

			var result = AllocateVirtualRegisterManagedPointer();

			stack.Push(new StackEntry(StackType.ManagedPointer, result));

			context.AppendInstruction(IRInstruction.AddressOf, result, local);

			return true;
		}

		private bool Ldobj(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var entry = stack.Pop();
			var address = entry.Operand;
			var type = (MosaType)instruction.Operand;

			var underlyingType = GetUnderlyingType(type);
			var isCompound = IsCompoundType(underlyingType);

			if (isCompound)
			{
				//var result = AllocateVirtualRegister(type);
				var result = MethodCompiler.AddStackLocal(type);
				context.AppendInstruction(IRInstruction.LoadCompound, result, address, ConstantZero);
				context.MosaType = type;
				stack.Push(new StackEntry(StackType.ValueType, result, type));
				return true;
			}
			else
			{
				var elementType = GetElementType(underlyingType);
				var stacktype = GetStackType(elementType);
				var result = AllocatedOperand(stacktype);

				stack.Push(new StackEntry(stacktype, result));

				var loadInstruction = GetLoadInstruction(elementType);
				context.AppendInstruction(loadInstruction, result, address, ConstantZero);

				return Ldind(context, stack, elementType);
			}
		}

		private bool Ldsfld(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var field = (MosaField)instruction.Operand;
			var type = field.FieldType;

			var underlyingType = GetUnderlyingType(type);
			var isCompound = IsCompoundType(underlyingType);

			var fieldOperand = Operand.CreateStaticField(field, TypeSystem);

			if (isCompound)
			{
				var result = AllocateVirtualRegister(type);
				context.AppendInstruction(IRInstruction.LoadCompound, result, fieldOperand, ConstantZero);
				context.MosaType = type;
				stack.Push(new StackEntry(StackType.ValueType, result, type));
			}
			else
			{
				var elementType = GetElementType(underlyingType);
				var stacktype = GetStackType(elementType);
				var result = AllocatedOperand(stacktype);

				stack.Push(new StackEntry(stacktype, result));

				var loadInstruction = GetLoadInstruction(elementType);

				if (type.IsReferenceType)
				{
					var symbol = GetStaticSymbol(field);
					var staticReference = Operand.CreateLabel(TypeSystem.BuiltIn.Object, symbol.Name);

					context.SetInstruction(IRInstruction.LoadObject, result, staticReference, ConstantZero);
				}
				else
				{
					context.SetInstruction(loadInstruction, result, fieldOperand, ConstantZero);
					context.MosaType = type;
				}
			}

			return true;
		}

		private bool Ldsflda(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var field = (MosaField)instruction.Operand;

			var result = AllocateVirtualRegisterManagedPointer();
			var fieldOperand = Operand.CreateStaticField(field, TypeSystem);

			context.AppendInstruction(IRInstruction.AddressOf, result, fieldOperand);

			stack.Push(new StackEntry(StackType.ManagedPointer, result));   // FIXME: transient pointer or unmanaged pointer

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

			stack.Push(new StackEntry(StackType.Object, result));

			return true;
		}

		private bool Mul(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
					{
						var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
						context.AppendInstruction(IRInstruction.MulR4, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R4, result));
						return true;
					}

				case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
					{
						var result = AllocateVirtualRegisterR8();
						context.AppendInstruction(IRInstruction.MulR8, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R8, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.MulUnsigned32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.MulUnsigned64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.MulUnsigned64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.MulUnsigned64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Neg(Context context, Stack<StackEntry> stack)
		{
			var entry = stack.Pop();

			switch (entry.StackType)
			{
				case StackType.R4:
					{
						var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
						var zero = CreateConstantR4(0);
						context.AppendInstruction(IRInstruction.SubR4, result, zero, entry.Operand);
						stack.Push(new StackEntry(StackType.R4, result));
						return true;
					}

				case StackType.R8:
					{
						var result = AllocateVirtualRegisterR8();
						var zero = CreateConstantR8(0);
						context.AppendInstruction(IRInstruction.SubR8, result, zero, entry.Operand);
						stack.Push(new StackEntry(StackType.R8, result));
						return true;
					}

				case StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Sub32, result, ConstantZero32, entry.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Sub32, result, ConstantZero64, entry.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Newarr(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var elements = stack.Pop();

			var arrayType = (MosaType)instruction.Operand;

			var elementSize = GetTypeSize(arrayType.ElementType, false);
			var methodTable = GetMethodTablePointer(arrayType);
			var size = CreateConstant32(elementSize);
			var result = AllocateVirtualRegisterObject();

			context.AppendInstruction(IRInstruction.NewArray, result, methodTable, size, elements.Operand);
			context.MosaType = arrayType;

			stack.Push(new StackEntry(StackType.Object, result));

			return true;
		}

		private bool Newobj(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var method = (MosaMethod)instruction.Operand;
			var classType = method.DeclaringType;
			int paramCount = method.Signature.Parameters.Count;

			var underlyingType = GetUnderlyingType(classType);
			var isCompound = IsCompoundType(underlyingType);
			var stackType = underlyingType != null ? GetStackType(underlyingType) : StackType.ValueType;

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			var operands = new List<Operand>();

			for (int i = 0; i < paramCount; i++)
			{
				var param = stack.Pop();
				operands.Add(param.Operand);
			}

			MethodScanner.TypeAllocated(classType, Method);

			//if (ReplaceWithInternalCall(context))
			//	return true;

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

				stack.Push(new StackEntry(StackType.Object, result));

				return true;
			}
			else if (stackType == StackType.ValueType)  // iscompound?
			{
				var newThisLocal = AddStackLocal(classType);
				var newThis = AllocateVirtualRegisterManagedPointer();

				context.AppendInstruction(IRInstruction.AddressOf, newThis, newThisLocal);

				operands.Insert(0, newThis);

				context.AppendInstruction(IRInstruction.CallStatic, null, symbol, operands);

				//context.InvokeMethod = method; // optional??

				stack.Push(new StackEntry(StackType.ValueType, newThisLocal));

				return true;
			}
			else if (stackType == StackType.Int32 || stackType == StackType.Int64)
			{
				var newThisLocal = AddStackLocal(classType);
				var newThis = AllocateVirtualRegisterManagedPointer();

				context.AppendInstruction(IRInstruction.AddressOf, newThis, newThisLocal);

				operands.Insert(0, newThis);

				context.AppendInstruction(IRInstruction.CallStatic, null, symbol, operands);

				//context.InvokeMethod = method;  // optional??

				stack.Push(new StackEntry(StackType.ManagedPointer, newThis));   // ManagedPointer??

				return true;
			}
			else if (stackType != StackType.ValueType)
			{
				// INCOMPLETE

				var newThisLocal = AddStackLocal(classType);
				var newThis = AllocateVirtualRegisterManagedPointer();

				context.AppendInstruction(IRInstruction.AddressOf, newThis, newThisLocal);

				// TODO: unboxing it (kinda of)

				operands.Insert(0, newThis);

				context.AppendInstruction(IRInstruction.CallStatic, null, symbol, operands);

				//context.InvokeMethod = method;  // optional??

				stack.Push(new StackEntry(StackType.ManagedPointer, newThis));   // ManagedPointer??
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
			var entry = stack.Pop();

			switch (entry.StackType)
			{
				case StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Not32, result, entry.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Not64, result, entry.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Or(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Or32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Or64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.Or64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.Or64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Pop(Context context, Stack<StackEntry> stack)
		{
			stack.Pop();

			return true;
		}

		private bool RemOperand(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.R4 when entry2.StackType == StackType.R4 && entry1.Operand.IsR4:
					{
						var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
						context.AppendInstruction(IRInstruction.RemR4, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R4, result));
						return true;
					}

				case StackType.R8 when entry2.StackType == StackType.R8 && entry1.Operand.IsR8:
					{
						var result = AllocateVirtualRegisterR8();
						context.AppendInstruction(IRInstruction.RemR8, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R8, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.RemSigned32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.RemSigned64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.RemSigned64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.RemSigned64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool RemUnsigned(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.R4 when entry2.StackType == StackType.R4:
					{
						var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
						context.AppendInstruction(IRInstruction.RemR4, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R4, result));
						return true;
					}

				case StackType.R8 when entry2.StackType == StackType.R8:
					{
						var result = AllocateVirtualRegisterR8();
						context.AppendInstruction(IRInstruction.RemR8, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R8, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.RemUnsigned32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.RemUnsigned64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.RemUnsigned64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.RemUnsigned64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Ret(Context context, Stack<StackEntry> stack)
		{
			if (!Method.Signature.ReturnType.IsVoid)
			{
				var entry = stack.Pop();

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
				}
			}

			var block = GetOrCreateBlock(BasicBlock.EpilogueLabel);
			context.AppendInstruction(IRInstruction.Jmp, block);

			return true;
		}

		private bool Rethrow(Context context, Stack<StackEntry> stack)
		{
			var entry = stack.Pop();

			context.AppendInstruction(IRInstruction.Rethrow, entry.Operand);

			return true;
		}

		private bool Shl(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			var shiftAmount = entry1.Operand;
			var shiftValue = entry2.Operand;

			switch (entry2.StackType)
			{
				case StackType.Int32 when entry1.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ShiftLeft32, result, shiftValue, shiftAmount);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry1.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ShiftLeft64, result, shiftValue, shiftAmount);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry1.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, shiftAmount);
						context.AppendInstruction(IRInstruction.ShiftLeft32, result, shiftValue, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry1.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, shiftAmount);
						context.AppendInstruction(IRInstruction.ShiftLeft64, result, shiftValue, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Shr(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			var shiftAmount = entry1.Operand;
			var shiftValue = entry2.Operand;

			switch (entry1.StackType)
			{
				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ArithShiftRight32, result, shiftValue, shiftAmount);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ArithShiftRight32, result, shiftValue, shiftAmount);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, shiftAmount);
						context.AppendInstruction(IRInstruction.ArithShiftRight32, result, shiftValue, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, shiftAmount);
						context.AppendInstruction(IRInstruction.ArithShiftRight32, result, shiftValue, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool ShrU(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			var shiftAmount = entry1.Operand;
			var shiftValue = entry2.Operand;

			switch (entry1.StackType)
			{
				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.ShiftRight32, result, shiftValue, shiftAmount);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ShiftRight32, result, shiftValue, shiftAmount);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI32();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Truncate64x32, v1, shiftAmount);
						context.AppendInstruction(IRInstruction.ShiftRight32, result, shiftValue, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, shiftAmount);
						context.AppendInstruction(IRInstruction.ShiftRight32, result, shiftValue, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Sizeof(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var type = (MosaType)instruction.Operand;
			var result = AllocateVirtualRegisterI32();

			var size = type.IsPointer ? NativePointerSize : (uint)MethodCompiler.TypeLayout.GetTypeSize(type);

			context.AppendInstruction(IRInstruction.Move32, result, CreateConstant32(size));

			stack.Push(new StackEntry(StackType.Int32, result));

			return true;
		}

		private bool Stelem(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();
			var entry3 = stack.Pop();

			var index = entry3.Operand;
			var array = entry2.Operand;
			var value = entry1.Operand;

			var type = (MosaType)instruction.Operand;
			var underlyingType = GetUnderlyingType(type);
			var isCompound = IsCompoundType(underlyingType);

			context.AppendInstruction(IRInstruction.CheckArrayBounds, null, array, index);

			var elementOffset = CalculateArrayElementOffset(context, type, index);
			var totalElementOffset = CalculateTotalArrayOffset(context, elementOffset);

			if (isCompound)
			{
				context.AppendInstruction(IRInstruction.StoreCompound, null, array, totalElementOffset);
				context.MosaType = type;

				return true;
			}
			else
			{
				var stacktype = GetStackType(underlyingType);
				var elementType = GetElementType(stacktype);
				var storeInstruction = GetStoreInstruction(elementType);

				context.AppendInstruction(storeInstruction, null, array, totalElementOffset, value);

				return true;
			}
		}

		private bool Stelem(Context context, Stack<StackEntry> stack, ElementType elementType)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();
			var entry3 = stack.Pop();

			var index = entry3.Operand;
			var array = entry2.Operand;
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
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			var field = (MosaField)instruction.Operand;
			uint offset = TypeLayout.GetFieldOffset(field);
			var type = field.FieldType;

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
						var isCompound = IsCompoundType(underlyingType);

						if (isCompound)
						{
							context.AppendInstruction(IRInstruction.StoreCompound, null, entry2.Operand, CreateConstant32(offset), entry1.Operand);
							context.MosaType = type;

							return true;
						}
						else
						{
							var stacktype = underlyingType != null ? GetStackType(underlyingType) : StackType.ValueType;
							var elementType = GetElementType(stacktype);
							var storeInstruction = GetStoreInstruction(elementType);

							context.AppendInstruction(storeInstruction, null, entry2.Operand, CreateConstant32(offset), entry1.Operand);

							//context.MosaType = type;

							return true;
						}
					}
				case StackType.ValueType:
					{
						//var underlyingType = GetUnderlyingType(type);
						//var isCompound = IsCompoundType(underlyingType);

						context.AppendInstruction(IRInstruction.StoreCompound, null, entry2.Operand, CreateConstant32(offset), entry1.Operand);
						context.MosaType = type;

						return true;
					}

				default: return false;
			}
		}

		private bool Stind(Context context, Stack<StackEntry> stack, ElementType elementType)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			var address = entry2.Operand;
			var value = entry1.Operand;

			var storeInstruction = GetStoreInstruction(elementType);
			context.AppendInstruction(storeInstruction, null, address, ConstantZero, value);

			return true;
		}

		private bool Stloc(Context context, Stack<StackEntry> stack, int index)
		{
			var entry = stack.Pop();
			var source = entry.Operand;

			var stacktype = LocalStackType[index];
			var local = LocalStack[index];

			if (stacktype == StackType.ValueType)
			{
				context.AppendInstruction(IRInstruction.MoveCompound, local, source);
				context.MosaType = local.Type;

				return true;
			}

			if (local.IsVirtualRegister)
			{
				switch (stacktype)
				{
					case StackType.Int32:
						context.AppendInstruction(IRInstruction.Move32, local, source);
						return true;

					case StackType.Int64:
						context.AppendInstruction(IRInstruction.Move64, local, source);
						return true;

					case StackType.Object:
						context.AppendInstruction(IRInstruction.MoveObject, local, source);
						return true;

					case StackType.R4:
						context.AppendInstruction(IRInstruction.MoveR4, local, source);
						return true;

					case StackType.R8:
						context.AppendInstruction(IRInstruction.MoveR8, local, source);
						return true;
				}
			}
			else
			{
				switch (stacktype)
				{
					case StackType.Int32:
						context.AppendInstruction(IRInstruction.StoreParam32, null, local, source);
						return true;

					case StackType.Int64:
						context.AppendInstruction(IRInstruction.StoreParam64, null, local, source);
						return true;

					case StackType.Object:
						context.AppendInstruction(IRInstruction.StoreParamObject, local, source);
						return true;

					case StackType.R4:
						context.AppendInstruction(IRInstruction.StoreParamR4, null, local, source);
						return true;

					case StackType.R8:
						context.AppendInstruction(IRInstruction.StoreParamR8, null, local, source);
						return true;
				}
			}

			return false;
		}

		private bool Stobj(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			var address = entry2.Operand;
			var value = entry1.Operand;

			var type = (MosaType)instruction.Operand;
			var underlyingType = GetUnderlyingType(type);
			bool isCompound = IsCompoundType(underlyingType);

			if (isCompound)
			{
				context.AppendInstruction(IRInstruction.StoreCompound, null, address, ConstantZero, value);
				return true;
			}
			else
			{
				var stackType = GetStackType(underlyingType);
				var elementType = GetElementType(stackType);
				var storeInstruction = GetStoreInstruction(elementType);

				context.AppendInstruction(storeInstruction, null, address, ConstantZero, value);
				return true;
			}
		}

		private bool Stsfld(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var entry = stack.Pop();
			var source = entry.Operand;

			var field = (MosaField)instruction.Operand;
			var type = field.FieldType;

			var underlyingType = GetUnderlyingType(type);
			var isCompound = IsCompoundType(underlyingType);

			var fieldOperand = Operand.CreateStaticField(field, TypeSystem);

			if (isCompound)
			{
				context.AppendInstruction(IRInstruction.StoreCompound, null, fieldOperand, ConstantZero, source);
				context.MosaType = type;
			}
			else
			{
				var elementType = GetElementType(underlyingType);

				var storeInstruction = GetStoreInstruction(elementType);

				if (type.IsReferenceType)
				{
					var symbol = GetStaticSymbol(field);
					var staticReference = Operand.CreateLabel(TypeSystem.BuiltIn.Object, symbol.Name);

					context.AppendInstruction(IRInstruction.StoreObject, null, staticReference, ConstantZero);

					//context.MosaType = type;
				}
				else
				{
					context.AppendInstruction(storeInstruction, null, fieldOperand, ConstantZero, source);
					context.MosaType = type;
				}
			}

			return true;
		}

		private bool StoreArgument(Context context, Stack<StackEntry> stack, int index)
		{
			var entry = stack.Pop();

			var value = entry.Operand;

			var parameter = MethodCompiler.Parameters[index];
			var type = parameter.Type;
			var underlyingType = GetUnderlyingType(type);
			var isCompound = IsCompoundType(underlyingType);

			if (isCompound)
			{
				context.AppendInstruction(IRInstruction.StoreParamCompound, null, parameter, value);
				context.MosaType = type;
				return true;
			}
			else
			{
				var elementType = GetElementType(underlyingType);

				var storeInstruction = GetStoreParamInstruction(elementType);
				context.AppendInstruction(storeInstruction, null, parameter, value);

				return true;
			}
		}

		private bool Sub(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.R4 when entry2.StackType == StackType.R4:
					{
						var result = AllocateVirtualRegister(TypeSystem.BuiltIn.R4);
						context.AppendInstruction(IRInstruction.SubR4, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R4, result));
						return true;
					}

				case StackType.R8 when entry2.StackType == StackType.R8:
					{
						var result = AllocateVirtualRegisterR8();
						context.AppendInstruction(IRInstruction.SubR8, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.R8, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Sub32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Sub64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.Sub64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.Sub64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.ManagedPointer when entry2.StackType == StackType.ManagedPointer && Is32BitPlatform:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Sub32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.ManagedPointer when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Sub64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is32BitPlatform:
				case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is32BitPlatform:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Sub32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.ManagedPointer, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Sub64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.ManagedPointer, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.ManagedPointer && Is64BitPlatform:
					{
						var result = AllocateVirtualRegisterI64();
						var v1 = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.Sub64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.ManagedPointer, result));
						return true;
					}

				case StackType.ManagedPointer when entry2.StackType == StackType.Int32 && Is64BitPlatform:
					{
						var result = AllocateVirtualRegisterI64();
						var v1 = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.Sub32, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.ManagedPointer, result));
						return true;
					}

				default:
					return false;
			}
		}

		private bool Throw(Context context, Stack<StackEntry> stack)
		{
			var entry = stack.Pop();

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
			var entry = stack.Pop();

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
			var entry = stack.Pop();

			var type = (MosaType)instruction.Operand;

			// FUTURE: Check for valid cast
			var methodTable = GetMethodTablePointer(type);

			var result = AllocatedOperand(StackType.ManagedPointer);
			stack.Push(new StackEntry(StackType.ManagedPointer, result));

			if (Is32BitPlatform)
				context.AppendInstruction(IRInstruction.Add32, result, entry.Operand, CreateConstant32(8));
			else
				context.AppendInstruction(IRInstruction.Add64, result, entry.Operand, CreateConstant64(8));

			return true;
		}

		private bool UnboxAny(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
		{
			var entry = stack.Pop();

			var type = (MosaType)instruction.Operand;

			// FUTURE: Check for valid cast
			var methodTable = GetMethodTablePointer(type);

			if (type.IsReferenceType)
			{
				// FUTURE: treat as castclass
				stack.Push(entry);
				return true;
			}

			var underlyingType = GetUnderlyingType(type);
			var isPrimitive = IsPrimitive(type);

			if (isPrimitive)
			{
				var elementType = GetElementType(underlyingType);

				var loadInstruction = GetLoadInstruction(elementType);
				var stackType = GetStackType(elementType);
				var result = AllocatedOperand(stackType);

				context.AppendInstruction(loadInstruction, result, entry.Operand, CreateConstant32(8));

				stack.Push(new StackEntry(stackType, result));
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

				stack.Push(new StackEntry(StackType.ValueType, result, type));
				return true;
			}
		}

		private bool Xor(Context context, Stack<StackEntry> stack)
		{
			var entry1 = stack.Pop();
			var entry2 = stack.Pop();

			switch (entry1.StackType)
			{
				case StackType.Int32 when entry2.StackType == StackType.Int32:
					{
						var result = AllocateVirtualRegisterI32();
						context.AppendInstruction(IRInstruction.Xor32, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int32, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int64:
					{
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.Xor64, result, entry1.Operand, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int32 when entry2.StackType == StackType.Int64:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
						context.AppendInstruction(IRInstruction.Xor64, result, v1, entry2.Operand);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				case StackType.Int64 when entry2.StackType == StackType.Int32:
					{
						var v1 = AllocateVirtualRegisterI64();
						var result = AllocateVirtualRegisterI64();
						context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
						context.AppendInstruction(IRInstruction.Xor64, result, entry1.Operand, v1);
						stack.Push(new StackEntry(StackType.Int64, result));
						return true;
					}

				default:
					return false;
			}
		}

		#endregion CIL

		public LinkerSymbol GetStaticSymbol(MosaField field)
		{
			return Linker.DefineSymbol($"{Metadata.StaticSymbolPrefix}{field.DeclaringType}+{field.Name}", SectionKind.BSS, Architecture.NativeAlignment, NativePointerSize);
		}

		#region Array Helpers

		/// <summary>
		/// Adds bounds check to the array access.
		/// </summary>
		/// <param name="context">The node.</param>
		/// <param name="arrayOperand">The array operand.</param>
		/// <param name="arrayIndexOperand">The index operand.</param>
		private void AddArrayBoundsCheck(Context context, Operand arrayOperand, Operand arrayIndexOperand)
		{
			// First create new block and split current block
			var exceptionContext = CreateNewBlockContexts(1, context.Label)[0];
			var nextContext = Split(context);

			Operand lengthOperand;

			if (Is32BitPlatform)
			{
				lengthOperand = AllocateVirtualRegisterI32();
				context.AppendInstruction(IRInstruction.Load32, lengthOperand, arrayOperand, ConstantZero32);
			}
			else
			{
				lengthOperand = AllocateVirtualRegisterI64();
				context.AppendInstruction(IRInstruction.Load64, lengthOperand, arrayOperand, ConstantZero64);
			}

			// Now compare length with index
			// If index is greater than or equal to the length then jump to exception block, otherwise jump to next block
			context.AppendInstruction(BranchInstruction, ConditionCode.UnsignedGreaterOrEqual, null, arrayIndexOperand, lengthOperand, exceptionContext.Block);
			context.AppendInstruction(IRInstruction.Jmp, nextContext.Block);

			// Build exception block which is just a call to throw exception
			var method = InternalRuntimeType.FindMethodByName("ThrowIndexOutOfRangeException");
			var symbolOperand = Operand.CreateSymbolFromMethod(method, TypeSystem);

			exceptionContext.AppendInstruction(IRInstruction.CallStatic, null, symbolOperand);
		}

		/// <summary>
		/// Calculates the element offset for the specified index.
		/// </summary>
		/// <param name="context">The node.</param>
		/// <param name="elementType">The array type.</param>
		/// <param name="index">The index operand.</param>
		/// <returns>
		/// Element offset operand.
		/// </returns>
		private Operand CalculateArrayElementOffset(Context context, MosaType elementType, Operand index)
		{
			var size = GetTypeSize(elementType, false);

			return CalculateArrayElementOffset(context, size, index);
		}

		/// <summary>
		/// Calculates the element offset for the specified index.
		/// </summary>
		/// <param name="context">The node.</param>
		/// <param name="size">The element size.</param>
		/// <param name="index">The index operand.</param>
		/// <returns>
		/// Element offset operand.
		/// </returns>
		private Operand CalculateArrayElementOffset(Context context, uint size, Operand index)
		{
			if (Is32BitPlatform)
			{
				var elementOffset = AllocateVirtualRegisterI32();
				var elementSize = CreateConstant32(size);

				context.AppendInstruction(IRInstruction.MulUnsigned32, elementOffset, index, elementSize);

				return elementOffset;
			}
			else
			{
				var elementOffset = AllocateVirtualRegisterI64();
				var elementSize = CreateConstant64(size);

				context.AppendInstruction(IRInstruction.MulUnsigned64, elementOffset, index, elementSize);

				return elementOffset;
			}
		}

		/// <summary>
		/// Calculates the base of the array elements.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="elementOffset">The array.</param>
		/// <returns>
		/// Base address for array elements.
		/// </returns>
		private Operand CalculateTotalArrayOffset(Context context, Operand elementOffset)
		{
			var fixedOffset = CreateConstant32(NativePointerSize);
			var arrayElement = Is32BitPlatform ? AllocateVirtualRegisterI32() : AllocateVirtualRegisterI64();

			if (Is32BitPlatform)
				context.AppendInstruction(IRInstruction.Add32, arrayElement, elementOffset, fixedOffset);
			else
				context.AppendInstruction(IRInstruction.Add64, arrayElement, elementOffset, fixedOffset);

			return arrayElement;
		}

		#endregion Array Helpers
	}
}
