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
using System.Text;

using Mosa.Runtime.CompilerFramework.IL;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// This compilation stage is used by method compilers after the
	/// IL decoding stage to build basic blocks out of the instruction list.
	/// </summary>
	public sealed class BasicBlockBuilderStage : IMethodCompilerStage, IBasicBlockProvider
	{
		#region Data members

		/// <summary>
		/// List of basic blocks found during decoding.
		/// </summary>
		private List<BasicBlock> basicBlocks;

		/// <summary>
		/// List of leaders
		/// </summary>
		private SortedDictionary<int, BasicBlock> leaders;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BasicBlockBuilderStage"/> class.
		/// </summary>
		public BasicBlockBuilderStage()
		{
			basicBlocks = new List<BasicBlock>();
			leaders = new SortedDictionary<int, BasicBlock>();
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
			// Retrieve the instruction provider
			IInstructionsProvider ip = (IInstructionsProvider)compiler.GetPreviousStage(typeof(IInstructionsProvider));

			// Architecture
			IArchitecture arch = compiler.Architecture;

			AddLeader(0);

			for (int index = 0; index < ip.Instructions.Count; index++) {
				// Retrieve the instruction
				Instruction instruction = ip.Instructions[index];

				// Does this instruction end a block?
				switch (instruction.FlowControl) {
					case FlowControl.Break: goto case FlowControl.Next;
					case FlowControl.Call: goto case FlowControl.Next;
					case FlowControl.Next: break;

					case FlowControl.Return:
						if (index + 1 < ip.Instructions.Count)
							AddLeader(ip.Instructions[index + 1].Offset);
						break;

					case FlowControl.Switch: goto case FlowControl.ConditionalBranch;
					case FlowControl.Branch: goto case FlowControl.ConditionalBranch;

					case FlowControl.ConditionalBranch:
						// Conditional branch with multiple targets
						foreach (int target in (instruction as IBranchInstruction).BranchTargets)
							AddLeader(target);
						goto case FlowControl.Throw;

					case FlowControl.Throw:
						// End the block, start a new one on the next statement
						if (index + 1 < ip.Instructions.Count)
							AddLeader(ip.Instructions[index + 1].Offset);
						break;

					default:
						Debug.Assert(false);
						break;
				}
			}

			basicBlocks.Capacity = leaders.Count + 2;

			// Start with a prologue block...
			BasicBlock prologue = new BasicBlock(-1);
			prologue.Index = 0;
			basicBlocks.Add(prologue);

			// Add a jump instruction to the first block from the prologue
			prologue.Instructions.Insert(0, arch.CreateInstruction(typeof(IL.BranchInstruction), OpCode.Br, new int[] { 0 }));

			// Create the epilogue block
			BasicBlock epilogue = new BasicBlock(Int32.MaxValue);
			epilogue.Index = leaders.Count + 1;

			// Add epilogue block to leaders (helps with loop below)
			leaders.Add(epilogue.Label, epilogue);

			// Link prologue block to the first leader
			LinkBlocks(prologue, leaders[0]);

			// Insert instructions into basic blocks
			KeyValuePair<int, BasicBlock> current = new KeyValuePair<int, BasicBlock>(-1, null);
			int blockIndex = 0;
			int lastInstructionIndex = 0;

			foreach (KeyValuePair<int, BasicBlock> next in leaders) {
				if (current.Key != -1) {
					// Insert block into list of basic blocks
					basicBlocks.Add(current.Value);

					// Insert instructions into basic block
					while ((lastInstructionIndex < ip.Instructions.Count) && (ip.Instructions[lastInstructionIndex].Offset < next.Value.Label))
						current.Value.Instructions.Add(ip.Instructions[lastInstructionIndex++]);

					current.Value.Index = ++blockIndex;

					Instruction lastInstruction = ip.Instructions[lastInstructionIndex - 1];

					switch (lastInstruction.FlowControl) {
						case FlowControl.Break: goto case FlowControl.Next;
						case FlowControl.Call: goto case FlowControl.Next;
						case FlowControl.Next:
							// Insert unconditional branch to next basic block
							BranchInstruction branch = new BranchInstruction(OpCode.Br_s, new int[] { next.Key });
							current.Value.Instructions.Add(branch);
							LinkBlocks(current.Value, leaders[next.Key]);
							break;

						case FlowControl.Return:
							// Insert unconditional branch to epilogue block
							LinkBlocks(current.Value, epilogue);
							break;

						case FlowControl.Switch:
							// Switch may fall through
							goto case FlowControl.ConditionalBranch;

						case FlowControl.Branch: goto case FlowControl.ConditionalBranch;

						case FlowControl.ConditionalBranch:
							// Conditional branch with multiple targets
							foreach (int target in (lastInstruction as IBranchInstruction).BranchTargets)
								LinkBlocks(current.Value, leaders[target]);
							goto case FlowControl.Throw;

						case FlowControl.Throw:
							// End the block, start a new one on the next statement
							break;

						default:
							Debug.Assert(false);
							break;
					}

				}

				current = next;
			}

			// Add the epilogue block
			basicBlocks.Add(epilogue);
		}

		/// <summary>
		/// Adds the leader.
		/// </summary>
		/// <param name="index">The index.</param>
		public void AddLeader(int index)
		{
			if (!leaders.ContainsKey(index))
				leaders.Add(index, new BasicBlock(index));
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
		/// Links the blocks.
		/// </summary>
		/// <param name="caller">The caller.</param>
		/// <param name="callee">The callee.</param>
		private void LinkBlocks(BasicBlock caller, BasicBlock callee)
		{
			// Chain the blocks together
			callee.PreviousBlocks.Add(caller);
			caller.NextBlocks.Add(callee);
		}

		#endregion // IMethodCompilerStage members

		#region IBasicBlockProvider members

		/// <summary>
		/// Gets the basic blocks.
		/// </summary>
		/// <value>The basic blocks.</value>
		public List<BasicBlock> Blocks
		{
			get { return this.basicBlocks; }
		}

		/// <summary>
		/// Retrieves a basic block from its label.
		/// </summary>
		/// <param name="label">The label of the basic block.</param>
		/// <returns>
		/// The basic block with the given label or null.
		/// </returns>
		public BasicBlock FromLabel(int label)
		{
			return this.basicBlocks.Find(delegate(BasicBlock block)
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
			return this.basicBlocks.GetEnumerator();
		}

		/// <summary>
		/// Gibt einen Enumerator zurück, der eine Auflistung durchläuft.
		/// </summary>
		/// <returns>
		/// Ein <see cref="T:System.Collections.IEnumerator"/>-Objekt, das zum Durchlaufen der Auflistung verwendet werden kann.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.basicBlocks.GetEnumerator();
		}

		#endregion // IBasicBlockProvider members
	}
}
