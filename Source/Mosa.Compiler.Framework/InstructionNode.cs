// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Compiler.Framework
{
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
		/// Holds the branch targets
		/// </summary>
		private List<BasicBlock> branchTargets;

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
			get
			{
				return basicBlock;
			}
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
			get
			{
				return operand1;
			}
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
			get
			{
				return operand2;
			}
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
			get
			{
				return operand3;
			}
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
					for (int i = 3; i < OperandCount; i++)
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
			get
			{
				return result;
			}
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
			get
			{
				return result2;
			}
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
		public List<BasicBlock> BranchTargets { get { return branchTargets; } }

		/// <summary>
		/// Gets the branch targets count.
		/// </summary>
		/// <value>
		/// The branch targets count.
		/// </value>
		public int BranchTargetsCount { get { return branchTargets?.Count ?? 0; } }

		/// <summary>
		/// Sets the branch target.
		/// </summary>
		/// <param name="block">The basic block.</param>
		public void AddBranchTarget(BasicBlock block)
		{
			Debug.Assert(block != null);

			(branchTargets ?? (branchTargets = new List<BasicBlock>(1))).Add(block);

			Block?.AddBranchInstruction(this);
		}

		public void UpdateBranchTarget(int index, BasicBlock block)
		{
			// no change, skip update
			if (branchTargets[index] == block)
				return;

			Block.RemoveBranchInstruction(this);

			branchTargets[index] = block;

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
		/// Gets or sets the invoke method.
		/// </summary>
		/// <value>
		/// The invoke method.
		/// </value>
		public MosaMethod InvokeMethod
		{
			get { return addition?.InvokeMethod; }
			set { CheckAddition(); addition.InvokeMethod = value; }
		}

		/// <summary>
		/// Gets or sets the runtime field.
		/// </summary>
		/// <value>The runtime field.</value>
		public MosaField MosaField
		{
			get { return addition?.MosaField; }
			set { CheckAddition(); addition.MosaField = value; }
		}

		/// <summary>
		/// Gets or sets the runtime field.
		/// </summary>
		/// <value>The runtime field.</value>
		public MosaType MosaType
		{
			get { return addition?.MosaType; }
			set { CheckAddition(); addition.MosaType = value; }
		}

		/// <summary>
		/// Gets or sets the phi blocks.
		/// </summary>
		/// <value>
		/// The phi blocks.
		/// </value>
		public List<BasicBlock> PhiBlocks
		{
			get { return addition?.PhiBlocks; }
			set { CheckAddition(); addition.PhiBlocks = value; }
		}

		/// <summary>
		/// Gets a value indicating whether this is the start instruction.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this is the first instruction; otherwise, <c>false</c>.
		/// </value>
		public bool IsBlockStartInstruction
		{
			get { return Instruction == IRInstruction.BlockStart; }
		}

		/// <summary>
		/// Gets a value indicating whether this is the last instruction.
		/// </summary>
		/// <value><c>true</c> if this is the last instruction; otherwise, <c>false</c>.</value>
		public bool IsBlockEndInstruction
		{
			get { return Instruction == IRInstruction.BlockEnd; }
		}

		/// <summary>
		/// Gets a value indicating whether this node is empty.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
		/// </value>
		public bool IsEmpty
		{
			get { return Instruction == null; }
		}

		/// <summary>
		/// Gets a value indicating whether this node is a NOP instruction.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is nop; otherwise, <c>false</c>.
		/// </value>
		public bool IsNop
		{
			get { return Instruction == IRInstruction.Nop; }
		}

		/// <summary>
		/// Gets a value indicating whether this node is empty or a NOP instruction.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is empty or nop; otherwise, <c>false</c>.
		/// </value>
		public bool IsEmptyOrNop
		{
			get { return Instruction == null || Instruction == IRInstruction.Nop; }
		}

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
			branchTargets = null;
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
			branchTargets = null;

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

		public void CutFrom(InstructionNode startNode, InstructionNode endNode)
		{
			//var cutBlock = startNode.Previous.Block;

			startNode.Previous.Next = endNode.Next;
			endNode.Next.Previous = startNode.Previous;

			//cutBlock.DebugCheck();

			startNode.Previous = null;
			endNode.Next = null;

			Insert(startNode);
		}

		public void CutFrom(InstructionNode node)
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

			for (int i = 3; i < OperandCount; i++)
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
			switch (opIndex)
			{
				case 0: return Operand1;
				case 1: return Operand2;
				case 2: return Operand3;
				default: return GetAdditionalOperand(opIndex);
			}
		}

		/// <summary>
		/// Gets the result.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public Operand GetResult(int index)
		{
			switch (index)
			{
				case 0: return Result;
				case 1: return Result2;
				default: throw new IndexOutOfRangeException();
			}
		}

		/// <summary>
		/// Adds the operand.
		/// </summary>
		/// <param name="operand">The operand.</param>
		public void AddOperand(Operand operand)
		{
			SetOperand(OperandCount, operand);
			OperandCount++;
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
			for (int i = offset; i < operands.Count; i++)
			{
				SetOperand(OperandCount++, operands[i]);
			}
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

			for (int i = 0; i < old.Length; i++)
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

			sb.AppendFormat("{0:X5}:", Label);

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

			for (int i = 0; i < ResultCount; i++)
			{
				var op = GetResult(i);
				sb.Append($" {(op == null ? "[NULL]" : op.ToString(false))},");
			}

			if (ResultCount > 0)
			{
				sb.Length--;
			}

			if (ResultCount > 0 && OperandCount > 0)
			{
				sb.Append(" <=");
			}

			for (int i = 0; i < OperandCount; i++)
			{
				var op = GetOperand(i);
				sb.Append($" {(op == null ? "[NULL]" : op.ToString(false))},");
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
				sb.Append("}");
			}

			if (BranchTargets != null)
			{
				sb.Append(' ');

				for (int i = 0; (i < 2) && (i < BranchTargetsCount); i++)
				{
					if (i != 0)
					{
						sb.Append(", ");
					}

					sb.Append(BranchTargets[i].ToString());
				}

				if (BranchTargetsCount > 2)
				{
					sb.Append(", [more]");
				}
			}

			if (InvokeMethod != null)
			{
				sb.Append($" {{m={InvokeMethod.DeclaringType.Name}.{InvokeMethod.Name}}}");
			}

			if (MosaType != null)
			{
				sb.Append($" {{t={MosaType.Name}}}");
			}

			if (MosaField != null)
			{
				sb.Append($" {{f={MosaField.DeclaringType.Name}.{MosaField.Name}}}");
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

		/// <summary>
		/// Returns the 1st non empty node (including the current) by traversing the instructions forward
		/// </summary>
		/// <returns></returns>
		public InstructionNode GoForwardToNonEmpty()
		{
			var node = this;

			while (node.IsEmpty)
			{
				node = node.Next;
			}

			return node;
		}

		/// <summary>
		/// Returns the 1st non empty node (including the current) by traversing the instructions backwards
		/// </summary>
		/// <returns></returns>
		public InstructionNode GoBackwardsToNonEmpty()
		{
			var node = this;

			while (node.IsEmpty)
			{
				node = node.Previous;
			}

			return node;
		}

		#endregion Methods

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

			int label = Label;
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
			SetInstruction(instruction, 1, (byte)((result == null) ? 0 : 1));
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
			SetInstruction(instruction, 0, (byte)((result == null) ? 0 : 1));
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
			SetInstruction(instruction, 1, (byte)((result == null) ? 0 : 1));
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
			SetInstruction(instruction, 2, (byte)((result == null) ? 0 : 1));
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
			SetInstruction(instruction, 3, (byte)((result == null) ? 0 : 1));
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
			SetInstruction(instruction, 4, (byte)((result == null) ? 0 : 1));
			Result = result;
			Operand1 = operand1;
			Operand2 = operand2;
			Operand3 = operand3;
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
			SetInstruction(instruction, 2, (byte)((result == null) ? 0 : 1));
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
			SetInstruction(instruction, 1, (byte)((result == null) ? 0 : 1));
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
			SetInstruction(instruction, 1, (byte)((result == null) ? 0 : 1));
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
			SetInstruction(instruction, 0, (byte)((result == null) ? 0 : 1));
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
			SetInstruction(instruction, 2, (byte)((result == null) ? 0 : 1));
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
			SetInstruction(instruction, 3, (byte)((result == null) ? 0 : 1));
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
			SetInstruction(instruction, 2, (byte)((result == null) ? 0 : 1));
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
			SetInstruction(instruction, 2, (byte)((result == null) ? 0 : 1));
			Result = result;
			Operand1 = operand1;
			Operand2 = operand2;
			ConditionCode = condition;
			StatusRegister = statusRegister;
		}

		#endregion SetInstructions
	}
}
