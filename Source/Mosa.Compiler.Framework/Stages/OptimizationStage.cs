// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.Framework.Transform;
using Mosa.Compiler.Framework.Transform.Auto;
using Mosa.Compiler.Framework.Transform.Manual;
using System.Collections.Generic;
using System.Linq;

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

		private List<BaseTransformation>[] transformations = new List<BaseTransformation>[MaximumInstructionID];

		private TraceLog trace;

		private TraceLog specialTrace;

		protected override void Initialize()
		{
			Register(OptimizationsCount);

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

			trace = null;
		}

		protected override void Run()
		{
			trace = CreateTraceLog(5);
			specialTrace = new TraceLog(TraceType.GlobalDebug, null, null, "Special Optimizations");

			Optimize();
		}

		private void Optimize()
		{
			var context = new Context(BasicBlocks.PrologueBlock);
			var transformContext = new TransformContext(MethodCompiler, trace, specialTrace);
			int pass = 0;
			var changed = true;
			while (changed)
			{
				pass++;
				trace?.Log($"*** Pass # {pass}");

				changed = false;

				if ((pass & 1) == 0)
				{
					for (int i = 0; i < BasicBlocks.Count; i++)
					{
						for (var node = BasicBlocks[i].AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
						{
							changed = Process(context, transformContext, changed, node);
						}
					}
				}
				else
				{
					for (int i = BasicBlocks.Count - 1; i >= 0; i--)
					{
						for (var node = BasicBlocks[i].BeforeLast; !node.IsBlockStartInstruction; node = node.Previous)
						{
							changed = Process(context, transformContext, changed, node);
						}
					}
				}

				if (pass > MaximumPasses)
					break;
			}
		}

		private bool Process(Context context, TransformContext transformContext, bool changed, InstructionNode node)
		{
			bool updated = true;

			while (updated)
			{
				if (node.IsEmptyOrNop)
					break;

				context.Node = node;

				updated = ApplyTransformations(context, transformContext);

				changed |= updated;
			}

			return changed;
		}

		private bool ApplyTransformations(Context context, TransformContext transformContext)
		{
			if (ApplyTransformations(context, transformContext, 0))
				return true;

			return ApplyTransformations(context, transformContext, context.Instruction.ID);
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
