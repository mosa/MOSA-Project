// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.Analysis;
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
public sealed class CILDecoderStage : BaseMethodCompilerStage
{
	#region StackEntry Class

	private class StackEntry
	{
		public readonly Operand Operand;

		public PrimitiveType PrimitiveType => Operand.Primitive;

		public MosaType Type => Operand.Type;

		public StackEntry(Operand operand)
		{
			Operand = operand;
		}

		public override string ToString()
		{
			return $"{PrimitiveType} {Operand}";
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

	#region Data Members

	private readonly Dictionary<BasicBlock, StackEntry> ExceptionStackEntries = new Dictionary<BasicBlock, StackEntry>();

	private readonly Dictionary<BasicBlock, StackEntry[]> OutgoingStacks = new Dictionary<BasicBlock, StackEntry[]>();

	private Operand[] LocalStack;
	private PrimitiveType[] LocalPrimitiveType;
	private ElementType[] LocalElementType;

	private SortedList<int, int> Targets;

	private readonly Dictionary<MosaMethod, IntrinsicMethodDelegate> InstrinsicMap = new Dictionary<MosaMethod, IntrinsicMethodDelegate>();

	private TraceLog trace;

	#endregion Data Members

	#region Overrides Methods

	protected override void Finish()
	{
		Targets = null;
		trace = null;
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

		SetParameters();

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

		AdditionalExceptionBlocks();

		InsertBlockProtectInstructions();

		InsertFlowOrJumpInstructions();
	}

	protected override void Setup()
	{
		Targets = new SortedList<int, int>();
	}

	#endregion Overrides Methods

	#region Initialize Methods

	/// <summary>
	/// Evaluates the parameter operands.
	/// </summary>
	private void SetParameters()
	{
		var offset = Architecture.OffsetOfFirstParameter;

		if (!MosaTypeLayout.IsUnderlyingPrimitive(Method.Signature.ReturnType))
		{
			offset += (int)MethodCompiler.GetSize(Method.Signature.ReturnType);
		}

		if (Method.HasThis || Method.HasExplicitThis)
		{
			var primativeType = Method.DeclaringType.IsValueType
				? PrimitiveType.ManagedPointer
				: PrimitiveType.Object;

			var elementType = Method.DeclaringType.IsValueType
				? ElementType.ManagedPointer
				: ElementType.Object;

			var operand = MethodCompiler.Parameters.Allocate(primativeType, elementType, "this", offset, Architecture.NativePointerSize);
			offset += (int)operand.Size;
		}

		foreach (var parameter in Method.Signature.Parameters)
		{
			var underlyingType = MosaTypeLayout.GetUnderlyingType(parameter.ParameterType);
			var primitiveType = MethodCompiler.GetPrimitiveType(underlyingType);
			var elementType = MethodCompiler.IsPrimitive(primitiveType)
				? MethodCompiler.GetElementType(underlyingType)
				: ElementType.ValueType;

			var size = MethodCompiler.GetSize(underlyingType);

			var operand = MethodCompiler.Parameters.Allocate(primitiveType, elementType, parameter.Name, offset, size, parameter.ParameterType);

			offset += (int)Alignment.AlignUp(size, Architecture.NativePointerSize);
		}
	}

	/// <summary>
	/// Evaluates the parameter operands.
	/// </summary>
	private static void SetParameters(MethodCompiler methodCompiler)
	{
		var offset = methodCompiler.Architecture.OffsetOfFirstParameter;

		if (!MosaTypeLayout.IsUnderlyingPrimitive(methodCompiler.Method.Signature.ReturnType))
		{
			offset += (int)methodCompiler.GetSize(methodCompiler.Method.Signature.ReturnType);
		}

		if (methodCompiler.Method.HasThis || methodCompiler.Method.HasExplicitThis)
		{
			var primativeType = methodCompiler.Method.DeclaringType.IsValueType
				? PrimitiveType.ManagedPointer
				: PrimitiveType.Object;

			var elementType = methodCompiler.Method.DeclaringType.IsValueType
				? ElementType.ManagedPointer
				: ElementType.Object;

			var operand = methodCompiler.Parameters.Allocate(primativeType, elementType, "this", offset, methodCompiler.Architecture.NativePointerSize);
			offset += (int)operand.Size;
		}

		foreach (var parameter in methodCompiler.Method.Signature.Parameters)
		{
			var underlyingType = MosaTypeLayout.GetUnderlyingType(parameter.ParameterType);
			var primitiveType = methodCompiler.GetPrimitiveType(underlyingType);
			var elementType = MethodCompiler.IsPrimitive(primitiveType)
				? methodCompiler.GetElementType(underlyingType)
				: ElementType.ValueType;

			var size = methodCompiler.GetSize(underlyingType);

			var operand = methodCompiler.Parameters.Allocate(primitiveType, elementType, parameter.Name, offset, size, parameter.ParameterType);

			offset += (int)Alignment.AlignUp(size, methodCompiler.Architecture.NativePointerSize);
		}
	}

	private void AddTarget(int target)
	{
		if (!Targets.ContainsKey(target))
			Targets.Add(target, target);
	}

	private void CollectTargets()
	{
		var code = Method.Code;

		for (var index = 0; index < code.Count; index++)
		{
			var instruction = code[index];

			var opcode = (CILOpCode)instruction.OpCode;

			if (opcode == CILOpCode.Br || opcode == CILOpCode.Br_s)
			{
				AddTarget((int)instruction.Operand);
			}
			else if (IsBranch(opcode))
			{
				AddTarget((int)instruction.Operand);
				AddTarget(code[index + 1].Offset);
			}
			else if (opcode == CILOpCode.Switch)
			{
				foreach (var target in (int[])instruction.Operand)
				{
					AddTarget(target);
				}

				AddTarget(code[index + 1].Offset);
			}
			else if (opcode == CILOpCode.Leave || opcode == CILOpCode.Leave_s)
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
				var exceptionObject = MethodCompiler.VirtualRegisters.AllocateObject();

				context.AppendInstruction(IRInstruction.ExceptionStart, exceptionObject);

				ExceptionStackEntries.Add(handler, new StackEntry(exceptionObject));
			}

			if (clause.ExceptionHandlerType == ExceptionHandlerType.Filter)
			{
				{
					var handler = GetOrCreateBlock(clause.HandlerStart);
					var context = new Context(handler);
					var exceptionObject = MethodCompiler.VirtualRegisters.AllocateObject();

					context.AppendInstruction(IRInstruction.ExceptionStart, exceptionObject);

					ExceptionStackEntries.Add(handler, new StackEntry(exceptionObject));
				}

				{
					var handler = GetOrCreateBlock(clause.FilterStart.Value);
					var context = new Context(handler);
					var exceptionObject = MethodCompiler.VirtualRegisters.AllocateObject();

					context.AppendInstruction(IRInstruction.FilterStart, exceptionObject);

					ExceptionStackEntries.Add(handler, new StackEntry(exceptionObject));
				}
			}
		}
	}

	private void AdditionalExceptionBlocks()
	{
		foreach (var block in BasicBlocks)
		{
			if (!block.HasPreviousBlocks && !block.IsHeadBlock)
			{
				// block was targeted (probably by an leave instruction within a protected region)
				BasicBlocks.AddHeadBlock(block);
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
				var exceptionObject = MethodCompiler.VirtualRegisters.AllocateObject();
				var finallyOperand = Is32BitPlatform
					? MethodCompiler.VirtualRegisters.Allocate32()
					: MethodCompiler.VirtualRegisters.Allocate64();

				context.AppendInstruction2(IRInstruction.FinallyStart, exceptionObject, finallyOperand);
			}
		}
	}

	private bool IsSourceAndTargetWithinSameTryOrException(InstructionNode node)
	{
		var leaveLabel = TraverseBackToNativeBlock(node.Block).Label;
		var targetLabel = TraverseBackToNativeBlock(node.BranchTargets[0]).Label;

		foreach (var handler in Method.ExceptionHandlers)
		{
			var one = handler.IsLabelWithinTry(leaveLabel);
			var two = handler.IsLabelWithinTry(targetLabel);

			if (one && !two)
				return false;

			if (!one && two)
				return false;

			if (one && two)
				return true;

			one = handler.IsLabelWithinHandler(leaveLabel);
			two = handler.IsLabelWithinHandler(targetLabel);

			if (one && !two)
				return false;

			if (!one && two)
				return false;

			if (one && two)
				return true;
		}

		// very odd
		return true;
	}

