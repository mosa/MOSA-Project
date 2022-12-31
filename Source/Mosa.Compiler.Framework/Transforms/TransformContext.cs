﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Transforms
{
	public sealed class TransformContext
	{
		public MethodCompiler MethodCompiler { get; private set; }

		public Compiler Compiler { get; private set; }

		public BitValueManager BitValueManager { get; private set; }

		public TypeSystem TypeSystem { get; private set; }

		public TraceLog TraceLog { get; private set; }

		public TraceLog SpecialTraceLog { get; private set; }

		public Operand Constant32_0 { get; private set; }

		public Operand Constant64_0 { get; private set; }

		public Operand ConstantR4_0 { get; private set; }

		public Operand ConstantR8_0 { get; private set; }

		public Operand Constant32_1 { get; private set; }

		public Operand Constant32_2 { get; private set; }

		public Operand Constant32_3 { get; private set; }

		public Operand Constant32_4 { get; private set; }

		public Operand Constant32_16 { get; private set; }

		public Operand Constant32_24 { get; private set; }

		public Operand Constant32_31 { get; private set; }

		public Operand Constant32_32 { get; private set; }

		public Operand Constant32_64 { get; private set; }

		public Operand Constant64_32 { get; private set; }

		public MosaType I4 { get; private set; }

		public MosaType I8 { get; private set; }

		public MosaType R4 { get; private set; }

		public MosaType R8 { get; private set; }

		public MosaType O { get; private set; }

		public VirtualRegisters VirtualRegisters { get; private set; }

		public BasicBlocks BasicBlocks { get; set; }

		public bool LowerTo32 { get; private set; }

		public bool IsInSSAForm { get; private set; }

		public bool AreCPURegistersAllocated { get; private set; }

		public bool Is32BitPlatform { get; private set; }

		public int Window { get; private set; }

		public Operand StackFrame
		{ get { return MethodCompiler.Compiler.StackFrame; } }

		public Operand StackPointer
		{ get { return MethodCompiler.Compiler.StackPointer; } }

		/// <summary>
		/// Gets the link register.
		/// </summary>
		public Operand LinkRegister
		{ get { return MethodCompiler.Compiler.LinkRegister; } }

		/// <summary>
		/// Gets the program counter
		/// </summary>
		public Operand ProgramCounter
		{ get { return MethodCompiler.Compiler.ProgramCounter; } }

		/// <summary>
		/// Gets the exception register.
		/// </summary>
		public Operand ExceptionRegister
		{ get { return MethodCompiler.Compiler.ExceptionRegister; } }

		/// <summary>
		/// Gets the leave target register.
		/// </summary>
		public Operand LeaveTargetRegister
		{ get { return MethodCompiler.Compiler.LeaveTargetRegister; } }

		public uint NativePointerSize
		{ get { return Compiler.Architecture.NativePointerSize; } }

		public BaseArchitecture Architecture
		{ get { return Compiler.Architecture; } }

		public MosaLinker Linker
		{ get { return Compiler.Linker; } }

		public TransformContext(MethodCompiler methodCompiler, BitValueManager bitValueManager = null)
		{
			MethodCompiler = methodCompiler;
			Compiler = methodCompiler.Compiler;
			BitValueManager = bitValueManager;

			TypeSystem = Compiler.TypeSystem;

			VirtualRegisters = MethodCompiler.VirtualRegisters;
			BasicBlocks = methodCompiler.BasicBlocks;

			I4 = TypeSystem.BuiltIn.I4;
			I8 = TypeSystem.BuiltIn.I8;
			R4 = TypeSystem.BuiltIn.R4;
			R8 = TypeSystem.BuiltIn.R8;
			O = TypeSystem.BuiltIn.Object;

			ConstantR4_0 = MethodCompiler.ConstantZeroR4;
			ConstantR8_0 = MethodCompiler.ConstantZeroR8;
			Constant32_0 = MethodCompiler.ConstantZero32;
			Constant64_0 = MethodCompiler.ConstantZero64;

			Constant32_1 = CreateConstant32(1);
			Constant32_2 = CreateConstant32(2);
			Constant32_3 = CreateConstant32(3);
			Constant32_4 = CreateConstant32(4);
			Constant32_16 = CreateConstant32(16);
			Constant32_31 = CreateConstant32(31);
			Constant32_32 = CreateConstant32(32);
			Constant32_64 = CreateConstant32(64);
			Constant32_24 = CreateConstant32(24);

			Constant64_32 = CreateConstant64(32);

			Is32BitPlatform = Compiler.Architecture.Is32BitPlatform;
			LowerTo32 = Compiler.CompilerSettings.LongExpansion;

			IsInSSAForm = MethodCompiler.IsInSSAForm;
			AreCPURegistersAllocated = MethodCompiler.AreCPURegistersAllocated;

			Window = Math.Max(Compiler.CompilerSettings.OptimizationWindow, 1);
		}

		public void SetLog(TraceLog traceLog)
		{
			TraceLog = traceLog;
		}

		public void SetLogs(TraceLog traceLog = null, TraceLog specialTraceLog = null)
		{
			TraceLog = traceLog;
			SpecialTraceLog = specialTraceLog;
		}

		public void SetStageOptions(bool lowerTo32)
		{
			LowerTo32 = Compiler.CompilerSettings.LongExpansion && lowerTo32;
		}

		public Operand AllocateVirtualRegister(MosaType type)
		{
			return VirtualRegisters.Allocate(type);
		}

		public Operand AllocateVirtualRegister32()
		{
			return VirtualRegisters.Allocate(I4);
		}

		public Operand AllocateVirtualRegister64()
		{
			return VirtualRegisters.Allocate(I8);
		}

		public Operand AllocateVirtualRegisterR4()
		{
			return VirtualRegisters.Allocate(R4);
		}

		public Operand AllocateVirtualRegisterR8()
		{
			return VirtualRegisters.Allocate(R8);
		}

		public Operand AllocateVirtualRegisterObject()
		{
			return VirtualRegisters.Allocate(O);
		}

		public bool ApplyTransform(Context context, BaseTransform transformation)
		{
			if (!transformation.Match(context, this))
				return false;

			TraceBefore(context, transformation);

			transformation.Transform(context, this);

			TraceAfter(context);

			return true;
		}

		#region Trace

		public void TraceBefore(Context context, BaseTransform transformation)
		{
			if (transformation.Name != null)
				TraceLog?.Log($"*** {transformation.Name}");

			if (transformation.Log)
				SpecialTraceLog?.Log($"{transformation.Name}\t{MethodCompiler.Method.FullName} at {context}");

			TraceLog?.Log($"BEFORE:\t{context}");
		}

		public void TraceAfter(Context context)
		{
			TraceLog?.Log($"AFTER: \t{context}");
		}

		#endregion Trace

		#region Constant Helper Methods

		public Operand CreateConstant(int value)
		{
			return value == 0 ? Constant32_0 : Operand.CreateConstant(I4, value);
		}

		public Operand CreateConstant32(int value)
		{
			return value == 0 ? Constant32_0 : Operand.CreateConstant(I4, value);
		}

		public Operand CreateConstant32(long value)
		{
			return (int)value == 0 ? Constant32_0 : Operand.CreateConstant(I4, (int)value);
		}

		public Operand CreateConstant(uint value)
		{
			return value == 0 ? Constant32_0 : Operand.CreateConstant(I4, value);
		}

		public Operand CreateConstant32(uint value)
		{
			return value == 0 ? Constant32_0 : Operand.CreateConstant(I4, value);
		}

		public Operand CreateConstant(long value)
		{
			return value == 0 ? Constant64_0 : Operand.CreateConstant(I8, value);
		}

		public Operand CreateConstant64(long value)
		{
			return value == 0 ? Constant64_0 : Operand.CreateConstant(I8, value);
		}

		public Operand CreateConstant(ulong value)
		{
			return value == 0 ? Constant64_0 : Operand.CreateConstant(I8, value);
		}

		public Operand CreateConstant64(ulong value)
		{
			return value == 0 ? Constant64_0 : Operand.CreateConstant(I8, value);
		}

		public Operand CreateConstant(float value)
		{
			return value == 0 ? ConstantR4_0 : Operand.CreateConstant(R4, value);
		}

		public Operand CreateConstant(double value)
		{
			return value == 0 ? ConstantR4_0 : Operand.CreateConstant(R8, value);
		}

		#endregion Constant Helper Methods

		#region Basic Block Helpers

		/// <summary>
		/// Splits the block.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public Context Split(Context context)
		{
			return new Context(Split(context.Node));
		}

		/// <summary>
		/// Splits the block.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <returns></returns>
		public BasicBlock Split(InstructionNode node)
		{
			var newblock = CreateNewBlock(-1, node.Label);

			node.Split(newblock);

			return newblock;
		}

		/// <summary>
		/// Creates the new block.
		/// </summary>
		/// <param name="blockLabel">The label.</param>
		/// <param name="instructionLabel">The instruction label.</param>
		/// <returns></returns>
		private BasicBlock CreateNewBlock(int blockLabel, int instructionLabel)
		{
			return BasicBlocks.CreateBlock(blockLabel, instructionLabel);
		}

		/// <summary>
		/// Creates empty blocks.
		/// </summary>
		/// <param name="blocks">The Blocks.</param>
		/// <param name="instructionLabel">The instruction label.</param>
		/// <returns></returns>
		public Context[] CreateNewBlockContexts(int blocks, int instructionLabel)
		{
			// Allocate the context array
			var result = new Context[blocks];

			for (int index = 0; index < blocks; index++)
			{
				result[index] = CreateNewBlockContext(instructionLabel);
			}

			return result;
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <param name="instructionLabel">The instruction label.</param>
		/// <returns></returns>
		private Context CreateNewBlockContext(int instructionLabel)
		{
			return new Context(CreateNewBlock(-1, instructionLabel));
		}

		#endregion Basic Block Helpers

		#region Phi Helpers

		public static void UpdatePhiTarget(BasicBlock target, BasicBlock source, BasicBlock newSource)
		{
			BaseMethodCompilerStage.UpdatePhiTarget(target, source, newSource);
		}

		public static void UpdatePhiTargets(List<BasicBlock> targets, BasicBlock source, BasicBlock newSource)
		{
			BaseMethodCompilerStage.UpdatePhiTargets(targets, source, newSource);
		}

		public static void UpdatePhiBlocks(List<BasicBlock> phiBlocks)
		{
			BaseMethodCompilerStage.UpdatePhiBlocks(phiBlocks);
		}

		public static void UpdatePhiBlock(BasicBlock phiBlock)
		{
			BaseMethodCompilerStage.UpdatePhiBlock(phiBlock);
		}

		public static void UpdatePhi(InstructionNode node)
		{
			BaseMethodCompilerStage.UpdatePhi(node);
		}

		public static void UpdatePhi(Context context)
		{
			BaseMethodCompilerStage.UpdatePhi(context);
		}

		#endregion Phi Helpers

		#region Move Helpers

		public void MoveOperand1ToVirtualRegister(Context context, BaseInstruction moveInstruction)
		{
			Debug.Assert(!context.Operand1.IsVirtualRegister);

			var operand1 = context.Operand1;

			var v1 = AllocateVirtualRegister(operand1.Type);

			context.InsertBefore().AppendInstruction(moveInstruction, v1, operand1);
			context.Operand1 = v1;
		}

		public void MoveOperand2ToVirtualRegister(Context context, BaseInstruction moveInstruction)
		{
			Debug.Assert(!context.Operand2.IsVirtualRegister);

			var operand2 = context.Operand2;

			var v1 = AllocateVirtualRegister(operand2.Type);

			context.InsertBefore().AppendInstruction(moveInstruction, v1, operand2);
			context.Operand2 = v1;
		}

		public void MoveOperand1And2ToVirtualRegisters(Context context, BaseInstruction moveInstruction)
		{
			Debug.Assert(!context.Operand1.IsVirtualRegister || !context.Operand2.IsVirtualRegister);

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			if (operand1.IsConstant && operand2.IsConstant && operand1.ConstantUnsigned64 == operand2.ConstantUnsigned64)
			{
				var v1 = AllocateVirtualRegister(operand1.Type);

				context.InsertBefore().AppendInstruction(moveInstruction, v1, operand1);
				context.Operand1 = v1;
				context.Operand2 = v1;
				return;
			}
			else if (operand1.IsConstant && operand2.IsConstant)
			{
				var v1 = AllocateVirtualRegister(operand1.Type);
				var v2 = AllocateVirtualRegister(operand2.Type);

				context.InsertBefore().AppendInstruction(moveInstruction, v1, operand1);
				context.InsertBefore().AppendInstruction(moveInstruction, v2, operand2);
				context.Operand1 = v1;
				context.Operand2 = v2;
				return;
			}
			else if (operand1.IsConstant)
			{
				var v1 = AllocateVirtualRegister(operand1.Type);

				context.InsertBefore().AppendInstruction(moveInstruction, v1, operand1);
				context.Operand1 = v1;
			}
			else if (operand2.IsConstant)
			{
				var v1 = AllocateVirtualRegister(operand2.Type);

				context.InsertBefore().AppendInstruction(moveInstruction, v1, operand2);
				context.Operand2 = v1;
			}
		}

		#endregion Move Helpers

		#region Linker Helpers

		public Operand CreateR4Label(float value)
		{
			var symbol = Linker.GetConstantSymbol(value);

			var label = Operand.CreateLabel(R4, symbol.Name);

			return label;
		}

		public Operand CreateR8Label(double value)
		{
			var symbol = Linker.GetConstantSymbol(value);

			var label = Operand.CreateLabel(R4, symbol.Name);

			return label;
		}

		public Operand CreateFloatingPointLabel(Operand operand)
		{
			var symbol = operand.IsR4 ? Linker.GetConstantSymbol(operand.ConstantFloat) : Compiler.Linker.GetConstantSymbol(operand.ConstantDouble);

			var label = Operand.CreateLabel(operand.IsR4 ? R4 : R8, symbol.Name);

			return label;
		}

		#endregion Linker Helpers

		public void SplitLongOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
		{
			MethodCompiler.SplitLongOperand(operand, out operandLow, out operandHigh);
		}

		#region BitValue (experimental)

		public BitValue GetBitValue(Operand operand)
		{
			return BitValueManager.GetBitValue(operand);
		}

		public BitValue GetBitValueWithDefault(Operand operand)
		{
			return BitValueManager.GetBitValueWithDefault(operand);
		}

		#endregion BitValue (experimental)

		public MosaMethod GetMethod(string namespaceName, string typeName, string methodName)
		{
			var type = TypeSystem.GetTypeByName(namespaceName, typeName);

			if (type == null)
				return null;

			var method = type.FindMethodByName(methodName);

			return method;
		}

		public void ReplaceWithCall(Context context, string namespaceName, string typeName, string methodName)
		{
			var method = GetMethod(namespaceName, typeName, methodName);

			Debug.Assert(method != null, $"Cannot find method: {methodName}");

			// FUTURE: throw compiler exception

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			if (context.OperandCount == 1)
			{
				context.SetInstruction(IRInstruction.CallStatic, context.Result, symbol, context.Operand1);
			}
			else if (context.OperandCount == 2)
			{
				context.SetInstruction(IRInstruction.CallStatic, context.Result, symbol, context.Operand1, context.Operand2);
			}
			else
			{
				// FUTURE: throw compiler exception
			}

			MethodCompiler.MethodScanner.MethodInvoked(method, MethodCompiler.Method);
		}

		public void MoveConstantRight(Context context) // static
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			if (operand2.IsConstant || operand1.IsVirtualRegister)
				return;

			// Move constant to the right
			context.Operand1 = operand2;
			context.Operand2 = operand1;
			context.ConditionCode = context.ConditionCode.GetReverse();
		}

		public void OrderLoadStoreOperands(Context context)  // FUTURE: Rename
		{
			if (context.Operand1.IsResolvedConstant && context.Operand2.IsResolvedConstant)
			{
				context.Operand1 = CreateConstant(context.Operand1.ConstantUnsigned64 + context.Operand2.ConstantUnsigned64);
				context.Operand2 = Constant32_0;
			}

			if (context.Operand1.IsConstant && !context.Operand2.IsConstant)
			{
				var operand1 = context.Operand1;
				var operand2 = context.Operand2;

				context.Operand2 = operand1;
				context.Operand1 = operand2;
			}
		}

		#region Floating Point Helpers

		public Operand LoadValueR4(Context context, float value, BaseInstruction loadInstruction)
		{
			var label = CreateR4Label(value);

			var v1 = AllocateVirtualRegisterR4();

			context.InsertBefore().SetInstruction(loadInstruction, v1, label, Constant32_0);

			return v1;
		}

		public Operand LoadValueR8(Context context, double value, BaseInstruction loadInstruction)
		{
			var label = CreateR8Label(value);

			var v1 = AllocateVirtualRegisterR8();

			context.InsertBefore().SetInstruction(loadInstruction, v1, label, Constant32_0);

			return v1;
		}

		public Operand MoveConstantToFloatRegister(Context context, Operand operand, BaseInstruction instructionR4, BaseInstruction instructionR8)
		{
			if (!operand.IsConstant)
				return operand;

			var label = CreateFloatingPointLabel(operand);

			var v1 = operand.IsR4 ? AllocateVirtualRegisterR4() : AllocateVirtualRegisterR8();

			var instruction = operand.IsR4 ? instructionR4 : instructionR8;

			context.InsertBefore().SetInstruction(instruction, v1, label, Constant32_0);

			return v1;
		}

		#endregion Floating Point Helpers
	}
}
