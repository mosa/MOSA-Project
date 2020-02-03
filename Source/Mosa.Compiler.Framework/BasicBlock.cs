// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Represents a block of instructions with no internal jumps and only one entry and exit.
	/// </summary>
	public sealed class BasicBlock
	{
		public const int PrologueLabel = 0x10000;
		public const int StartLabel = 0;
		public const int EpilogueLabel = 0xFFFFF;
		public const int CompilerBlockStartLabel = 0x10001;
		public const int ReservedLabel = StartLabel - 1;

		#region Data Fields

		/// <summary>
		/// The branch instructions
		/// </summary>
		private readonly List<InstructionNode> branchInstructions = new List<InstructionNode>(2);

		#endregion Data Fields

		#region Properties

		/// <summary>
		/// Gets the first instruction node.
		/// </summary>
		public InstructionNode First { get; }

		/// <summary>
		/// Gets the last instruction node.
		/// </summary>
		public InstructionNode Last { get; }

		/// <summary>
		/// Gets the before last instruction node.
		/// </summary>
		public InstructionNode BeforeLast { get { return Last.Previous; } }

		/// <summary>
		/// Gets the instruction after the first instruction.
		/// </summary>
		public InstructionNode AfterFirst { get { return First.Next; } }

		/// <summary>
		/// Retrieves the label, which uniquely identifies this block.
		/// </summary>
		/// <value>The label.</value>
		public int Label { get; }

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
		public bool HasNextBlocks { get { return NextBlocks.Count > 0; } }

		/// <summary>
		/// <True/> if this Block has previous blocks
		/// </summary>
		public bool HasPreviousBlocks { get { return PreviousBlocks.Count > 0; } }

		/// <summary>
		/// Gets a value indicating whether this instance is prologue.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is prologue; otherwise, <c>false</c>.
		/// </value>
		public bool IsPrologue { get { return Label == PrologueLabel; } }

		/// <summary>
		/// Gets a value indicating whether this instance is epilogue.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is epilogue; otherwise, <c>false</c>.
		/// </value>
		public bool IsEpilogue { get { return Label == EpilogueLabel; } }

		/// <summary>
		/// Gets a value indicating whether this instance is compiler block.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is compiler block; otherwise, <c>false</c>.
		/// </value>
		public bool IsCompilerBlock { get { return (Label >= CompilerBlockStartLabel) && (Label != EpilogueLabel) && (Label != PrologueLabel); } }

		public bool IsHandlerHeadBlock { get; internal set; }

		public bool IsTryHeadBlock { get; internal set; }

		public bool IsHeadBlock { get; internal set; }

		#endregion Properties

		#region Construction

		internal BasicBlock(int sequence, int blockLabel, int instructionLabel)
		{
			Debug.Assert(blockLabel != ReservedLabel);

			NextBlocks = new List<BasicBlock>(2);
			PreviousBlocks = new List<BasicBlock>(1);
			Label = blockLabel;
			Sequence = sequence;

			First = new InstructionNode(IRInstruction.BlockStart)
			{
				Label = instructionLabel,
				Block = this
			};

			var middle = new InstructionNode()
			{
				Label = instructionLabel,
				Block = this
			};

			Last = new InstructionNode(IRInstruction.BlockEnd)
			{
				Label = instructionLabel,
				Block = this,
			};

			middle.Next = Last;
			middle.Previous = First;

			First.Next = middle;
			Last.Previous = middle;
		}

		#endregion Construction

		#region Methods

		internal void AddBranchInstruction(InstructionNode node)
		{
			if (node.Instruction?.IgnoreInstructionBasicBlockTargets == true)
				return;

			if (node.BranchTargets == null || node.BranchTargetsCount == 0)
				return;

			Debug.Assert(node.Block != null);

			// Note: The list only has 1 unless it's a switch statement, so actual performance is very close to O(1) for non-switch statements

			branchInstructions.AddIfNew(node);

			var currentBlock = node.Block;

			foreach (var target in node.BranchTargets)
			{
				currentBlock.NextBlocks.AddIfNew(target);
				target.PreviousBlocks.AddIfNew(currentBlock);
			}
		}

		internal void RemoveBranchInstruction(InstructionNode node)
		{
			if (node.BranchTargets == null || node.BranchTargetsCount == 0)
				return;

			branchInstructions.Remove(node);

			var currentBlock = node.Block;

			// Note: The list only has 1 or 2 entries, so actual performance is very close to O(1)

			foreach (var target in node.BranchTargets)
			{
				if (!FindTarget(target))
				{
					currentBlock.NextBlocks.Remove(target);
					target.PreviousBlocks.Remove(currentBlock);
				}
			}
		}

		private bool FindTarget(BasicBlock block)
		{
			foreach (var b in branchInstructions)
			{
				if (b.BranchTargets.Contains(block))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>The code as a string value.</returns>
		public override string ToString()
		{
			return String.Format("L_{0:X5}", Label);
		}

		/// <summary>
		/// Checks that all instructions are part of the block.
		/// </summary>
		public void DebugCheck()
		{
			Debug.Assert(First.Block == Last.Block);
			Debug.Assert(First.Label == Last.Label);

			var node = First;

			while (!node.IsBlockEndInstruction)
			{
				Debug.Assert(node.Block == this);
				node = node.Next;
				Debug.Assert(node != null);
			}

			Debug.Assert(node == Last);

			node = Last;

			while (!node.IsBlockStartInstruction)
			{
				Debug.Assert(node.Block == this);
				node = node.Previous;
				Debug.Assert(node != null);
			}

			Debug.Assert(node == First);
		}

		#endregion Methods
	}
}
