// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Common;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Value Numbering Stage
	/// </summary>
	public sealed class LoopInvariantCodeMotionStage : BaseMethodCompilerStage
	{
		private Counter CodeMotionCount = new Counter("LoopInvariantCodeMotionStage.CodeMotionCount");

		private SimpleFastDominance AnalysisDominance;

		private TraceLog trace;

		protected override void Initialize()
		{
			Register(CodeMotionCount);
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

			if (trace.Active) DumpLoops(loops);

			//FindLoopInvariantInstructions(loops);
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

			if (trace.Active) trace.Log("Loop: " + loop.Header.ToString());

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

						if (trace.Active) trace.Log("  " + node.ToString());

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
			var preheader = CreateNewBlock();

			var preheaderCtx = new Context(preheader);
			preheaderCtx.AppendInstruction(IRInstruction.Jmp, header);

			// TODO: need to move PHI instructions to header AND update previous branch targets
			var headerCtx = new Context(header.AfterFirst);

			// TODO: not quite right
			foreach (var previous in header.PreviousBlocks.ToArray())
			{
				ReplaceBranchTargets(previous, header, preheader);
			}

			return preheader;
		}

		private void MoveToPreHeader(List<InstructionNode> nodes, Loop loop)
		{
			if (loop.Header.PreviousBlocks.Count == 0)
				return; // special case - a pre-header can not be made because the loop header is already the first block

			var preheader = CreatePreHeader(loop);

			var at = preheader.BeforeLast;

			foreach (var node in nodes)
			{
				at.CutFrom(node);
				at = node;
			}
		}
	}
}
