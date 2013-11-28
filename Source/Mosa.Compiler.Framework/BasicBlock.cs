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

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Represents a block of instructions with no internal jumps and only one entry and exit.
	/// </summary>
	public class BasicBlock
	{
		public static readonly int PrologueLabel = -1;
		public static readonly int EpilogueLabel = Int32.MaxValue;

		#region Data members

		/// <summary>
		/// The index to the start of the block within the instruction set
		/// </summary>
		private int start;

		/// <summary>
		/// The index to the end of the block within the instruction set
		/// </summary>
		private int end;

		/// <summary>
		/// The label of the block. (For simplicity this is actually the original instruction offset.)
		/// </summary>
		private int label;

		/// <summary>
		/// The creation sequence number of the block; unique within a method. (For use with stages that require an integer id for blocks starting from 0).
		/// </summary>
		private int sequence;

		/// <summary>
		/// Hints at which target the block will most likely branch to
		/// </summary>
		private int hintTarget;

		/// <summary>
		/// Links this block to all blocks invoked by the final branch instruction.
		/// </summary>
		/// <remarks>
		/// Usually there are two blocks in this list: The branch destination and
		/// the immediately following block. If the final branch instruction is a
		/// switch, there are potentially more blocks in this list.
		/// </remarks>
		private List<BasicBlock> nextBlocks;

		/// <summary>
		/// A list of all Blocks, whose final branch instruction refers to this block.
		/// </summary>
		private List<BasicBlock> previousBlocks;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes common fields of the BasicBlock.
		/// </summary>
		/// <param name="sequence">The sequence.</param>
		/// <param name="label">The label.</param>
		/// <param name="start">The start.</param>
		public BasicBlock(int sequence, int label, int start)
		{
			nextBlocks = new List<BasicBlock>(2);
			previousBlocks = new List<BasicBlock>(2);
			this.sequence = sequence;
			this.label = label;
			this.start = start;
			this.end = -1;
			this.hintTarget = -1;
		}

		/// <summary>
		/// Initializes common fields of the BasicBlock.
		/// </summary>
		/// <param name="sequence">The sequence.</param>
		/// <param name="label">The label.</param>
		/// <param name="start">The index.</param>
		/// <param name="end">The end.</param>
		public BasicBlock(int sequence, int label, int start, int end)
		{
			nextBlocks = new List<BasicBlock>(2);
			previousBlocks = new List<BasicBlock>(2);
			this.sequence = sequence;
			this.label = label;
			this.start = start;
			this.end = end;
			this.hintTarget = -1;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// The index to the start of the block within the instruction set
		/// </summary>
		public int StartIndex
		{
			get { return start; }
			set { start = value; }
		}

		/// <summary>
		/// The index to the end of the block within the instruction set
		/// </summary>
		public int EndIndex
		{
			get { return end; }
			set { end = value; }
		}

		/// <summary>
		/// Retrieves the label, which uniquely identifies this block.
		/// </summary>
		/// <value>The label.</value>
		public int Label
		{
			get { return label; }
		}

		/// <summary>
		/// Retrieves the label, which uniquely identifies this block.
		/// </summary>
		/// <value>The label.</value>
		public int Sequence
		{
			get { return sequence; }
			internal set { sequence = value; }
		}

		/// <summary>
		/// Returns a list of all Blocks, which are potential branch targets
		/// of the last instruction in this block.
		/// </summary>
		public List<BasicBlock> NextBlocks
		{
			get { return nextBlocks; }
		}

		/// <summary>
		/// Returns a list of all Blocks, which branch to this block.
		/// </summary>
		public List<BasicBlock> PreviousBlocks
		{
			get { return previousBlocks; }
		}

		/// <summary>
		/// <True/> if this Block has following blocks
		/// </summary>
		public bool HasNextBlocks
		{
			get { return NextBlocks.Count > 0; }
		}

		/// <summary>
		/// <True/> if this Block has previous blocks
		/// </summary>
		public bool HasPreviousBlocks
		{
			get { return PreviousBlocks.Count > 0; }
		}

		/// <summary>
		/// Hints at which target the block will most likely branch to
		/// </summary>
		public int HintTarget
		{
			get { return hintTarget; }
			set { hintTarget = value; }
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>The code as a string value.</returns>
		public override string ToString()
		{
			return String.Format("L_{0:X4}", Label);
		}

		#endregion Methods
	}
}