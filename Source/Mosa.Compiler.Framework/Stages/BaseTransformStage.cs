// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Framework.Trace;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
///	Base Transform Stage
/// </summary>
public abstract class BaseTransformStage : BaseMethodCompilerStage
{
	private const int MaximumInstructionID = 1000;
	private const int MaximumPasses = 20;

	private int TotalTransformCount;
	private int TransformCount;
	private int OptimizationCount;
	private int SkippedEmptyBlocksCount;
	private int RemoveUnreachableBlocksCount;
	private int BlocksMergedCount;

	private readonly List<BaseTransform>[] transforms = new List<BaseTransform>[MaximumInstructionID];

	private readonly TransformContext TransformContext = new TransformContext();

	protected TraceLog trace;

	protected TraceLog specialTrace;

	protected bool EnableTransformOptimizations;
	protected bool EnableBlockOptimizations;
	protected bool IsInSSAForm;
	protected bool AreCPURegistersAllocated;

	protected int MaxPasses;
	protected int Steps;

	protected BitArray EmptyBlocks;

	protected readonly Dictionary<string, Counter> TransformCounters = new Dictionary<string, Counter>();

	private bool SortedByPriority;

	private string TransformCountStage;
	private string OptimizationCountStage;
	private string SkippedEmptyBlocksCountStage;
	private string RemoveUnreachableBlocksCountStage;
	private string BlocksMergedCountStage;

	public BaseTransformStage(bool enableTransformOptimizations = false, bool enableBlockOptimizations = false, int maxPasses = MaximumPasses)
	{
		EnableTransformOptimizations = enableTransformOptimizations;
		EnableBlockOptimizations = enableBlockOptimizations;
		MaxPasses = maxPasses;
	}

	protected override void Initialize()
	{
		TransformContext.SetCompiler(Compiler);

		TransformCountStage = $"{Name}.Transforms";
		OptimizationCountStage = $"{Name}.Optimizations";
		SkippedEmptyBlocksCountStage = $"{Name}.SkippedEmptyBlocks";
		RemoveUnreachableBlocksCountStage = $"{Name}.RemoveUnreachableBlocks";
		BlocksMergedCountStage = $"{Name}.BlocksMerged";
	}

	protected override void Finish()
	{
		UpdateCounter("Transform.Total", TotalTransformCount);
		UpdateCounter("Transform.Transforms", TransformCount);
		UpdateCounter("Transform.Optimizations", OptimizationCount);
		UpdateCounter("Transform.SkippedEmptyBlocks", SkippedEmptyBlocksCount);
		UpdateCounter("Transform.RemoveUnreachableBlocks", RemoveUnreachableBlocksCount);
		UpdateCounter("Transform.BlocksMerged", BlocksMergedCount);

		UpdateCounter(TransformCountStage, TransformCount);
		UpdateCounter(OptimizationCountStage, OptimizationCount);
		UpdateCounter(SkippedEmptyBlocksCountStage, SkippedEmptyBlocksCount);
		UpdateCounter(RemoveUnreachableBlocksCountStage, RemoveUnreachableBlocksCount);
		UpdateCounter(BlocksMergedCountStage, BlocksMergedCount);

		MethodCompiler.Compiler.PostTraceLog(specialTrace);

		TotalTransformCount = 0;
		TransformCount = 0;
		OptimizationCount = 0;
		SkippedEmptyBlocksCount = 0;
		RemoveUnreachableBlocksCount = 0;
		BlocksMergedCount = 0;

		EmptyBlocks = null;
		trace = null;
	}

	protected override void Run()
	{
		TransformContext.SetMethodCompiler(MethodCompiler);

		SortByPriority();

		trace = CreateTraceLog(5);

		IsInSSAForm = MethodCompiler.IsInSSAForm;
		AreCPURegistersAllocated = MethodCompiler.AreCPURegistersAllocated;

		Steps = 0;
		MethodCompiler.CreateTranformInstructionTrace(this, Steps++);

		specialTrace = new TraceLog(TraceType.GlobalDebug, null, null, "Special Optimizations");

		TransformContext.SetMethodCompiler(MethodCompiler);
		TransformContext.SetLogs(trace, specialTrace);

		CustomizeTransform(TransformContext);

		ExecutePasses();
	}

	protected void AddTranforms(List<BaseTransform> list)
	{
		foreach (var transform in list)
		{
			AddTranform(transform);
		}
	}

	public void AddTranform(BaseTransform transform)
	{
		var id = transform.Instruction == null ? 0 : transform.Instruction.ID;

		if (transforms[id] == null)
		{
			transforms[id] = new List<BaseTransform>();
		}

		transforms[id].Add(transform);
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

	protected virtual void CustomizeTransform(TransformContext transformContext)
	{ }

	private void ExecutePasses()
	{
		var pass = 1;
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

		var context = new Context(BasicBlocks.PrologueBlock);

		var changed = false;

		for (var i = 0; i < BasicBlocks.Count; i++)
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
		var updated = true;
		var changed = false;

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

		var count = instructionTransforms.Count;

		for (var i = 0; i < count; i++)
		{
			var transform = instructionTransforms[i];

			var updated = TransformContext.ApplyTransform(context, transform, TotalTransformCount);

			if (updated)
			{
				TotalTransformCount++;

				if (transform.IsOptimization)
					OptimizationCount++;
				else if (transform.IsTranformation)
					TransformCount++;

				if (MethodCompiler.Statistics)
					UpdateCounter(transform.Name, 1);

				MethodCompiler.CreateTranformInstructionTrace(this, Steps++);

				if (MosaSettings.FullCheckMode)
					FullCheck(false);

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
		var emptied = 0;

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

		RemoveUnreachableBlocksCount += emptied;

		return emptied != 0;
	}

	protected bool SkipEmptyBlocks()
	{
		var emptied = 0;

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

			//if (mosaSettings.FullValidationMode)
			//	CheckAllPhiInstructions();
		}

		SkippedEmptyBlocksCount += emptied;

		return emptied != 0;
	}

	protected bool MergeBlocks()
	{
		var emptied = 0;
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

				var insertPoint = block.BeforeLast.BackwardsToNonEmpty;

				var beforeInsertPoint = insertPoint.Previous.BackwardsToNonEmpty;

				if (beforeInsertPoint.BranchTargetsCount != 0)
				{
					Debug.Assert(beforeInsertPoint.BranchTargets[0] == next);
					beforeInsertPoint.Empty();
				}

				insertPoint.Empty();
				insertPoint.MoveFrom(next.AfterFirst.ForwardToNonEmpty, next.Last.Previous.BackwardsToNonEmpty);
				emptied++;
				changed = true;
			}
		}

		BlocksMergedCount += emptied;

		return emptied != 0;
	}
}
