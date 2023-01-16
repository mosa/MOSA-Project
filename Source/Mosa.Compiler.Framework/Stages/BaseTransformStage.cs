// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	Optimization Stage
	/// </summary>
	public abstract class BaseTransformStage : BaseMethodCompilerStage
	{
		private const int MaximumInstructionID = 1000;
		private const int MaximumPasses = 20;

		private readonly Counter OptimizationCount;
		private readonly Counter TransformCount;
		private readonly Counter SkippedEmptyBlocksCount;
		private readonly Counter RemoveUnreachableBlocksCount;
		private readonly Counter BlocksMergedCount;

		private readonly List<BaseTransform>[] transforms = new List<BaseTransform>[MaximumInstructionID];

		protected TransformContext TransformContext;

		protected TraceLog trace;

		protected TraceLog specialTrace;

		protected bool EnableTransformOptimizations;
		protected bool EnableBlockOptimizations;
		protected bool IsInSSAForm;
		protected bool AreCPURegistersAllocated;

		protected int MaxPasses;

		protected BitArray EmptyBlocks;

		protected Dictionary<string, Counter> TransformCounters = new Dictionary<string, Counter>();

		private bool SortedByPriority = false;

		public BaseTransformStage(bool enableTransformOptimizations = false, bool enableBlockOptimizations = false, int maxPasses = MaximumPasses)
		{
			EnableTransformOptimizations = enableTransformOptimizations;
			EnableBlockOptimizations = enableBlockOptimizations;
			MaxPasses = maxPasses;

			TransformCount = new Counter($"{Name}.Transforms");
			OptimizationCount = new Counter($"{Name}.Optimizations");
			SkippedEmptyBlocksCount = new Counter($"{Name}.SkippedEmptyBlocks");
			RemoveUnreachableBlocksCount = new Counter($"{Name}.RemoveUnreachableBlocks");
			BlocksMergedCount = new Counter($"{Name}.BlockMergeStage.BlocksMerged");
		}

		protected override void Initialize()
		{
			Register(TransformCount);
			Register(OptimizationCount);
			Register(SkippedEmptyBlocksCount);
			Register(RemoveUnreachableBlocksCount);
			Register(BlocksMergedCount);
		}

		protected void AddTranformations(List<BaseTransform> list)
		{
			foreach (var transform in list)
			{
				AddTranformation(transform);
			}
		}

		public void AddTranformation(BaseTransform transform)
		{
			int id = transform.Instruction == null ? 0 : transform.Instruction.ID;

			if (transforms[id] == null)
			{
				transforms[id] = new List<BaseTransform>();
			}

			transforms[id].Add(transform);
		}

		protected override void Finish()
		{
			MethodCompiler.Compiler.PostTraceLog(specialTrace);

			EmptyBlocks = null;
			TransformContext = null;
			specialTrace = null;
			trace = null;
		}

		protected override void Run()
		{
			SortByPriority();

			trace = CreateTraceLog(5);

			IsInSSAForm = MethodCompiler.IsInSSAForm;
			AreCPURegistersAllocated = MethodCompiler.AreCPURegistersAllocated;

			ExecutePasses();

			if (CompilerSettings.FullCheckMode)
				CheckAllPhiInstructions();
		}

		private void SortByPriority()
		{
			if (SortedByPriority)
				return;

			foreach (var list in transforms)
			{
				if (list == null)
					continue;

				list.Sort();
				list.Reverse();
			}

			SortedByPriority = true;
		}

		protected virtual void CustomizeTransformation()
		{ }

		private void ExecutePasses()
		{
			int pass = 1;

			var changed = true;

			while (changed)
			{
				trace?.Log($"*** Pass # {pass}");

				var changed1 = InstructionTransformationPass();
				var changed2 = (!changed1 || pass == 1) && BranchOptimizationPass();

				changed = changed1 || changed2;

				pass++;

				if (pass >= MaxPasses && MaxPasses != 0)
					break;
			}
		}

		private bool InstructionTransformationPass()
		{
			if (!EnableTransformOptimizations)
				return false;

			if (TransformContext == null)
			{
				specialTrace = new TraceLog(TraceType.GlobalDebug, null, null, "Special Optimizations");

				TransformContext = new TransformContext(MethodCompiler);
				TransformContext.SetLogs(trace, specialTrace);

				CustomizeTransformation();
			}

			var context = new Context(BasicBlocks.PrologueBlock);

			var changed = false;

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
			var instructionTransforms = transforms[id];

			if (instructionTransforms == null)
				return false;

			int count = instructionTransforms.Count;

			for (int i = 0; i < count; i++)
			{
				var transform = instructionTransforms[i];

				var updated = TransformContext.ApplyTransform(context, transform);

				if (updated)
				{
					if (transform.IsOptimization)
						OptimizationCount.Increment();
					else if (transform.IsTranformation)
						TransformCount.Increment();

					if (MethodCompiler.Statistics)
						UpdateCounter(transform.Name, 1);

					MethodCompiler.CreateTranformInstructionTrace(this);

					if (CompilerSettings.FullCheckMode)
						CheckAllPhiInstructions();

					return true;
				}
			}

			return false;
		}

		private bool BranchOptimizationPass()
		{
			if (!EnableBlockOptimizations)
				return false;

			if (EmptyBlocks == null)
			{
				EmptyBlocks = new BitArray(BasicBlocks.Count, false);
			}

			var changed1 = MergeBlocks();
			var changed2 = RemoveUnreachableBlocks();
			var changed3 = SkipEmptyBlocks();

			return changed1 || changed2 || changed3;
		}

		protected bool RemoveUnreachableBlocks()
		{
			int emptied = 0;

			var stack = new Stack<BasicBlock>();
			var bitmap = new BitArray(BasicBlocks.Count, false);

			foreach (var block in BasicBlocks)
			{
				if (block.IsHeadBlock || block.IsHandlerHeadBlock || block.IsTryHeadBlock)
				{
					bitmap.Set(block.Sequence, true);
					stack.Push(block);
				}
			}

			while (stack.Count != 0)
			{
				var block = stack.Pop();

				//trace?.Log($"Used Block: {block}");

				foreach (var next in block.NextBlocks)
				{
					//trace?.Log($"Visited Block: {block}");

					if (!bitmap.Get(next.Sequence))
					{
						bitmap.Set(next.Sequence, true);
						stack.Push(next);
					}
				}
			}

			foreach (var block in BasicBlocks)
			{
				if (bitmap.Get(block.Sequence))
					continue;

				if (block.IsHandlerHeadBlock || block.IsTryHeadBlock)
					continue;

				if (HasProtectedRegions && !block.IsCompilerBlock)
					continue;

				if (EmptyBlocks.Get(block.Sequence))
					continue;

				var nextBlocks = block.NextBlocks.ToArray();

				EmptyBlockOfAllInstructions(block, true);

				UpdatePhiBlocks(nextBlocks);

				EmptyBlocks.Set(block.Sequence, true);

				trace?.Log($"Removed Unreachable Block: {block}");
			}

			RemoveUnreachableBlocksCount.Increment(emptied);

			return emptied != 0;
		}

		protected bool SkipEmptyBlocks()
		{
			int emptied = 0;

			foreach (var block in BasicBlocks)
			{
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

				if (EmptyBlocks.Get(block.Sequence))
					continue;

				if (block.PreviousBlocks.Contains(block))
					continue;

				if (!IsEmptyBlockWithSingleJump(block))
					continue;

				var hasPhi = IsInSSAForm && HasPhiInstruction(block.NextBlocks[0]);

				if (hasPhi && IsInSSAForm && (block.PreviousBlocks.Count != 1 || block.NextBlocks[0].PreviousBlocks.Count != 1))
					continue;

				trace?.Log($"Removed Block: {block} - Skipped to: {block.NextBlocks[0]}");

				if (IsInSSAForm)
				{
					UpdatePhiTargets(block.NextBlocks, block, block.PreviousBlocks[0]);
				}

				RemoveEmptyBlockWithSingleJump(block, true);

				EmptyBlocks.Set(block.Sequence, true);

				emptied++;

				//if (CompilerSettings.FullValidationMode)
				//	CheckAllPhiInstructions();
			}

			SkippedEmptyBlocksCount.Increment(emptied);

			return emptied != 0;
		}

		protected bool MergeBlocks()
		{
			int emptied = 0;
			var changed = true;

			while (changed)
			{
				changed = false;

				foreach (var block in BasicBlocks)
				{
					if (block.NextBlocks.Count != 1)
						continue;

					if (block.IsEpilogue
						|| block.IsPrologue
						|| block.IsTryHeadBlock
						|| block.IsHandlerHeadBlock
						|| (!block.IsCompilerBlock && HasProtectedRegions))
						continue;

					// don't remove block if it jumps back to itself
					if (block.PreviousBlocks.Contains(block))
						continue;

					var next = block.NextBlocks[0];

					if (next.PreviousBlocks.Count != 1)
						continue;

					if (next.IsEpilogue
						|| next.IsPrologue
						|| next.IsTryHeadBlock
						|| next.IsHandlerHeadBlock)
						continue;

					trace?.Log($"Merge Blocking: {block} with: {next}");

					if (IsInSSAForm)
					{
						UpdatePhiTargets(next.NextBlocks, next, block);
					}

					var insertPoint = block.BeforeLast.GoBackwardsToNonEmpty();

					var beforeInsertPoint = insertPoint.Previous.GoBackwardsToNonEmpty();

					if (beforeInsertPoint.BranchTargetsCount != 0)
					{
						Debug.Assert(beforeInsertPoint.BranchTargets[0] == next);
						beforeInsertPoint.Empty();
					}

					insertPoint.Empty();
					insertPoint.CutFrom(next.AfterFirst.GoForwardToNonEmpty(), next.Last.Previous.GoBackwardsToNonEmpty());
					emptied++;
					changed = true;
				}
			}

			BlocksMergedCount.Increment(emptied);

			return emptied != 0;
		}
	}
}
