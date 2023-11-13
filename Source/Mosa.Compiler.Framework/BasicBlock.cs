// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Represents a block of instructions with no internal jumps and only one entry and exit.
/// </summary>
public sealed class BasicBlock : IComparable<BasicBlock>
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
	private readonly List<Node> branchInstructions = new List<Node>(2);

	#endregion Data Fields

	#region Properties

	/// <summary>
	/// Gets the first instruction node.
	/// </summary>
	public Node First { get; }

	/// <summary>
	/// Gets the last instruction node.
	/// </summary>
	public Node Last { get; }

	/// <summary>
	/// Gets the before last instruction node.
	/// </summary>
	public Node BeforeLast => Last.Previous;

	/// <summary>
	/// Gets the instruction after the first instruction.
	/// </summary>
	public Node AfterFirst => First.Next;

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
	public bool HasNextBlocks => NextBlocks.Count > 0;

	/// <summary>
	/// <True/> if this Block has previous blocks
	/// </summary>
	public bool HasPreviousBlocks => PreviousBlocks.Count > 0;

	/// <summary>
	/// Gets a value indicating whether this instance is prologue.
	/// </summary>
	/// <value>
	/// <c>true</c> if this instance is prologue; otherwise, <c>false</c>.
	/// </value>
	public bool IsPrologue => Label == PrologueLabel;

	/// <summary>
	/// Gets a value indicating whether this instance is epilogue.
	/// </summary>
	/// <value>
	/// <c>true</c> if this instance is epilogue; otherwise, <c>false</c>.
	/// </value>
	public bool IsEpilogue => Label == EpilogueLabel;

	/// <summary>
	/// Gets a value indicating whether this instance is compiler block.
	/// </summary>
	/// <value>
	/// <c>true</c> if this instance is compiler block; otherwise, <c>false</c>.
	/// </value>
	public bool IsCompilerBlock => Label >= CompilerBlockStartLabel && Label != EpilogueLabel && Label != PrologueLabel;

	public bool IsHandlerHeadBlock { get; internal set; }

	public bool IsTryHeadBlock { get; internal set; }

	public bool IsHeadBlock { get; internal set; }

	public Context ContextBeforeBranch => new(BeforeBranch);

	public Node BeforeBranch
	{
		get
		{
			var node = BeforeLast;

			while (node.IsEmpty
				|| node.Instruction.IsUnconditionalBranch
				|| node.Instruction.IsConditionalBranch
				|| node.Instruction.IsReturn)
			{
				node = node.Previous;
			}

			return node;
		}
	}

	/// <summary>
	/// Gets if the block is completely empty.
	/// </summary>
	public bool IsCompletelyEmpty { get; private set; } = false;

	#endregion Properties

	#region Construction

	internal BasicBlock(int sequence, int blockLabel, int instructionLabel)
	{
		Debug.Assert(blockLabel != ReservedLabel);

		NextBlocks = new List<BasicBlock>(2);
		PreviousBlocks = new List<BasicBlock>(1);
		Label = blockLabel;
		Sequence = sequence;

		First = new Node(IR.BlockStart)
		{
			Label = instructionLabel,
			Block = this
		};

		var middle = new Node
		{
			Label = instructionLabel,
			Block = this
		};

		Last = new Node(IR.BlockEnd)
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

	internal void AddBranchInstruction(Node node)
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

	internal void RemoveBranchInstruction(Node node)
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
		return $"L_{Label:X5}";
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

	#region Advanced Methods

	/// <summary>
	/// Determines whether [is empty block with single jump] [the specified block].
	/// </summary>
	/// <returns>
	///   <c>true</c> if [is empty block with single jump] [the specified block]; otherwise, <c>false</c>.
	/// </returns>
	public bool IsEmptyBlockWithSingleJump()
	{
		if (NextBlocks.Count != 1)
			return false;

		for (var node = AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
		{
			if (node.IsEmptyOrNop)
				continue;

			if (!node.Instruction.IsUnconditionalBranch)
				return false;
		}

		return true;
	}

	public void RemoveEmptyBlockWithSingleJump(bool useNop = false)
	{
		Debug.Assert(NextBlocks.Count == 1);

		var target = NextBlocks[0];

		foreach (var previous in PreviousBlocks.ToArray())
		{
			BasicBlocks.ReplaceBranchTargets(previous, this, target);
		}

		EmptyBlockOfAllInstructions(useNop);

		Debug.Assert(PreviousBlocks.Count == 0);
	}

	/// <summary>
	/// Empties the block of all instructions.
	/// </summary>
	/// <param name="useNop"></param>
	public bool EmptyBlockOfAllInstructions(bool useNop = false)
	{
		var found = false;

		for (var node = AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
		{
			if (node.IsEmpty)
				continue;

			if (node.IsNop)
			{
				if (!useNop)
					node.Empty();

				continue;
			}

			node.SetNop();

			found = true;
		}

		IsCompletelyEmpty = true;

		return found;
	}

	public bool HasPhiInstruction()
	{
		for (var node = AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
		{
			if (node.IsEmptyOrNop)
				continue;

			if (node.Instruction.IsPhi)
				return true;

			return false;
		}

		return false;
	}

	public void RemoveNops()
	{
		for (var node = AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
		{
			if (node.IsEmpty || !node.IsNop)
				continue;

			node.Empty();
		}
	}

	#endregion Advanced Methods

	public int CompareTo(BasicBlock other)
	{
		if (other == null)
			return 1;

		return Label.CompareTo(other.Label);
	}
}
