// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.Text;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Instruction Node
/// </summary>
public sealed class InstructionNode
{
	#region Data Members

	/// <summary>
	/// Holds the first operand of the instruction.
	/// </summary>
	private Operand operand1;

	/// <summary>
	/// Holds the second operand of the instruction.
	/// </summary>
	private Operand operand2;

	/// <summary>
	/// Holds the third operand of the instruction.
	/// </summary>
	private Operand operand3;

	/// <summary>
	/// Holds the result first operand of the instruction.
	/// </summary>
	private Operand result;

	/// <summary>
	/// Holds the second result operand of the instruction.
	/// </summary>
	private Operand result2;

	/// <summary>
	/// Holds the basic block that this instruction belongs to
	/// </summary>
	private BasicBlock basicBlock;

	/// <summary>
	/// The additional properties of an instruction node
	/// </summary>
	private InstructionNodeAddition addition;

	#endregion Data Members

	#region Properties

	/// <summary>
	/// Gets or sets the previous instruction node.
	/// </summary>
	public InstructionNode Previous { get; set; }

	/// <summary>
	/// Gets or sets the next instruction node.
	/// </summary>
	public InstructionNode Next { get; set; }

	/// <summary>
	/// Holds the instruction type of this instruction
	/// </summary>
	public BaseInstruction Instruction { get; set; }

	/// <summary>
	/// Label of the instruction
	/// </summary>
	public int Label { get; set; }

	/// <summary>
	/// The order slot number (initialized by some stage)
	/// </summary>
	public int Offset { get; set; }

	/// <summary>
	/// Gets the basic block of this instruction
	/// </summary>
	public BasicBlock Block
	{
		get => basicBlock;
		internal set
		{
			basicBlock?.RemoveBranchInstruction(this);

			basicBlock = value;

			basicBlock?.AddBranchInstruction(this);
		}
	}

	/// <summary>
	/// Gets or sets the first operand.
	/// </summary>
	/// <value>The first operand.</value>
	public Operand Operand1
	{
		get => operand1;
		set
		{
			var current = operand1;
			if (current == value) return;
			if (current != null)
			{
				current.Uses.Remove(this);
			}
			if (value != null)
			{
				if (value.IsVirtualRegister || value.IsOnStack)
				{
					value.Uses.Add(this);
				}
			}

			operand1 = value;
		}
	}

	/// <summary>
	/// Gets or sets the second operand.
	/// </summary>
	/// <value>The second operand.</value>
	public Operand Operand2
	{
		get => operand2;
		set
		{
			var current = operand2;
			if (current == value) return;
			if (current != null)
			{
				current.Uses.Remove(this);
			}
			if (value != null)
			{
				if (value.IsVirtualRegister || value.IsOnStack)
				{
					value.Uses.Add(this);
				}
			}
			operand2 = value;
		}
	}

	/// <summary>
	/// Gets or sets the third operand.
	/// </summary>
	/// <value>The third operand.</value>
	public Operand Operand3
	{
		get => operand3;
		set
		{
			var current = operand3;
			if (current == value) return;
			if (current != null)
			{
				current.Uses.Remove(this);
			}
			if (value != null)
			{
				if (value.IsVirtualRegister || value.IsOnStack)
				{
					value.Uses.Add(this);
				}
			}
			operand3 = value;
		}
	}

	/// <summary>
	/// Gets or sets the fourth operand.
	/// </summary>
	/// <value>The third operand.</value>
	public Operand Operand4
	{
		get => GetOperand(3);
		set
		{
			SetOperand(3, value);
		}
	}

	/// <summary>
	/// Gets or sets the fifth operand.
	/// </summary>
	/// <value>The third operand.</value>
	public Operand Operand5
	{
		get => GetOperand(4);
		set
		{
			SetOperand(4, value);
		}
	}

	/// <summary>
	/// Gets all operands.
	/// </summary>
	/// <value>The operands.</value>
	public IEnumerable<Operand> Operands
	{
		get
		{
			if (operand1 != null)
				yield return operand1;
			if (operand2 != null)
				yield return operand2;
			if (operand3 != null)
				yield return operand3;

			if (OperandCount >= 3)
			{
				for (var i = 3; i < OperandCount; i++)
				{
					yield return GetAdditionalOperand(i);
				}
			}
		}
	}

	/// <summary>
	/// Gets all results.
	/// </summary>
	/// <value>The operands.</value>
	public IEnumerable<Operand> Results
	{
		get
		{
			if (result != null)
				yield return result;
			if (result2 != null)
				yield return result2;
		}
	}

