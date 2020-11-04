// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Transform
{
	public sealed class TransformContext
	{
		public MethodCompiler MethodCompiler { get; private set; }
		public Compiler Compiler { get; private set; }

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

		public TransformContext(MethodCompiler methodCompiler)
		{
			MethodCompiler = methodCompiler;
			Compiler = methodCompiler.Compiler;

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

		public bool ApplyTransform(Context context, BaseTransformation transformation, List<Operand> virtualRegisters = null)
		{
			if (!transformation.Match(context, this))
				return false;

			TraceBefore(context, transformation);

			if (virtualRegisters != null)
			{
				CollectVirtualRegisters(context, virtualRegisters);
			}

			// TODO: note last virtual register #
			// TODO: note the node

			transformation.Transform(context, this);

			// TODO: add all NEW virtual register to the collection

			TraceAfter(context);

			return true;
		}

		#region WorkList

		private static void CollectVirtualRegisters(Context context, List<Operand> virtualRegisters)
		{
			if (context.Result != null)
			{
				virtualRegisters.AddIfNew(context.Result);
			}
			if (context.Result2 != null)
			{
				virtualRegisters.AddIfNew(context.Result2);
			}
			foreach (var operand in context.Operands)
			{
				virtualRegisters.AddIfNew(operand);
			}
		}

		#endregion WorkList

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

		public Operand CreateConstant(uint value)
		{
			return value == 0 ? ConstantZero32 : Operand.CreateConstant(I4, value);
		}

		public Operand CreateConstant(long value)
		{
			return value == 0 ? ConstantZero64 : Operand.CreateConstant(I8, value);
		}

		public Operand CreateConstant(ulong value)
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

		public static void UpdatePHIInstructionTarget(BasicBlock target, BasicBlock source, BasicBlock newSource)
		{
			BaseMethodCompilerStage.UpdatePHIInstructionTarget(target, source, newSource);
		}

		public static void UpdatePHIInstructionTargets(List<BasicBlock> targets, BasicBlock source, BasicBlock newSource)
		{
			BaseMethodCompilerStage.UpdatePHIInstructionTargets(targets, source, newSource);
		}

		public static void RemoveBlocksFromPHIInstructions(BasicBlock removedBlock, BasicBlock[] nextBlocks)
		{
			BaseMethodCompilerStage.RemoveBlocksFromPHIInstructions(removedBlock, nextBlocks);
		}

		public static void RemoveBlockFromPHIInstructions(BasicBlock removedBlock, BasicBlock nextBlock)
		{
			BaseMethodCompilerStage.RemoveBlockFromPHIInstructions(removedBlock, nextBlock);
		}

		public void UpdatePHI(Context context)
		{
			Debug.Assert(context.OperandCount != context.Block.PreviousBlocks.Count);

			// One or more of the previous blocks was removed, fix up the operand blocks

			var node = context.Node;
			var previousBlocks = node.Block.PreviousBlocks;
			var phiBlocks = node.PhiBlocks;

			for (int index = 0; index < node.OperandCount; index++)
			{
				var phiBlock = phiBlocks[index];

				if (previousBlocks.Contains(phiBlock))
					continue;

				phiBlocks.RemoveAt(index);

				for (int i = index; index < node.OperandCount - 1; index++)
				{
					context.SetOperand(i, node.GetOperand(i + 1));
				}

				context.SetOperand(node.OperandCount - 1, null);
				node.OperandCount--;

				index--;
			}

			Debug.Assert(context.OperandCount == context.Block.PreviousBlocks.Count);
		}

		public void SplitLongOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
		{
			MethodCompiler.SplitLongOperand(operand, out operandLow, out operandHigh);
		}
	}
}
