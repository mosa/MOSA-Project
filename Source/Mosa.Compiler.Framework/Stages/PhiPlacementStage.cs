/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Collections.Generic;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	Places phi instructions for the SSA transformation
	/// </summary>
	public class PhiPlacementStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
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
		public Dictionary<Operand, List<BasicBlock>> Assignments
		{
			get { return assignments; }
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			// Method is empty - must be a plugged method
			if (basicBlocks.HeadBlocks.Count == 0)
				return;

			CollectAssignments();

			switch (strategy)
			{
				case PhiPlacementStrategy.Minimal: PlacePhiFunctionsMinimal(); return;
				case PhiPlacementStrategy.SemiPruned: PlacePhiFunctionsSemiPruned(); return;
				case PhiPlacementStrategy.Pruned: PlacePhiFunctionsPruned(); return;
			}
		}

		/// <summary>
		/// Determines whether [is assignment to stack variable] [the specified instruction].
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		///   <c>true</c> if [is assignment to stack variable] [the specified instruction]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsAssignmentToStackVariable(Context instruction)
		{
			return instruction.Result != null && instruction.Result.IsStackLocal;
		}

		/// <summary>
		/// Collects the assignments.
		/// </summary>
		private void CollectAssignments()
		{
			foreach (var block in basicBlocks)
				for (var context = new Context(instructionSet, block); !context.EndOfInstruction; context.GotoNext())
					if (!context.IsEmpty && context.Result != null)
						if (context.Result.IsStackLocal)
							AddToAssignments(context.Result, block);

			// FUTURE: Only include parameter operands if reachable from the given header block
			foreach (var headBlock in basicBlocks.HeadBlocks)
				foreach (var op in methodCompiler.Parameters)
					AddToAssignments(op, headBlock);
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
			var context = new Context(instructionSet, block).InsertBefore();
			context.SetInstruction(IRInstruction.Phi, variable);

			for (var i = 0; i < block.PreviousBlocks.Count; ++i)
				context.SetOperand(i, variable);

			context.OperandCount = (byte)block.PreviousBlocks.Count;
		}


		/// <summary>
		/// Places the phi functions minimal.
		/// </summary>
		private void PlacePhiFunctionsMinimal()
		{
			foreach (var headBlock in basicBlocks.HeadBlocks)
				PlacePhiFunctionsMinimal(headBlock);
		}

		/// <summary>
		/// Places the phi functions minimal.
		/// </summary>
		private void PlacePhiFunctionsMinimal(BasicBlock headBlock)
		{
			var dominanceCalculation = methodCompiler.Pipeline.FindFirst<DominanceCalculationStage>().GetDominanceProvider(headBlock);

			foreach (var t in assignments)
			{
				var blocks = t.Value;

				if (blocks.Count < 2)
					continue;

				blocks.AddIfNew(headBlock);

				var idf = dominanceCalculation.IteratedDominanceFrontier(blocks);

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