	/// <summary>
	/// Gets or sets the result operand.
	/// </summary>
	/// <value>The result operand.</value>
	public Operand Result
	{
		get => result;
		set
		{
			var current = result;
			if (current != null)
			{
				current.Definitions.Remove(this);
			}
			if (value != null)
			{
				if (value.IsVirtualRegister || value.IsOnStack)
				{
					value.Definitions.Add(this);
				}
			}
			result = value;
		}
	}

	/// <summary>
	/// Gets or sets the result operand.
	/// </summary>
	/// <value>The result operand.</value>
	public Operand Result2
	{
		get => result2;
		set
		{
			var current = result2;
			if (current != null)
			{
				current.Definitions.Remove(this);
			}
			if (value != null)
			{
				if (value.IsVirtualRegister || value.IsOnStack)
				{
					value.Definitions.Add(this);
				}
			}
			result2 = value;
		}
	}

	/// <summary>
	/// The condition code
	/// </summary>
	public ConditionCode ConditionCode { get; set; }

	public StatusRegister StatusRegister { get; set; }

	/// <summary>
	/// Holds branch targets
	/// </summary>
	public List<BasicBlock> BranchTargets { get; private set; }

	/// <summary>
	/// Gets the branch targets count.
	/// </summary>
	/// <value>
	/// The branch targets count.
	/// </value>
	public int BranchTargetsCount => BranchTargets?.Count ?? 0;

	/// <summary>
	/// Sets the branch target.
	/// </summary>
	/// <param name="block">The basic block.</param>
	public void AddBranchTarget(BasicBlock block)
	{
		Debug.Assert(block != null);

		(BranchTargets ?? (BranchTargets = new List<BasicBlock>(1))).Add(block);

		Block?.AddBranchInstruction(this);
	}

