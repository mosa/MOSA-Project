// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

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

		public Operand ConstantZero32 { get; private set; }
		public Operand ConstantZero64 { get; private set; }
		public Operand ConstantZeroR4 { get; private set; }
		public Operand ConstantZeroR8 { get; private set; }

		public MosaType I4 { get; private set; }
		public MosaType I8 { get; private set; }
		public MosaType R4 { get; private set; }
		public MosaType R8 { get; private set; }
		public MosaType O { get; private set; }

		public VirtualRegisters VirtualRegisters { get; private set; }
		public BasicBlocks BasicBlocks { get; set; }

		public bool LowerTo32 { get; private set; }
		public bool IsInSSAForm { get; private set; }

		public bool Is32BitPlatform { get; private set; }

		public int Window { get; private set; }

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

			ConstantZero32 = MethodCompiler.ConstantZero32;
			ConstantZero64 = MethodCompiler.ConstantZero64;
			ConstantZeroR4 = MethodCompiler.ConstantZeroR4;
			ConstantZeroR8 = MethodCompiler.ConstantZeroR8;

			Is32BitPlatform = Compiler.Architecture.Is32BitPlatform;
			LowerTo32 = Compiler.CompilerSettings.LongExpansion;

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

		public void SetStageOptions(bool inSSAForm, bool lowerTo32)
		{
			LowerTo32 = Compiler.CompilerSettings.LongExpansion && lowerTo32;
			IsInSSAForm = inSSAForm;
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

		public bool ApplyTransform(Context context, BaseTransformation transformation)
		{
			if (!transformation.Match(context, this))
				return false;

			TraceBefore(context, transformation);

			transformation.Transform(context, this);

			TraceAfter(context);

			return true;
		}

		#region Trace

		public void TraceBefore(Context context, BaseTransformation transformation)
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
			return value == 0 ? ConstantZero32 : Operand.CreateConstant(I4, value);
		}

		public Operand CreateConstant32(int value)
		{
			return value == 0 ? ConstantZero32 : Operand.CreateConstant(I4, value);
		}

		public Operand CreateConstant(uint value)
		{
			return value == 0 ? ConstantZero32 : Operand.CreateConstant(I4, value);
		}

		public Operand CreateConstant32(uint value)
		{
			return value == 0 ? ConstantZero32 : Operand.CreateConstant(I4, value);
		}

		public Operand CreateConstant(long value)
		{
			return value == 0 ? ConstantZero64 : Operand.CreateConstant(I8, value);
		}

		public Operand CreateConstant64(long value)
		{
			return value == 0 ? ConstantZero64 : Operand.CreateConstant(I8, value);
		}

		public Operand CreateConstant(ulong value)
		{
			return value == 0 ? ConstantZero64 : Operand.CreateConstant(I8, value);
		}

		public Operand CreateConstant64(ulong value)
		{
			return value == 0 ? ConstantZero64 : Operand.CreateConstant(I8, value);
		}

		public Operand CreateConstant(float value)
		{
			return value == 0 ? ConstantZeroR4 : Operand.CreateConstant(R4, value);
		}

		public Operand CreateConstant(double value)
		{
			return value == 0 ? ConstantZeroR4 : Operand.CreateConstant(R8, value);
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
			var operand1 = context.Operand1;

			var v1 = AllocateVirtualRegister(operand1.Type);

			context.InsertBefore().AppendInstruction(moveInstruction, v1, operand1);
			context.Operand1 = v1;
		}

		public void MoveOperand2ToVirtualRegister(Context context, BaseInstruction moveInstruction)
		{
			var operand2 = context.Operand1;

			var v1 = AllocateVirtualRegister(operand2.Type);

			context.InsertBefore().AppendInstruction(moveInstruction, v1, operand2);
			context.Operand2 = v1;
		}

		public void MoveOperand1And2ToVirtualRegisters(Context context, BaseInstruction moveInstruction)
		{
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

		public void SplitLongOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
		{
			MethodCompiler.SplitLongOperand(operand, out operandLow, out operandHigh);
		}

		public BitValue GetBitValue(Operand operand)
		{
			return BitValueManager.GetBitValue(operand);
		}

		public BitValue GetBitValueWithDefault(Operand operand)
		{
			return BitValueManager.GetBitValueWithDefault(operand);
		}

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
	}
}
