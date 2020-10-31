// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.Framework.Transform;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	Optimization Stage
	/// </summary>
	public abstract class BaseOptimizationStage : BaseMethodCompilerStage
	{
		private const int MaximumInstructionID = 1000;
		private const int MaximumPasses = 20;

		private readonly Counter OptimizationsCount;
		private Counter SkippedEmptyBlocksCount;
		private Counter RemovedDeadBlocksCount;

		private readonly List<BaseTransformation>[] transformations = new List<BaseTransformation>[MaximumInstructionID];

		protected TransformContext TransformContext;

		protected TraceLog trace;

		protected TraceLog specialTrace;

		protected bool IsInSSAForm;
		protected bool EnableBlockOptimizations;

		public BaseOptimizationStage(bool enableBlockOptimizations = false)
		{
			EnableBlockOptimizations = enableBlockOptimizations;

			OptimizationsCount = new Counter($"{Name}.Optimizations");
			SkippedEmptyBlocksCount = new Counter($"{Name}.SkippedEmptyBlocks");
			RemovedDeadBlocksCount = new Counter($"{Name}.RemovedDeadBlocks");
		}

		protected override void Initialize()
		{
			Register(OptimizationsCount);
			Register(RemovedDeadBlocksCount);
			Register(SkippedEmptyBlocksCount);
		}

		protected void AddTranformations(List<BaseTransformation> list)
		{
			foreach (var transformation in list)
			{
				AddTranformation(transformation);
			}
		}

		protected void AddTranformation(BaseTransformation transformation)
		{
			int id = transformation.Instruction == null ? 0 : transformation.Instruction.ID;

			if (transformations[id] == null)
			{
				transformations[id] = new List<BaseTransformation>();
			}

			transformations[id].Add(transformation);
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

			IsInSSAForm = MethodCompiler.IsInSSAForm;

			Optimize();

			if (CompilerSettings.FullCheckMode)
				CheckAllPhiInstructions();
		}

		protected abstract void CustomizeTransformationContract();

		private void Optimize()
		{
			var context = new Context(BasicBlocks.PrologueBlock);

			int pass = 0;
			int maximumPasses = MaximumPasses;

			CustomizeTransformationContract();

			var changed = true;

			while (changed)
			{
				pass++;
				trace?.Log($"*** Pass # {pass}");

				var changed1 = OptimizationPass(context);

				var changed2 = !changed1 ? OptimizationBranchPass() : false;

				changed = changed1 || changed2;

				if (pass >= maximumPasses)
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

					if (CompilerSettings.FullCheckMode)
						CheckAllPhiInstructions();

					return true;
				}
			}

			return false;
		}

		private bool OptimizationBranchPass()
		{
			if (!EnableBlockOptimizations)
				return false;

			var changed1 = RemoveDeadBlocks();
			var changed2 = SkipEmptyBlocks();

			return changed1 || changed2;
		}

		private bool RemoveDeadBlocks()
		{
			bool changed = true;
			int removed = 0;

			while (changed)
			{
				changed = false;

				foreach (var block in BasicBlocks)
				{
					if (block.IsKnownEmpty)
						continue;

					if (block.IsPrologue || block.IsEpilogue)
						continue;

					if (block.IsHandlerHeadBlock || block.IsTryHeadBlock)
						continue;

					if (block.PreviousBlocks.Count != 0)
						continue;

					if (HasProtectedRegions && !block.IsCompilerBlock)
						continue;

					// don't remove block if it jumps back to itself
					if (block.PreviousBlocks.Contains(block))
						continue;

					trace?.Log($"Removed Block: {block}");

					if (IsInSSAForm)
					{
						RemoveBlocksFromPHIInstructions(block, block.NextBlocks.ToArray());
					}

					if (EmptyBlockOfAllInstructions(block, true))
					{
						changed = true;
						removed++;
					}

					//if (CompilerSettings.FullValidationMode)
					//	CheckAllPhiInstructions();
				}
			}

			RemovedDeadBlocksCount += removed;

			return removed != 0;
		}

		private bool SkipEmptyBlocks()
		{
			int emptied = 0;

			foreach (var block in BasicBlocks)
			{
				if (block.IsKnownEmpty)
					continue;

				if (block.NextBlocks.Count != 1)
					continue;

				if (block.IsPrologue || block.IsEpilogue)
					continue;

				if (block.IsHandlerHeadBlock || block.IsTryHeadBlock)
					continue;

				if (HasProtectedRegions && !block.IsCompilerBlock)
					continue;

				if (block.PreviousBlocks.Count == 0)
					continue;

				if (block.PreviousBlocks.Contains(block))
					continue;

				if (IsInSSAForm && (block.PreviousBlocks.Count != 1 || block.NextBlocks[0].PreviousBlocks.Count != 1))
					continue;

				if (!IsEmptyBlockWithSingleJump(block))
					continue;

				trace?.Log($"Removed Block: {block} - Skipped to: {block.NextBlocks[0]}");

				if (IsInSSAForm)
				{
					UpdatePHIInstructionTargets(block.NextBlocks, block, block.PreviousBlocks[0]);
				}

				RemoveEmptyBlockWithSingleJump(block, true);

				emptied++;

				//if (CompilerSettings.FullValidationMode)
				//	CheckAllPhiInstructions();
			}

			SkippedEmptyBlocksCount += emptied;

			return emptied != 0;
		}
	}
}
