/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class BasicBlocks : IEnumerable<BasicBlock>
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private List<BasicBlock> basicBlocks;

		/// <summary>
		/// Holds the blocks indexed by label
		/// </summary>
		private Dictionary<int, BasicBlock> basicBlocksByLabel = new Dictionary<int, BasicBlock>();

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BasicBlocks"/> class.
		/// </summary>
		public BasicBlocks()
		{
			basicBlocks = new List<BasicBlock>();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the count.
		/// </summary>
		public int Count { get { return basicBlocks.Count; } }

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<BasicBlock> GetEnumerator()
		{
			foreach (var basicBlock in basicBlocks)
			{
				yield return basicBlock;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Gets the <see cref="Mosa.Compiler.Framework.BasicBlock"/> at the specified index.
		/// </summary>
		public BasicBlock this[int index]
		{
			get { return basicBlocks[index]; }
			set { basicBlocks[index] = value; }
		}

		/// <summary>
		/// Gets the prologue block.
		/// </summary>
		public BasicBlock PrologueBlock { get { return this.GetByLabel(BasicBlock.PrologueLabel); } }

		/// <summary>
		/// Gets the epilogue block.
		/// </summary>
		public BasicBlock EpilogueBlock { get { return this.GetByLabel(BasicBlock.EpilogueLabel); } }

		#endregion

		#region Methods

		/// <summary>
		/// Creates the block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public BasicBlock CreateBlock(int label, int index)
		{
			BasicBlock basicBlock = new BasicBlock(basicBlocks.Count, label, index);
			basicBlocks.Add(basicBlock);
			basicBlocksByLabel.Add(label, basicBlock);
			return basicBlock;
		}

		/// <summary>
		/// Creates the block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		public BasicBlock CreateBlock(int label)
		{
			BasicBlock basicBlock = new BasicBlock(basicBlocks.Count, label, -1);
			basicBlocks.Add(basicBlock);
			basicBlocksByLabel.Add(label, basicBlock);
			return basicBlock;
		}

		/// <summary>
		/// Retrieves a basic block from its label.
		/// </summary>
		/// <param name="label">The label of the basic block.</param>
		/// <returns>
		/// The basic block with the given label.
		/// </returns>
		public BasicBlock GetByLabel(int label)
		{
			BasicBlock basicBlock = null;
			basicBlocksByLabel.TryGetValue(label, out basicBlock);
			return basicBlock;
		}

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		public void LinkBlocks(BasicBlock source, BasicBlock destination)
		{
			if (!source.NextBlocks.Contains(destination))
				source.NextBlocks.Add(destination);

			if (!destination.PreviousBlocks.Contains(source))
				destination.PreviousBlocks.Add(source);
		}

		#endregion

		public List<BasicBlock> GetConnectedBlocksStartingAtHead(BasicBlock start)
		{
			List<BasicBlock> connected = new List<BasicBlock>();
			BitArray visited = new BitArray(Count, false);

			Stack<BasicBlock> stack = new Stack<BasicBlock>();
			stack.Push(start);

			while (stack.Count != 0)
			{
				BasicBlock at = stack.Pop();
				if (!visited.Get(at.Sequence))
				{
					visited.Set(at.Sequence, true);
					connected.Add(at);

					foreach (BasicBlock next in at.NextBlocks)
						if (!visited.Get(next.Sequence))
							stack.Push(next);
				}
			}

			return connected;
		}

		public BasicBlock GetExitBlock(BasicBlock start)
		{
			BitArray visited = new BitArray(Count, false);

			Stack<BasicBlock> stack = new Stack<BasicBlock>();
			stack.Push(start);

			while (stack.Count != 0)
			{
				BasicBlock at = stack.Pop();
				if (!visited.Get(at.Sequence))
				{
					visited.Set(at.Sequence, true);
					
					if (at.NextBlocks.Count == 0)
						return at;

					foreach (BasicBlock next in at.NextBlocks)
						if (!visited.Get(next.Sequence))
							stack.Push(next);
				}
			}

			return null;
		}


		public static List<BasicBlock> ReversePostorder(BasicBlock head)
		{
			List<BasicBlock> result = new List<BasicBlock>();
			Queue<BasicBlock> workList = new Queue<BasicBlock>();

			// Add next blocks
			foreach (BasicBlock next in head.NextBlocks)
				workList.Enqueue(next);

			while (workList.Count != 0)
			{
				BasicBlock current = workList.Dequeue();
				if (!result.Contains(current))
				{
					result.Add(current);
					foreach (BasicBlock next in current.NextBlocks)
						workList.Enqueue(next);
				}
			}

			return result;
		}
	}
}
