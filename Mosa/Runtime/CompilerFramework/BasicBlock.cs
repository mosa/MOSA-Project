/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */


using System;
using System.Collections;
using System.Collections.Generic;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Represents a block of instructions with no internal jumps and only one
	/// entry and exit.
	/// </summary>
	public class BasicBlock
	{
		#region Data members

		/// <summary>
		/// The block index.
		/// </summary>
		private int _index;

		/// <summary>
		/// The index of the first instruction in the block.
		/// </summary>
		private List<Instruction> _instructions;

		/// <summary>
		/// The label of the block. (For simplicity this is actually the original instruction offset.)
		/// </summary>
		private int _label;

		/// <summary>
		/// Links this block to all blocks invoked by the final branch instruction.
		/// </summary>
		/// <remarks>
		/// Usually there are two blocks in this list: The branch destination and
		/// the immediately following block. If the final branch instruction is a
		/// switch, there are potentially more blocks in this list.
		/// </remarks>
		private List<BasicBlock> _nextBlocks;

		/// <summary>
		/// A list of all blocks, whose final branch instruction refers to this block.
		/// </summary>
		private List<BasicBlock> _previousBlocks;

		/// <summary>
		/// Holds state for compilation stages.
		/// </summary>
		private Dictionary<string, object> _state;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes common fields of the BasicBlock.
		/// </summary>
		private BasicBlock()
		{
			_nextBlocks = new List<BasicBlock>();
			_previousBlocks = new List<BasicBlock>();
			_index = -1;
		}

		/// <summary>
		/// Initializes a new instance of <see cref="BasicBlock"/>.
		/// </summary>
		/// <param name="label">The label of the block (IL instruction offset from the method start.)</param>
		public BasicBlock(int label) :
			this()
		{
			_instructions = new List<Instruction>();
			_label = label;
		}

		/// <summary>
		/// Initializes a new instance of <see cref="BasicBlock"/>.
		/// </summary>
		/// <param name="instructions">The instructions of the basic block.</param>
        /// <param name="label">The label of the newly created block.</param>
		private BasicBlock(List<Instruction> instructions, int label) :
			this()
		{
			_instructions = instructions;
			_label = label;
		}

		#endregion // Construction

		#region Properties

        /// <summary>
        /// Gets/Sets the index of the basic block.
        /// </summary>
        /// <value>The index.</value>
		public int Index
		{
			get { return _index; }
			set { _index = value; }
		}

        /// <summary>
        /// Retrieves the instruction list, which belongs to the block.
        /// </summary>
        /// <value>The instructions.</value>
		public List<Instruction> Instructions
		{
			get { return _instructions; }
		}

        /// <summary>
        /// Retrieves the label, which uniquely identifies this block.
        /// </summary>
        /// <value>The label.</value>
		public int Label
		{
			get { return _label; }
		}

		/// <summary>
		/// Returns a list of all blocks, which are potential branch targets
		/// of the last instruction in this block.
		/// </summary>
		public List<BasicBlock> NextBlocks
		{
			get { return _nextBlocks; }
		}

		/// <summary>
		/// Returns a list of all blocks, which branch to this block.
		/// </summary>
		public List<BasicBlock> PreviousBlocks
		{
			get { return _previousBlocks; }
		}

		/// <summary>
		/// Holds state for compilation stages.
		/// </summary>
		public IDictionary<string, object> State
		{
			get
			{
				if (null != _state)
					return _state;

				_state = new Dictionary<string, object>();
				return _state;
			}
		}

		/// <summary>
		/// Gets the last instruction.
		/// </summary>
		/// <value>The last instruction.</value>
		public Instruction LastInstruction
		{
			get
			{
				if (_instructions.Count == 0)
					return null;
				else
					return _instructions[_instructions.Count - 1];
			}
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Splits the basic block at the given instruction index.
		/// </summary>
		/// <param name="index">The index of the first instruction of the block to create.</param>
        /// <param name="label">The label of the new block.</param>
		/// <returns>The new block with instructions starting at index.</returns>
		public BasicBlock Split(int index, int label)
		{
			// Calculate the length of the instruction range
			int length = _instructions.Count - index;
			// Create a new basic block
			BasicBlock result = new BasicBlock(_instructions.GetRange(index, length), label);

			// Remove the range of instructions from this block
			_instructions.RemoveRange(index, length);

			return result;
		}

		#endregion // Methods
	}
}