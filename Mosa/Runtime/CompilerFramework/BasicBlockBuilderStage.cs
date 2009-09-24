/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// This compilation stage is used by method compilers after the
	/// IL decoding stage to build basic Blocks out of the instruction list.
	/// </summary>
	public sealed class BasicBlockBuilderStage : IMethodCompilerStage, IBasicBlockProvider
	{
		#region Data members

		/// <summary>
		/// List of basic Blocks found during decoding.
		/// </summary>
		private List<BasicBlock> _basicBlocks;

		/// <summary>
		/// List of leaders
		/// </summary>
		private SortedDictionary<int, BasicBlock> _loopHeads;

		/// <summary>
		/// Holds the instruction set
		/// </summary>
		private InstructionSet _instructionset;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BasicBlockBuilderStage"/> class.
		/// </summary>
		public BasicBlockBuilderStage()
		{
			_basicBlocks = new List<BasicBlock>();
			_loopHeads = new SortedDictionary<int, BasicBlock>();
		}

		#endregion // Construction

		#region IMethodCompilerStage members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value></value>
		public string Name
		{
			get { return @"Basic Block Builder"; }
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public void Run(IMethodCompiler compiler)
		{
			// Retrieve the instruction provider and the instruction set
			_instructionset = (compiler.GetPreviousStage(typeof(IInstructionsProvider)) as IInstructionsProvider).InstructionSet;

			AddLoopHead(0);

			FindLoopHeads(new Context(_instructionset, 0));

			_basicBlocks.Capacity = _loopHeads.Count + 2;

			// Start with a prologue block...
			BasicBlock prologue = new BasicBlock(-1);
			prologue.Index = 0;
			_basicBlocks.Add(prologue);

			// Add a jump instruction to the first block from the prologue
			Context ctx = new Context(_instructionset, 0).InsertBefore();
			ctx.SetInstruction(CIL.Instruction.Get(OpCode.Br));
			//ctx.Instruction = Map.GetInstruction(OpCode.Br);
			ctx.Branch = new Branch(1);
			ctx.Branch.Targets[0] = 0;

			// Create the epilogue block
			BasicBlock epilogue = new BasicBlock(Int32.MaxValue);
			epilogue.Index = _loopHeads.Count + 1;

			// Add epilogue block to leaders (helps with loop below)
			_loopHeads.Add(epilogue.Label, epilogue);

			// Link prologue block to the first leader
			LinkBlocks(prologue, _loopHeads[0]);

			InsertInstructionsIntoBlocks(_loopHeads, epilogue);

			// Add the epilogue block
			_basicBlocks.Add(epilogue);
		}

		/// <summary>
		/// Finds the loop heads.
		/// </summary>
		/// <param name="ctx">The CTX.</param>
		private void FindLoopHeads(Context ctx)
		{
			while (!ctx.EndOfInstruction) {
				// Retrieve the instruction
				ICILInstruction instruction = ctx.Instruction as ICILInstruction;

				// Does this instruction end a block?
				switch (instruction.FlowControl) {
					case FlowControl.Break: goto case FlowControl.Next;
					case FlowControl.Call: goto case FlowControl.Next;
					case FlowControl.Next: break;

					case FlowControl.Return:
						if (!ctx.IsLastInstruction)
							AddLoopHead(ctx.Next.Offset);
						break;

					case FlowControl.Switch: goto case FlowControl.ConditionalBranch;
					case FlowControl.Branch: goto case FlowControl.ConditionalBranch;

					case FlowControl.ConditionalBranch:
						// Conditional branch with multiple targets
						foreach (int target in (instruction as IBranchInstruction).BranchTargets)
							AddLoopHead(target);
						goto case FlowControl.Throw;

					case FlowControl.Throw:
						// End the block, start a new one on the next statement
						if (!ctx.IsLastInstruction)
							AddLoopHead(ctx.Next.Offset);
						break;

					default:
						Debug.Assert(false);
						break;
				}
				ctx.Forward();
			}
		}

		/// <summary>
		/// Adds the leader.
		/// </summary>
		/// <param name="index">The index.</param>
		public void AddLoopHead(int index)
		{
			if (!_loopHeads.ContainsKey(index))
				_loopHeads.Add(index, new BasicBlock(index));
		}

		/// <summary>
		/// Inserts the instructions into blocks.
		/// </summary>
		/// <param name="leaders">The leaders.</param>
		/// <param name="epilogue">The epilogue.</param>
		private void InsertInstructionsIntoBlocks(IDictionary<int, BasicBlock> leaders, BasicBlock epilogue)
		{
			KeyValuePair<int, BasicBlock> current = new KeyValuePair<int, BasicBlock>(-1, null);
			int blockIndex = 0;

			foreach (KeyValuePair<int, BasicBlock> next in leaders) {
				if (current.Key != -1) {

					// Insert block into list of basic Blocks
					_basicBlocks.Add(current.Value);

					// Set the block index
					current.Value.Index = ++blockIndex;

					Context ctx = new Context(_instructionset, current.Key);
					ctx.BasicBlock = current.Value;

					// Set the block index on all the instructions
					while ((ctx.Index != next.Key) && !ctx.EndOfInstruction) {
						ctx.Block = blockIndex;
						ctx.Forward();
					}

					ctx.Backwards();

					InsertFlowControl(ctx, current.Value, next.Key, epilogue);
				}

				current = next;
			}
		}

		/// <summary>
		/// Inserts the flow control.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="current">The current.</param>
		/// <param name="nextBlock">The next block.</param>
		/// <param name="epilogue">The epilogue.</param>
		private void InsertFlowControl(Context ctx, BasicBlock current, int nextBlock, BasicBlock epilogue)
		{
			switch ((ctx.Instruction as BaseInstruction).FlowControl) {
				case FlowControl.Break: goto case FlowControl.Next;
				case FlowControl.Call: goto case FlowControl.Next;
				case FlowControl.Next:
					// Insert unconditional branch to next basic block
					Context inserted = ctx.InsertAfter();
					inserted.SetInstruction(CIL.Instruction.Get(OpCode.Br_s));
//					inserted.Instruction = Map.GetInstruction(OpCode.Br_s);
					inserted.Branch = new Branch(1);
					inserted.Branch.Targets[0] = nextBlock;

					ctx.SliceAfter();
					LinkBlocks(current, _loopHeads[nextBlock]);
					break;

				case FlowControl.Return:
					// Insert unconditional branch to epilogue block
					LinkBlocks(current, epilogue);
					break;

				case FlowControl.Switch:
					// Switch may fall through
					goto case FlowControl.ConditionalBranch;

				case FlowControl.Branch: goto case FlowControl.ConditionalBranch;

				case FlowControl.ConditionalBranch:
					// Conditional branch with multiple targets
					foreach (int target in ctx.Branch.Targets)
						LinkBlocks(current, _loopHeads[target]);
					goto case FlowControl.Throw;

				case FlowControl.Throw:
					// End the block, start a new one on the next statement
					break;

				default:
					Debug.Assert(false);
					break;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pipeline"></param>
		public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertBefore<CilToIrTransformationStage>(this);
		}

		/// <summary>
		/// Links the Blocks.
		/// </summary>
		/// <param name="caller">The caller.</param>
		/// <param name="callee">The callee.</param>
		private void LinkBlocks(BasicBlock caller, BasicBlock callee)
		{
			// Chain the Blocks together
			callee.PreviousBlocks.Add(caller);
			caller.NextBlocks.Add(callee);
		}

		#endregion // IMethodCompilerStage members

		#region IBasicBlockProvider members

		/// <summary>
		/// Gets the basic Blocks.
		/// </summary>
		/// <value>The basic Blocks.</value>
		public List<BasicBlock> Blocks
		{
			get { return _basicBlocks; }
		}

		/// <summary>
		/// Retrieves a basic block From its label.
		/// </summary>
		/// <param name="label">The label of the basic block.</param>
		/// <returns>
		/// The basic block with the given label or null.
		/// </returns>
		public BasicBlock FromLabel(int label)
		{
			return _basicBlocks.Find(delegate(BasicBlock block)
			{
				return (label == block.Label);
			});
		}

		/// <summary>
		/// Gibt einen Enumerator zurück, der die Auflistung durchläuft.
		/// </summary>
		/// <returns>
		/// Ein <see cref="T:System.Collections.Generic.IEnumerator`1"/>, der zum Durchlaufen der Auflistung verwendet werden kann.
		/// </returns>
		public IEnumerator<BasicBlock> GetEnumerator()
		{
			return _basicBlocks.GetEnumerator();
		}

		/// <summary>
		/// Gibt einen Enumerator zurück, der eine Auflistung durchläuft.
		/// </summary>
		/// <returns>
		/// Ein <see cref="T:System.Collections.IEnumerator"/>-Objekt, das zum Durchlaufen der Auflistung verwendet werden kann.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _basicBlocks.GetEnumerator();
		}

		#endregion // IBasicBlockProvider members
	}
}
