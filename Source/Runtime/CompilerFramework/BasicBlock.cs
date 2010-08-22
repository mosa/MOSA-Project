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
using Mosa.Runtime.CompilerFramework.Operands;

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
		/// The index of the block within the InstructionSet
		/// </summary>
		public int Index;

		/// <summary>
		/// The label of the block. (For simplicity this is actually the original instruction offset.)
		/// </summary>
		private int _label;

		/// <summary>
		/// The creation sequence number of the block; unique within a method. (For use with stage that require an integer id for blocks starting from 0).
		/// </summary>
		private int _sequence;

		/// <summary>
		/// Hints at which target the block will most likely branch to
		/// </summary>
		public int HintTarget;

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

		/// <summary>
		///
		/// </summary>
		private Stack<Operand> _initialStack;

		#endregion

		#region Construction

		/// <summary>
		/// Initializes common fields of the BasicBlock.
		/// </summary>
		private BasicBlock()
		{
			_nextBlocks = new List<BasicBlock>();
			_previousBlocks = new List<BasicBlock>();
		}

		/// <summary>
		/// Initializes a new instance of <see cref="BasicBlock"/>.
		/// </summary>
		/// <param name="sequence">The sequence.</param>
		/// <param name="label">The label of the block (IL instruction offset from the method start.)</param>
		/// <param name="index">The index.</param>
		public BasicBlock(int sequence, int label, int index)
			: this()
		{
			_sequence = sequence;
			_label = label;
			Index = index;
			HintTarget = -1;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Retrieves the label, which uniquely identifies this block.
		/// </summary>
		/// <value>The label.</value>
		public int Label
		{
			get { return _label; }
		}

		/// <summary>
		/// Retrieves the label, which uniquely identifies this block.
		/// </summary>
		/// <value>The label.</value>
		public int Sequence
		{
			get { return _sequence; }
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
		/// Gets or sets the initial ingoing operand stack
		/// </summary>
		public Stack<Operand> InitialStack
		{
			get { return _initialStack; }
			set { _initialStack = value; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>The code as a string value.</returns>
		public override string ToString()
		{
			return String.Format("L_{0:X4}", Label);
		}

		#endregion
	}
}
