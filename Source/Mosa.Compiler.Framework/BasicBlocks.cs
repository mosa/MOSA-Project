// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

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
		public List<BasicBlock> HeadBlocks { get; } = new List<BasicBlock>();

		/// <summary>
		/// Gets the handler head blocks.
		/// </summary>
		public List<BasicBlock> HandlerHeadBlocks { get; } = new List<BasicBlock>();

		/// <summary>
		/// Gets the try head blocks.
		/// </summary>
		public List<BasicBlock> TryHeadBlocks { get; } = new List<BasicBlock>();

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
		/// <param name="instructionLabel">The instruction label.</param>
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
			HeadBlocks.Add(basicBlock);
			basicBlock.IsHeadBlock = true;
		}

		/// <summary>
		/// Adds the handler block.
		/// </summary>
		/// <param name="basicBlock">The basic block.</param>
		public void AddHandlerHeadBlock(BasicBlock basicBlock)
		{
			HandlerHeadBlocks.Add(basicBlock);
			basicBlock.IsHandlerHeadBlock = true;
		}

		/// <summary>
		/// Adds the try block.
		/// </summary>
		/// <param name="basicBlock">The basic block.</param>
		public void AddTryHeadBlock(BasicBlock basicBlock)
		{
			TryHeadBlocks.Add(basicBlock);
			basicBlock.IsTryHeadBlock = true;
		}

		/// <summary>
		/// Removes the header block.
		/// </summary>
		/// <param name="basicBlock">The basic block.</param>
		public void RemoveHeaderBlock(BasicBlock basicBlock)
		{
			if (HeadBlocks.Contains(basicBlock))
			{
				HeadBlocks.Remove(basicBlock);
				basicBlock.IsHeadBlock = false;
			}
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

		public List<BasicBlock> ReversePostorder(BasicBlock head)
		{
			var array = new BitArray(Count, false);
			var result = new List<BasicBlock>();
			var worklist = new Queue<BasicBlock>();

			worklist.Enqueue(head);

			while (worklist.Count != 0)
			{
				var current = worklist.Dequeue();

				if (!array.Get(current.Sequence))
				{
					result.Add(current);
					array.Set(current.Sequence, true);

					foreach (var next in current.NextBlocks)
					{
						worklist.Enqueue(next);
					}
				}
			}

			return result;
		}

		public void RuntimeValidationWithFail()
		{
			Debug.Assert(RuntimeValidation(), "BasicBlocks Validation Failed");
		}

		public bool RuntimeValidation()
		{
			foreach (var block in basicBlocks)
			{
				foreach (var previous in block.PreviousBlocks)
				{
					if (!basicBlocks.Contains(previous))
					{
						return false;
					}
				}

				foreach (var next in block.NextBlocks)
				{
					if (!basicBlocks.Contains(next))
					{
						return false;
					}
				}
			}

			return true;
		}
	}
}