	private void InsertFlowOrJumpInstructions()
	{
		foreach (var block in BasicBlocks)
		{
			var label = TraverseBackToNativeBlock(block).Label;

			for (var node = block.BeforeLast; !node.IsBlockStartInstruction; node = node.Previous)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (!(node.Instruction == IRInstruction.TryEnd
					|| node.Instruction == IRInstruction.ExceptionEnd))
					continue;

				var target = node.BranchTargets[0];

				if (IsSourceAndTargetWithinSameTryOrException(node))
				{
					// Leave instruction can be converted into a simple jump instruction
					node.SetInstruction(IRInstruction.Jmp, target);
					BasicBlocks.RemoveHeaderBlock(target);
					continue;
				}

				var entry = FindImmediateExceptionHandler(label);

				if (!entry.IsLabelWithinTry(label))
					break;

				var flowNode = new InstructionNode(IRInstruction.Flow, target);

				node.Insert(flowNode);

				if (target.IsHeadBlock)
				{
					BasicBlocks.RemoveHeaderBlock(target);
				}

				break;
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

		for (var index = 0; index < totalCode; index++)
		{
			var instruction = code[index];
			var opcode = (CILOpCode)instruction.OpCode;
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

			var processed = Translate(stack, context, instruction, opcode, block, prefixValues, label);

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

			var peekNextblock = index + 1 == totalCode ? null : BasicBlocks.GetByLabel(code[index + 1].Offset);

			if (peekNextblock != null || index + 1 == totalCode)
			{
				if (opcode != CILOpCode.Leave
					&& opcode != CILOpCode.Leave
					&& opcode != CILOpCode.Leave_s
					&& opcode != CILOpCode.Endfilter
					&& opcode != CILOpCode.Endfinally
					&& opcode != CILOpCode.Jmp
					&& opcode != CILOpCode.Br
					&& opcode != CILOpCode.Br_s
					&& opcode != CILOpCode.Ret
					&& opcode != CILOpCode.Throw
					&& opcode != CILOpCode.Brfalse
					&& opcode != CILOpCode.Brfalse_s
					&& opcode != CILOpCode.Brtrue
					&& opcode != CILOpCode.Brtrue_s
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

	#endregion Initialize Methods

	private bool Translate(Stack<StackEntry> stack, Context context, MosaInstruction instruction, CILOpCode opcode, BasicBlock block, PrefixValues prefixValues, int label)
	{
		prefixValues.Reset = true;

		trace?.Log($"   {label:X5}: {opcode}");

		switch (opcode)
		{
			case CILOpCode.Add: return Add(context, stack);
			case CILOpCode.Add_ovf: return AddSigned(context, stack);
			case CILOpCode.Add_ovf_un: return AddUnsigned(context, stack);
			case CILOpCode.And: return And(context, stack);
			case CILOpCode.Arglist: return false;   // TODO: Not implemented in v1 either
			case CILOpCode.Beq: return Branch(context, stack, ConditionCode.Equal, instruction);
			case CILOpCode.Beq_s: return Branch(context, stack, ConditionCode.Equal, instruction);
			case CILOpCode.Bge: return Branch(context, stack, ConditionCode.GreaterOrEqual, instruction);
			case CILOpCode.Bge_s: return Branch(context, stack, ConditionCode.GreaterOrEqual, instruction);
			case CILOpCode.Bge_un: return Branch(context, stack, ConditionCode.UnsignedGreaterOrEqual, instruction);
			case CILOpCode.Bge_un_s: return Branch(context, stack, ConditionCode.UnsignedGreaterOrEqual, instruction);
			case CILOpCode.Bgt: return Branch(context, stack, ConditionCode.Greater, instruction);
			case CILOpCode.Bgt_s: return Branch(context, stack, ConditionCode.Greater, instruction);
			case CILOpCode.Bgt_un: return Branch(context, stack, ConditionCode.UnsignedGreater, instruction);
			case CILOpCode.Bgt_un_s: return Branch(context, stack, ConditionCode.UnsignedGreater, instruction);
			case CILOpCode.Ble: return Branch(context, stack, ConditionCode.LessOrEqual, instruction);
			case CILOpCode.Ble_s: return Branch(context, stack, ConditionCode.LessOrEqual, instruction);
			case CILOpCode.Ble_un: return Branch(context, stack, ConditionCode.UnsignedLessOrEqual, instruction);
			case CILOpCode.Ble_un_s: return Branch(context, stack, ConditionCode.UnsignedLessOrEqual, instruction);
			case CILOpCode.Blt: return Branch(context, stack, ConditionCode.Less, instruction);
			case CILOpCode.Blt_s: return Branch(context, stack, ConditionCode.Less, instruction);
			case CILOpCode.Blt_un: return Branch(context, stack, ConditionCode.UnsignedLess, instruction);
			case CILOpCode.Blt_un_s: return Branch(context, stack, ConditionCode.UnsignedLess, instruction);
			case CILOpCode.Bne_un: return Branch(context, stack, ConditionCode.NotEqual, instruction);
			case CILOpCode.Bne_un_s: return Branch(context, stack, ConditionCode.NotEqual, instruction);
			case CILOpCode.Box: return Box(context, stack, instruction);
			case CILOpCode.Br: return Branch(context, stack, instruction);
			case CILOpCode.Br_s: return Branch(context, stack, instruction);
			case CILOpCode.Break: return Break(context, stack);
			case CILOpCode.Brfalse: return Branch1(context, stack, ConditionCode.Equal, instruction);
			case CILOpCode.Brfalse_s: return Branch1(context, stack, ConditionCode.Equal, instruction);
			case CILOpCode.Brtrue: return Branch1(context, stack, ConditionCode.NotEqual, instruction);
			case CILOpCode.Brtrue_s: return Branch1(context, stack, ConditionCode.NotEqual, instruction);
			case CILOpCode.Call: return Call(context, stack, instruction);
			case CILOpCode.Calli: return false; // TODO: Not implemented in v1 either
			case CILOpCode.Callvirt: return Callvirt(context, stack, instruction, prefixValues);
			case CILOpCode.Castclass: return Castclass(context, stack);
			case CILOpCode.Ceq: return Compare(context, stack, ConditionCode.Equal);
			case CILOpCode.Cgt: return Compare(context, stack, ConditionCode.Greater);
			case CILOpCode.Cgt_un: return Compare(context, stack, ConditionCode.UnsignedGreater);
			case CILOpCode.Ckfinite: return false;  // TODO: Not implemented in v1 either
			case CILOpCode.Clt: return Compare(context, stack, ConditionCode.Less);
			case CILOpCode.Clt_un: return Compare(context, stack, ConditionCode.UnsignedLess);
			case CILOpCode.Conv_i: return ConvertI(context, stack);
			case CILOpCode.Conv_i1: return ConvertI1(context, stack);
			case CILOpCode.Conv_i2: return ConvertI2(context, stack);
			case CILOpCode.Conv_i4: return ConvertI4(context, stack);
			case CILOpCode.Conv_i8: return ConvertI8(context, stack);
			case CILOpCode.Conv_ovf_i: return ConvertToIWithOverflow(context, stack);
			case CILOpCode.Conv_ovf_i_un: return ConvertUToIWithOverflow(context, stack);
			case CILOpCode.Conv_ovf_i1: return ConvertI1WithOverflow(context, stack);
			case CILOpCode.Conv_ovf_i1_un: return ConvertUToI1WithOverflow(context, stack);
			case CILOpCode.Conv_ovf_i2: return ConvertI2WithOverflow(context, stack);
			case CILOpCode.Conv_ovf_i2_un: return ConvertUToI2WithOverflow(context, stack);
			case CILOpCode.Conv_ovf_i4: return ConvertI4WithOverflow(context, stack);
			case CILOpCode.Conv_ovf_i4_un: return ConvertUToI4WithOverflow(context, stack);
			case CILOpCode.Conv_ovf_i8: return ConvertI8WithOverflow(context, stack);
			case CILOpCode.Conv_ovf_i8_un: return ConvertUToI8WithOverflow(context, stack);
			case CILOpCode.Conv_ovf_u: return ConvertToUWithOverflow(context, stack);
			case CILOpCode.Conv_ovf_u_un: return ConvertUToUWithOverflow(context, stack);
			case CILOpCode.Conv_ovf_u1: return ConvertU1WithOverflow(context, stack);
			case CILOpCode.Conv_ovf_u1_un: return ConvertUToU1WithOverflow(context, stack);
			case CILOpCode.Conv_ovf_u2: return ConvertU2WithOverflow(context, stack);
			case CILOpCode.Conv_ovf_u2_un: return ConvertUToU2WithOverflow(context, stack);
			case CILOpCode.Conv_ovf_u4: return ConvertU4WithOverflow(context, stack);
			case CILOpCode.Conv_ovf_u4_un: return ConvertUToU4WithOverflow(context, stack);
			case CILOpCode.Conv_ovf_u8: return ConvertU8WithOverflow(context, stack);
			case CILOpCode.Conv_ovf_u8_un: return ConvertUToU8WithOverflow(context, stack);
			case CILOpCode.Conv_r_un: return ConvertUToF(context, stack);
			case CILOpCode.Conv_r4: return ConvertR4(context, stack);
			case CILOpCode.Conv_r8: return ConvertR8(context, stack);
			case CILOpCode.Conv_u: return ConvertU(context, stack);
			case CILOpCode.Conv_u1: return ConvertU1(context, stack);
			case CILOpCode.Conv_u2: return ConvertU2(context, stack);
			case CILOpCode.Conv_u4: return ConvertU4(context, stack);
			case CILOpCode.Conv_u8: return ConvertU8(context, stack);
			case CILOpCode.Cpblk: return Cpblk(context, stack, instruction);
			case CILOpCode.Cpobj: return Cpobj(context, stack, instruction);
			case CILOpCode.Div: return Div(context, stack);
			case CILOpCode.Div_un: return DivUnsigned(context, stack);
			case CILOpCode.Dup: return Dup(context, stack);
			case CILOpCode.Endfilter: return false; // TODO: Not implemented in v1 either
			case CILOpCode.Endfinally: return Endfinally(context);
			case CILOpCode.Extop: return false; // TODO: Not implemented in v1 either
			case CILOpCode.Initblk: return Initblk(context, stack);
			case CILOpCode.InitObj: return InitObj(context, stack, instruction);
			case CILOpCode.Isinst: return Isinst(context, stack, instruction);
			case CILOpCode.Jmp: return false;   // TODO: Not implemented in v1 either
			case CILOpCode.Ldarg: return Ldarg(context, stack, (int)instruction.Operand);
			case CILOpCode.Ldarg_0: return Ldarg(context, stack, 0);
			case CILOpCode.Ldarg_1: return Ldarg(context, stack, 1);
			case CILOpCode.Ldarg_2: return Ldarg(context, stack, 2);
			case CILOpCode.Ldarg_3: return Ldarg(context, stack, 3);
			case CILOpCode.Ldarg_s: return Ldarg(context, stack, (int)instruction.Operand);
			case CILOpCode.Ldarga: return Ldarga(context, stack, (int)instruction.Operand);
			case CILOpCode.Ldarga_s: return Ldarga(context, stack, (int)instruction.Operand);
			case CILOpCode.Ldc_i4: return Constant32(context, stack, (int)instruction.Operand);
			case CILOpCode.Ldc_i4_0: return Constant32(context, stack, 0);
			case CILOpCode.Ldc_i4_1: return Constant32(context, stack, 1);
			case CILOpCode.Ldc_i4_2: return Constant32(context, stack, 2);
			case CILOpCode.Ldc_i4_3: return Constant32(context, stack, 3);
			case CILOpCode.Ldc_i4_4: return Constant32(context, stack, 4);
			case CILOpCode.Ldc_i4_5: return Constant32(context, stack, 5);
			case CILOpCode.Ldc_i4_6: return Constant32(context, stack, 6);
			case CILOpCode.Ldc_i4_7: return Constant32(context, stack, 7);
			case CILOpCode.Ldc_i4_8: return Constant32(context, stack, 8);
			case CILOpCode.Ldc_i4_m1: return Constant32(context, stack, -1);
			case CILOpCode.Ldc_i4_s: return Constant32(context, stack, (sbyte)instruction.Operand);
			case CILOpCode.Ldc_i8: return Constant64(context, stack, (long)instruction.Operand);
			case CILOpCode.Ldc_r4: return ConstantR4(context, stack, (float)instruction.Operand);
			case CILOpCode.Ldc_r8: return ConstantR8(context, stack, (double)instruction.Operand);
			case CILOpCode.Ldelem: return Ldelem(context, stack, instruction);
			case CILOpCode.Ldelem_i: return Ldelem(context, stack, ElementType.I);
			case CILOpCode.Ldelem_i1: return Ldelem(context, stack, ElementType.I1);
			case CILOpCode.Ldelem_i2: return Ldelem(context, stack, ElementType.I2);
			case CILOpCode.Ldelem_i4: return Ldelem(context, stack, ElementType.I4);
			case CILOpCode.Ldelem_i8: return Ldelem(context, stack, ElementType.I8);
			case CILOpCode.Ldelem_r4: return Ldelem(context, stack, ElementType.R4);
			case CILOpCode.Ldelem_r8: return Ldelem(context, stack, ElementType.R8);
			case CILOpCode.Ldelem_ref: return Ldelem(context, stack, ElementType.Object);
			case CILOpCode.Ldelem_u1: return Ldelem(context, stack, ElementType.U1);
			case CILOpCode.Ldelem_u2: return Ldelem(context, stack, ElementType.U2);
			case CILOpCode.Ldelem_u4: return Ldelem(context, stack, ElementType.U4);
			case CILOpCode.Ldelema: return Ldelema(context, stack, instruction);
			case CILOpCode.Ldfld: return Ldfld(context, stack, instruction);
			case CILOpCode.Ldflda: return Ldflda(context, stack, instruction);
			case CILOpCode.Ldftn: return Ldftn(context, stack, instruction);
			case CILOpCode.Ldind_i: return Ldind(context, stack, ElementType.I);
			case CILOpCode.Ldind_i1: return Ldind(context, stack, ElementType.I1);
			case CILOpCode.Ldind_i2: return Ldind(context, stack, ElementType.I2);
			case CILOpCode.Ldind_i4: return Ldind(context, stack, ElementType.I4);
			case CILOpCode.Ldind_i8: return Ldind(context, stack, ElementType.I8);
			case CILOpCode.Ldind_r4: return Ldind(context, stack, ElementType.R4);
			case CILOpCode.Ldind_r8: return Ldind(context, stack, ElementType.R8);
			case CILOpCode.Ldind_ref: return Ldind(context, stack, ElementType.Object);
			case CILOpCode.Ldind_u1: return Ldind(context, stack, ElementType.U1);
			case CILOpCode.Ldind_u2: return Ldind(context, stack, ElementType.U2);
			case CILOpCode.Ldind_u4: return Ldind(context, stack, ElementType.U4);
			case CILOpCode.Ldlen: return Ldlen(context, stack);
			case CILOpCode.Ldloc: return Ldloc(context, stack, (int)instruction.Operand);
			case CILOpCode.Ldloc_0: return Ldloc(context, stack, 0);
			case CILOpCode.Ldloc_1: return Ldloc(context, stack, 1);
			case CILOpCode.Ldloc_2: return Ldloc(context, stack, 2);
			case CILOpCode.Ldloc_3: return Ldloc(context, stack, 3);
			case CILOpCode.Ldloc_s: return Ldloc(context, stack, (int)instruction.Operand);
			case CILOpCode.Ldloca: return Ldloca(context, stack, instruction);
			case CILOpCode.Ldloca_s: return Ldloca(context, stack, instruction);
			case CILOpCode.Ldnull: return Ldnull(context, stack);
			case CILOpCode.Ldobj: return Ldobj(context, stack, instruction);
			case CILOpCode.Ldsfld: return Ldsfld(context, stack, instruction);
			case CILOpCode.Ldsflda: return Ldsflda(context, stack, instruction);
			case CILOpCode.Ldstr: return Ldstr(context, stack, instruction);
			case CILOpCode.Ldtoken: return Ldtoken(context, stack, instruction);
			case CILOpCode.Ldvirtftn: return false; // TODO: Not implemented in v1 either
			case CILOpCode.Leave: return Leave(context, stack, instruction, block);
			case CILOpCode.Leave_s: return Leave(context, stack, instruction, block);
			case CILOpCode.Localalloc: return false;    // TODO: Not implemented in v1 either
			case CILOpCode.Mkrefany: return false;  // TODO: Not implemented in v1 either
			case CILOpCode.Mul: return Mul(context, stack);
			case CILOpCode.Mul_ovf: return MulSigned(context, stack);
			case CILOpCode.Mul_ovf_un: return MulUnsigned(context, stack);
			case CILOpCode.Neg: return Neg(context, stack);
			case CILOpCode.Newarr: return Newarr(context, stack, instruction);
			case CILOpCode.Newobj: return Newobj(context, stack, instruction);
			case CILOpCode.Nop: return Nop(context);
			case CILOpCode.Not: return Not(context, stack);
			case CILOpCode.Or: return Or(context, stack);
			case CILOpCode.Pop: return Pop(context, stack);
			case CILOpCode.Constrained: return Constrained(prefixValues, instruction);
			case CILOpCode.No: /* TODO */ prefixValues.Reset = false; return true;
			case CILOpCode.ReadOnly: prefixValues.Readonly = true; prefixValues.Reset = false; return true;
			case CILOpCode.Tailcall: prefixValues.Tailcall = true; prefixValues.Reset = false; return true;
			case CILOpCode.Unaligned: prefixValues.Unaligned = true; prefixValues.Reset = false; return true;
			case CILOpCode.Volatile: prefixValues.Volatile = true; prefixValues.Reset = false; return true;
			case CILOpCode.Refanytype: return false;    // TODO: Not implemented in v1 either
			case CILOpCode.Refanyval: return false; // TODO: Not implemented in v1 either
			case CILOpCode.Rem: return RemOperand(context, stack);
			case CILOpCode.Rem_un: return RemUnsigned(context, stack);
			case CILOpCode.Ret: return Ret(context, stack);
			case CILOpCode.Rethrow: return Rethrow(context, stack);
			case CILOpCode.Shl: return Shl(context, stack, instruction);
			case CILOpCode.Shr: return Shr(context, stack, instruction);
			case CILOpCode.Shr_un: return ShrU(context, stack, instruction);
			case CILOpCode.Sizeof: return Sizeof(context, stack, instruction);
			case CILOpCode.Starg: return StoreArgument(context, stack, (int)instruction.Operand);
			case CILOpCode.Starg_s: return StoreArgument(context, stack, (int)instruction.Operand);
			case CILOpCode.Stelem: return Stelem(context, stack, instruction);
			case CILOpCode.Stelem_i: return Stelem(context, stack, ElementType.I);
			case CILOpCode.Stelem_i1: return Stelem(context, stack, ElementType.I1);
			case CILOpCode.Stelem_i2: return Stelem(context, stack, ElementType.I2);
			case CILOpCode.Stelem_i4: return Stelem(context, stack, ElementType.I4);
			case CILOpCode.Stelem_i8: return Stelem(context, stack, ElementType.I8);
			case CILOpCode.Stelem_r4: return Stelem(context, stack, ElementType.R4);
			case CILOpCode.Stelem_r8: return Stelem(context, stack, ElementType.R8);
			case CILOpCode.Stelem_ref: return Stelem(context, stack, ElementType.Object);
			case CILOpCode.Stfld: return Stfld(context, stack, instruction);
			case CILOpCode.Stind_i: return Stind(context, stack, ElementType.I);
			case CILOpCode.Stind_i1: return Stind(context, stack, ElementType.I1);
			case CILOpCode.Stind_i2: return Stind(context, stack, ElementType.I2);
			case CILOpCode.Stind_i4: return Stind(context, stack, ElementType.I4);
			case CILOpCode.Stind_i8: return Stind(context, stack, ElementType.I8);
			case CILOpCode.Stind_r4: return Stind(context, stack, ElementType.R4);
			case CILOpCode.Stind_r8: return Stind(context, stack, ElementType.R8);
			case CILOpCode.Stind_ref: return Stind(context, stack, ElementType.Object);
			case CILOpCode.Stloc: return Stloc(context, stack, (int)instruction.Operand);
			case CILOpCode.Stloc_0: return Stloc(context, stack, 0);
			case CILOpCode.Stloc_1: return Stloc(context, stack, 1);
			case CILOpCode.Stloc_2: return Stloc(context, stack, 2);
			case CILOpCode.Stloc_3: return Stloc(context, stack, 3);
			case CILOpCode.Stloc_s: return Stloc(context, stack, (int)instruction.Operand);
			case CILOpCode.Stobj: return Stobj(context, stack, instruction);
			case CILOpCode.Stsfld: return Stsfld(context, stack, instruction);
			case CILOpCode.Sub: return Sub(context, stack);
			case CILOpCode.Sub_ovf: return SubSigned(context, stack);
			case CILOpCode.Sub_ovf_un: return SubUnsigned(context, stack);
			case CILOpCode.Switch: return Switch(context, stack, instruction);
			case CILOpCode.Throw: return Throw(context, stack);
			case CILOpCode.Unbox: return Unbox(context, stack, instruction);
			case CILOpCode.Unbox_any: return UnboxAny(context, stack, instruction);
			case CILOpCode.Xor: return Xor(context, stack);

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

		for (var index = 0; index < total; index++)
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

			if (first.PrimitiveType == PrimitiveType.ValueType)
			{
				destination = MethodCompiler.LocalStack.Allocate(first.Operand);
				instruction = IRInstruction.MoveCompound;
			}
			else
			{
				destination = MethodCompiler.VirtualRegisters.Allocate(first.PrimitiveType);
				instruction = GetMoveInstruction(first.PrimitiveType);
			}

			foreach (var previousBlock in block.PreviousBlocks)
			{
				var source = OutgoingStacks[previousBlock][index].Operand;

				previousBlock.ContextBeforeBranch.AppendInstruction(instruction, destination, source);
			}

			var entry = new StackEntry(destination);

			incomingStack.Push(entry);
		}

		return incomingStack;
	}

	private void CreateLocalVariables()
	{
		var count = Method.LocalVariables.Count;

		LocalStack = new Operand[count];

		if (count == 0)
			return;

		var arg = new bool[count];
		var argCount = 0;

		var code = Method.Code;

		for (var label = 0; label < code.Count; label++)
		{
			var instruction = code[label];

			var opcode = (CILOpCode)instruction.OpCode;

			if (opcode == CILOpCode.Ldloca || opcode == CILOpCode.Ldloca_s)
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

		LocalPrimitiveType = new PrimitiveType[count];
		LocalElementType = new ElementType[count];

		for (var index = 0; index < count; index++)
		{
			var type = Method.LocalVariables[index];

			var underlyingType = MosaTypeLayout.GetUnderlyingType(type.Type);
			var primitiveType = MethodCompiler.GetPrimitiveType(underlyingType);

			var isPrimitive = MosaTypeLayout.IsPrimitive(underlyingType);

			if (primitiveType == PrimitiveType.ValueType || arg[index] || type.IsPinned)
			{
				LocalStack[index] = MethodCompiler.LocalStack.Allocate(primitiveType, type.IsPinned, type.Type);
			}
			else
			{
				LocalStack[index] = MethodCompiler.VirtualRegisters.Allocate(primitiveType);
			}

			LocalPrimitiveType[index] = primitiveType;
			LocalElementType[index] = isPrimitive ? MethodCompiler.GetElementType(underlyingType) : Is32BitPlatform ? ElementType.I4 : ElementType.I8;
		}
	}

	private void InitializeLocalVariables()
	{
		var prologue = new Context(BasicBlocks.PrologueBlock.AfterFirst);

		for (var index = 0; index < LocalStack.Length; index++)
		{
			var local = LocalStack[index];
			var localstacktype = LocalPrimitiveType[index];

			if (!local.IsVirtualRegister)
				continue;

			switch (localstacktype)
			{
				case PrimitiveType.Object:
					prologue.AppendInstruction(IRInstruction.MoveObject, local, Operand.GetNullObject());
					break;

				case PrimitiveType.Int32:
					prologue.AppendInstruction(IRInstruction.Move32, local, Constant32_0);
					break;

				case PrimitiveType.Int64:
					prologue.AppendInstruction(IRInstruction.Move64, local, Constant64_0);
					break;

				case PrimitiveType.R4:
					prologue.AppendInstruction(IRInstruction.MoveR4, local, ConstantR4_0);
					break;

				case PrimitiveType.R8:
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

		for (var i = 0; i < count; i++)
		{
			var operand = PopStack(stack).Operand;
			operands.Add(operand);
		}

		operands.Reverse();

		return operands;
	}

	private static bool IsBranch(CILOpCode opcode)
	{
		return opcode switch
		{
			CILOpCode.Beq => true,
			CILOpCode.Beq_s => true,
			CILOpCode.Bge => true,
			CILOpCode.Bge_s => true,
			CILOpCode.Bge_un => true,
			CILOpCode.Bge_un_s => true,
			CILOpCode.Bgt => true,
			CILOpCode.Bgt_s => true,
			CILOpCode.Bgt_un => true,
			CILOpCode.Bgt_un_s => true,
			CILOpCode.Ble => true,
			CILOpCode.Ble_s => true,
			CILOpCode.Ble_un => true,
			CILOpCode.Ble_un_s => true,
			CILOpCode.Blt => true,
			CILOpCode.Blt_s => true,
			CILOpCode.Blt_un => true,
			CILOpCode.Blt_un_s => true,
			CILOpCode.Bne_un => true,
			CILOpCode.Bne_un_s => true,
			CILOpCode.Brfalse_s => true,
			CILOpCode.Brtrue_s => true,
			CILOpCode.Brfalse => true,
			CILOpCode.Brtrue => true,
			_ => false,
		};
	}

	private StackEntry CreateStackEntry(MosaType type)
	{
		if (type == null)
			return null;

		var underlyingType = MosaTypeLayout.GetUnderlyingType(type);
		var primitiveType = MethodCompiler.GetPrimitiveType(underlyingType);
		var isPrimitive = MethodCompiler.IsPrimitive(primitiveType);

		var operand = isPrimitive
			? MethodCompiler.VirtualRegisters.Allocate(primitiveType)
			: MethodCompiler.LocalStack.Allocate(primitiveType, false, type);

		return new StackEntry(operand);
	}

	private string EmitString(string data, uint token)
	{
		var symbolName = $"$ldstr${Method.Module.Name}${token}";
		var linkerSymbol = Linker.DefineSymbol(symbolName, SectionKind.ROData, NativeAlignment, (uint)(ObjectHeaderSize + NativePointerSize + data.Length * 2));
		var writer = new BinaryWriter(linkerSymbol.Stream);

		Linker.Link(LinkType.AbsoluteAddress, PatchType.I32, linkerSymbol, ObjectHeaderSize - NativePointerSize, Metadata.TypeDefinition + "System.String", 0);

		writer.WriteZeroBytes(ObjectHeaderSize);
		writer.Write(data.Length, NativePointerSize);
		writer.Write(Encoding.Unicode.GetBytes(data));
		return symbolName;
	}

	private BasicBlock GetOrCreateBlock(int label)
	{
		var block = BasicBlocks.GetByLabel(label);

		block ??= BasicBlocks.CreateBlock(label, label);

		return block;
	}

	private Operand GetMethodTablePointer(MosaType runtimeType)
	{
		return Operand.CreateLabel(Metadata.TypeDefinition + runtimeType.FullName, Is32BitPlatform);
	}

	private Operand GetRuntimeTypeHandle(MosaType runtimeType)
	{
		return Operand.CreateLabel(Metadata.TypeDefinition + runtimeType.FullName, Is32BitPlatform);
	}

	#endregion Helpers

	#region Instruction Maps

	private BaseInstruction GetBoxInstruction(ElementType elementType)
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
			ElementType.ManagedPointer when Is32BitPlatform => IRInstruction.Box32,
			ElementType.ManagedPointer when Is64BitPlatform => IRInstruction.Box64,
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

	private BaseInstruction GetMoveInstruction(PrimitiveType type)
	{
		return type switch
		{
			PrimitiveType.Object => IRInstruction.MoveObject,
			PrimitiveType.Int32 => IRInstruction.Move32,
			PrimitiveType.Int64 => IRInstruction.Move64,
			PrimitiveType.R4 => IRInstruction.MoveR4,
			PrimitiveType.R8 => IRInstruction.MoveR8,
			PrimitiveType.ManagedPointer when Is32BitPlatform => IRInstruction.Move32,
			PrimitiveType.ManagedPointer when Is64BitPlatform => IRInstruction.Move64,
			_ => throw new CompilerException($"Invalid StackType = {type}"),
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

	protected BaseInstruction GetLoadInstruction(MosaType type)
	{
		type = MosaTypeLayout.GetUnderlyingType(type);

		if (type.IsValueType)
			return IRInstruction.LoadCompound;
		else if (type.IsReferenceType)
			return IRInstruction.LoadObject;
		else if (type.IsPointer)
			return Select(IRInstruction.Load32, IRInstruction.Load64);
		else if (type.IsI1)
			return Select(IRInstruction.LoadSignExtend8x32, IRInstruction.LoadSignExtend8x64);
		else if (type.IsI2)
			return Select(IRInstruction.LoadSignExtend16x32, IRInstruction.LoadSignExtend16x64);
		else if (type.IsI4)
			return Select(IRInstruction.Load32, IRInstruction.LoadSignExtend32x64);
		else if (type.IsI8)
			return IRInstruction.Load64;
		else if (type.IsU1 || type.IsBoolean)
			return Select(IRInstruction.LoadZeroExtend8x32, IRInstruction.LoadZeroExtend8x64);
		else if (type.IsU2 || type.IsChar)
			return Select(IRInstruction.LoadZeroExtend16x32, IRInstruction.LoadZeroExtend16x64);
		else if (type.IsU4)
			return Select(IRInstruction.Load32, IRInstruction.LoadZeroExtend32x64);
		else if (type.IsU8)
			return IRInstruction.Load64;
		else if (type.IsR4)
			return IRInstruction.LoadR4;
		else if (type.IsR8)
			return IRInstruction.LoadR8;
		else if (Is32BitPlatform)   // review
			return IRInstruction.Load32;
		else if (Is64BitPlatform)
			return IRInstruction.Load64;

		throw new InvalidOperationException();
	}

	#endregion Instruction Maps

	#region Constant Helpers

	private bool Constant32(Context context, Stack<StackEntry> stack, int value)
	{
		var result = MethodCompiler.VirtualRegisters.Allocate32();
		context.AppendInstruction(IRInstruction.Move32, result, CreateConstant32(value));
		PushStack(stack, new StackEntry(result));
		return true;
	}

	private bool Constant64(Context context, Stack<StackEntry> stack, long value)
	{
		var result = MethodCompiler.VirtualRegisters.Allocate64();
		context.AppendInstruction(IRInstruction.Move64, result, CreateConstant64(value));
		PushStack(stack, new StackEntry(result));
		return true;
	}

	private bool ConstantR4(Context context, Stack<StackEntry> stack, float value)
	{
		var result = MethodCompiler.VirtualRegisters.AllocateR4();
		context.AppendInstruction(IRInstruction.MoveR4, result, CreateConstantR4(value));
		PushStack(stack, new StackEntry(result));
		return true;
	}

	private bool ConstantR8(Context context, Stack<StackEntry> stack, double value)
	{
		var result = MethodCompiler.VirtualRegisters.AllocateR8();
		context.AppendInstruction(IRInstruction.MoveR8, result, CreateConstantR8(value));
		PushStack(stack, new StackEntry(result));
		return true;
	}

	private bool Ldnull(Context context, Stack<StackEntry> stack)
	{
		var result = MethodCompiler.VirtualRegisters.AllocateObject();
		context.AppendInstruction(IRInstruction.MoveObject, result, Operand.GetNullObject());
		PushStack(stack, new StackEntry(result));
		return true;
	}

	#endregion Constant Helpers

	#region CIL

	private bool Add(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.R4 when entry2.PrimitiveType == PrimitiveType.R4 && entry1.Operand.IsR4:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateR4();
					context.AppendInstruction(IRInstruction.AddR4, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.R8 when entry2.PrimitiveType == PrimitiveType.R8 && entry1.Operand.IsR8:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateR8();
					context.AppendInstruction(IRInstruction.AddR8, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.Add32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.Add64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.Add64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.Add64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is32BitPlatform:
			case PrimitiveType.ManagedPointer when entry2.PrimitiveType == PrimitiveType.Int32 && Is32BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					context.AppendInstruction(IRInstruction.Add32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					context.AppendInstruction(IRInstruction.Add64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.Add64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.ManagedPointer when entry2.PrimitiveType == PrimitiveType.Int32 && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.Add64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			default: return false;
		}
	}

	private bool AddUnsigned(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					var result2 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction2(IRInstruction.AddCarryOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction2(IRInstruction.AddCarryOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.AddCarryOut64, result, result2, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.AddCarryOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is32BitPlatform:
			case PrimitiveType.ManagedPointer when entry2.PrimitiveType == PrimitiveType.Int32 && Is32BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var result2 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction2(IRInstruction.AddCarryOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is64BitPlatform:
			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction2(IRInstruction.AddCarryOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.AddCarryOut64, result, result2, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.ManagedPointer when entry2.PrimitiveType == PrimitiveType.Int32 && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.AddCarryOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			default: return false;
		}
	}

	private bool AddSigned(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					var result2 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction2(IRInstruction.AddOverflowOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction2(IRInstruction.AddOverflowOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.AddOverflowOut64, result, result2, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.AddOverflowOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is32BitPlatform:
			case PrimitiveType.ManagedPointer when entry2.PrimitiveType == PrimitiveType.Int32 && Is32BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var result2 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction2(IRInstruction.AddOverflowOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction2(IRInstruction.AddOverflowOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.AddOverflowOut64, result, result2, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.ManagedPointer when entry2.PrimitiveType == PrimitiveType.Int32 && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.AddOverflowOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			default: return false;
		}
	}

	private bool And(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.And32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.And64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.And64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.And64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(result));
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

		var result = MethodCompiler.VirtualRegisters.AllocateObject();
		PushStack(stack, new StackEntry(result));

		if (type.IsReferenceType)
		{
			context.AppendInstruction(MoveInstruction, result, entry.Operand);
			return true;
		}

		var methodTable = GetMethodTablePointer(type);

		var underlyingType = MosaTypeLayout.GetUnderlyingType(type);
		var isPrimitive = MosaTypeLayout.IsPrimitive(underlyingType);

		if (isPrimitive)
		{
			var elementType = MethodCompiler.GetElementType(underlyingType);
			var boxInstruction = GetBoxInstruction(elementType);

			context.AppendInstruction(boxInstruction, result, methodTable, entry.Operand);
		}
		else
		{
			var typeSize = Alignment.AlignUp(TypeLayout.GetTypeSize(type), TypeLayout.NativePointerAlignment);

			if (typeSize == 8)
			{
				context.AppendInstruction(IRInstruction.Box64, result, methodTable, entry.Operand);
			}
			else
			{
				var address = MethodCompiler.VirtualRegisters.AllocateManagedPointer();

				context.AppendInstruction(IRInstruction.AddressOf, address, entry.Operand);
				context.AppendInstruction(IRInstruction.Box, result, methodTable, address, CreateConstant32(typeSize));
			}
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

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.R4 when entry2.PrimitiveType == PrimitiveType.R4 && entry1.Operand.IsR4:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.CompareR4, conditionCode, result, entry1.Operand, entry2.Operand);
					context.AppendInstruction(IRInstruction.Branch32, ConditionCode.NotEqual, null, result, Constant32_0, block);
					return true;
				}

			case PrimitiveType.R8 when entry2.PrimitiveType == PrimitiveType.R8 && entry1.Operand.IsR8:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.CompareR8, conditionCode, result, entry1.Operand, entry2.Operand);
					context.AppendInstruction(IRInstruction.Branch32, ConditionCode.NotEqual, null, result, Constant32_0, block);
					return true;
				}

			case PrimitiveType.Object when entry2.PrimitiveType == PrimitiveType.Object:
				context.AppendInstruction(IRInstruction.BranchObject, conditionCode, null, entry1.Operand, entry2.Operand, block);
				return true;

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.Branch32, conditionCode, null, entry1.Operand, entry2.Operand, block);
				return true;

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
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

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.Branch32, conditionCode, null, entry.Operand, Constant32_0, block);
				context.AppendInstruction(IRInstruction.Jmp, nextblock);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.Branch64, conditionCode, null, entry.Operand, Constant64_0, block);
				context.AppendInstruction(IRInstruction.Jmp, nextblock);
				return true;

			case PrimitiveType.Object:
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
			var resultStackType = CreateStackEntry(method.Signature.ReturnType);

			result = resultStackType.Operand;

			PushStack(stack, resultStackType);
		}

		var symbol = Operand.CreateLabel(method, Is32BitPlatform);

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
			var resultStackType = CreateStackEntry(method.Signature.ReturnType);

			result = resultStackType.Operand;

			PushStack(stack, resultStackType);
		}

		if (prefixValues.Constrained)
		{
			var type = prefixValues.ContrainedType;

			if (type.IsValueType)
			{
				method = GetMethodOrOverride(type, method);

				var symbol2 = Operand.CreateLabel(method, Is32BitPlatform);
				context.AppendInstruction(IRInstruction.CallStatic, result, symbol2, operands);

				// PocessExternalCall(context))

				return true;
			}
		}

		var symbol = Operand.CreateLabel(method, Is32BitPlatform);

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

		var result = MethodCompiler.VirtualRegisters.AllocateObject();
		PushStack(stack, new StackEntry(result));

		// FUTURE: Check for valid cast
		//var methodTable = GetMethodTablePointer(type);

		if (entry.PrimitiveType == PrimitiveType.Object)
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

		var result = MethodCompiler.VirtualRegisters.Allocate32();
		PushStack(stack, new StackEntry(result));

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.R4 when entry2.PrimitiveType == PrimitiveType.R4 && entry1.Operand.IsR4:
				context.AppendInstruction(IRInstruction.CompareR4, conditionCode, result, entry1.Operand, entry2.Operand);
				return true;

			case PrimitiveType.R8 when entry2.PrimitiveType == PrimitiveType.R8 && entry1.Operand.IsR8:
				context.AppendInstruction(IRInstruction.CompareR8, conditionCode, result, entry1.Operand, entry2.Operand);
				return true;

			case PrimitiveType.Object when entry2.PrimitiveType == PrimitiveType.Object:
				context.AppendInstruction(IRInstruction.CompareObject, conditionCode, result, entry1.Operand, entry2.Operand);
				return true;

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.Compare32x32, conditionCode, result, entry1.Operand, entry2.Operand);
				return true;

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
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
			var result = MethodCompiler.VirtualRegisters.Allocate32();
			PushStack(stack, new StackEntry(result));

			switch (entry.PrimitiveType)
			{
				case PrimitiveType.Int32:
					context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
					return true;

				case PrimitiveType.Int64:
					context.AppendInstruction(IRInstruction.Truncate64x32, result, entry.Operand);
					return true;

				case PrimitiveType.R4:
					context.AppendInstruction(IRInstruction.ConvertR4ToI32, result, entry.Operand);
					return true;

				case PrimitiveType.R8:
					context.AppendInstruction(IRInstruction.ConvertR8ToI32, result, entry.Operand);
					return true;
			}
		}
		else
		{
			var result = MethodCompiler.VirtualRegisters.Allocate64();
			PushStack(stack, new StackEntry(result));

			switch (entry.PrimitiveType)
			{
				case PrimitiveType.Int32:
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry.Operand);
					return true;

				case PrimitiveType.Int64:
					context.AppendInstruction(IRInstruction.Move64, result, entry.Operand);
					return true;

				case PrimitiveType.R4:
					context.AppendInstruction(IRInstruction.ConvertR4ToI64, result, entry.Operand);
					return true;

				case PrimitiveType.R8:
					context.AppendInstruction(IRInstruction.ConvertR8ToI64, result, entry.Operand);
					return true;
			}
		}

		return false;
	}

	private bool ConvertI1(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				{
					context.AppendInstruction(IRInstruction.SignExtend8x32, result, entry.Operand);
					return true;
				}

			case PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.SignExtend8x32, result, v1);
					return true;
				}

			case PrimitiveType.R4:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.ConvertR4ToI32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
					return true;
				}

			case PrimitiveType.R8:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate32();
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
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				{
					context.AppendInstruction(IRInstruction.SignExtend16x32, result, entry.Operand);
					return true;
				}

			case PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.SignExtend16x32, result, v1);
					return true;
				}

			case PrimitiveType.R4:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.ConvertR4ToI32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));
					return true;
				}

