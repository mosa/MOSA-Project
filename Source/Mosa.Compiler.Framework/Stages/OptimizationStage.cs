// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.Framework.Transform;
using Mosa.Compiler.Framework.Transform.Auto;
using Mosa.Compiler.Framework.Transform.Manual;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	Optimization Stage
	/// </summary>
	public class OptimizationStage : BaseMethodCompilerStage
	{
		private const int MaximumInstructionID = 1000;
		private const int MaximumPasses = 20;

		private Counter OptimizationsCount = new Counter("OptimizationStage.Optimizations");

		private Counter EmptyBlocksRemovedCount = new Counter("OptimizationStage.EmptyBlocksRemoved");
		private Counter SkipEmptyBlocksCount = new Counter("OptimizationStage.SkipEmptyBlocks");

		private List<BaseTransformation>[] transformations = new List<BaseTransformation>[MaximumInstructionID];

		private TransformContext TransformContext;

		private TraceLog trace;

		private TraceLog specialTrace;

		private bool IsInSSAForm;
		private bool LowerTo32;

		public OptimizationStage(bool inSSAForm, bool lowerTo32)
		{
			IsInSSAForm = inSSAForm;
			LowerTo32 = lowerTo32;
		}

		protected override void Initialize()
		{
			Register(OptimizationsCount);
			Register(EmptyBlocksRemovedCount);
			Register(SkipEmptyBlocksCount);

			CreateTransformationList(ManualTransforms.List);

			CreateTransformationList(AutoTransforms.List);
		}

		private void CreateTransformationList(List<BaseTransformation> list)
		{
			foreach (var transformation in list)
			{
				int id = transformation.Instruction == null ? 0 : transformation.Instruction.ID;

				if (transformations[id] == null)
				{
					transformations[id] = new List<BaseTransformation>();
				}

				transformations[id].Add(transformation);
			}
		}

		protected override void Finish()
		{
			MethodCompiler.Compiler.PostTraceLog(specialTrace);

			TransformContext = null;
			specialTrace = null;
			trace = null;
		}

		protected override void Run()
		{
			trace = CreateTraceLog(5);
			specialTrace = new TraceLog(TraceType.GlobalDebug, null, null, "Special Optimizations");

			TransformContext = new TransformContext(MethodCompiler);
			TransformContext.SetLogs(trace, specialTrace);

			Optimize();
		}

		private void Optimize()
		{
			var context = new Context(BasicBlocks.PrologueBlock);

			int pass = 0;
			int maximumPasses = MaximumPasses;

			TransformContext.SetStageOptions(IsInSSAForm, LowerTo32 && CompilerSettings.LongExpansion && Is32BitPlatform);

			var changed = true;

			while (changed)
			{
				pass++;
				trace?.Log($"*** Pass # {pass}");

				var changed1 = OptimizationPass(context);

				var changed2 = OptimizationBranchPass();

				changed = changed1 || changed2;

				if (pass > maximumPasses)
					break;
			}
		}

		private bool OptimizationPass(Context context)
		{
			bool changed = false;

			for (int i = 0; i < BasicBlocks.Count; i++)
			{
				for (var node = BasicBlocks[i].AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
				{
					context.Node = node;

					var updated = Process(context);
					changed = changed || updated;
				}
			}

			return changed;
		}

		private bool Process(Context context)
		{
			bool updated = true;
			bool changed = false;

			while (updated)
			{
				if (context.IsEmptyOrNop)
					break;

				updated = ApplyTransformations(context);

				changed |= updated;
			}

			return changed;
		}

		private bool ApplyTransformations(Context context)
		{
			if (ApplyTransformations(context, 0))
				return true;

			return ApplyTransformations(context, context.Instruction.ID);
		}

		private bool ApplyTransformations(Context context, int id)
		{
			var instructionTransformations = transformations[id];

			if (instructionTransformations == null)
				return false;

			int count = instructionTransformations.Count;

			for (int i = 0; i < count; i++)
			{
				var transformation = instructionTransformations[i];

				var updated = TransformContext.ApplyTransform(context, transformation);

				if (updated)
				{
					OptimizationsCount.Increment();
					return true;
				}
			}

			return false;
		}

		private bool OptimizationBranchPass()
		{
			bool changed = false;

			//changed = SkipEmptyBlocks();

			return changed;
		}

		private bool SkipEmptyBlocks()
		{
			bool changed = true;

			foreach (var block in BasicBlocks)
			{
				if (block.IsPrologue || block.IsEpilogue)
					continue;

				if (block.IsHandlerHeadBlock || block.IsTryHeadBlock)
					continue;

				if (HasProtectedRegions && !block.IsCompilerBlock)
					continue;

				// don't remove block if it jumps back to itself
				if (block.PreviousBlocks.Contains(block))
					continue;

				if (!IsEmptyBlockWithSingleJump(block))
					continue;

				RemoveEmptyBlockWithSingleJump(block);
				changed = true;

				SkipEmptyBlocksCount++;
			}

			return changed;
		}
	}
}
