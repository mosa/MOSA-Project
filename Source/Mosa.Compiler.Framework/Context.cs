// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
		/// Gets or sets the node.
		/// </summary>
		/// <value>
		/// The node.
		/// </value>
		public InstructionNode Node { get; set; }

		/// <summary>
		/// Gets or sets the basic block currently processed.
		/// </summary>
		public BasicBlock Block { get { return Node.Block; } internal set { Node.Block = value; } }

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
		/// The order slot number (initialized by some stage)
		/// </summary>
		/// <value>The label.</value>
		public int Offset { get { return Node.Offset; } set { Node.Offset = value; } }

		/// <summary>
		/// Gets or sets the label.
		/// </summary>
		/// <value>The label.</value>
		public bool Marked { get { return Node.Marked; } set { Node.Marked = value; } }

		/// <summary>
		/// Gets or sets status resister setting.
		/// </summary>
		public StatusRegister StatusRegister { get { return Node.StatusRegister; } set { Node.StatusRegister = value; } }

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
		public int OperandCount { get { return Node.OperandCount; } set { Node.OperandCount = value; } }

		/// <summary>
		/// Gets or sets number of operand results.
		/// </summary>
		/// <value>The number of operand results.</value>
		public int ResultCount { get { return Node.ResultCount; } set { Node.ResultCount = value; } }

		/// <summary>
		/// Gets a value indicating whether this instance is empty.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
		/// </value>
		public bool IsEmpty { get { return Node.IsEmpty; } }

		public bool IsEmptyOrNop { get { return Node.IsEmptyOrNop; } }

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
		/// Appends an (empty) instruction after the current index.
		/// </summary>
		private void AppendInstruction()
		{
			Debug.Assert(!IsBlockEndInstruction);
			Debug.Assert(Block != null);

			var node = new InstructionNode()
			{
				Label = Label
			};
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

			var node = new InstructionNode()
			{
				Label = Label
			};
			Node.Previous.Insert(node);

			return new Context(node);
		}

		/// <summary>
		/// Inserts an instruction before the current instruction.
		/// </summary>
		/// <returns></returns>
		public Context InsertAfter()
		{
			Debug.Assert(!IsBlockStartInstruction);

			var node = new InstructionNode()
			{
				Label = Label
			};
			Node.Insert(node);

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
		/// <param name="index">The index.</param>
		/// <param name="operand">The operand.</param>
		public void SetOperand(int index, Operand operand)
		{
			Node.SetOperand(index, operand);
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
		/// Appends the operands.
		/// </summary>
		/// <param name="operands">The operands.</param>
		/// <param name="offset">The offset.</param>
		public void AppendOperands(IList<Operand> operands, int offset = 0)
		{
			Node.AppendOperands(operands, offset);
		}

		/// <summary>
		/// Gets the operands.
		/// </summary>
		/// <returns></returns>
		public List<Operand> GetOperands()
		{
			return Node.GetOperands();
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
		public void ReplaceInstruction(BaseInstruction instruction)
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
		public void SetInstruction(BaseInstruction instruction, int operandCount, byte resultCount)
		{
			Node.SetInstruction(instruction, operandCount, resultCount);
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
		/// <param name="statusRegister">if set to <c>true</c> [update status].</param>
		/// <param name="result">The result.</param>
		public void SetInstruction(BaseInstruction instruction, StatusRegister statusRegister, Operand result)
		{
			Node.SetInstruction(instruction, statusRegister, result);
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
		/// <param name="result">The result.</param>
		/// <param name="operands">The operands.</param>
		public void SetInstruction(BaseInstruction instruction, Operand result, List<Operand> operands)
		{
			Node.SetInstruction(instruction, result, operands);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="statusRegister">if set to <c>true</c> [update status].</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		public void SetInstruction(BaseInstruction instruction, StatusRegister statusRegister, Operand result, Operand operand1)
		{
			Node.SetInstruction(instruction, statusRegister, result, operand1);
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
			Node.SetInstruction(instruction, result, operand1, operands);
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
		/// <param name="statusRegister">if set to <c>true</c> [update status].</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		public void SetInstruction(BaseInstruction instruction, StatusRegister statusRegister, Operand result, Operand operand1, Operand operand2)
		{
			Node.SetInstruction(instruction, statusRegister, result, operand1, operand2);
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
		/// <param name="statusRegister">if set to <c>true</c> [update status].</param>
		/// <param name="condition">The condition.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		public void SetInstruction(BaseInstruction instruction, StatusRegister statusRegister, ConditionCode condition, Operand result, Operand operand1)
		{
			Node.SetInstruction(instruction, statusRegister, condition, result, operand1);
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
		/// <param name="operand3">The operand3.</param>
		public void SetInstruction(BaseInstruction instruction, ConditionCode condition, Operand result, Operand operand1, Operand operand2, Operand operand3)
		{
			Node.SetInstruction(instruction, condition, result, operand1, operand2, operand3);
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
		/// <param name="statusRegister">if set to <c>true</c> [update status].</param>
		/// <param name="condition">The condition.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		public void SetInstruction(BaseInstruction instruction, StatusRegister statusRegister, ConditionCode condition, Operand result, Operand operand1, Operand operand2)
		{
			Node.SetInstruction(instruction, statusRegister, condition, result, operand1, operand2);
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
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		/// <param name="operand3">The operand3.</param>
		/// <param name="operand4">The operand4.</param>
		public void SetInstruction(BaseInstruction instruction, Operand result, Operand operand1, Operand operand2, Operand operand3, Operand operand4)
		{
			Node.SetInstruction(instruction, result, operand1, operand2, operand3, operand4);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		public void SetInstruction(SimpleInstruction instruction)
		{
			Node.SetInstruction(instruction);
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
		/// <param name="result">The result.</param>
		/// <param name="block">The block.</param>
		public void AppendInstruction(BaseInstruction instruction, Operand result, BasicBlock block)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, result, block);
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
		/// <param name="statusRegister">if set to <c>true</c> [update status].</param>
		/// <param name="result">The result.</param>
		public void AppendInstruction(BaseInstruction instruction, StatusRegister statusRegister, Operand result)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, statusRegister, result);
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
		/// <param name="statusRegister">if set to <c>true</c> [update status].</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		public void AppendInstruction(BaseInstruction instruction, StatusRegister statusRegister, Operand result, Operand operand1)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, statusRegister, result, operand1);
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
		/// <param name="statusRegister">if set to <c>true</c> [update status].</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		public void AppendInstruction(BaseInstruction instruction, StatusRegister statusRegister, Operand result, Operand operand1, Operand operand2)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, statusRegister, result, operand1, operand2);
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
		/// <param name="result">The result.</param>
		/// <param name="operands">The operands.</param>
		public void AppendInstruction(BaseInstruction instruction, Operand result, List<Operand> operands)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, result, operands);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operands">The operands.</param>
		public void AppendInstruction(BaseInstruction instruction, Operand result, Operand operand1, List<Operand> operands)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, result, operand1, operands);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		/// <param name="operands">The operands.</param>
		public void AppendInstruction(BaseInstruction instruction, Operand result, Operand operand1, Operand operand2, List<Operand> operands)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, result, operand1, operand2, operands);
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
		/// <param name="statusRegister">if set to <c>true</c> [update status].</param>
		/// <param name="condition">The condition.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		public void AppendInstruction(BaseInstruction instruction, StatusRegister statusRegister, ConditionCode condition, Operand result, Operand operand1)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, statusRegister, condition, result, operand1);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="condition">The condition.</param>
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
		/// <param name="statusRegister">if set to <c>true</c> [update status].</param>
		/// <param name="condition">The condition.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		public void AppendInstruction(BaseInstruction instruction, StatusRegister statusRegister, ConditionCode condition, Operand result, Operand operand1, Operand operand2)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, statusRegister, condition, result, operand1, operand2);
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
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		/// <param name="operand3">The operand3.</param>
		/// <param name="operand4">The operand4.</param>
		public void AppendInstruction(BaseInstruction instruction, Operand result, Operand operand1, Operand operand2, Operand operand3, Operand operand4)
		{
			AppendInstruction();
			Node.SetInstruction(instruction, result, operand1, operand2, operand3, operand4);
		}

		/// <summary>
		/// Appends the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		public void AppendInstruction(SimpleInstruction instruction)
		{
			AppendInstruction();
			Node.SetInstruction(instruction);
		}

		#endregion Append Instruction Methods
	}
}