			case PrimitiveType.R8:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate32();
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
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.Truncate64x32, result, entry.Operand);
				return true;

			case PrimitiveType.R4:
				context.AppendInstruction(IRInstruction.ConvertR4ToI32, result, entry.Operand);
				return true;

			case PrimitiveType.R8:
				context.AppendInstruction(IRInstruction.ConvertR8ToI32, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertI8(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate64();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.SignExtend32x64, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.Move64, result, entry.Operand);
				return true;

			case PrimitiveType.R4:
				context.AppendInstruction(IRInstruction.ConvertR4ToI64, result, entry.Operand);
				return true;

			case PrimitiveType.R8:
				context.AppendInstruction(IRInstruction.ConvertR8ToI64, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertR4(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.AllocateR4();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.ConvertI32ToR4, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.ConvertI64ToR4, result, entry.Operand);
				return true;

			case PrimitiveType.R4:
				context.AppendInstruction(IRInstruction.MoveR4, result, entry.Operand);
				return true;

			case PrimitiveType.R8:
				context.AppendInstruction(IRInstruction.ConvertR8ToR4, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertR8(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.AllocateR8();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.ConvertI32ToR8, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.ConvertI64ToR8, result, entry.Operand);
				return true;

			case PrimitiveType.R4:
				context.AppendInstruction(IRInstruction.ConvertR4ToR8, result, entry.Operand);
				return true;

			case PrimitiveType.R8:
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
			var result = MethodCompiler.VirtualRegisters.Allocate32();
			PushStack(stack, new StackEntry(result));

			switch (entry1.PrimitiveType)
			{
				case PrimitiveType.Int32:
					context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);
					return true;

				case PrimitiveType.Int64:
					context.AppendInstruction(IRInstruction.Truncate64x32, result, entry1.Operand);
					return true;

				case PrimitiveType.R4:
					context.AppendInstruction(IRInstruction.ConvertR4ToU32, result, entry1.Operand);
					return true;

				case PrimitiveType.R8:
					context.AppendInstruction(IRInstruction.ConvertR8ToU32, result, entry1.Operand);
					return true;

				case PrimitiveType.ManagedPointer:
					context.AppendInstruction(IRInstruction.Move32, result, entry1.Operand);
					return true;
			}

			// TODO: Float
		}
		else
		{
			var result = MethodCompiler.VirtualRegisters.Allocate64();
			PushStack(stack, new StackEntry(result));

			switch (entry1.PrimitiveType)
			{
				case PrimitiveType.Int32:
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry1.Operand);
					return true;

				case PrimitiveType.Int64:
					context.AppendInstruction(IRInstruction.Move64, result, entry1.Operand);
					return true;

				case PrimitiveType.R4:
					context.AppendInstruction(IRInstruction.ConvertR4ToU64, result, entry1.Operand);
					return true;

				case PrimitiveType.R8:
					context.AppendInstruction(IRInstruction.ConvertR8ToU64, result, entry1.Operand);
					return true;

				case PrimitiveType.ManagedPointer:
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
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.And32, result, entry.Operand, CreateConstant32(0xFF));
				return true;

			case PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
					return true;
				}

