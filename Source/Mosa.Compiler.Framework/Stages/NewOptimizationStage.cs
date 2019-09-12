// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.Framework.Transform;
using Mosa.Compiler.Framework.Transform.Auto;
using Mosa.Compiler.Framework.Transform.Manual;
using System;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	Optimization Stage
	/// </summary>
	public class NewOptimizationStage : BaseMethodCompilerStage
	{
		private const int MaximumInstructionID = 1000;

		private Counter OptimizationsCount = new Counter("NewOptimizationStage.Optimizations");

		private List<BaseTransformation>[] transformations = new List<BaseTransformation>[MaximumInstructionID];

		private TraceLog trace;

		protected override void Initialize()
		{
			Register(OptimizationsCount);

			CreateTransformationList(AutoTransforms.List);

			CreateTransformationList(ManualTransforms.List);
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
			trace = null;
		}

		protected override void Run()
		{
			if (HasProtectedRegions)
				return;

			// Method is empty - must be a plugged method
			if (BasicBlocks.HeadBlocks.Count != 1)
				return;

			if (BasicBlocks.PrologueBlock == null)
				return;

			trace = CreateTraceLog(5);

			Optimize();
		}

		private void Optimize()
		{
			var context = new Context(BasicBlocks.PrologueBlock);
			var transformContext = new TransformContext(MethodCompiler, trace);

			var updated = false;
			var changed = true;
			while (changed)
			{
				changed = false;

				foreach (var block in BasicBlocks)
				{
					for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
					{
						updated = true;

						while (updated)
						{
							if (node.IsEmptyOrNop)
								break;

							context.Node = node;

							updated = ApplyTransformations(context, transformContext);

							changed |= updated;
						}
					}
				}
			}
		}

		private bool ApplyTransformations(Context context, TransformContext transformContext)
		{
			if (ApplyTransformations(context, transformContext, context.Instruction.ID))
				return true;

			return ApplyTransformations(context, transformContext, 0);
		}

		private bool ApplyTransformations(Context context, TransformContext transformContext, int id)
		{
			var instructionTransformations = transformations[id];

			if (instructionTransformations == null)
				return false;

			int count = instructionTransformations.Count;

			for (int i = 0; i < count; i++)
			{
				var transformation = instructionTransformations[i];

				var updated = transformContext.ApplyTransform(context, transformation);

				if (updated)
				{
					OptimizationsCount.Increment();
					return true;
				}
			}

			return false;
		}
	}
}
