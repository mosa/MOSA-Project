/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
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
		/// The label of the block. (For simplicity this is actually the original instruction offset.)
		/// </summary>
		private int _label;

		/// <summary>
		/// Links this block to all Blocks invoked by the final branch instruction.
		/// </summary>
		/// <remarks>
		/// Usually there are two Blocks in this list: The branch destination and
		/// the immediately following block. If the final branch instruction is a
		/// switch, there are potentially more Blocks in this list.
		/// </remarks>
		private List<BasicBlock> _nextBlocks;

		/// <summary>
		/// A list of all Blocks, whose final branch instruction refers to this block.
		/// </summary>
		private List<BasicBlock> _previousBlocks;

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
			_label = label;
		}

		/// <summary>
		/// Initializes a new instance of <see cref="BasicBlock"/>.
		/// </summary>
		/// <param name="label">The label of the block (IL instruction offset from the method start.)</param>
		/// <param name="index">The index.</param>
		public BasicBlock(int label, int index) :
			this()
		{
			_label = label;
			_index = index;
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
		/// Retrieves the label, which uniquely identifies this block.
		/// </summary>
		/// <value>The label.</value>
		public int Label
		{
			get { return _label; }
		}

		/// <summary>
		/// Returns a list of all Blocks, which are potential branch targets
		/// of the last instruction in this block.
		/// </summary>
		public List<BasicBlock> NextBlocks
		{
			get { return _nextBlocks; }
		}

		/// <summary>
		/// Returns a list of all Blocks, which branch to this block.
		/// </summary>
		public List<BasicBlock> PreviousBlocks
		{
			get { return _previousBlocks; }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>The code as a string value.</returns>
		public override string ToString()
		{
			return String.Format(@"L_{0:X4}", Label);
		}

		#endregion // Methods
	}
}