	public void UpdateBranchTarget(int index, BasicBlock block)
	{
		// no change, skip update
		if (BranchTargets[index] == block)
			return;

		Block.RemoveBranchInstruction(this);

		BranchTargets[index] = block;

		Block.AddBranchInstruction(this);
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="InstructionNode"/> is marked.
	/// </summary>
	public bool Marked { get; set; }

	/// <summary>
	/// Gets or sets the number of operands
	/// </summary>
	public int OperandCount { get; set; }

	/// <summary>
	/// Gets or sets the number of operand results
	/// </summary>
	public int ResultCount { get; set; }

	private void CheckAddition()
	{
		if (addition == null)
		{
			addition = new InstructionNodeAddition();
		}
	}

	/// <summary>
	/// Gets or sets the phi blocks.
	/// </summary>
	/// <value>
	/// The phi blocks.
	/// </value>
	public List<BasicBlock> PhiBlocks
	{
		get => addition?.PhiBlocks;
		set { CheckAddition(); addition.PhiBlocks = value; }
	}

	/// <summary>
	/// Gets a value indicating whether this is the start instruction.
	/// </summary>
	/// <value>
	/// 	<c>true</c> if this is the first instruction; otherwise, <c>false</c>.
	/// </value>
	public bool IsBlockStartInstruction => Instruction == IRInstruction.BlockStart;

	/// <summary>
	/// Gets a value indicating whether this is the last instruction.
	/// </summary>
	/// <value><c>true</c> if this is the last instruction; otherwise, <c>false</c>.</value>
	public bool IsBlockEndInstruction => Instruction == IRInstruction.BlockEnd;

	/// <summary>
	/// Gets a value indicating whether this node is empty.
	/// </summary>
	/// <value>
	///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
	/// </value>
	public bool IsEmpty => Instruction == null;

	/// <summary>
	/// Gets a value indicating whether this node is a NOP instruction.
	/// </summary>
	/// <value>
	///   <c>true</c> if this instance is nop; otherwise, <c>false</c>.
	/// </value>
	public bool IsNop => Instruction == IRInstruction.Nop;

	/// <summary>
	/// Gets a value indicating whether this node is empty or a NOP instruction.
	/// </summary>
	/// <value>
	///   <c>true</c> if this instance is empty or nop; otherwise, <c>false</c>.
	/// </value>
	public bool IsEmptyOrNop => Instruction == null || Instruction == IRInstruction.Nop;

	#endregion Properties

	#region Methods

	/// <summary>
	/// Clears this instance.
	/// </summary>
	private void Clear()
	{
		Label = -1;
		Instruction = null;

		ClearOperands();

		ConditionCode = ConditionCode.Undefined;
		StatusRegister = StatusRegister.NotSet;
		addition = null;
		Block = null;
		BranchTargets = null;
	}

	/// <summary>
	/// Empties this node.
	/// </summary>
	public void Empty()
	{
		ClearOperands();

		ConditionCode = ConditionCode.Undefined;
		StatusRegister = StatusRegister.NotSet;
		Instruction = null;
		addition = null;
		Block.RemoveBranchInstruction(this);
		BranchTargets = null;

		//Block.DebugCheck();
	}

	public void Insert(InstructionNode node)
	{
		//Block.DebugCheck();

		var firstnode = node;
		var lastnode = node;

		node.Block = Block;

		while (firstnode.Previous != null)
		{
			firstnode = firstnode.Previous;
			firstnode.Block = Block;
		}

		while (lastnode.Next != null)
		{
			lastnode = lastnode.Next;
			lastnode.Block = Block;
		}

		lastnode.Next = Next;
		firstnode.Previous = this;

		Next.Previous = lastnode;
		Next = firstnode;

		Debug.Assert(this != Next);
		Debug.Assert(this != Previous);

		//Block.DebugCheck();
	}

	public void MoveFrom(InstructionNode startNode, InstructionNode endNode)
	{
		//var cutBlock = startNode.Previous.Block;

		startNode.Previous.Next = endNode.Next;
		endNode.Next.Previous = startNode.Previous;

		//cutBlock.DebugCheck();

		startNode.Previous = null;
		endNode.Next = null;

		Insert(startNode);
	}

	public void MoveFrom(InstructionNode node)
	{
		//var cutBlock = node.Previous.Block;

		node.Previous.Next = node.Next;
		node.Next.Previous = node.Previous;

		//cutBlock.DebugCheck();

		node.Previous = null;
		node.Next = null;

		Insert(node);
	}

	/// <summary>
	/// Replaces the specified node with the given node, the existing node is invalid afterwards
	/// </summary>
	/// <param name="node">The node.</param>
	public void Replace(InstructionNode node)
	{
		//Block.DebugCheck();

		Debug.Assert(!IsBlockStartInstruction);
		Debug.Assert(!IsBlockEndInstruction);
		Debug.Assert(!node.IsBlockStartInstruction);
		Debug.Assert(!node.IsBlockEndInstruction);

		node.Label = Label;

		node.Previous = Previous;
		node.Next = Next;

		node.Previous.Next = node;
		node.Next.Previous = node;

		// clear operations
		ClearOperands();
		Instruction = null;

		//Block.DebugCheck();
	}

	/// <summary>
	/// Splits the node list by moving the next instructions into the new block.
	/// </summary>
	/// <param name="newblock">The newblock.</param>
	public void Split(BasicBlock newblock)
	{
		//Debug.Assert(!IsBlockEndInstruction);

		if (Next == Block.Last)
			return;

		//Block.DebugCheck();
		//newblock.DebugCheck();

		// check that new block is empty
		//Debug.Assert(newblock.First.Next == newblock.Last);
		//Debug.Assert(newblock.Last.Previous == newblock.First);

		newblock.First.Next = Next;
		newblock.Last.Previous = Block.Last.Previous;
		newblock.First.Next.Previous = newblock.First;
		newblock.Last.Previous.Next = newblock.Last;

		Next = Block.Last;
		Block.Last.Previous = this;

		for (var node = newblock.First.Next; !node.IsBlockEndInstruction; node = node.Next)
		{
			node.Block = newblock;
		}

		//Block.DebugCheck();
		//newblock.DebugCheck();
	}

	private void ClearOperands()
	{
		// Remove operands
		Operand1 = null;
		Operand2 = null;
		Operand3 = null;
		Result = null;
		Result2 = null;

		for (var i = 3; i < OperandCount; i++)
		{
			SetOperand(i, null);
		}
	}

	/// <summary>
	/// Sets the operand by index.
	/// </summary>
	/// <param name="index">The index.</param>
	/// <param name="operand">The operand.</param>
	public void SetOperand(int index, Operand operand)
	{
		switch (index)
		{
			case 0: Operand1 = operand; return;
			case 1: Operand2 = operand; return;
			case 2: Operand3 = operand; return;
			default:
				{
					var current = GetAdditionalOperand(index);
					if (current == operand) return;
					if (current != null)
					{
						current.Uses.Remove(this);
					}

					if (operand != null)
					{
						if (operand.IsVirtualRegister || operand.IsOnStack)
						{
							operand.Uses.Add(this);
						}
					}

					SetAdditionalOperand(index, operand);
					return;
				}
		}
	}

	/// <summary>
	/// Gets the operand by index.
	/// </summary>
	/// <param name="opIndex">The index.</param>
	/// <returns></returns>
	public Operand GetOperand(int opIndex)
	{
		return opIndex switch
		{
			0 => Operand1,
			1 => Operand2,
			2 => Operand3,
			_ => GetAdditionalOperand(opIndex)
		};
	}

	/// <summary>
	/// Gets the result.
	/// </summary>
	/// <param name="index">The index.</param>
	/// <returns></returns>
	public Operand GetResult(int index)
	{
		return index switch
		{
			0 => Result,
			1 => Result2,
			_ => throw new IndexOutOfRangeException()
		};
	}

	/// <summary>
	/// Sets the result by index.
	/// </summary>
	/// <param name="index">The index.</param>
	/// <param name="operand">The operand.</param>
	public void SetResult(int index, Operand operand)
	{
		switch (index)
		{
			case 0: Result = operand; return;
			case 1: Result2 = operand; return;
			default: throw new IndexOutOfRangeException();
		}
	}

	/// <summary>
	/// Appends the operands.
	/// </summary>
	/// <param name="operands">The operands.</param>
	/// <param name="offset">The offset.</param>
	public void AppendOperands(IList<Operand> operands, int offset = 0)
	{
		for (var i = offset; i < operands.Count; i++)
		{
			SetOperand(OperandCount++, operands[i]);
		}
	}

	public void RemoveOperand(int index)
	{
		SetOperand(index, null);

		for (var i = index; i < OperandCount - 1; i++)
		{
			switch (i)
			{
				case 0: operand1 = operand2; continue;
				case 1: operand2 = operand3; continue;
				case 2: operand3 = addition.AdditionalOperands[i - 2]; continue;
				case 3: addition.AdditionalOperands[i - 3] = addition.AdditionalOperands[i - 2 + 1]; continue;
			}
		}

		OperandCount--;
	}

	/// <summary>
	/// Gets the operands.
	/// </summary>
	/// <returns></returns>
	public List<Operand> GetOperands()
	{
		return new List<Operand>(Operands);
	}

	/// <summary>
	/// Sets the additional operand.
	/// </summary>
	/// <param name="index">The index.</param>
	/// <param name="operand">The operand.</param>
	private void SetAdditionalOperand(int index, Operand operand)
	{
		CheckAddition();

		//if (addition.AdditionalOperands == null) addition.AdditionalOperands = new Operand[253];
		//Debug.Assert(index < 255, @"No Index");
		Debug.Assert(index >= 3, "No Index");

		SizeAdditionalOperands(index - 3 + 1);

		addition.AdditionalOperands[index - 3] = operand;
	}

	private void SizeAdditionalOperands(int index)
	{
		if (addition.AdditionalOperands == null)
		{
			var minsize = Math.Max(index, 8);

			addition.AdditionalOperands = new Operand[minsize];
			return;
		}

		if (index < addition.AdditionalOperands.Length)
			return;

		var old = addition.AdditionalOperands;

		var newsize = Math.Max(index, old.Length * 2);

		addition.AdditionalOperands = new Operand[newsize];

		for (var i = 0; i < old.Length; i++)
		{
			addition.AdditionalOperands[i] = old[i];
		}
	}

	/// <summary>
	/// Gets the additional operand.
	/// </summary>
	/// <param name="index">The index.</param>
	/// <returns></returns>
	private Operand GetAdditionalOperand(int index)
	{
		if (addition == null || addition.AdditionalOperands == null)
			return null;

		Debug.Assert(index >= 3, "No Index");

		//Debug.Assert(index < 255, @"No Index");

		SizeAdditionalOperands(index - 3 + 1);

		return addition.AdditionalOperands[index - 3];
	}

	/// <summary>
	/// Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <returns>
	/// A <see cref="System.String" /> that represents this instance.
	/// </returns>
	public override string ToString()
	{
		if (Instruction == null)
			return "<none>";

		// TODO: Copy this method into calling class
		var sb = new StringBuilder();

		sb.AppendFormat($"{Label:X5}:");

		if (Marked)
			sb.Append('*');
		else
			sb.Append(' ');

		sb.Append(Instruction.FullName);

		if (ConditionCode != ConditionCode.Undefined)
		{
			sb.Append($" [{ConditionCode.GetConditionString()}]");
		}

		if (Instruction.Modifier != null)
		{
			sb.Append($" [{Instruction.Modifier}]");
		}

		for (var i = 0; i < ResultCount; i++)
		{
			var op = GetResult(i);
			sb.Append($" {(op == null ? "[NULL]" : op.ToString())},");
		}

		if (ResultCount > 0)
		{
			sb.Length--;
		}

		if (ResultCount > 0 && OperandCount > 0)
		{
			sb.Append(" <=");
		}

		for (var i = 0; i < OperandCount; i++)
		{
			var op = GetOperand(i);
			sb.Append($" {(op == null ? "[NULL]" : op.ToString())},");
		}

		if (OperandCount > 0)
		{
			sb.Length--;
		}

		if (PhiBlocks != null)
		{
			sb.Append(" {");
			foreach (var block in PhiBlocks)
			{
				sb.AppendFormat("{0}, ", block);
			}
			sb.Length -= 2;
			sb.Append('}');
		}

		if (BranchTargets != null)
		{
			sb.Append(' ');

			for (var i = 0; i < 2 && i < BranchTargetsCount; i++)
			{
				if (i != 0)
				{
					sb.Append(", ");
				}

				sb.Append(BranchTargets[i]);
			}

			if (BranchTargetsCount > 2)
			{
				sb.Append(", [more]");
			}
		}

		return sb.ToString();
	}

	/// <summary>
	/// Replaces the instruction only.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	public void ReplaceInstruction(BaseInstruction instruction)
	{
		Instruction = instruction;
	}

	public bool ContainsOperand(Operand operand)
	{
		foreach (var op in Operands)
		{
			if (op == operand)
				return true;
		}

		return false;
	}

	public bool ContainsResult(Operand operand)
	{
		if (ResultCount == 0)
			return false;

		if (ResultCount >= 1 && Result == operand)
			return true;

		if (ResultCount == 2 && Result2 == operand)
			return true;

		return false;
	}

	public void ReplaceOperand(Operand target, Operand replacement)
	{
		for (var i = 0; i < OperandCount; i++)
		{
			var operand = GetOperand(i);

			if (operand == target)
			{
				SetOperand(i, replacement);
			}
		}
	}

	public void ReplaceResult(Operand target, Operand replacement)
	{
		for (var i = 0; i < ResultCount; i++)
		{
			var operand = GetResult(i);

			if (operand == target)
			{
				SetResult(i, replacement);
			}
		}
	}

	#endregion Methods

	#region Navigation

	public InstructionNode NextNonEmpty
	{
		get
		{
			var next = Next;

			while (next.IsEmptyOrNop)
			{
				next = next.Next;
			}

			return next.IsBlockEndInstruction ? null : next;
		}
	}

	public InstructionNode PreviousNonEmpty
	{
		get
		{
			var previous = Previous;

			while (previous.IsEmptyOrNop)
			{
				previous = previous.Previous;
			}

			return previous.IsBlockStartInstruction ? null : previous;
		}
	}

	/// <summary>Returns the 1st non empty node (including the current) by traversing the instructions forward</summary>
	public InstructionNode ForwardToNonEmpty
	{
		get
		{
			var node = this;

			while (node.IsEmpty)
			{
				node = node.Next;
			}

			return node;
		}
	}

	/// <summary>Returns the 1st non empty node (including the current) by traversing the instructions backwards</summary>
	public InstructionNode BackwardsToNonEmpty
	{
		get
		{
			var node = this;

			while (node.IsEmpty)
			{
				node = node.Previous;
			}

			return node;
		}
	}

	#endregion Navigation

	#region Constructors

	public InstructionNode()
	{
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="operandCount">The operand count.</param>
	/// <param name="resultCount">The result count.</param>
	public InstructionNode(BaseInstruction instruction, int operandCount, int resultCount)
	{
		Instruction = instruction;
		OperandCount = operandCount;
		ResultCount = resultCount;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	public InstructionNode(BaseInstruction instruction)
		: this(instruction, instruction.DefaultOperandCount, instruction.DefaultResultCount)
	{
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="block">The block.</param>
	public InstructionNode(BaseInstruction instruction, BasicBlock block)
		: this(instruction)
	{
		AddBranchTarget(block);
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="block1">The block1.</param>
	/// <param name="block2">The block2.</param>
	public InstructionNode(BaseInstruction instruction, BasicBlock block1, BasicBlock block2)
		: this(instruction)
	{
		AddBranchTarget(block1);
		AddBranchTarget(block2);
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="condition">The condition.</param>
	public InstructionNode(BaseInstruction instruction, ConditionCode condition)
		: this(instruction)
	{
		ConditionCode = condition;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="condition">The condition.</param>
	/// <param name="block">The block.</param>
	public InstructionNode(BaseInstruction instruction, ConditionCode condition, BasicBlock block)
		: this(instruction, condition)
	{
		AddBranchTarget(block);
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="result">The result.</param>
	public InstructionNode(BaseInstruction instruction, Operand result)
		: this(instruction, 0, 1)
	{
		Result = result;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="updateStatus">if set to <c>true</c> [update status].</param>
	/// <param name="result">The result.</param>
	public InstructionNode(BaseInstruction instruction, StatusRegister statusRegister, Operand result)
		: this(instruction, result)
	{
		StatusRegister = statusRegister;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="result">The result.</param>
	/// <param name="operand1">The operand1.</param>
	public InstructionNode(BaseInstruction instruction, Operand result, Operand operand1)
		: this(instruction, 1, 1)
	{
		Result = result;
		Operand1 = operand1;
	}

	public InstructionNode(BaseInstruction instruction, Operand result, Operand operand1, Operand operand2)
		: this(instruction, 2, 1)
	{
		Result = result;
		Operand1 = operand1;
		Operand2 = operand2;
	}

	#endregion Constructors

	#region SetInstructions

	public void SetNop()
	{
		SetInstruction(IRInstruction.Nop);
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="operandCount">The operand count.</param>
	/// <param name="resultCount">The result count.</param>
	public void SetInstruction(BaseInstruction instruction, int operandCount, int resultCount)
	{
		Debug.Assert(!IsBlockStartInstruction);
		Debug.Assert(!IsBlockEndInstruction);

		var label = Label;
		var block = Block;

		Clear();

		Instruction = instruction;
		OperandCount = operandCount;
		ResultCount = resultCount;
		Label = label;
		Block = block;

		//Block.DebugCheck();
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	public void SetInstruction(BaseInstruction instruction)
	{
		if (instruction != null)
			SetInstruction(instruction, instruction.DefaultOperandCount, instruction.DefaultResultCount);
		else
			SetInstruction(null, 0, 0);

		//Block.DebugCheck();
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="block">The block.</param>
	public void SetInstruction(BaseInstruction instruction, BasicBlock block)
	{
		SetInstruction(instruction);
		AddBranchTarget(block);
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="result">The result.</param>
	/// <param name="block">The block.</param>
	public void SetInstruction(BaseInstruction instruction, Operand result, BasicBlock block)
	{
		SetInstruction(instruction, result);
		AddBranchTarget(block);
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="block1">The block1.</param>
	/// <param name="block2">The block2.</param>
	public void SetInstruction(BaseInstruction instruction, BasicBlock block1, BasicBlock block2)
	{
		SetInstruction(instruction);
		AddBranchTarget(block1);
		AddBranchTarget(block2);
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="condition">The condition.</param>
	public void SetInstruction(BaseInstruction instruction, ConditionCode condition)
	{
		SetInstruction(instruction);
		ConditionCode = condition;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="condition">The condition.</param>
	/// <param name="block">The block.</param>
	public void SetInstruction(BaseInstruction instruction, ConditionCode condition, BasicBlock block)
	{
		SetInstruction(instruction);
		ConditionCode = condition;
		AddBranchTarget(block);
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="result">The result.</param>
	public void SetInstruction(BaseInstruction instruction, Operand result)
	{
		SetInstruction(instruction, 0, 1);
		Result = result;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="updateStatus">if set to <c>true</c> [update status].</param>
	/// <param name="result">The result.</param>
	public void SetInstruction(BaseInstruction instruction, StatusRegister statusRegister, Operand result)
	{
		SetInstruction(instruction, 0, 1);
		Result = result;
		StatusRegister = statusRegister;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="result">The result.</param>
	/// <param name="operand1">The operand1.</param>
	public void SetInstruction(BaseInstruction instruction, Operand result, Operand operand1)
	{
		SetInstruction(instruction, 1, (byte)(result == null ? 0 : 1));
		Result = result;
		Operand1 = operand1;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="result">The result.</param>
	/// <param name="operands">The operands.</param>
	public void SetInstruction(BaseInstruction instruction, Operand result, List<Operand> operands)
	{
		SetInstruction(instruction, 0, (byte)(result == null ? 0 : 1));
		Result = result;
		AppendOperands(operands);
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="result">The result.</param>
	/// <param name="operand1">The operand1.</param>
	/// <param name="operands">The operands.</param>
	public void SetInstruction(BaseInstruction instruction, Operand result, Operand operand1, List<Operand> operands)
	{
		SetInstruction(instruction, result, operand1);
		AppendOperands(operands);
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="result">The result.</param>
	/// <param name="operand1">The operand1.</param>
	/// <param name="operand2">The operand2.</param>
	/// <param name="operands">The operands.</param>
	public void SetInstruction(BaseInstruction instruction, Operand result, Operand operand1, Operand operand2, IList<Operand> operands)
	{
		SetInstruction(instruction, result, operand1, operand2);
		AppendOperands(operands);
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="updateStatus">if set to <c>true</c> [update status].</param>
	/// <param name="result">The result.</param>
	/// <param name="operand1">The operand1.</param>
	public void SetInstruction(BaseInstruction instruction, StatusRegister statusRegister, Operand result, Operand operand1)
	{
		SetInstruction(instruction, 1, (byte)(result == null ? 0 : 1));
		Result = result;
		Operand1 = operand1;
		StatusRegister = statusRegister;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="result">The result.</param>
	/// <param name="result2">The result2.</param>
	public void SetInstruction2(BaseInstruction instruction, Operand result, Operand result2)
	{
		SetInstruction(instruction, 1, 2);
		Result = result;
		Result2 = result2;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="result">The result.</param>
	/// <param name="result2">The result2.</param>
	/// <param name="operand1">The operand1.</param>
	public void SetInstruction2(BaseInstruction instruction, Operand result, Operand result2, Operand operand1)
	{
		SetInstruction(instruction, 1, 2);
		Result = result;
		Result2 = result2;
		Operand1 = operand1;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="result">The result.</param>
	/// <param name="result2">The result2.</param>
	/// <param name="operand1">The operand1.</param>
	/// <param name="operand2">The operand2.</param>
	public void SetInstruction2(BaseInstruction instruction, Operand result, Operand result2, Operand operand1, Operand operand2)
	{
		SetInstruction(instruction, 2, 2);
		Result = result;
		Result2 = result2;
		Operand1 = operand1;
		Operand2 = operand2;

		//Debug.Assert(instruction.DefaultResultCount == 2);
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="result">The result.</param>
	/// <param name="result2">The result2.</param>
	/// <param name="operand1">The operand1.</param>
	/// <param name="operand2">The operand2.</param>
	/// <param name="operand3">The operand3.</param>
	public void SetInstruction2(BaseInstruction instruction, Operand result, Operand result2, Operand operand1, Operand operand2, Operand operand3)
	{
		SetInstruction(instruction, 3, 2);
		Result = result;
		Result2 = result2;
		Operand1 = operand1;
		Operand2 = operand2;
		Operand3 = operand3;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="result">The result.</param>
	/// <param name="operand1">The operand1.</param>
	/// <param name="operand2">The operand2.</param>
	public void SetInstruction(BaseInstruction instruction, Operand result, Operand operand1, Operand operand2)
	{
		SetInstruction(instruction, 2, (byte)(result == null ? 0 : 1));
		Result = result;
		Operand1 = operand1;
		Operand2 = operand2;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="result">The result.</param>
	/// <param name="operand1">The operand1.</param>
	/// <param name="operand2">The operand2.</param>
	/// <param name="operand3">The operand3.</param>
	public void SetInstruction(BaseInstruction instruction, Operand result, Operand operand1, Operand operand2, Operand operand3)
	{
		SetInstruction(instruction, 3, (byte)(result == null ? 0 : 1));
		Result = result;
		Operand1 = operand1;
		Operand2 = operand2;
		Operand3 = operand3;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="result">The result.</param>
	/// <param name="operand1">The operand1.</param>
	/// <param name="operand2">The operand2.</param>
	/// <param name="operand3">The operand3.</param>
	/// <param name="operand4">The operand4.</param>
	public void SetInstruction(BaseInstruction instruction, Operand result, Operand operand1, Operand operand2, Operand operand3, Operand operand4)
	{
		SetInstruction(instruction, 4, (byte)(result == null ? 0 : 1));
		Result = result;
		Operand1 = operand1;
		Operand2 = operand2;
		Operand3 = operand3;
		Operand4 = operand4;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="result">The result.</param>
	/// <param name="operand1">The operand1.</param>
	/// <param name="operand2">The operand2.</param>
	/// <param name="operand3">The operand3.</param>
	/// <param name="operand4">The operand4.</param>
	/// <param name="operand5">The operand4.</param>
	public void SetInstruction(BaseInstruction instruction, Operand result, Operand operand1, Operand operand2, Operand operand3, Operand operand4, Operand operand5)
	{
		SetInstruction(instruction, 4, (byte)(result == null ? 0 : 1));
		Result = result;
		Operand1 = operand1;
		Operand2 = operand2;
		Operand3 = operand3;
		Operand4 = operand4;
		Operand5 = operand5;
	}

	public void SetInstruction(BaseInstruction instruction, ConditionCode conditionCode, Operand result, Operand operand1, Operand operand2, Operand operand3, Operand operand4)
	{
		SetInstruction(instruction, 4, (byte)(result == null ? 0 : 1));
		Result = result;
		Operand1 = operand1;
		Operand2 = operand2;
		Operand3 = operand3;
		ConditionCode = conditionCode;
		SetOperand(3, operand4);
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="updateStatus">if set to <c>true</c> [update status].</param>
	/// <param name="result">The result.</param>
	/// <param name="operand1">The operand1.</param>
	/// <param name="operand2">The operand2.</param>
	public void SetInstruction(BaseInstruction instruction, StatusRegister statusRegister, Operand result, Operand operand1, Operand operand2)
	{
		SetInstruction(instruction, 2, (byte)(result == null ? 0 : 1));
		Result = result;
		Operand1 = operand1;
		Operand2 = operand2;
		StatusRegister = statusRegister;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="condition">The condition.</param>
	/// <param name="result">The result.</param>
	/// <param name="operand1">The operand1.</param>
	public void SetInstruction(BaseInstruction instruction, ConditionCode condition, Operand result, Operand operand1)
	{
		SetInstruction(instruction, 1, (byte)(result == null ? 0 : 1));
		Result = result;
		Operand1 = operand1;
		ConditionCode = condition;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="condition">The condition.</param>
	/// <param name="updateStatus">if set to <c>true</c> [update status].</param>
	/// <param name="result">The result.</param>
	/// <param name="operand1">The operand1.</param>
	public void SetInstruction(BaseInstruction instruction, StatusRegister statusRegister, ConditionCode condition, Operand result, Operand operand1)
	{
		SetInstruction(instruction, 1, (byte)(result == null ? 0 : 1));
		Result = result;
		Operand1 = operand1;
		ConditionCode = condition;
		StatusRegister = statusRegister;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="condition">The condition.</param>
	/// <param name="result">The result.</param>
	public void SetInstruction(BaseInstruction instruction, ConditionCode condition, Operand result)
	{
		SetInstruction(instruction, 0, (byte)(result == null ? 0 : 1));
		Result = result;
		ConditionCode = condition;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="condition">The condition.</param>
	/// <param name="result">The result.</param>
	/// <param name="operand1">The operand1.</param>
	/// <param name="operand2">The operand2.</param>
	public void SetInstruction(BaseInstruction instruction, ConditionCode condition, Operand result, Operand operand1, Operand operand2)
	{
		SetInstruction(instruction, 2, (byte)(result == null ? 0 : 1));
		Result = result;
		Operand1 = operand1;
		Operand2 = operand2;
		ConditionCode = condition;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="condition">The condition.</param>
	/// <param name="result">The result.</param>
	/// <param name="operand1">The operand1.</param>
	/// <param name="operand2">The operand2.</param>
	/// <param name="operand3">The operand3.</param>
	public void SetInstruction(BaseInstruction instruction, ConditionCode condition, Operand result, Operand operand1, Operand operand2, Operand operand3)
	{
		SetInstruction(instruction, 3, (byte)(result == null ? 0 : 1));
		Result = result;
		Operand1 = operand1;
		Operand2 = operand2;
		Operand3 = operand3;
		ConditionCode = condition;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="condition">The condition.</param>
	/// <param name="result">The result.</param>
	/// <param name="operand1">The operand1.</param>
	/// <param name="operand2">The operand2.</param>
	/// <param name="block">The block.</param>
	public void SetInstruction(BaseInstruction instruction, ConditionCode condition, Operand result, Operand operand1, Operand operand2, BasicBlock block)
	{
		SetInstruction(instruction, 2, (byte)(result == null ? 0 : 1));
		Result = result;
		Operand1 = operand1;
		Operand2 = operand2;
		ConditionCode = condition;
		AddBranchTarget(block);
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="condition">The condition.</param>
	/// <param name="updateStatus">if set to <c>true</c> [update status].</param>
	/// <param name="result">The result.</param>
	/// <param name="operand1">The operand1.</param>
	/// <param name="operand2">The operand2.</param>
	public void SetInstruction(BaseInstruction instruction, StatusRegister statusRegister, ConditionCode condition, Operand result, Operand operand1, Operand operand2)
	{
		SetInstruction(instruction, 2, (byte)(result == null ? 0 : 1));
		Result = result;
		Operand1 = operand1;
		Operand2 = operand2;
		ConditionCode = condition;
		StatusRegister = statusRegister;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="condition">The condition.</param>
	/// <param name="updateStatus">if set to <c>true</c> [update status].</param>
	/// <param name="result">The result.</param>
	/// <param name="operand1">The operand1.</param>
	/// <param name="operand2">The operand2.</param>
	/// <param name="operand3">The operand3.</param>
	public void SetInstruction(BaseInstruction instruction, StatusRegister statusRegister, Operand result, Operand operand1, Operand operand2, Operand operand3)
	{
		SetInstruction(instruction, 3, (byte)(result == null ? 0 : 1));
		Result = result;
		Operand1 = operand1;
		Operand2 = operand2;
		Operand3 = operand3;
		StatusRegister = statusRegister;
	}

	/// <summary>
	/// Sets the instruction.
	/// </summary>
	/// <param name="instruction">The instruction.</param>
	/// <param name="condition">The condition.</param>
	/// <param name="updateStatus">if set to <c>true</c> [update status].</param>
	/// <param name="result">The result.</param>
	/// <param name="operand1">The operand1.</param>
	/// <param name="operand2">The operand2.</param>
	/// <param name="operand3">The operand3.</param>
	public void SetInstruction(BaseInstruction instruction, StatusRegister statusRegister, Operand result, Operand operand1, Operand operand2, Operand operand3, Operand operand4)
	{
		SetInstruction(instruction, 3, (byte)(result == null ? 0 : 1));
		Result = result;
		Operand1 = operand1;
		Operand2 = operand2;
		Operand3 = operand3;
		Operand4 = operand4;
		StatusRegister = statusRegister;
	}

	#endregion SetInstructions
}
