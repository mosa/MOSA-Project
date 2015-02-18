/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Provides context for transformations.
	/// </summary>
	public sealed class Context
	{
		#region Properties

		/// <summary>
		/// Gets or sets the basic block currently processed.
		/// </summary>
		public BasicBlock Block { get { return Node.Block; } internal set { Node.Block = value; } }

		public InstructionNode Node { get; set; }

		/// <summary>
		/// Gets the instruction.
		/// </summary>
		/// <value>The instruction at the current index.</value>
		public BaseInstruction Instruction { get { return Node.Instruction; } private set { Node.Instruction = value; } }

		/// <summary>
		/// Gets or sets the label.
		/// </summary>
		/// <value>The label.</value>
		public int Label { get { return Node.Label; } set { Node.Label = value; } }

		/// <summary>
		/// The order slot number (initalized by some stage)
		/// </summary>
		/// <value>The label.</value>
		public int SlotNumber { get { return Node.SlotNumber; } set { Node.SlotNumber = value; } }

		/// <summary>
		/// Gets or sets the label.
		/// </summary>
		/// <value>The label.</value>
		public bool Marked { get { return Node.Marked; } set { Node.Marked = value; } }

		/// <summary>
		/// Gets or sets a value indicating whether [update status flags].
		/// </summary>
		/// <value>
		///   <c>true</c> if [update status flags]; otherwise, <c>false</c>.
		/// </value>
		public bool UpdateStatus { get { return Node.UpdateStatus; } set { Node.UpdateStatus = value; } }

		/// <summary>
		/// Gets the branch targets.
		/// </summary>
		/// <value>
		/// The targets.
		/// </value>
		public IList<BasicBlock> BranchTargets { get { return Node.BranchTargets; } }

		/// <summary>
		/// Gets the branch targets count.
		/// </summary>
		/// <value>
		/// The branch targets count.
		/// </value>
		public int BranchTargetsCount { get { return Node.BranchTargetsCount; } }

		/// <summary>
		/// Gets or sets the first operand.
		/// </summary>
		/// <value>The first operand.</value>
		public Operand Operand1 { get { return Node.Operand1; } set { Node.Operand1 = value; } }

		/// <summary>
		/// Gets or sets the second operand.
		/// </summary>
		/// <value>The second operand.</value>
		public Operand Operand2 { get { return Node.Operand2; } set { Node.Operand2 = value; } }

		/// <summary>
		/// Gets or sets the third operand.
		/// </summary>
		/// <value>The third operand.</value>
		public Operand Operand3 { get { return Node.Operand3; } set { Node.Operand3 = value; } }

		/// <summary>
		/// Gets all operands.
		/// </summary>
		/// <value>The operands.</value>
		public IEnumerable<Operand> Operands { get { return Node.Operands; } }

		/// <summary>
		/// Gets all results.
		/// </summary>
		/// <value>The operands.</value>
		public IEnumerable<Operand> Results { get { return Node.Results; } }

		/// <summary>
		/// Gets or sets the result operand.
		/// </summary>
		/// <value>The result operand.</value>
		public Operand Result { get { return Node.Result; } set { Node.Result = value; } }

		/// <summary>
		/// Gets or sets the result operand.
		/// </summary>
		/// <value>The result operand.</value>
		public Operand Result2 { get { return Node.Result2; } set { Node.Result2 = value; } }

		/// <summary>
		/// Gets or sets the number of operands.
		/// </summary>
		/// <value>The number of operands.</value>
		public byte OperandCount { get { return Node.OperandCount; } set { Node.OperandCount = value; } }

		/// <summary>
		/// Gets or sets number of operand results.
		/// </summary>
		/// <value>The number of operand results.</value>
		public byte ResultCount { get { return Node.ResultCount; } set { Node.ResultCount = value; } }

		/// <summary>
		/// Gets or sets the instruction size.
		/// </summary>
		/// <value>
		/// The instruction size.
		/// </value>
		public InstructionSize Size { get { return Node.Size; } set { Node.Size = value; } }

		/// <summary>
		/// Gets a value indicating whether this instance is empty.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
		/// </value>
		public bool IsEmpty { get { return Node.IsEmpty; } }

		/// <summary>
		/// Gets or sets a value indicating whether this instance has prefix.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has prefix; otherwise, <c>false</c>.
		/// </value>
		public bool HasPrefix { get { return Node.HasPrefix; } set { Node.HasPrefix = value; } }

		/// <summary>
		/// Gets or sets the branch hint (true means branch likely).
		/// </summary>
		/// <value><c>true</c> if the branch is likely; otherwise <c>false</c>.</value>
		public bool BranchHint { get { return Node.BranchHint; } set { Node.BranchHint = value; } }

		/// <summary>
		/// Gets or sets the runtime method.
		/// </summary>
		public MosaMethod InvokeMethod { get { return Node.InvokeMethod; } set { Node.InvokeMethod = value; } }

		/// <summary>
		/// Gets or sets the runtime field.
		/// </summary>
		/// <value>The runtime field.</value>
		public MosaField MosaField { get { return Node.MosaField; } set { Node.MosaField = value; } }

		/// <summary>
		/// Gets or sets the runtime field.
		/// </summary>
		/// <value>The runtime field.</value>
		public MosaType MosaType { get { return Node.MosaType; } set { Node.MosaType = value; } }

		/// <summary>
		/// Gets or sets the condition code.
		/// </summary>
		/// <value>The condition code.</value>
		public ConditionCode ConditionCode { get { return Node.ConditionCode; } set { Node.ConditionCode = value; } }

		/// <summary>
		/// Gets or sets the phi blocks.
		/// </summary>
		/// <value>
		/// The phi blocks.
		/// </value>
		public List<BasicBlock> PhiBlocks { get { return Node.PhiBlocks; } set { Node.PhiBlocks = value; } }

		/// <summary>
		/// Gets a value indicating whether this is the start instruction.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this is the first instruction; otherwise, <c>false</c>.
		/// </value>
		public bool IsBlockStartInstruction { get { return Node.IsBlockStartInstruction; } }

		/// <summary>
		/// Gets a value indicating whether this is the last instruction.
		/// </summary>
		/// <value><c>true</c> if this is the last instruction; otherwise, <c>false</c>.</value>
		public bool IsBlockEndInstruction { get { return Node.IsBlockEndInstruction; } }

		/// <summary>
		/// Gets the context of the next instruction.
		/// </summary>
		/// <value>The context of the next instruction.</value>
		public Context Next { get { return new Context(Node.Next); } }

		/// <summary>
		/// Gets the context of the previous instruction.
		/// </summary>
		/// <value>The context of the previous instruction.</value>
		public Context Previous { get { return new Context(Node.Previous); } }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Context" /> class.
		/// </summary>
		/// <param name="instructionNode">The instruction node.</param>
		public Context(InstructionNode instructionNode)
		{
			Node = instructionNode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Context" /> class.
		/// </summary>
		/// <param name="block">The block.</param>
		public Context(BasicBlock block)
			: this(block.First)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Visits the instruction by the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		public void Visit(IVisitor visitor)
		{
			Instruction.Visit(visitor, this);
		}

		/// <summary>
		/// Clones this instance.
		/// </summary>
		/// <returns>A new instance with the same instruction set, basic block and index.</returns>
		public Context Clone()
		{
			return new Context(Node);
		}

		/// <summary>
		/// Goes to the next instruction.
		/// </summary>
		public void GotoNext()
		{
			Node = Node.Next;
		}

		/// <summary>
		/// Gotos to the previous instruction.
		/// </summary>
		public void GotoPrevious()
		{
			Node = Node.Previous;
		}

		/// <summary>
		/// Goto the first instruction
		/// </summary>
		public void GotoFirst()
		{
			while (true)
			{
				if (IsBlockStartInstruction)
					break;

				GotoPrevious();
			}
		}

		/// <summary>
		/// Goto the last instruction.
		/// </summary>
		public void GotoLast()
		{
			while (true)
			{
				if (IsBlockEndInstruction)
					break;

				GotoNext();
			}
		}

		/// <summary>
		/// Appends an (empty) instruction after the current index.
		/// </summary>
		private void AppendInstruction()
		{
			Debug.Assert(!IsBlockEndInstruction);
			Debug.Assert(Block != null);

			var node = new InstructionNode();
			node.Label = Label;

			Node.Insert(node);

			Node = node;
		}

		/// <summary>
		/// Inserts an instruction before the current instruction.
		/// </summary>
		/// <returns></returns>
		public Context InsertBefore()
		{
			Debug.Assert(!IsBlockStartInstruction);

			var node = new InstructionNode();
			node.Label = Label;

			Node.Previous.Insert(node);

			return new Context(node);
		}

		/// <summary>
		/// Empties this context.
		/// </summary>
		public void Empty()
		{
			Node.Empty();
		}

		/// <summary>
		/// Sets the branch target.
		/// </summary>
		/// <param name="block">The basic block.</param>
		public void AddBranchTarget(BasicBlock block)
		{
			Node.AddBranchTarget(block);
		}

		/// <summary>
		/// Updates the branch target.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="block">The block.</param>
		public void UpdateBranchTarget(int index, BasicBlock block)
		{
			Node.UpdateBranchTarget(index, block);
		}

		/// <summary>
		/// Gets the operand by index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public Operand GetOperand(int index)
		{
			return Node.GetOperand(index);
		}

		/// <summary>
		/// Gets the result.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public Operand GetResult(int index)
		{
			return Node.GetResult(index);
		}

		/// <summary>
		/// Adds the operand.
		/// </summary>
		/// <param name="operand">The operand.</param>
		public void AddOperand(Operand operand)
		{
			Node.AddOperand(operand);
		}

		/// <summary>
		/// Sets the operand by index.
		/// </summary>
		/// <param name="opIndex">The index.</param>
		/// <param name="operand">The operand.</param>
		public void SetOperand(int opIndex, Operand operand)
		{
			Node.SetOperand(opIndex, operand);
		}

		/// <summary>
		/// Sets the result by index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="operand">The operand.</param>
		public void SetResult(int index, Operand operand)
		{
			Node.SetResult(index, operand);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return Node.ToString();
		}

		/// <summary>
		/// Replaces the instruction only.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		public void ReplaceInstructionOnly(BaseInstruction instruction)
		{
			Instruction = instruction;
		}

		#endregion Methods

		#region Set Instruction Methods

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		public void SetInstruction(BaseInstruction instruction)
		{
			Node.SetInstruction(instruction);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="operandCount">The operand count.</param>
		/// <param name="resultCount">The result count.</param>
		public void SetInstruction(BaseInstruction instruction, byte operandCount, byte resultCount)
		{
			Node.SetInstruction(instruction, operandCount, resultCount);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="target">The target.</param>
		public void SetInstruction(BaseInstruction instruction, MosaMethod target)
		{
			Node.SetInstruction(instruction, target);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="block">The block.</param>
		public void SetInstruction(BaseInstruction instruction, BasicBlock block)
		{
			Node.SetInstruction(instruction, block);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="condition">The condition.</param>
		public void SetInstruction(BaseInstruction instruction, ConditionCode condition)
		{
			Node.SetInstruction(instruction, condition);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="condition">The condition.</param>
		/// <param name="block">The block.</param>
		public void SetInstruction(BaseInstruction instruction, ConditionCode condition, BasicBlock block)
		{
			Node.SetInstruction(instruction, condition, block);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		public void SetInstruction(BaseInstruction instruction, Operand result)
		{
			Node.SetInstruction(instruction, result);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="updateStatus">if set to <c>true</c> [update status].</param>
		/// <param name="result">The result.</param>
		public void SetInstruction(BaseInstruction instruction, bool updateStatus, Operand result)
		{
			Node.SetInstruction(instruction, updateStatus, result);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		public void SetInstruction(BaseInstruction instruction, Operand result, Operand operand1)
		{
			Node.SetInstruction(instruction, result, operand1);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="size">The size.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		public void SetInstruction(BaseInstruction instruction, InstructionSize size, Operand result, Operand operand1)
		{
			Node.SetInstruction(instruction, size, result, operand1);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		public void SetInstruction(BaseInstruction instruction, bool updateStatus, Operand result, Operand operand1)
		{
			Node.SetInstruction(instruction, updateStatus, result, operand1);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		/// <param name="result2">The result2.</param>
		public void SetInstruction2(BaseInstruction instruction, Operand result, Operand result2)
		{
			Node.SetInstruction2(instruction, result, result2);
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
			Node.SetInstruction2(instruction, result, result2, operand1);
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
			Node.SetInstruction2(instruction, result, result2, operand1, operand2);
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
			Node.SetInstruction2(instruction, result, result2, operand1, operand2, operand3);
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
			Node.SetInstruction(instruction, result, operand1, operand2);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="updateStatus">if set to <c>true</c> [update status].</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		public void SetInstruction(BaseInstruction instruction, bool updateStatus, Operand result, Operand operand1, Operand operand2)
		{
			Node.SetInstruction(instruction, updateStatus, result, operand1, operand2);
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
			Node.SetInstruction(instruction, condition, result, operand1);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="condition">The condition.</param>
		/// <param name="updateStatus">if set to <c>true</c> [update status].</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		public void SetInstruction(BaseInstruction instruction, ConditionCode condition, bool updateStatus, Operand result, Operand operand1)
		{
			Node.SetInstruction(instruction, condition, updateStatus, result, operand1);
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
			Node.SetInstruction(instruction, condition, result, operand1, operand2);
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
			Node.SetInstruction(instruction, condition, result, operand1, operand2, block);
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
		public void SetInstruction(BaseInstruction instruction, ConditionCode condition, bool updateStatus, Operand result, Operand operand1, Operand operand2)
		{
			Node.SetInstruction(instruction, condition, updateStatus, result, operand1, operand2);
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
			Node.SetInstruction(instruction, result, operand1, operand2, operand3);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="size">The size.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		public void SetInstruction(BaseInstruction instruction, InstructionSize size, Operand result, Operand operand1, Operand operand2)
		{
			Node.SetInstruction(instruction, size, result, operand1, operand2);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="size">The size.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		/// <param name="operand3">The operand3.</param>
		public void SetInstruction(BaseInstruction instruction, InstructionSize size, Operand result, Operand operand1, Operand operand2, Operand operand3)
		{
			Node.SetInstruction(instruction, size, result, operand1, operand2, operand3);
		}

		#endregion Set Instruction Methods

		#region Append Instruction Methods

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		public void AppendInstruction(BaseInstruction instruction)
		{
			AppendInstruction();
			Node.SetInstruction(instruction);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="operandCount">The operand count.</param>
		/// <param name="resultCount">The result count.</param>
		public void AppendInstruction(BaseInstruction instruction, byte operandCount, byte resultCount)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, operandCount, resultCount);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="target">The target.</param>
		public void AppendInstruction(BaseInstruction instruction, MosaMethod target)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, target);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="block">The block.</param>
		public void AppendInstruction(BaseInstruction instruction, BasicBlock block)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, block);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="block1">The block1.</param>
		/// <param name="block2">The block2.</param>
		public void AppendInstruction(BaseInstruction instruction, BasicBlock block1, BasicBlock block2)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, block1, block2);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="condition">The condition.</param>
		public void AppendInstruction(BaseInstruction instruction, ConditionCode condition)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, condition);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="condition">The condition.</param>
		/// <param name="block">The block.</param>
		public void AppendInstruction(BaseInstruction instruction, ConditionCode condition, BasicBlock block)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, condition, block);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		public void AppendInstruction(BaseInstruction instruction, Operand result)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, result);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="updateStatus">if set to <c>true</c> [update status].</param>
		/// <param name="result">The result.</param>
		public void AppendInstruction(BaseInstruction instruction, bool updateStatus, Operand result)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, updateStatus, result);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		public void AppendInstruction(BaseInstruction instruction, Operand result, Operand operand1)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, result, operand1);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="size">The size.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		public void AppendInstruction(BaseInstruction instruction, InstructionSize size, Operand result, Operand operand1)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, size, result, operand1);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		public void AppendInstruction(BaseInstruction instruction, bool updateStatus, Operand result, Operand operand1)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, updateStatus, result, operand1);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		/// <param name="result2">The result2.</param>
		public void AppendInstruction2(BaseInstruction instruction, Operand result, Operand result2)
		{
			AppendInstruction();
			Node.SetInstruction2(instruction, result, result2);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		/// <param name="result2">The result2.</param>
		/// <param name="operand1">The operand1.</param>
		public void AppendInstruction2(BaseInstruction instruction, Operand result, Operand result2, Operand operand1)
		{
			AppendInstruction();
			Node.SetInstruction2(instruction, result, result2, operand1);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		/// <param name="result2">The result2.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		public void AppendInstruction2(BaseInstruction instruction, Operand result, Operand result2, Operand operand1, Operand operand2)
		{
			AppendInstruction();
			Node.SetInstruction2(instruction, result, result2, operand1, operand2);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		/// <param name="result2">The result2.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		/// <param name="operand3">The operand3.</param>
		public void AppendInstruction2(BaseInstruction instruction, Operand result, Operand result2, Operand operand1, Operand operand2, Operand operand3)
		{
			AppendInstruction();
			Node.SetInstruction2(instruction, result, result2, operand1, operand2, operand3);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		public void AppendInstruction(BaseInstruction instruction, Operand result, Operand operand1, Operand operand2)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, result, operand1, operand2);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="updateStatus">if set to <c>true</c> [update status].</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		public void AppendInstruction(BaseInstruction instruction, bool updateStatus, Operand result, Operand operand1, Operand operand2)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, updateStatus, result, operand1, operand2);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="condition">The condition.</param>
		/// <param name="result">The result.</param>
		public void AppendInstruction(BaseInstruction instruction, ConditionCode condition, Operand result)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, condition, result);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="condition">The condition.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		public void AppendInstruction(BaseInstruction instruction, ConditionCode condition, Operand result, Operand operand1)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, condition, result, operand1);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="condition">The condition.</param>
		/// <param name="updateStatus">if set to <c>true</c> [update status].</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		public void AppendInstruction(BaseInstruction instruction, ConditionCode condition, bool updateStatus, Operand result, Operand operand1)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, condition, updateStatus, result, operand1);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		public void AppendInstruction(BaseInstruction instruction, ConditionCode condition, Operand result, Operand operand1, Operand operand2)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, condition, result, operand1, operand2);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="condition">The condition.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		/// <param name="block">The block.</param>
		public void AppendInstruction(BaseInstruction instruction, ConditionCode condition, Operand result, Operand operand1, Operand operand2, BasicBlock block)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, condition, result, operand1, operand2, block);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="condition">The condition.</param>
		/// <param name="updateStatus">if set to <c>true</c> [update status].</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		public void AppendInstruction(BaseInstruction instruction, ConditionCode condition, bool updateStatus, Operand result, Operand operand1, Operand operand2)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, condition, updateStatus, result, operand1, operand2);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		/// <param name="operand3">The operand3.</param>
		public void AppendInstruction(BaseInstruction instruction, Operand result, Operand operand1, Operand operand2, Operand operand3)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, result, operand1, operand2, operand3);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="size">The size.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		public void AppendInstruction(BaseInstruction instruction, InstructionSize size, Operand result, Operand operand1, Operand operand2)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, size, result, operand1, operand2);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="size">The size.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		/// <param name="operand3">The operand3.</param>
		public void AppendInstruction(BaseInstruction instruction, InstructionSize size, Operand result, Operand operand1, Operand operand2, Operand operand3)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, size, result, operand1, operand2, operand3);
		}

		#endregion Append Instruction Methods
	};
}
