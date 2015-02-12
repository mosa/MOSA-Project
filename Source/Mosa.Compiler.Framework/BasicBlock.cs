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

		#region Construction

		/// <summary>
		/// Initializes common fields of the BasicBlock.
		/// </summary>
		/// <param name="sequence">The sequence.</param>
		/// <param name="label">The label.</param>
		/// <param name="start">The start.</param>
		public BasicBlock(int sequence, int label, int start)
		{
			NextBlocks = new List<BasicBlock>(2);
			PreviousBlocks = new List<BasicBlock>(1);
			Sequence = sequence;
			Label = label;
			StartIndex = start;
			EndIndex = -1;
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
			NextBlocks = new List<BasicBlock>(2);
			PreviousBlocks = new List<BasicBlock>(1);
			Sequence = sequence;
			Label = label;
			StartIndex = start;
			EndIndex = end;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// The index to the start of the block within the instruction set
		/// </summary>
		public int StartIndex { get; set; }

		/// <summary>
		/// The index to the end of the block within the instruction set
		/// </summary>
		public int EndIndex { get; set; }

		/// <summary>
		/// Retrieves the label, which uniquely identifies this block.
		/// </summary>
		/// <value>The label.</value>
		public int Label { get; private set; }

		/// <summary>
		/// Retrieves the label, which uniquely identifies this block.
		/// </summary>
		/// <value>The label.</value>
		public int Sequence { get; internal set; }

		/// <summary>
		/// Returns a list of all Blocks, which are potential branch targets
		/// of the last instruction in this block.
		/// </summary>
		public List<BasicBlock> NextBlocks { get; internal set; }

		/// <summary>
		/// Returns a list of all Blocks, which branch to this block.
		/// </summary>
		public List<BasicBlock> PreviousBlocks { get; internal set; }

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
