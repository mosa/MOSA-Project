// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Common;
using Mosa.Compiler.Framework.Trace;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Value Numbering Stage
	/// </summary>
	public sealed class LoopInvariantCodeMotionStage : BaseMethodCompilerStage
	{
		private Counter PreHeadersCount = new Counter("LoopInvariantCodeMotionStage.PreHeaders");
		private Counter CodeMotionCount = new Counter("LoopInvariantCodeMotionStage.CodeMotion");
		private Counter Methods = new Counter("LoopInvariantCodeMotionStage.Methods");

		private SimpleFastDominance AnalysisDominance;

		private TraceLog trace;

		protected override void Initialize()
		{
			Register(CodeMotionCount);
			Register(PreHeadersCount);
			Register(Methods);
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

			AnalysisDominance = new SimpleFastDominance(BasicBlocks, BasicBlocks.PrologueBlock);

			var loops = FindLoops();

			if (loops.Count == 0)
				return;

			if (trace != null)
			{
				DumpLoops(loops);
			}

			SortLoops(loops);

			FindLoopInvariantInstructions(loops);

			if (CodeMotionCount.Count != 0)
			{
				Methods++;
			}
		}

		protected override void Finish()
		{
			AnalysisDominance = null;
		}

		private List<Loop> FindLoops()
		{
			var loops = new List<Loop>();
			var lookup = new Dictionary<BasicBlock, Loop>();

			foreach (var block in BasicBlocks)
			{
				if (block.PreviousBlocks.Count == 0)
					continue;

				foreach (var previous in block.PreviousBlocks)
				{
					// Is this a back-edge? Yes, if "block" dominates "previous"
					if (AnalysisDominance.IsDominator(block, previous))
					{
						if (lookup.TryGetValue(block, out Loop loop))
						{
							loop.AddBackEdge(previous);
						}
						else
						{
							loop = new Loop(block, previous);
							loops.Add(loop);
							lookup.Add(block, loop);
						}
					}
				}
			}

			foreach (var loop in loops)
			{
				PopulateLoopNodes(loop);
			}

			return loops;
		}

		private void PopulateLoopNodes(Loop loop)
		{
			var worklist = new Stack<BasicBlock>();
			var array = new BitArray(BasicBlocks.Count);

			foreach (var backedge in loop.Backedges)
			{
				worklist.Push(backedge);
			}

			loop.AddNode(loop.Header);
			array.Set(loop.Header.Sequence, true);

			while (worklist.Count != 0)
			{
				var node = worklist.Pop();

				if (!array.Get(node.Sequence))
				{
					array.Set(node.Sequence, true);
					loop.LoopBlocks.Add(node);

					foreach (var previous in node.PreviousBlocks)
					{
						if (previous == loop.Header)
							continue;

						worklist.Push(previous);
					}
				}
			}
		}

		public void DumpLoops(List<Loop> loops)
		{
			var loopTrace = CreateTraceLog("Loops");

			if (loopTrace == null)
				return;

			foreach (var loop in loops)
			{
				loopTrace.Log($"Header: {loop.Header}");
				foreach (var backedge in loop.Backedges)
				{
					loopTrace.Log($"   Backedge: {backedge}");
				}

				var sb = new StringBuilder();

				foreach (var block in loop.LoopBlocks)
				{
					sb.Append(block);
					sb.Append(", ");
				}

				sb.Length -= 2;

				loopTrace.Log($"   Members: {sb}");
			}
		}

		private void SortLoops(List<Loop> loops)
		{
			loops.Sort((Loop p1, Loop p2) => p1.LoopBlocks.Count < p2.LoopBlocks.Count ? 0 : 1);
		}

		private void FindLoopInvariantInstructions(List<Loop> loops)
		{
			foreach (var loop in loops)
			{
				var invariants = FindLoopInvariantInstructions(loop);

				MoveToPreHeader(invariants, loop);
			}
		}

		private List<InstructionNode> FindLoopInvariantInstructions(Loop loop)
		{
			var invariantsSet = new HashSet<InstructionNode>();
			var invariantsList = new List<InstructionNode>();

			bool changed = true;

			trace?.Log($"Loop: {loop.Header}");

			while (changed)
			{
				changed = false;
				foreach (var block in loop.LoopBlocks)
				{
					for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
					{
						if (node.IsEmpty)
							continue;

						// note - same code from ValueNumberingStage::CanAssignValueNumberToExpression()
						if (node.ResultCount != 1
							|| node.OperandCount == 0
							|| node.OperandCount > 2
							|| node.Instruction.IsMemoryWrite
							|| node.Instruction.IsMemoryRead
							|| node.Instruction.IsIOOperation
							|| node.Instruction.HasUnspecifiedSideEffect
							|| node.Instruction.VariableOperands
							|| node.Instruction.FlowControl != FlowControl.Next
							|| node.Instruction.IgnoreDuringCodeGeneration
							|| node.Operand1.IsUnresolvedConstant
							|| (node.OperandCount == 2 && node.Operand2.IsUnresolvedConstant))
							continue;

						if (invariantsSet.Contains(node))
							continue;

						// Check operand 1
						if (!IsInvariant(node.Operand1, loop, invariantsSet))
							continue;

						// check operand 2
						if (node.OperandCount == 2 && !IsInvariant(node.Operand2, loop, invariantsSet))
							continue;

						invariantsSet.Add(node);
						invariantsList.Add(node);

						trace?.Log($"  {node}");

						changed = true;
					}
				}
			}

			return invariantsList;
		}

		private static bool IsInvariant(Operand operand, Loop loop, HashSet<InstructionNode> invariants)
		{
			// constant
			if (operand.IsResolvedConstant)
				return true;

			if (operand.IsVirtualRegister && operand.Definitions.Count == 1)
			{
				var def = operand.Definitions[0];

				// defined is invariant instruction
				if (/*invariants != null &&*/ invariants.Contains(def))
					return true;

				// defined outside of the loop
				if (!loop.LoopBlocks.Contains(def.Block))
					return true;
			}

			return false;
		}

		private BasicBlock CreatePreHeader(Loop loop)
		{
			var header = loop.Header;

			// Create pre-header block
			var preheaderBlock = CreateNewBlock();
			var preheader = new Context(preheaderBlock);

			foreach (var previous in header.PreviousBlocks.ToArray())
			{
				if (!loop.Backedges.Contains(previous))
				{
					ReplaceBranchTargets(previous, header, preheaderBlock);
				}
			}

			// PHIs in loop header
			for (var node = header.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmpty)
					continue;

				if (IsSimpleIRMoveInstruction(node.Instruction))
					continue; // sometimes PHI are converted to moves

				//if (node.Instruction != IRInstruction.Phi32 && node.Instruction != IRInstruction.Phi64 && node.Instruction != IRInstruction.PhiR4 && node.Instruction != IRInstruction.PhiR8)
				if (!IsPhiInstruction(node.Instruction))
					break;

				var phiInstruction = node.Instruction;

				var internalSourceBlocks = new List<BasicBlock>(node.OperandCount);
				var internalSourceOperands = new List<Operand>(node.OperandCount);
				var externalSourceBlocks = new List<BasicBlock>(node.OperandCount);
				var externalSourceOperands = new List<Operand>(node.OperandCount);

				var transitionOperand = AllocateVirtualRegister(node.Result.Type);

				internalSourceOperands.Add(transitionOperand);
				internalSourceBlocks.Add(preheaderBlock);

				for (int i = 0; i < node.PhiBlocks.Count; i++)
				{
					var sourceOperand = node.GetOperand(i);
					var sourceBlock = node.PhiBlocks[i];

					if (loop.Backedges.Contains(sourceBlock))
					{
						Debug.Assert(loop.LoopBlocks.Contains(sourceBlock));

						internalSourceBlocks.Add(sourceBlock);
						internalSourceOperands.Add(sourceOperand);
					}
					else
					{
						Debug.Assert(!loop.LoopBlocks.Contains(sourceBlock));

						externalSourceBlocks.Add(sourceBlock);
						externalSourceOperands.Add(sourceOperand);
					}
				}

				preheader.AppendInstruction(phiInstruction, transitionOperand, externalSourceOperands);
				preheader.PhiBlocks = externalSourceBlocks;

				node.SetInstruction(phiInstruction, node.Result, internalSourceOperands);
				node.PhiBlocks = internalSourceBlocks;

				//OptimizePhi(preheader.Node);
				//OptimizePhi(node);
			}

			preheader.AppendInstruction(IRInstruction.Jmp, header);

			PreHeadersCount++;

			return preheaderBlock;
		}

		//private void OptimizePhi(InstructionNode node)
		//{
		//	var newInstruction = BuiltInOptimizations.PhiSimplication(node);
		//	if (newInstruction != null)
		//	{
		//		if (IsPhiInstruction(newInstruction.Instruction))
		//		{
		//			node.SetInstruction(newInstruction);
		//			return;
		//		}

		//		// move node after all other phi instructions
		//		for (var at = node.Next; ; at = at.Next)
		//		{
		//			if (at.IsEmptyOrNop)
		//				continue;

		//			if (IsPhiInstruction(at.Instruction))
		//				continue;

		//			at = at.Previous;

		//			node.Empty();

		//			var context = new Context(at);
		//			context.AppendInstruction(newInstruction);

		//			return;
		//		}
		//	}
		//}

		private void MoveToPreHeader(List<InstructionNode> nodes, Loop loop)
		{
			if (nodes.Count == 0)
				return;

			if (loop.Header.PreviousBlocks.Count == 0)
				return; // special case - a pre-header can not be made because the loop header is already the first block

			var preheader = CreatePreHeader(loop);

			var at = preheader.BeforeLast;

			while (at.IsEmpty || at.Instruction != IRInstruction.Jmp)
			{
				at = at.Previous;
			}

			at = at.Previous;

			foreach (var node in nodes)
			{
				at.CutFrom(node);
				at = node;
			}

			CodeMotionCount += nodes.Count;
		}
	}
}