			case PrimitiveType.R4:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.ConvertR4ToU32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFF));
					return true;
				}

			case PrimitiveType.R8:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate32();
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
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.And32, result, entry.Operand, CreateConstant32(0xFFFF));
				return true;

			case PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.Truncate64x32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));
					return true;
				}

			case PrimitiveType.R4:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.ConvertR4ToU32, v1, entry.Operand);
					context.AppendInstruction(IRInstruction.And32, result, v1, CreateConstant32(0xFFFF));

					return true;
				}

			case PrimitiveType.R8:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate32();
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
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.Truncate64x32, result, entry.Operand);
				return true;

			case PrimitiveType.R4:
				context.AppendInstruction(IRInstruction.ConvertR4ToU32, result, entry.Operand);
				return true;

			case PrimitiveType.R8:
				context.AppendInstruction(IRInstruction.ConvertR8ToU32, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertU8(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate64();
		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.Move64, result, entry.Operand);
				return true;

			case PrimitiveType.R4:
				context.AppendInstruction(IRInstruction.ConvertR4ToU64, result, entry.Operand);
				return true;

			case PrimitiveType.R8:
				context.AppendInstruction(IRInstruction.ConvertR8ToU64, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToF(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.AllocateR8();
		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.ConvertU32ToR8, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
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
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.And32, v1, entry.Operand, CreateConstant32(0xFF));
					context.AppendInstruction(IRInstruction.Or32, result, v1, CreateConstant32(~0xFF));
					return true;
				}

			case PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate32();
					var v2 = MethodCompiler.VirtualRegisters.Allocate32();
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
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.And32, v1, entry.Operand, CreateConstant32(0xFFFF));
					context.AppendInstruction(IRInstruction.Or32, result, v1, CreateConstant32(~0xFFFF));
					return true;
				}

			case PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate32();
					var v2 = MethodCompiler.VirtualRegisters.Allocate32();
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
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
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
		var result = MethodCompiler.VirtualRegisters.Allocate64();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.SignExtend32x64, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
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
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.And32, result, entry.Operand, CreateConstant32(0xFF));
				return true;

			case PrimitiveType.Int64:
				var v1 = MethodCompiler.VirtualRegisters.Allocate32();
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
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.And32, result, entry.Operand, CreateConstant32(0xFFFF));
				return true;

			case PrimitiveType.Int64:
				var v1 = MethodCompiler.VirtualRegisters.Allocate32();
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
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
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
		var result = MethodCompiler.VirtualRegisters.Allocate64();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.Move64, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToIWithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);

		if (Is32BitPlatform)
		{
			var result = MethodCompiler.VirtualRegisters.Allocate32();
			PushStack(stack, new StackEntry(result));

			switch (entry.PrimitiveType)
			{
				case PrimitiveType.Int32:
					context.AppendInstruction(IRInstruction.CheckedConversionU32ToI32, result, entry.Operand);
					return true;

				case PrimitiveType.Int64:
					context.AppendInstruction(IRInstruction.CheckedConversionU64ToI32, result, entry.Operand);
					return true;
			}
		}
		else
		{
			var result = MethodCompiler.VirtualRegisters.Allocate64();
			PushStack(stack, new StackEntry(result));

			switch (entry.PrimitiveType)
			{
				case PrimitiveType.Int32:
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry.Operand);
					return true;

				case PrimitiveType.Int64:
					context.AppendInstruction(IRInstruction.CheckedConversionU64ToI64, result, entry.Operand);
					return true;
			}
		}

		return false;
	}

	private bool ConvertToIWithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);

		if (Is32BitPlatform)
		{
			var result = MethodCompiler.VirtualRegisters.Allocate32();
			PushStack(stack, new StackEntry(result));

			switch (entry.PrimitiveType)
			{
				case PrimitiveType.Int32:
					context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
					return true;

				case PrimitiveType.Int64:
					context.AppendInstruction(IRInstruction.CheckedConversionI64ToI32, result, entry.Operand);
					return true;

				case PrimitiveType.R4:
					context.AppendInstruction(IRInstruction.CheckedConversionR4ToI32, result, entry.Operand);
					return true;

				case PrimitiveType.R8:
					context.AppendInstruction(IRInstruction.CheckedConversionR8ToI32, result, entry.Operand);
					return true;
			}
		}
		else
		{
			var result = MethodCompiler.VirtualRegisters.Allocate64();
			PushStack(stack, new StackEntry(result));

			switch (entry.PrimitiveType)
			{
				case PrimitiveType.Int32:
					context.AppendInstruction(IRInstruction.SignExtend32x64, result, entry.Operand);
					return true;

				case PrimitiveType.Int64:
					context.AppendInstruction(IRInstruction.Move64, result, entry.Operand);
					return true;

				case PrimitiveType.R4:
					context.AppendInstruction(IRInstruction.CheckedConversionR4ToI64, result, entry.Operand);
					return true;

				case PrimitiveType.R8:
					context.AppendInstruction(IRInstruction.CheckedConversionR8ToI64, result, entry.Operand);
					return true;
			}
		}

		return false;
	}

	private bool ConvertToUWithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);

		if (Is32BitPlatform)
		{
			var result = MethodCompiler.VirtualRegisters.Allocate32();
			PushStack(stack, new StackEntry(result));

			switch (entry.PrimitiveType)
			{
				case PrimitiveType.Int32:
					context.AppendInstruction(IRInstruction.CheckedConversionU32ToI32, result, entry.Operand);
					return true;

				case PrimitiveType.Int64:
					context.AppendInstruction(IRInstruction.CheckedConversionU64ToI32, result, entry.Operand);
					return true;
			}
		}
		else
		{
			var result = MethodCompiler.VirtualRegisters.Allocate64();
			PushStack(stack, new StackEntry(result));

			switch (entry.PrimitiveType)
			{
				case PrimitiveType.Int32:
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry.Operand);
					return true;

				case PrimitiveType.Int64:
					context.AppendInstruction(IRInstruction.CheckedConversionU64ToI64, result, entry.Operand);
					return true;
			}
		}

		return false;
	}

	private bool ConvertI1WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionI32ToI8, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionI64ToI8, result, entry.Operand);
				return true;

			case PrimitiveType.R4:
				context.AppendInstruction(IRInstruction.CheckedConversionR4ToI8, result, entry.Operand);
				return true;

			case PrimitiveType.R8:
				context.AppendInstruction(IRInstruction.CheckedConversionR8ToI8, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToI1WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionU32ToI8, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionU64ToI8, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertI2WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionI32ToI16, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionI64ToI16, result, entry.Operand);
				return true;

			case PrimitiveType.R4:
				context.AppendInstruction(IRInstruction.CheckedConversionR4ToI16, result, entry.Operand);
				return true;

			case PrimitiveType.R8:
				context.AppendInstruction(IRInstruction.CheckedConversionR8ToI16, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToI2WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionU32ToI16, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionU64ToI16, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertI4WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionI64ToI32, result, entry.Operand);
				return true;

			case PrimitiveType.R4:
				context.AppendInstruction(IRInstruction.CheckedConversionR4ToI32, result, entry.Operand);
				return true;

			case PrimitiveType.R8:
				context.AppendInstruction(IRInstruction.CheckedConversionR8ToI32, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToI4WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionU32ToI32, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionU64ToI32, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertI8WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate64();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.SignExtend32x64, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.Move64, result, entry.Operand);
				return true;

			case PrimitiveType.R4:
				context.AppendInstruction(IRInstruction.CheckedConversionR4ToI64, result, entry.Operand);
				return true;

			case PrimitiveType.R8:
				context.AppendInstruction(IRInstruction.CheckedConversionR8ToI64, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToI8WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate64();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionU64ToI64, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToUWithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);

		if (Is32BitPlatform)
		{
			var result = MethodCompiler.VirtualRegisters.Allocate32();
			PushStack(stack, new StackEntry(result));

			switch (entry.PrimitiveType)
			{
				case PrimitiveType.Int32:
					context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
					return true;

				case PrimitiveType.Int64:
					context.AppendInstruction(IRInstruction.CheckedConversionU64ToU32, result, entry.Operand);
					return true;
			}
		}
		else
		{
			var result = MethodCompiler.VirtualRegisters.Allocate64();
			PushStack(stack, new StackEntry(result));

			switch (entry.PrimitiveType)
			{
				case PrimitiveType.Int32:
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry.Operand);
					return true;

				case PrimitiveType.Int64:
					context.AppendInstruction(IRInstruction.Move64, result, entry.Operand);
					return true;
			}
		}

		return false;
	}

	private bool ConvertU1WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionI32ToU8, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionI64ToU8, result, entry.Operand);
				return true;

			case PrimitiveType.R4:
				context.AppendInstruction(IRInstruction.CheckedConversionR4ToU8, result, entry.Operand);
				return true;

			case PrimitiveType.R8:
				context.AppendInstruction(IRInstruction.CheckedConversionR8ToU8, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToU1WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionU32ToU8, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionU64ToU8, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertU2WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionI32ToU16, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionI64ToU16, result, entry.Operand);
				return true;

			case PrimitiveType.R4:
				context.AppendInstruction(IRInstruction.CheckedConversionR4ToU16, result, entry.Operand);
				return true;

			case PrimitiveType.R8:
				context.AppendInstruction(IRInstruction.CheckedConversionR8ToU16, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToU2WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionU32ToU16, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionU64ToU16, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertU4WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionI32ToU32, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionI64ToU32, result, entry.Operand);
				return true;

			case PrimitiveType.R4:
				context.AppendInstruction(IRInstruction.CheckedConversionR4ToU32, result, entry.Operand);
				return true;

			case PrimitiveType.R8:
				context.AppendInstruction(IRInstruction.CheckedConversionR8ToU32, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToU4WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.Move32, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionU64ToU32, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertU8WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate64();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.CheckedConversionI32ToU64, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
				context.AppendInstruction(IRInstruction.CheckedConversionI64ToU64, result, entry.Operand);
				return true;

			case PrimitiveType.R4:
				context.AppendInstruction(IRInstruction.CheckedConversionR4ToU64, result, entry.Operand);
				return true;

			case PrimitiveType.R8:
				context.AppendInstruction(IRInstruction.CheckedConversionR8ToU64, result, entry.Operand);
				return true;

			default:
				return false;
		}
	}

	private bool ConvertUToU8WithOverflow(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		PushStack(stack, new StackEntry(result));

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				context.AppendInstruction(IRInstruction.ZeroExtend32x64, result, entry.Operand);
				return true;

			case PrimitiveType.Int64:
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

		if ((source.PrimitiveType == PrimitiveType.Int32 || source.PrimitiveType == PrimitiveType.Int64) && (destination.PrimitiveType == PrimitiveType.Int32 || destination.PrimitiveType == PrimitiveType.Int64))
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

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.R4 when entry2.PrimitiveType == PrimitiveType.R4 && entry1.Operand.IsR4:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateR4();
					context.AppendInstruction(IRInstruction.DivR4, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.R8 when entry2.PrimitiveType == PrimitiveType.R8 && entry1.Operand.IsR8:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateR8();
					context.AppendInstruction(IRInstruction.DivR8, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.DivSigned32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.DivSigned64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.DivSigned64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.DivSigned64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(result));
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

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.R4 when entry2.PrimitiveType == PrimitiveType.R4 && entry1.Operand.IsR4:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateR4();
					context.AppendInstruction(IRInstruction.DivR4, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.R8 when entry2.PrimitiveType == PrimitiveType.R8 && entry1.Operand.IsR8:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateR8();
					context.AppendInstruction(IRInstruction.DivR8, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.DivUnsigned32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.DivUnsigned64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.DivUnsigned64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.DivUnsigned64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(result));
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
			context.AppendInstruction(IRInstruction.StoreObject, null, entry.Operand, ConstantZero, Operand.GetNullObject());
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

		var result = MethodCompiler.VirtualRegisters.AllocateObject();

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

		PushStack(stack, new StackEntry(result));

		return true;
	}

	private bool Ldarg(Context context, Stack<StackEntry> stack, int index)
	{
		var parameter = MethodCompiler.Parameters[index];
		var isPrimitive = parameter.IsPrimitive;

		if (isPrimitive)
		{
			var loadInstruction = GetLoadParamInstruction(parameter.Element);
			var result = MethodCompiler.VirtualRegisters.Allocate(parameter);

			context.AppendInstruction(loadInstruction, result, parameter);

			PushStack(stack, new StackEntry(result));
		}
		else
		{
			var result = MethodCompiler.LocalStack.Allocate(parameter);

			context.AppendInstruction(IRInstruction.LoadParamCompound, result, parameter);
			context.MosaType = parameter.Type;

			PushStack(stack, new StackEntry(result));
		}

		return true;
	}

	private bool Ldarga(Context context, Stack<StackEntry> stack, int index)
	{
		var parameter = MethodCompiler.Parameters[index];
		var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();

		context.AppendInstruction(IRInstruction.AddressOf, result, parameter);

		PushStack(stack, new StackEntry(result));

		return true;
	}

	private bool Ldelem(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		var index = entry1.Operand;
		var array = entry2.Operand;

		var type = (MosaType)instruction.Operand;

		var underlyingType = MosaTypeLayout.GetUnderlyingType(type);
		var isPrimitive = MosaTypeLayout.IsPrimitive(underlyingType);

		context.AppendInstruction(IRInstruction.CheckArrayBounds, null, array, index);

		var elementOffset = CalculateArrayElementOffset(context, type, index);
		var totalElementOffset = CalculateTotalArrayOffset(context, elementOffset);

		if (isPrimitive)
		{
			var stacktype = MethodCompiler.GetPrimitiveType(underlyingType);
			var result = MethodCompiler.VirtualRegisters.Allocate(stacktype);
			var elementType = MethodCompiler.GetElementType(underlyingType);
			var loadInstruction = GetLoadInstruction(elementType);

			context.AppendInstruction(loadInstruction, result, array, totalElementOffset);

			PushStack(stack, new StackEntry(result));
		}
		else
		{
			var result = MethodCompiler.LocalStack.Allocate(PrimitiveType.ValueType, false, type);

			context.AppendInstruction(IRInstruction.LoadCompound, result, array, totalElementOffset);

			PushStack(stack, new StackEntry(result));
		}

		return true;
	}

	private bool Ldelem(Context context, Stack<StackEntry> stack, ElementType elementType)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		var array = entry2.Operand;
		var index = entry1.Operand;

		//var underlyingType = MosaTypeLayout.GetUnderlyingType(type.ElementType);

		context.AppendInstruction(IRInstruction.CheckArrayBounds, null, array, index);

		var elementOffset = CalculateArrayElementOffset(context, MethodCompiler.GetSize(elementType), index);
		var totalElementOffset = CalculateTotalArrayOffset(context, elementOffset);

		var stacktype = MethodCompiler.GetPrimitiveType(elementType);
		var result = MethodCompiler.VirtualRegisters.Allocate(stacktype);
		PushStack(stack, new StackEntry(result));

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

		var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();

		context.AppendInstruction(IRInstruction.CheckArrayBounds, null, array, index);

		var elementOffset = CalculateArrayElementOffset(context, type, index);
		var totalElementOffset = CalculateTotalArrayOffset(context, elementOffset);

		context.AppendInstruction(Is32BitPlatform ? (BaseIRInstruction)IRInstruction.Add32 : IRInstruction.Add64, result, array, totalElementOffset);

		PushStack(stack, new StackEntry(result));

		return true;
	}

	private bool Ldfld(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry = PopStack(stack);

		var field = (MosaField)instruction.Operand;
		var offset = TypeLayout.GetFieldOffset(field);
		var fieldType = field.FieldType;

		var fieldUnderlyingType = MosaTypeLayout.GetUnderlyingType(fieldType);
		var fieldStacktype = MethodCompiler.GetPrimitiveType(fieldUnderlyingType);
		var isFieldPrimitive = MosaTypeLayout.IsPrimitive(fieldUnderlyingType);

		//var result = MethodCompiler.AllocateVirtualRegisterOrLocalStack(fieldStacktype, isFieldPrimitive ? fieldUnderlyingType : fieldType);

		var stackEntry = CreateStackEntry(fieldType); // new StackEntry(fieldStacktype, result, result.Type);
		var result = stackEntry.Operand;

		PushStack(stack, stackEntry);

		var operand = entry.Operand;

		if (entry.PrimitiveType == PrimitiveType.ValueType)
		{
			var address = MethodCompiler.VirtualRegisters.AllocateManagedPointer();

			context.AppendInstruction(IRInstruction.AddressOf, address, operand);

			operand = address;
		}

		var classUnderlyingType = MosaTypeLayout.GetUnderlyingType(field.DeclaringType);
		var isClassPrimitive = MosaTypeLayout.IsPrimitive(classUnderlyingType);

		var isPointer = operand.IsManagedPointer || operand.Type == TypeSystem.BuiltIn.I || operand.Type == TypeSystem.BuiltIn.U;

		if (isFieldPrimitive && isClassPrimitive && field.DeclaringType.IsValueType && !isPointer)
		{
			Debug.Assert(offset == 0);

			var elementType = MethodCompiler.GetElementType(fieldStacktype);
			var moveInstruction = GetMoveInstruction(elementType);

			context.AppendInstruction(moveInstruction, result, entry.Operand);

			return true;
		}

		var fixedOffset = CreateConstant32(offset);

		if (fieldStacktype == PrimitiveType.ValueType)
		{
			if (isFieldPrimitive)
			{
				var elementType = MethodCompiler.GetElementType(fieldStacktype);
				var loadInstruction = GetLoadInstruction(elementType);

				context.AppendInstruction(loadInstruction, result, operand, fixedOffset);
			}
			else
			{
				context.AppendInstruction(IRInstruction.LoadCompound, result, operand, fixedOffset);
			}
		}
		else if (isFieldPrimitive)
		{
			var loadInstruction = GetLoadInstruction(fieldUnderlyingType); // TODO: replace with element
			context.AppendInstruction(loadInstruction, result, operand, fixedOffset);
		}
		else
		{
			context.AppendInstruction(IRInstruction.LoadCompound, result, operand, fixedOffset);
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
			source = Operand.CreateLabel(Metadata.TypeDefinition + type.FullName, Is32BitPlatform);
		}
		else if (instruction.Operand is MosaMethod)
		{
			var method = (MosaMethod)instruction.Operand;
			source = Operand.CreateLabel(Metadata.MethodDefinition + method.FullName, Is32BitPlatform);
		}
		else if (instruction.Operand is MosaField)
		{
			var field = (MosaField)instruction.Operand;
			source = Operand.CreateLabel(Metadata.FieldDefinition + field.FullName, Is32BitPlatform);
			MethodScanner.AccessedField(context.MosaField);
		}

		var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
		context.AppendInstruction(move, result, source);

		PushStack(stack, new StackEntry(result));

		return true;
	}

	private bool Leave(Context context, Stack<StackEntry> stack, MosaInstruction instruction, BasicBlock currentBlock)
	{
		var leaveBlock = BasicBlocks.GetByLabel((int)instruction.Operand);

		// Traverse to the header block
		var headerBlock = TraverseBackToNativeBlock(currentBlock);

		// Find enclosing try or finally handler
		var exceptionHandler = FindImmediateExceptionHandler(headerBlock.Label);
		var inTry = exceptionHandler.IsLabelWithinTry(headerBlock.Label);

		var endInstruction = inTry ? (BaseInstruction)IRInstruction.TryEnd : IRInstruction.ExceptionEnd;

		context.AppendInstruction(endInstruction, leaveBlock);  // added header block

		return true;
	}

	private bool Ldflda(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry = PopStack(stack);

		var field = (MosaField)instruction.Operand;

		MethodScanner.AccessedField(field);

		var offset = TypeLayout.GetFieldOffset(field);
		//var fieldPtr = field.FieldType.ToManagedPointer(); // FUTURE: MethodCompiler.VirtualRegisters.AllocateManagedPointer();

		var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();

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

		PushStack(stack, new StackEntry(result));

		return true;
	}

	private bool Ldftn(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var method = (MosaMethod)instruction.Operand;

		var functionPointer = TypeSystem.ToFnPtr(method.Signature);

		var stacktype = MethodCompiler.GetPrimitiveType(functionPointer);
		var result = MethodCompiler.VirtualRegisters.Allocate(stacktype);

		var move = GetMoveInstruction(ElementType.I);

		context.AppendInstruction(move, result, Operand.CreateLabel(method, Is32BitPlatform));

		PushStack(stack, new StackEntry(result));

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

		var stacktype = MethodCompiler.GetPrimitiveType(elementType);
		var result = MethodCompiler.VirtualRegisters.Allocate(stacktype);

		PushStack(stack, new StackEntry(result));

		var loadInstruction = GetLoadInstruction(elementType);
		context.AppendInstruction(loadInstruction, result, entry.Operand, ConstantZero);

		return true;
	}

	private bool Ldlen(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);

		if (entry.PrimitiveType == PrimitiveType.Object)
		{
			if (Is32BitPlatform)
			{
				var result = MethodCompiler.VirtualRegisters.Allocate32();
				context.AppendInstruction(IRInstruction.Load32, result, entry.Operand, ConstantZero);
				PushStack(stack, new StackEntry(result));
				return true;
			}
			else
			{
				var result = MethodCompiler.VirtualRegisters.Allocate64();
				context.AppendInstruction(IRInstruction.Load64, result, entry.Operand, ConstantZero);
				PushStack(stack, new StackEntry(result));
				return true;
			}
		}

		return false;
	}

	private bool Ldloc(Context context, Stack<StackEntry> stack, int index)
	{
		var local = LocalStack[index];

		if (local.Primitive == PrimitiveType.ValueType)
		{
			var result2 = MethodCompiler.LocalStack.Allocate(local);
			context.AppendInstruction(IRInstruction.MoveCompound, result2, local);
			PushStack(stack, new StackEntry(result2));
			return true;
		}

		var stacktype = LocalPrimitiveType[index];
		var result = MethodCompiler.VirtualRegisters.Allocate(stacktype);

		PushStack(stack, new StackEntry(result));

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

		var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();

		PushStack(stack, new StackEntry(result));

		context.AppendInstruction(IRInstruction.AddressOf, result, local);

		return true;
	}

	private bool Ldobj(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry = PopStack(stack);
		var address = entry.Operand;
		var type = (MosaType)instruction.Operand;

		var underlyingType = MosaTypeLayout.GetUnderlyingType(type);
		var isPrimitive = MosaTypeLayout.IsPrimitive(underlyingType);

		if (isPrimitive)
		{
			var elementType = MethodCompiler.GetElementType(underlyingType);
			var stacktype = MethodCompiler.GetPrimitiveType(elementType);
			var result = MethodCompiler.VirtualRegisters.Allocate(stacktype);

			PushStack(stack, new StackEntry(result));

			var loadInstruction = GetLoadInstruction(elementType);
			context.AppendInstruction(loadInstruction, result, address, ConstantZero);

			return true; // Ldind(context, stack, elementType);
		}
		else
		{
			var result = MethodCompiler.LocalStack.Allocate(PrimitiveType.ValueType, false, type);

			context.AppendInstruction(IRInstruction.LoadCompound, result, address, ConstantZero);
			PushStack(stack, new StackEntry(result));
			return true;
		}
	}

	private bool Ldsfld(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var field = (MosaField)instruction.Operand;
		var type = field.FieldType;

		var underlyingType = MosaTypeLayout.GetUnderlyingType(type);
		var isPrimitive = MosaTypeLayout.IsPrimitive(underlyingType);

		var fieldOperand = Operand.CreateStaticField(
			underlyingType.IsReferenceType
				? PrimitiveType.Object
				: PrimitiveType.ManagedPointer,
			field);

		if (isPrimitive)
		{
			var elementType = MethodCompiler.GetElementType(underlyingType);
			var stacktype = MethodCompiler.GetPrimitiveType(elementType);
			var result = MethodCompiler.VirtualRegisters.Allocate(stacktype);

			PushStack(stack, new StackEntry(result));

			if (type.IsReferenceType)
			{
				var symbol = GetStaticSymbol(field);
				var staticReference = Operand.CreateLabel(symbol.Name, Is32BitPlatform);

				context.AppendInstruction(IRInstruction.LoadObject, result, staticReference, ConstantZero);
			}
			else
			{
				var loadInstruction = GetLoadInstruction(elementType);

				context.AppendInstruction(loadInstruction, result, fieldOperand, ConstantZero);
			}
		}
		else
		{
			var result = MethodCompiler.LocalStack.Allocate(PrimitiveType.ValueType, false, type);
			context.AppendInstruction(IRInstruction.LoadCompound, result, fieldOperand, ConstantZero);
			PushStack(stack, new StackEntry(result));
		}

		return true;
	}

	private bool Ldsflda(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var field = (MosaField)instruction.Operand;

		var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
		var fieldOperand = Operand.CreateStaticField(PrimitiveType.ManagedPointer, field);

		context.AppendInstruction(IRInstruction.AddressOf, result, fieldOperand);

		PushStack(stack, new StackEntry(result));   // FIXME: transient pointer or unmanaged pointer

		MethodScanner.AccessedField(field);

		return true;
	}

	private bool Ldstr(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var result = MethodCompiler.VirtualRegisters.AllocateObject();

		var token = (uint)instruction.Operand;

		var stringdata = TypeSystem.LookupUserString(Method.Module, token);
		var symbolName = EmitString(stringdata, token);

		var symbol = Operand.CreateStringLabel(symbolName, MethodCompiler.Compiler.ObjectHeaderSize, stringdata);

		context.AppendInstruction(IRInstruction.MoveObject, result, symbol);

		PushStack(stack, new StackEntry(result));

		return true;
	}

	private bool Mul(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.R4 when entry2.PrimitiveType == PrimitiveType.R4 && entry1.Operand.IsR4:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateR4();
					context.AppendInstruction(IRInstruction.MulR4, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.R8 when entry2.PrimitiveType == PrimitiveType.R8 && entry1.Operand.IsR8:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateR8();
					context.AppendInstruction(IRInstruction.MulR8, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.MulSigned32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.MulSigned64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.MulSigned64, result, entry2.Operand, v1);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.MulSigned64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			default: return false;
		}
	}

	private bool MulSigned(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					var result2 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction2(IRInstruction.MulOverflowOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction2(IRInstruction.MulOverflowOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.MulOverflowOut64, result, result2, entry2.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.MulOverflowOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			default: return false;
		}
	}

	private bool MulUnsigned(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					var result2 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction2(IRInstruction.MulCarryOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction2(IRInstruction.MulCarryOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.MulCarryOut64, result, result2, entry2.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.MulCarryOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			default: return false;
		}
	}

	private bool Neg(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.R4:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateR4();
					var zero = CreateConstantR4(0);
					context.AppendInstruction(IRInstruction.SubR4, result, zero, entry.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.R8:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateR8();
					var zero = CreateConstantR8(0);
					context.AppendInstruction(IRInstruction.SubR8, result, zero, entry.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.Sub32, result, Constant32_0, entry.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.Sub64, result, Constant64_0, entry.Operand);
					PushStack(stack, new StackEntry(result));
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

		var elementSize = MethodCompiler.GetSize(arrayType);
		var methodTable = GetMethodTablePointer(arrayType);
		var size = CreateConstant32(elementSize);
		var result = MethodCompiler.VirtualRegisters.AllocateObject();

		context.AppendInstruction(IRInstruction.NewArray, result, methodTable, size, elements.Operand);
		context.MosaType = arrayType;

		PushStack(stack, new StackEntry(result));

		return true;
	}

	private bool Newobj(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var method = (MosaMethod)instruction.Operand;
		var classType = method.DeclaringType;
		var paramCount = method.Signature.Parameters.Count;

		var underlyingType = MosaTypeLayout.GetUnderlyingType(classType);
		var elementType = MethodCompiler.GetElementType(underlyingType);
		var primitiveType = MethodCompiler.GetPrimitiveType(elementType);

		var symbol = Operand.CreateLabel(method, Is32BitPlatform);

		var operands = new List<Operand>();

		for (var i = 0; i < paramCount; i++)
		{
			var param = PopStack(stack);
			operands.Add(param.Operand);
		}

		operands.Reverse();

		// FUTURE: Replace with new stage
		if (ProcessInternalCall(method, context, operands, stack))
			return true;

		MethodScanner.TypeAllocated(classType, Method);

		if (primitiveType == PrimitiveType.Object)
		{
			var result = MethodCompiler.VirtualRegisters.AllocateObject();

			var methodTable = GetMethodTablePointer(classType);
			var size = CreateConstant32(TypeLayout.GetTypeSize(classType));

			context.AppendInstruction(IRInstruction.NewObject, result, methodTable, size);
			operands.Insert(0, result);

			context.AppendInstruction(IRInstruction.CallStatic, null, symbol, operands);

			PushStack(stack, new StackEntry(result));

			return true;
		}
		else if (primitiveType == PrimitiveType.ValueType)
		{
			var newThisLocal = MethodCompiler.LocalStack.Allocate(PrimitiveType.ValueType, false, classType);
			var newThis = MethodCompiler.VirtualRegisters.AllocateManagedPointer();

			context.AppendInstruction(IRInstruction.AddressOf, newThis, newThisLocal);
			operands.Insert(0, newThis);

			context.AppendInstruction(IRInstruction.CallStatic, null, symbol, operands);

			PushStack(stack, new StackEntry(newThisLocal));

			return true;
		}
		else if (primitiveType == PrimitiveType.Int32 || primitiveType == PrimitiveType.Int64)
		{
			var result = MethodCompiler.VirtualRegisters.Allocate(primitiveType);
			var newThisLocal = MethodCompiler.LocalStack.Allocate(primitiveType);
			var newThis = MethodCompiler.VirtualRegisters.AllocateManagedPointer();

			var loadInstruction = GetLoadInstruction(elementType);

			context.AppendInstruction(IRInstruction.AddressOf, newThis, newThisLocal);
			operands.Insert(0, newThis);

			context.AppendInstruction(IRInstruction.CallStatic, null, symbol, operands);
			context.AppendInstruction(loadInstruction, result, StackFrame, newThisLocal);

			PushStack(stack, new StackEntry(result));

			return true;
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

		switch (entry.PrimitiveType)
		{
			case PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.Not32, result, entry.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.Not64, result, entry.Operand);
					PushStack(stack, new StackEntry(result));
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

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.Or32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.Or64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.Or64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.Or64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(result));
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

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.R4 when entry2.PrimitiveType == PrimitiveType.R4 && entry1.Operand.IsR4:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateR4();
					context.AppendInstruction(IRInstruction.RemR4, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.R8 when entry2.PrimitiveType == PrimitiveType.R8 && entry1.Operand.IsR8:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateR8();
					context.AppendInstruction(IRInstruction.RemR8, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.RemSigned32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.RemSigned64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.RemSigned64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.RemSigned64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(result));
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

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.R4 when entry2.PrimitiveType == PrimitiveType.R4:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateR4();
					context.AppendInstruction(IRInstruction.RemR4, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.R8 when entry2.PrimitiveType == PrimitiveType.R8:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateR8();
					context.AppendInstruction(IRInstruction.RemR8, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.RemUnsigned32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.RemUnsigned64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.RemUnsigned64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.RemUnsigned64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(result));
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

			switch (entry.PrimitiveType)
			{
				case PrimitiveType.Int32:
					context.AppendInstruction(IRInstruction.SetReturn32, null, entry.Operand);
					break;

				case PrimitiveType.Int64:
					context.AppendInstruction(IRInstruction.SetReturn64, null, entry.Operand);
					break;

				case PrimitiveType.R4:
					context.AppendInstruction(IRInstruction.SetReturnR4, null, entry.Operand);
					break;

				case PrimitiveType.R8:
					context.AppendInstruction(IRInstruction.SetReturnR8, null, entry.Operand);
					break;

				case PrimitiveType.Object:
					context.AppendInstruction(IRInstruction.SetReturnObject, null, entry.Operand);
					break;

				case PrimitiveType.ValueType:
					context.AppendInstruction(IRInstruction.SetReturnCompound, null, entry.Operand);
					//context.MosaType = entry.Type;
					break;

				case PrimitiveType.ManagedPointer when Is32BitPlatform:
					context.AppendInstruction(IRInstruction.SetReturn32, null, entry.Operand);
					break;

				case PrimitiveType.ManagedPointer when Is64BitPlatform:
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

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.ShiftLeft32, result, value, shift);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ShiftLeft64, result, value, shift);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ShiftLeft64, result, value, shift);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ShiftLeft64, result, value, shift);
					PushStack(stack, new StackEntry(result));
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

		switch (entry2.PrimitiveType)
		{
			case PrimitiveType.Int32 when entry1.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.ArithShiftRight32, result, value, shiftAmount);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry1.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ArithShiftRight64, result, value, shiftAmount);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry1.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ArithShiftRight64, result, value, shiftAmount);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry1.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ArithShiftRight64, result, value, shiftAmount);
					PushStack(stack, new StackEntry(result));
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

		switch (entry2.PrimitiveType)
		{
			case PrimitiveType.Int32 when entry1.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.ShiftRight32, result, value, shiftAmount);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry1.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ShiftRight32, result, value, shiftAmount);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry1.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.ShiftRight64, result, value, shiftAmount);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry1.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ShiftRight64, result, value, shiftAmount);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			default: return false;
		}
	}

	private bool Sizeof(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var type = (MosaType)instruction.Operand;
		var result = MethodCompiler.VirtualRegisters.Allocate32();

		var size = type.IsPointer ? NativePointerSize : MethodCompiler.TypeLayout.GetTypeSize(type);

		context.AppendInstruction(IRInstruction.Move32, result, CreateConstant32(size));

		PushStack(stack, new StackEntry(result));

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

		var underlyingType = MosaTypeLayout.GetUnderlyingType(type);
		var isPrimitive = MosaTypeLayout.IsPrimitive(underlyingType);

		context.AppendInstruction(IRInstruction.CheckArrayBounds, null, array, index);

		var elementOffset = CalculateArrayElementOffset(context, type, index);
		var totalElementOffset = CalculateTotalArrayOffset(context, elementOffset);

		if (isPrimitive)
		{
			//var stacktype = GetStackType(underlyingType);
			var elementType = MethodCompiler.GetElementType(underlyingType);
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

		var elementOffset = CalculateArrayElementOffset(context, MethodCompiler.GetSize(elementType), index);
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

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.Int32:
			case PrimitiveType.Int64:
			case PrimitiveType.R4:
			case PrimitiveType.R8:
			case PrimitiveType.ManagedPointer:
			case PrimitiveType.Object:
				{
					var underlyingType = MosaTypeLayout.GetUnderlyingType(type);
					var isPrimitive = MosaTypeLayout.IsPrimitive(underlyingType);

					if (isPrimitive)
					{
						var elementType = MethodCompiler.GetElementType(underlyingType);
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
			case PrimitiveType.ValueType:
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

		var stackType = LocalPrimitiveType[index];
		var local = LocalStack[index];

		if (stackType == PrimitiveType.ValueType)
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
			//var stackType = GetStackType(underlyingType);
			var stackElementType = MethodCompiler.GetElementType(stackType);

			var storeInstruction = GetStoreInstruction(stackElementType);

			context.AppendInstruction(storeInstruction, null, StackFrame, local, source);
			return true;
		}
	}

	private bool Stobj(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		var address = entry2.Operand;
		var value = entry1.Operand;

		var type = (MosaType)instruction.Operand;
		var underlyingType = MosaTypeLayout.GetUnderlyingType(type);
		var isPrimitive = MosaTypeLayout.IsPrimitive(underlyingType);

		if (isPrimitive)
		{
			var stackType = MethodCompiler.GetPrimitiveType(underlyingType);
			var elementType = MethodCompiler.GetElementType(stackType);
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

		var underlyingType = MosaTypeLayout.GetUnderlyingType(type);
		var isPrimitive = MosaTypeLayout.IsPrimitive(underlyingType);

		var fieldOperand = Operand.CreateStaticField(
			type.IsReferenceType
				? PrimitiveType.Object
				: PrimitiveType.ManagedPointer,
			field);

		if (isPrimitive)
		{
			var elementType = MethodCompiler.GetElementType(underlyingType);

			var storeInstruction = GetStoreInstruction(elementType);

			if (type.IsReferenceType)
			{
				var symbol = GetStaticSymbol(field);
				var staticReference = Operand.CreateLabel(symbol.Name, Is32BitPlatform);

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
		var isPrimitive = parameter.IsPrimitive;

		if (isPrimitive)
		{
			var storeInstruction = GetStoreParamInstruction(parameter.Element);
			context.AppendInstruction(storeInstruction, null, parameter, value);
			return true;
		}
		else
		{
			context.AppendInstruction(IRInstruction.StoreParamCompound, null, parameter, value);
			context.MosaType = parameter.Type;
			return true;
		}
	}

	private bool Sub(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.R4 when entry2.PrimitiveType == PrimitiveType.R4:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateR4();
					context.AppendInstruction(IRInstruction.SubR4, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.R8 when entry2.PrimitiveType == PrimitiveType.R8:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateR8();
					context.AppendInstruction(IRInstruction.SubR8, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.Sub32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.Sub64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.Sub64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.Sub64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.ManagedPointer when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is32BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.Sub32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.ManagedPointer when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.Sub64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is32BitPlatform:
			case PrimitiveType.ManagedPointer when entry2.PrimitiveType == PrimitiveType.Int32 && Is32BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					context.AppendInstruction(IRInstruction.Sub32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					context.AppendInstruction(IRInstruction.Sub64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.Sub64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.ManagedPointer when entry2.PrimitiveType == PrimitiveType.Int32 && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.Sub64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			default: return false;
		}
	}

	private bool SubSigned(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					var result2 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction2(IRInstruction.SubOverflowOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction2(IRInstruction.SubOverflowOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.SubOverflowOut64, result, result2, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.SubOverflowOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.ManagedPointer when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is32BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					var result2 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction2(IRInstruction.SubOverflowOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.ManagedPointer when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction2(IRInstruction.SubOverflowOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is32BitPlatform:
			case PrimitiveType.ManagedPointer when entry2.PrimitiveType == PrimitiveType.Int32 && Is32BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var result2 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction2(IRInstruction.SubOverflowOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction2(IRInstruction.SubOverflowOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.SubOverflowOut64, result, result2, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.ManagedPointer when entry2.PrimitiveType == PrimitiveType.Int32 && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.SignExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.SubOverflowOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			default: return false;
		}
	}

	private bool SubUnsigned(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					var result2 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction2(IRInstruction.SubCarryOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction2(IRInstruction.SubCarryOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.SubCarryOut64, result, result2, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.SubCarryOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.ManagedPointer when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is32BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					var result2 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction2(IRInstruction.SubCarryOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.ManagedPointer when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction2(IRInstruction.SubCarryOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is32BitPlatform:
			case PrimitiveType.ManagedPointer when entry2.PrimitiveType == PrimitiveType.Int32 && Is32BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var result2 = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction2(IRInstruction.SubCarryOut32, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction2(IRInstruction.SubCarryOut64, result, result2, entry2.Operand, entry1.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.ManagedPointer && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
					context.AppendInstruction2(IRInstruction.SubCarryOut64, result, result2, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.ManagedPointer when entry2.PrimitiveType == PrimitiveType.Int32 && Is64BitPlatform:
				{
					var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
					var result2 = MethodCompiler.VirtualRegisters.Allocate64();
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
					context.AppendInstruction2(IRInstruction.SubCarryOut64, result, result2, entry1.Operand, v1);
					context.AppendInstruction(IRInstruction.CheckThrowOverflow, null, result2);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			default: return false;
		}
	}

	private bool Throw(Context context, Stack<StackEntry> stack)
	{
		var entry = PopStack(stack);

		if (entry.PrimitiveType == PrimitiveType.Object)
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

		var result = MethodCompiler.VirtualRegisters.AllocateManagedPointer();
		PushStack(stack, new StackEntry(result));

		context.AppendInstruction(IRInstruction.Unbox, result, entry.Operand);

		return true;
	}

	private bool UnboxAny(Context context, Stack<StackEntry> stack, MosaInstruction instruction)
	{
		var type = (MosaType)instruction.Operand;

		if (type.IsReferenceType)
		{
			// treat as castclass, per spec
			return Castclass(context, stack);
		}

		Debug.Assert(type.IsValueType);

		var entry = PopStack(stack);

		//equivalent to unbox followed by ldobj

		var underlyingType = MosaTypeLayout.GetUnderlyingType(type);
		var isPrimitive = MosaTypeLayout.IsPrimitive(underlyingType);

		if (isPrimitive)
		{
			var elementType = MethodCompiler.GetElementType(underlyingType);

			var loadInstruction = GetLoadInstruction(elementType);
			var stackType = MethodCompiler.GetPrimitiveType(elementType);
			var result = MethodCompiler.VirtualRegisters.Allocate(stackType);

			PushStack(stack, new StackEntry(result));

			context.AppendInstruction(loadInstruction, result, entry.Operand, ConstantZero);

			return true;
		}
		else
		{
			var result = MethodCompiler.LocalStack.Allocate(PrimitiveType.ValueType, false, type);
			var address = MethodCompiler.VirtualRegisters.Allocate(PrimitiveType.ManagedPointer);

			context.AppendInstruction(MoveInstruction, address, entry.Operand);
			// FUTURE: Add type check
			context.AppendInstruction(IRInstruction.LoadCompound, result, address, ConstantZero);

			PushStack(stack, new StackEntry(result));
			return true;
		}
	}

	private bool Xor(Context context, Stack<StackEntry> stack)
	{
		var entry1 = PopStack(stack);
		var entry2 = PopStack(stack);

		switch (entry1.PrimitiveType)
		{
			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate32();
					context.AppendInstruction(IRInstruction.Xor32, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.Xor64, result, entry2.Operand, entry1.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int32 when entry2.PrimitiveType == PrimitiveType.Int64:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry1.Operand);
					context.AppendInstruction(IRInstruction.Xor64, result, v1, entry2.Operand);
					PushStack(stack, new StackEntry(result));
					return true;
				}

			case PrimitiveType.Int64 when entry2.PrimitiveType == PrimitiveType.Int32:
				{
					var v1 = MethodCompiler.VirtualRegisters.Allocate64();
					var result = MethodCompiler.VirtualRegisters.Allocate64();
					context.AppendInstruction(IRInstruction.ZeroExtend32x64, v1, entry2.Operand);
					context.AppendInstruction(IRInstruction.Xor64, result, entry1.Operand, v1);
					PushStack(stack, new StackEntry(result));
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

		var result = MethodCompiler.VirtualRegisters.AllocateObject();
		var symbol = Operand.CreateLabel(newmethod, Is32BitPlatform);

		context.AppendInstruction(IRInstruction.CallStatic, result, symbol);
		context.AppendOperands(operands);

		PushStack(stack, new StackEntry(result));

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
			var operand1 = Operand.CreateLabel(context.InvokeMethod, Is32BitPlatform);
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
		var size = MethodCompiler.GetSize(elementType);

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
			var elementOffset = MethodCompiler.VirtualRegisters.Allocate32();
			var elementSize = CreateConstant32(size);

			context.AppendInstruction(IRInstruction.MulUnsigned32, elementOffset, index, elementSize);

			return elementOffset;
		}
		else
		{
			var elementOffset = MethodCompiler.VirtualRegisters.Allocate64();
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
		var arrayElement = Is32BitPlatform ?
			MethodCompiler.VirtualRegisters.Allocate32()
			: MethodCompiler.VirtualRegisters.Allocate64();

		if (Is32BitPlatform)
			context.AppendInstruction(IRInstruction.Add32, arrayElement, elementOffset, fixedOffset);
		else
			context.AppendInstruction(IRInstruction.Add64, arrayElement, elementOffset, fixedOffset);

		return arrayElement;
	}

	#endregion Array Helpers
}
