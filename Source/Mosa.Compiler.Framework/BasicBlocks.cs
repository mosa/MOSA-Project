// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Basic Blocks
	/// </summary>
	public sealed class BasicBlocks : IEnumerable<BasicBlock>
	{
		#region Data Members

		/// <summary>
		/// The basic blocks
		/// </summary>
		private readonly List<BasicBlock> basicBlocks = new List<BasicBlock>();

		/// <summary>
		/// Holds the blocks indexed by label
		/// </summary>
		private readonly Dictionary<int, BasicBlock> basicBlocksByLabel = new Dictionary<int, BasicBlock>();

		/// <summary>
		/// The head blocks
		/// </summary>
		private readonly List<BasicBlock> headBlocks = new List<BasicBlock>();

		/// <summary>
		/// The handler blocks
		/// </summary>
		private readonly List<BasicBlock> handlerHeadBlocks = new List<BasicBlock>();

		/// <summary>
		/// The try blocks
		/// </summary>
		private readonly List<BasicBlock> tryHeadBlocks = new List<BasicBlock>();

		/// <summary>
		/// The prologue block
		/// </summary>
		private BasicBlock prologueBlock = null;

		/// <summary>
		/// The epilogue block
		/// </summary>
		private BasicBlock epilogueBlock = null;

		private int nextAvailableLabel = BasicBlock.CompilerBlockStartLabel;

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BasicBlocks"/> class.
		/// </summary>
		public BasicBlocks()
		{
		}

		#endregion Construction

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
		/// Gets the <see cref="Mosa.Compiler.Framework.BasicBlock" /> at the specified index.
		/// </summary>
		/// <value>
		/// The <see cref="BasicBlock"/>.
		/// </value>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public BasicBlock this[int index]
		{
			get { return basicBlocks[index]; }
			set { basicBlocks[index] = value; }
		}

		/// <summary>
		/// Gets the head blocks.
		/// </summary>
		public IList<BasicBlock> HeadBlocks { get { return headBlocks.AsReadOnly(); } }

		/// <summary>
		/// Gets the handler head blocks.
		/// </summary>
		public IList<BasicBlock> HandlerHeadBlocks { get { return handlerHeadBlocks.AsReadOnly(); } }

		/// <summary>
		/// Gets the try head blocks.
		/// </summary>
		public IList<BasicBlock> TryHeadBlocks { get { return tryHeadBlocks.AsReadOnly(); } }

		/// <summary>
		/// Gets the prologue block.
		/// </summary>
		public BasicBlock PrologueBlock
		{
			get
			{
				return prologueBlock ?? (prologueBlock = GetByLabel(BasicBlock.PrologueLabel));
			}
		}

		/// <summary>
		/// Gets the epilogue block.
		/// </summary>
		public BasicBlock EpilogueBlock
		{
			get
			{
				return epilogueBlock ?? (epilogueBlock = GetByLabel(BasicBlock.EpilogueLabel));
			}
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Creates the block.
		/// </summary>
		/// <param name="blockLabel">The label.</param>
		/// <returns></returns>
		public BasicBlock CreateBlock(int blockLabel = -1, int instructionLabel = -1)
		{
			if (blockLabel < 0)
				blockLabel = nextAvailableLabel++;

			if (instructionLabel < 0)
				instructionLabel = blockLabel;

			var basicBlock = new BasicBlock(basicBlocks.Count, blockLabel, instructionLabel);
			basicBlocks.Add(basicBlock);
			basicBlocksByLabel.Add(blockLabel, basicBlock);
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
			basicBlocksByLabel.TryGetValue(label, out BasicBlock basicBlock);
			return basicBlock;
		}

		/// <summary>
		/// Determines whether [contains] [the specified block].
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns></returns>
		public bool Contains(BasicBlock block)
		{
			return basicBlocks.Contains(block);
		}

		/// <summary>
		/// Adds the header block.
		/// </summary>
		/// <param name="basicBlock">The basic block.</param>
		public void AddHeadBlock(BasicBlock basicBlock)
		{
			headBlocks.Add(basicBlock);
		}

		/// <summary>
		/// Adds the handler block.
		/// </summary>
		/// <param name="basicBlock">The basic block.</param>
		public void AddHandlerHeadBlock(BasicBlock basicBlock)
		{
			handlerHeadBlocks.Add(basicBlock);
			basicBlock.IsHandlerHeadBlock = true;
		}

		/// <summary>
		/// Adds the try block.
		/// </summary>
		/// <param name="basicBlock">The basic block.</param>
		public void AddTryHeadBlock(BasicBlock basicBlock)
		{
			tryHeadBlocks.Add(basicBlock);
			basicBlock.IsTryHeadBlock = true;
		}

		/// <summary>
		/// Removes the header block.
		/// </summary>
		/// <param name="basicBlock">The basic block.</param>
		public void RemoveHeaderBlock(BasicBlock basicBlock)
		{
			if (headBlocks.Contains(basicBlock))
				headBlocks.Remove(basicBlock);
		}

		/// <summary>
		/// Determines whether [is header block] [the specified basic block].
		/// </summary>
		/// <param name="basicBlock">The basic block.</param>
		/// <returns>
		///   <c>true</c> if [is header block] [the specified basic block]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsHeadBlock(BasicBlock basicBlock)
		{
			return headBlocks.Contains(basicBlock);
		}

		/// <summary>
		/// Reorders the blocks.
		/// </summary>
		/// <param name="newBlockOrder">The new block order.</param>
		public void ReorderBlocks(IList<BasicBlock> newBlockOrder)
		{
			basicBlocks.Clear();
			basicBlocksByLabel.Clear();

			int sequence = 0;
			foreach (var block in newBlockOrder)
			{
				if (block != null)
				{
					basicBlocks.Add(block);
					block.Sequence = sequence++;
					basicBlocksByLabel.Add(block.Label, block);
				}
			}

			epilogueBlock = null;
		}

		public void OrderByLabel()
		{
			var blocks = new SortedList<int, BasicBlock>(basicBlocks.Count);

			foreach (var block in basicBlocks)
			{
				blocks.Add(block.Label, block);
			}

			ReorderBlocks(blocks.Values);
		}

		#endregion Methods

		public List<BasicBlock> GetConnectedBlocksStartingAtHead(BasicBlock start)
		{
			var connected = new List<BasicBlock>();
			var visited = new BitArray(Count, false);

			var stack = new Stack<BasicBlock>();
			stack.Push(start);

			while (stack.Count != 0)
			{
				var at = stack.Pop();
				if (!visited.Get(at.Sequence))
				{
					visited.Set(at.Sequence, true);
					connected.Add(at);

					foreach (var next in at.NextBlocks)
					{
						if (!visited.Get(next.Sequence))
						{
							stack.Push(next);
						}
					}
				}
			}

			return connected;
		}

		public BasicBlock GetExitBlock(BasicBlock start)
		{
			var visited = new BitArray(Count, false);

			var stack = new Stack<BasicBlock>();
			stack.Push(start);

			while (stack.Count != 0)
			{
				var at = stack.Pop();
				if (!visited.Get(at.Sequence))
				{
					visited.Set(at.Sequence, true);

					if (at.NextBlocks.Count == 0)
						return at;

					foreach (var next in at.NextBlocks)
					{
						if (!visited.Get(next.Sequence))
						{
							stack.Push(next);
						}
					}
				}
			}

			return null;
		}

		public static List<BasicBlock> ReversePostorder(BasicBlock head)
		{
			var result = new List<BasicBlock>();
			var workList = new Queue<BasicBlock>();

			// Add next block
			workList.Enqueue(head);

			while (workList.Count != 0)
			{
				var current = workList.Dequeue();
				if (!result.Contains(current))
				{
					result.Add(current);
					foreach (var next in current.NextBlocks)
						workList.Enqueue(next);
				}
			}

			return result;
		}
	}
}
