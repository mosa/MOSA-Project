﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.Framework.Transform;
using Mosa.Compiler.Framework.Transform.Auto;
using Mosa.Compiler.Framework.Transform.Manual;
using System.Collections.Generic;
using System.Diagnostics;

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

		private Counter SkippedEmptyBlocksCount = new Counter("OptimizationStage.SkippedEmptyBlocks");
		private Counter RemovedDeadBlocksCount = new Counter("OptimizationStage.RemovedDeadBlocks");

		private List<BaseTransformation>[] transformations = new List<BaseTransformation>[MaximumInstructionID];

		private TransformContext TransformContext;

		private TraceLog trace;

		private TraceLog specialTrace;

		private bool IsInSSAForm;
		private bool LowerTo32;

		private HashSet<BasicBlock> EmptiedBlocks;

		public OptimizationStage(bool lowerTo32)
		{
			LowerTo32 = lowerTo32;
		}

		protected override void Initialize()
		{
			Register(OptimizationsCount);
			Register(RemovedDeadBlocksCount);
			Register(SkippedEmptyBlocksCount);

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
			EmptiedBlocks = null;
		}

		protected override void Run()
		{
			IsInSSAForm = MethodCompiler.IsInSSAForm;

			trace = CreateTraceLog(5);
			specialTrace = new TraceLog(TraceType.GlobalDebug, null, null, "Special Optimizations");

			EmptiedBlocks = new HashSet<BasicBlock>();

			TransformContext = new TransformContext(MethodCompiler);
			TransformContext.SetLogs(trace, specialTrace);

			Optimize();

			Debug.Assert(CheckAllPhiInstructions());    // comment me out --- otherwise this will be turtle
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

					Debug.Assert(CheckAllPhiInstructions());    // comment me out --- otherwise this will be turtle

					return true;
				}
			}

			return false;
		}

		private bool OptimizationBranchPass()
		{
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
					if (block.IsPrologue || block.IsEpilogue)
						continue;

					if (block.IsHandlerHeadBlock || block.IsTryHeadBlock)
						continue;

					if (block.PreviousBlocks.Count != 0)
						continue;

					if (HasProtectedRegions && !block.IsCompilerBlock)
						continue;

					if (EmptiedBlocks.Contains(block))
						continue;

					// don't remove block if it jumps back to itself
					if (block.PreviousBlocks.Contains(block))
						continue;

					trace.Log($"Removed Block: {block}");

					if (IsInSSAForm)
					{
						RemoveBlocksFromPHIInstructions(block, block.NextBlocks.ToArray());
					}

					EmptyBlockOfAllInstructions(block, true);

					EmptiedBlocks.Add(block);

					changed = true;
					removed++;

					Debug.Assert(CheckAllPhiInstructions());    // comment me out --- otherwise this will be turtle
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

				trace.Log($"Removed Block: {block} - Skipped to: {block.NextBlocks[0]}");

				if (IsInSSAForm)
				{
					UpdatePHIInstructionTargets(block.NextBlocks, block, block.PreviousBlocks[0]);
				}

				RemoveEmptyBlockWithSingleJump(block, true);

				EmptiedBlocks.Add(block);

				emptied++;

				Debug.Assert(CheckAllPhiInstructions());    // comment me out --- otherwise this will be turtle
			}

			SkippedEmptyBlocksCount += emptied;

			return emptied != 0;
		}
	}
}
