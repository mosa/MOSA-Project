// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.Trace;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// SimpleMovePropagationStage
	/// </summary>
	public sealed class SimpleMovePropagationStage : BaseTransformationStage
	{
		private int instructionsRemovedCount = 0;

		private int changeCount = 0;

		private TraceLog trace;

		protected override void PopulateVisitationDictionary()
		{
			// Nothing to do
		}

		protected override void Run()
		{
			// Method is empty - must be a plugged method
			if (!HasCode)
				return;

			trace = CreateTraceLog();

			Optimize();

			UpdateCounter("X86.SimpleMovePropagationStage.IRInstructionRemoved", instructionsRemovedCount);
		}

		private void Optimize()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction != X86.Mov)
						continue;

					Move(node);
				}
			}
		}

		/// <summary>
		/// Simple copy propagation.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Move(InstructionNode node)
		{
			var source = node.Operand1;
			var destination = node.Result;

			if (!destination.IsVirtualRegister)
				return;

			if (!source.IsVirtualRegister)
				return;

			if (destination.Definitions.Count != 1)
				return;

			if (source.Definitions.Count != 1)
				return;

			if (source.IsResolvedConstant)
				return;

			Debug.Assert(destination != source);

			foreach (var useNode in destination.Uses.ToArray())
			{
				for (int i = 0; i < useNode.OperandCount; i++)
				{
					var operand = useNode.GetOperand(i);

					if (destination == operand)
					{
						if (trace.Active) trace.Log("*** SimpleForwardCopyPropagation");
						if (trace.Active) trace.Log("BEFORE:\t" + useNode);
						useNode.SetOperand(i, source);
						changeCount++;
						if (trace.Active) trace.Log("AFTER: \t" + useNode);
					}
				}
			}

			Debug.Assert(destination.Uses.Count == 0);

			if (trace.Active) trace.Log("REMOVED:\t" + node);
			node.SetInstruction(X86.Nop);
			instructionsRemovedCount++;
			changeCount++;
		}
	}
}
