/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	Places phi instructions for the SSA transformation
	/// </summary>
	public class PhiPlacementStage : BaseMethodCompilerStage
	{
		/// <summary>
		///
		/// </summary>
		public enum PhiPlacementStrategy
		{
			Minimal,
			SemiPruned,
			Pruned
		}

		/// <summary>
		///
		/// </summary>
		private PhiPlacementStrategy strategy;

		/// <summary>
		///
		/// </summary>
		private Dictionary<Operand, List<BasicBlock>> assignments = new Dictionary<Operand, List<BasicBlock>>();

		/// <summary>
		/// Initializes a new instance of the <see cref="PhiPlacementStage"/> class.
		/// </summary>
		/// <param name="strategy">The strategy.</param>
		public PhiPlacementStage(PhiPlacementStrategy strategy)
		{
			this.strategy = strategy;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PhiPlacementStage"/> class.
		/// </summary>
		public PhiPlacementStage()
			: this(PhiPlacementStrategy.Minimal)
		{
		}

		/// <summary>
		/// Gets the assignments.
		/// </summary>
		public Dictionary<Operand, List<BasicBlock>> Assignments { get { return assignments; } }

		protected override void Run()
		{
			// Method is empty - must be a plugged method
			if (BasicBlocks.HeadBlocks.Count == 0)
				return;

			if (HasProtectedRegions)
				return;

			CollectAssignments();

			switch (strategy)
			{
				case PhiPlacementStrategy.Minimal: PlacePhiFunctionsMinimal(); return;
				case PhiPlacementStrategy.SemiPruned: PlacePhiFunctionsSemiPruned(); return;
				case PhiPlacementStrategy.Pruned: PlacePhiFunctionsPruned(); return;
			}
		}

		protected override void Finish()
		{
			UpdateCounter("PhiPlacement.IRInstructions", instructionCount);
		}

		/// <summary>
		/// Collects the assignments.
		/// </summary>
		private void CollectAssignments()
		{
			foreach (var block in BasicBlocks)
			{
				for (var context = new Context(InstructionSet, block); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (context.IsEmpty)
						continue;

					instructionCount++;

					if (context.Result == null)
						continue;

					if (context.Result.IsVirtualRegister)
					{
						AddToAssignments(context.Result, block);
					}
				}
			}
		}

		/// <summary>
		/// Adds to assignments.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <param name="block">The block.</param>
		private void AddToAssignments(Operand operand, BasicBlock block)
		{
			List<BasicBlock> blocks;

			if (!assignments.TryGetValue(operand, out blocks))
			{
				blocks = new List<BasicBlock>();
				assignments.Add(operand, blocks);
			}

			blocks.AddIfNew(block);
		}

		/// <summary>
		/// Inserts the phi instruction.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="variable">The variable.</param>
		private void InsertPhiInstruction(BasicBlock block, Operand variable)
		{
			var context = new Context(InstructionSet, block);
			context.AppendInstruction(IRInstruction.Phi, variable);

			//var sourceBlocks = new BasicBlock[block.PreviousBlocks.Count];
			var sourceBlocks = new List<BasicBlock>(block.PreviousBlocks.Count);
			context.Other = sourceBlocks;

			for (var i = 0; i < block.PreviousBlocks.Count; i++)
			{
				context.SetOperand(i, variable);
				sourceBlocks.Add(block.PreviousBlocks[i]);
			}

			context.OperandCount = (byte)block.PreviousBlocks.Count;

			Debug.Assert(context.OperandCount == context.BasicBlock.PreviousBlocks.Count);
		}

		/// <summary>
		/// Places the phi functions minimal.
		/// </summary>
		private void PlacePhiFunctionsMinimal()
		{
			foreach (var headBlock in BasicBlocks.HeadBlocks)
			{
				PlacePhiFunctionsMinimal(headBlock);
			}
		}

		/// <summary>
		/// Places the phi functions minimal.
		/// </summary>
		private void PlacePhiFunctionsMinimal(BasicBlock headBlock)
		{
			var analysis = MethodCompiler.DominanceAnalysis.GetDominanceAnalysis(headBlock);

			foreach (var t in assignments)
			{
				var blocks = t.Value;

				if (blocks.Count < 2)
					continue;

				blocks.AddIfNew(headBlock);

				var idf = analysis.IteratedDominanceFrontier(blocks);

				foreach (var n in idf)
				{
					InsertPhiInstruction(n, t.Key);
				}
			}
		}

		/// <summary>
		/// Places the phi functions semi pruned.
		/// </summary>
		private void PlacePhiFunctionsSemiPruned()
		{
			throw new NotImplementedException("PhiPlacementStage.PlacePhiFunctionsSemiPruned");
		}

		/// <summary>
		/// Places the phi functions pruned.
		/// </summary>
		private void PlacePhiFunctionsPruned()
		{
			throw new NotImplementedException("PhiPlacementStage.PlacePhiFunctionsSemiPruned");
		}
	}
}
