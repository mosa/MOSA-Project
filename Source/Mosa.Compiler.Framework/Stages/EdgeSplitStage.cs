/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	Transformed to edge-split SSA form
	/// </summary>
	public class EdgeSplitStage : BaseCodeTransformationStage, IMethodCompilerStage, IPipelineStage
	{


		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			List<KeyValuePair<BasicBlock, BasicBlock>> worklist = new List<KeyValuePair<BasicBlock, BasicBlock>>();

			foreach (var a in basicBlocks)
			{
				if (a.NextBlocks.Count > 1)
				{
					foreach (var b in a.NextBlocks)
					{
						if (b.PreviousBlocks.Count > 2)
						{
							worklist.Add(new KeyValuePair<BasicBlock, BasicBlock>(a, b));
						}
					}
				}
			}

			foreach (var edge in worklist)
			{
				if (edge.Key.NextBlocks.Count > 1 || edge.Value.PreviousBlocks.Count > 1)
					SplitEdge(edge.Key, edge.Value);
				else
					continue;
			}

		}

		void SplitEdge(BasicBlock a, BasicBlock b)
		{
			// Create new block z
			var z = basicBlocks.CreateBlock();
			Context ctx = new Context(instructionSet);
			ctx.AppendInstruction(IR.IRInstruction.Jmp, a);
			ctx.Label = -1;
			z.Index = ctx.Index;

			// Unlink blocks
			a.NextBlocks.Remove(b);
			b.PreviousBlocks.Remove(a);

			// Link a to z 
			a.NextBlocks.Add(z);
			z.PreviousBlocks.Add(a);

			// Link z to b
			b.PreviousBlocks.Add(z);
			z.NextBlocks.Add(b);

			// Insert jump in z to b
			ctx.SetInstruction(IRInstruction.Jmp, b);

			// Replace any jump/branch target in block a with z
			ctx = new Context(instructionSet, a);
			ctx.GotoLast();

			// Find branch or jump to b and replace it with z
			while (ctx.BranchTargets != null)
			{
				int[] targets = ctx.BranchTargets;
				for (int index = 0; index < targets.Length; index++)
				{
					if (targets[index] == b.Label)
						targets[index] = z.Label;
				}
				ctx.GotoPrevious();
			}

		}
	}
}
