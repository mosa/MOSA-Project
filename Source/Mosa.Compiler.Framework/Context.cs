/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework
{

	/// <summary>
	/// Provides context for transformations.
	/// </summary>
	public sealed class Context
	{
		#region Data members

		/// <summary>
		/// Holds the instruction index operated on.
		/// </summary>
		private int index;

		/// <summary>
		/// Holds the list of instructions
		/// </summary>
		private InstructionSet instructionSet;

		/// <summary>
		/// Holds the block being operated on.
		/// </summary>
		private BasicBlock block;

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Gets or sets the basic block currently processed.
		/// </summary>
		public BasicBlock BasicBlock
		{
			get { return block; }
			set { block = value; }
		}

		/// <summary>
		/// Gets or sets the instruction set.
		/// </summary>
		public InstructionSet InstructionSet
		{
			get { return instructionSet; }
			set { instructionSet = value; }
		}

		/// <summary>
		/// Gets or sets the instruction index currently processed.
		/// </summary>
		public int Index
		{
			get { return index; }
			set { index = value; }
		}

		/// <summary>
		/// Gets or sets the type of the token.
		/// </summary>
		/// <value>The type of the token.</value>
		public HeapIndexToken TokenType
		{
			get { return instructionSet.Data[index].TokenType; }
			set { instructionSet.Data[index].TokenType = value; }
		}

		/// <summary>
		/// Gets the instruction.
		/// </summary>
		/// <value>The instruction at the current index.</value>
		public IInstruction Instruction
		{
			get { return instructionSet.Data[index].Instruction; }
			private set { instructionSet.Data[index].Instruction = value; }
		}

		/// <summary>
		/// Gets or sets the label.
		/// </summary>
		/// <value>The label.</value>
		public int Label
		{
			get { return instructionSet.Data[index].Label; }
			set { instructionSet.Data[index].Label = value; }
		}

		/// <summary>
		/// Gets or sets the label.
		/// </summary>
		/// <value>The label.</value>
		public bool Marked
		{
			get { return instructionSet.Data[index].Marked; }
			set { instructionSet.Data[index].Marked = value; }
		}

		/// <summary>
		/// Gets or sets the offset.
		/// </summary>
		/// <value>The offset.</value>
		public int Offset
		{
			get { return instructionSet.Data[index].Offset; }
			set { instructionSet.Data[index].Offset = value; }
		}

		/// <summary>
		/// Gets or sets the result operand.
		/// </summary>
		/// <value>The result operand.</value>
		public IBranch Branch
		{
			get { return instructionSet.Data[index].Branch; }
			set { instructionSet.Data[index].Branch = value; }
		}

		/// <summary>
		/// Gets or sets the first operand.
		/// </summary>
		/// <value>The first operand.</value>
		public Operand Operand1
		{
			get { return instructionSet.Data[index].Operand1; }
			set
			{
				Operand current = instructionSet.Data[index].Operand1;
				if (current != null) current.Uses.Remove(index);
				if (value != null) value.Uses.Add(index);
				instructionSet.Data[index].Operand1 = value;
			}
		}

		/// <summary>
		/// Gets or sets the second operand.
		/// </summary>
		/// <value>The second operand.</value>
		public Operand Operand2
		{
			get { return instructionSet.Data[index].Operand2; }
			set
			{
				Operand current = instructionSet.Data[index].Operand2;
				if (current != null) current.Uses.Remove(index);
				if (value != null) value.Uses.Add(index);
				instructionSet.Data[index].Operand2 = value;
			}
		}

		/// <summary>
		/// Gets or sets the third operand.
		/// </summary>
		/// <value>The third operand.</value>
		public Operand Operand3
		{
			get { return instructionSet.Data[index].Operand3; }
			set
			{
				Operand current = instructionSet.Data[index].Operand3;
				if (current != null) current.Uses.Remove(index);
				if (value != null) value.Uses.Add(index);
				instructionSet.Data[index].Operand3 = value;
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
				if (Operand1 != null)
					yield return Operand1;
				if (Operand2 != null)
					yield return Operand2;
				if (Operand3 != null)
					yield return Operand3;

				if (OperandCount >= 3)
					for (int i = 3; i < OperandCount; i++)
						yield return instructionSet.Data[index].GetAdditionalOperand(i);
			}
		}

		/// <summary>
		/// Gets or sets the result operand.
		/// </summary>
		/// <value>The result operand.</value>
		public Operand Result
		{
			get { return instructionSet.Data[index].Result; }
			set
			{
				Operand current = instructionSet.Data[index].Result;
				if (current != null) current.Definitions.Remove(index);
				if (value != null) value.Definitions.Add(index);
				instructionSet.Data[index].Result = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of operands.
		/// </summary>
		/// <value>The number of operands.</value>
		public byte OperandCount
		{
			get { return instructionSet.Data[index].OperandCount; }
			set { instructionSet.Data[index].OperandCount = value; }
		}

		/// <summary>
		/// Gets or sets number of operand results.
		/// </summary>
		/// <value>The number of operand results.</value>
		public byte ResultCount
		{
			get { return instructionSet.Data[index].ResultCount; }
			set { instructionSet.Data[index].ResultCount = value; }
		}

		/// <summary>
		/// Gets or sets the ignored attribute.
		/// </summary>
		/// <value>The ignored attribute.</value>
		public bool Ignore
		{
			get { return instructionSet.Data[index].Ignore; }
			set { instructionSet.Data[index].Ignore = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance has prefix.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has prefix; otherwise, <c>false</c>.
		/// </value>
		public bool HasPrefix
		{
			get { return instructionSet.Data[index].HasPrefix; }
			set { instructionSet.Data[index].HasPrefix = value; }
		}

		/// <summary>
		/// Gets or sets the branch hint (true means branch likely).
		/// </summary>
		/// <value><c>true</c> if the branch is likely; otherwise <c>false</c>.</value>
		public bool BranchHint
		{
			get { return instructionSet.Data[index].BranchHint; }
			set
			{
				instructionSet.Data[index].BranchHint = value;
				if (block != null && Branch != null)
					if (Branch.Targets.Length == 1)
						if (value)
							block.HintTarget = Branch.Targets[0];
						else
							block.HintTarget = -1;
			}
		}

		/// <summary>
		/// Holds the function being called.
		/// </summary>
		public RuntimeMethod InvokeTarget
		{
			get { return instructionSet.Data[index].InvokeTarget; }
			set { instructionSet.Data[index].InvokeTarget = value; }
		}

		/// <summary>
		/// Gets or sets the runtime field.
		/// </summary>
		/// <value>The runtime field.</value>
		public RuntimeField RuntimeField
		{
			get { return instructionSet.Data[index].RuntimeField; }
			set { instructionSet.Data[index].RuntimeField = value; }
		}

		/// <summary>
		/// Gets or sets the token.
		/// </summary>
		/// <value>The token.</value>
		public Token Token
		{
			get { return instructionSet.Data[index].Token; }
			set { instructionSet.Data[index].Token = value; }
		}

		/// <summary>
		/// Gets or sets the condition code.
		/// </summary>
		/// <value>The condition code.</value>
		public IR.ConditionCode ConditionCode
		{
			get { return instructionSet.Data[index].ConditionCode; }
			set { instructionSet.Data[index].ConditionCode = value; }
		}

		/// <summary>
		/// Gets or sets the literal data.
		/// </summary>
		/// <value>The literal data.</value>
		public IR.LiteralData LiteralData
		{
			get { return (IR.LiteralData)Other; }
			set { Other = value; }
		}

		/// <summary>
		/// Gets or sets the "other" object.
		/// </summary>
		/// <value>The "other" object.</value>
		public object Other
		{
			get { return instructionSet.Data[index].Other; }
			set { instructionSet.Data[index].Other = value; }
		}

		/// <summary>
		/// Gets a value indicating whether this is the first instruction.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this is the first instruction; otherwise, <c>false</c>.
		/// </value>
		public bool IsFirstInstruction
		{
			get { return instructionSet.Previous(index) < 0; }
		}

		/// <summary>
		/// Gets a value indicating whether this is the last instruction.
		/// </summary>
		/// <value><c>true</c> if this is the last instruction; otherwise, <c>false</c>.</value>
		public bool IsLastInstruction
		{
			get { return instructionSet.Next(index) < 0; }
		}

		/// <summary>
		/// Gets a value indicating whether the end of the instruction is reached.
		/// </summary>
		/// <value><c>true</c> if the end of the instruction is reached; otherwise, <c>false</c>.</value>
		public bool EndOfInstruction
		{
			get { return index < 0; }
		}

		/// <summary>
		/// Gets the context of the next instruction.
		/// </summary>
		/// <value>The context of the next instruction.</value>
		public Context Next
		{
			get { return new Context(instructionSet, instructionSet.Next(index)); }
		}

		/// <summary>
		/// Gets the context of the previous instruction.
		/// </summary>
		/// <value>The context of the previous instruction.</value>
		public Context Previous
		{
			get { return new Context(instructionSet, instructionSet.Previous(index)); }
		}

		#endregion // Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Context"/> class.
		/// </summary>
		/// <param name="instructionSet">The instruction set.</param>
		/// <param name="block">The basic block.</param>
		/// <param name="index">The index.</param>
		public Context(InstructionSet instructionSet, BasicBlock block, int index)
		{
			this.instructionSet = instructionSet;
			this.index = index;
			this.block = block;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Context"/> class.
		/// </summary>
		/// <param name="instructionSet">The instruction set.</param>
		/// <param name="index">The index.</param>
		public Context(InstructionSet instructionSet, int index)
			: this(instructionSet, null, index)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Context"/> class.
		/// </summary>
		/// <param name="instructionSet">The instruction set.</param>
		/// <param name="basicBlock">The basic block.</param>
		public Context(InstructionSet instructionSet, BasicBlock basicBlock)
			: this(instructionSet, basicBlock, basicBlock.Index)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Context"/> class.
		/// </summary>
		/// <param name="instructionSet">The instruction set.</param>
		public Context(InstructionSet instructionSet)
			: this(instructionSet, null, -1)
		{
		}
		#endregion // Construction

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
			return new Context(instructionSet, block, index);
		}

		/// <summary>
		/// Clones this context, beginning at the start of the basic block.
		/// </summary>
		/// <returns></returns>
		public Context CloneAtStart()
		{
			return new Context(instructionSet, block, block.Index);
		}

		/// <summary>
		/// Goes to the next instruction.
		/// </summary>
		public void GotoNext()
		{
			index = instructionSet.Next(index);
		}

		/// <summary>
		/// Gotos to the previous instruction.
		/// </summary>
		public void GotoPrevious()
		{
			index = instructionSet.Previous(index);
		}

		/// <summary>
		/// Goes to the last instruction.
		/// </summary>
		public void GotoLast()
		{
			if (index < 0)
				return;

			for (; ; )
			{
				int next = instructionSet.Next(index);
				if (next < 0)
					break;

				index = next;
			}
		}

		/// <summary>
		/// Clears this context.
		/// </summary>
		public void Clear()
		{
			Operand1 = null;
			Operand2 = null;
			Operand3 = null;
			Result = null;
			instructionSet.Data[index].Clear();

			if (OperandCount >= 3)
				for (int i = 3; i < OperandCount; i++)
					SetOperand(i, null);
		}

		/// <summary>
		/// Inserts an instruction before the current instruction.
		/// </summary>
		/// <returns></returns>
		public Context InsertBefore()
		{
			int label = Label;
			int beforeIndex = -1;

			if (IsFirstInstruction)
			{
				Debug.Assert(BasicBlock != null, @"Cannot insert before first instruction without basic block");
				Debug.Assert(BasicBlock.Index == index, @"Cannot be first instruction since basic block does not start here");
				beforeIndex = instructionSet.InsertBefore(index);
				BasicBlock.Index = beforeIndex;
			}
			else
				beforeIndex = instructionSet.InsertBefore(index);

			Context ctx = new Context(instructionSet, beforeIndex);
			ctx.Clear();
			ctx.Label = label;
			return ctx;
		}

		/// <summary>
		/// Slices the instruction flow before the current instruction.
		/// </summary>
		public void SliceBefore()
		{
			instructionSet.SliceBefore(index);
		}

		/// <summary>
		/// Slices the instruction flow after the current instruction.
		/// </summary>
		public void SliceAfter()
		{
			instructionSet.SliceAfter(index);
		}

		/// <summary>
		/// Remove this instance.
		/// </summary>
		public void Remove()
		{
			int label = Label;
			Clear();

			Instruction = null;
			Ignore = true;
			Label = label;
		}

		/// <summary>
		/// Replaces the instruction only.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		public void ReplaceInstructionOnly(IInstruction instruction)
		{
			Instruction = instruction;
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="operandCount">The operand count.</param>
		/// <param name="resultCount">The result count.</param>
		public void SetInstruction(IInstruction instruction, byte operandCount, byte resultCount)
		{
			int label = -1;

			if (index == -1)
				index = instructionSet.CreateRoot();
			else
			{
				label = Label;
				Clear();
			}

			Instruction = instruction;
			OperandCount = operandCount;
			ResultCount = resultCount;
			Ignore = false;
			Label = label;
			Operand1 = null;
			Operand2 = null;
			Operand3 = null;
			Result = null;
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		public void SetInstruction(IInstruction instruction)
		{
			if (instruction != null)
				SetInstruction(instruction, instruction.DefaultOperandCount, instruction.DefaultResultCount);
			else
				SetInstruction(null, 0, 0);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="target">The target.</param>
		public void SetInstruction(IInstruction instruction, RuntimeMethod target)
		{
			SetInstruction(instruction);
			InvokeTarget = target;
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="block">The block.</param>
		public void SetInstruction(IInstruction instruction, BasicBlock block)
		{
			SetInstruction(instruction);
			SetBranch(block);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="code">The code.</param>
		public void SetInstruction(IInstruction instruction, IR.ConditionCode code)
		{
			SetInstruction(instruction);
			ConditionCode = code;
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="code">The code.</param>
		/// <param name="block">The block.</param>
		public void SetInstruction(IInstruction instruction, IR.ConditionCode code, BasicBlock block)
		{
			SetInstruction(instruction);
			ConditionCode = code;
			SetBranch(block);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="code">The code.</param>
		/// <param name="block">The block.</param>
		/// <param name="branchHint">if set to <c>true</c> [branch hint].</param>
		public void SetInstruction(IInstruction instruction, IR.ConditionCode code, BasicBlock block, bool branchHint)
		{
			SetInstruction(instruction, code, block);
			BranchHint = branchHint;
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		public void SetInstruction(IInstruction instruction, Operand result)
		{
			SetInstruction(instruction, 0, 1);
			Result = result;
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		public void SetInstruction(IInstruction instruction, Operand result, Operand operand1)
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
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		public void SetInstruction(IInstruction instruction, Operand result, Operand operand1, Operand operand2)
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
		public void SetInstruction(IInstruction instruction, Operand result, Operand operand1, Operand operand2, Operand operand3)
		{
			SetInstruction(instruction, 3, (byte)((result == null) ? 0 : 1));
			Result = result;
			Operand1 = operand1;
			Operand2 = operand2;
			Operand3 = operand3;
		}

		/// <summary>
		/// Appends an (empty) instruction after the current index.
		/// </summary>
		private void AppendInstruction()
		{
			int label = -1;

			if (index == -1)
				index = instructionSet.InsertAfter(index);
			else
			{
				label = Label;
				if (Instruction != null)
					index = instructionSet.InsertAfter(index);
			}

			Label = label;
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="code">The condition code.</param>
		public void AppendInstruction(IInstruction instruction, IR.ConditionCode code)
		{
			AppendInstruction();
			SetInstruction(instruction);
			ConditionCode = code;
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="code">The condition code.</param>
		/// <param name="result">The result operand.</param>
		public void AppendInstruction(IInstruction instruction, IR.ConditionCode code, Operand result)
		{
			AppendInstruction();
			SetInstruction(instruction, result);
			ConditionCode = code;
			Result = result;
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		public void AppendInstruction(IInstruction instruction)
		{
			AppendInstruction();
			SetInstruction(instruction);
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="target">The invoke target.</param>
		public void AppendInstruction(IInstruction instruction, RuntimeMethod target)
		{
			AppendInstruction(instruction);
			InvokeTarget = target;
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="block">The basic block.</param>
		public void AppendInstruction(IInstruction instruction, BasicBlock block)
		{
			AppendInstruction();
			SetInstruction(instruction, block);
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="code">The condition code.</param>
		/// <param name="block">The basic block.</param>
		public void AppendInstruction(IInstruction instruction, IR.ConditionCode code, BasicBlock block)
		{
			AppendInstruction();
			SetInstruction(instruction, code, block);
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="code">The condition code.</param>
		/// <param name="block">The basic block.</param>
		/// <param name="branchHint">The branch hint value.</param>
		public void AppendInstruction(IInstruction instruction, IR.ConditionCode code, BasicBlock block, bool branchHint)
		{
			AppendInstruction(instruction, code, block);
			BranchHint = branchHint;
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="result">The operand result.</param>
		public void AppendInstruction(IInstruction instruction, Operand result)
		{
			AppendInstruction();
			SetInstruction(instruction, 0, 1);
			Result = result;
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="result">The result operand.</param>
		/// <param name="operand1">The first operand.</param>
		public void AppendInstruction(IInstruction instruction, Operand result, Operand operand1)
		{
			AppendInstruction();
			SetInstruction(instruction, result, operand1);
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="result">The result operand.</param>
		/// <param name="operand1">The first operand.</param>
		/// <param name="operand2">The second operand.</param>
		public void AppendInstruction(IInstruction instruction, Operand result, Operand operand1, Operand operand2)
		{
			AppendInstruction();
			SetInstruction(instruction, result, operand1, operand2);
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="result">The result operand.</param>
		/// <param name="operand1">The first operand.</param>
		/// <param name="operand2">The second operand.</param>
		/// <param name="operand3">The third operand.</param>
		public void AppendInstruction(IInstruction instruction, Operand result, Operand operand1, Operand operand2, Operand operand3)
		{
			AppendInstruction();
			SetInstruction(instruction, result, operand1, operand2, operand3);
		}

		/// <summary>
		/// Sets the result operand.
		/// </summary>
		/// <param name="result">The result operand.</param>
		public void SetResult(Operand result)
		{
			Result = result;
			ResultCount = 1;
		}
		
		/// <summary>
		/// Sets the operands.
		/// </summary>
		/// <param name="result">The result operand.</param>
		/// <param name="operand1">The first operand.</param>
		public void SetOperands(Operand result, Operand operand1)
		{
			Result = result;
			Operand1 = operand1;
			ResultCount = (byte)((result == null) ? 0 : 1);
			OperandCount = 1;
		}

		/// <summary>
		/// Sets the operands.
		/// </summary>
		/// <param name="result">The result operand.</param>
		/// <param name="operand1">The first operand.</param>
		/// <param name="operand2">The second operand.</param>
		public void SetOperands(Operand result, Operand operand1, Operand operand2)
		{
			Result = result;
			Operand1 = operand1;
			Operand2 = operand2;
			ResultCount = (byte)((result == null) ? 0 : 1);
			OperandCount = 2;
		}

		/// <summary>
		/// Sets the operands.
		/// </summary>
		/// <param name="result">The result operand.</param>
		/// <param name="operand1">The first operand.</param>
		/// <param name="operand2">The second operand.</param>
		/// <param name="operand3">The third operand.</param>
		public void SetOperands(Operand result, Operand operand1, Operand operand2, Operand operand3)
		{
			Result = result;
			Operand1 = operand1;
			Operand2 = operand2;
			Operand3 = operand3;
			ResultCount = (byte)((result == null) ? 0 : 1);
			OperandCount = 3;
		}

		/// <summary>
		/// Sets the branch target.
		/// </summary>
		/// <param name="block">The basic block.</param>
		public void SetBranch(BasicBlock block)
		{
			SetBranch(block.Label);
		}

		/// <summary>
		/// Sets the branch target.
		/// </summary>
		/// <param name="target1">The first target.</param>
		public void SetBranch(int target1)
		{
			if (Branch == null)
				Branch = new Branch(1);

			Branch.Targets[0] = target1;
		}

		/// <summary>
		/// Sets the branch target.
		/// </summary>
		/// <param name="target1">The first target.</param>
		/// <param name="target2">The second target.</param>
		public void SetBranch(int target1, int target2)
		{
			if (Branch == null)
				Branch = new Branch(2);
			else
				if (Branch.Targets.Length < 2)
				{
					Branch newBranch = new Branch(2);
					newBranch.Targets[0] = Branch.Targets[0];
					Branch = newBranch;
				}

			Branch.Targets[0] = target1;
			Branch.Targets[1] = target2;
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
				default: return instructionSet.Data[index].GetAdditionalOperand(opIndex);
			}
		}

		/// <summary>
		/// Sets the operand by index.
		/// </summary>
		/// <param name="opIndex">The index.</param>
		/// <param name="operand">The operand.</param>
		public void SetOperand(int opIndex, Operand operand)
		{
			switch (opIndex)
			{
				case 0: Operand1 = operand; return;
				case 1: Operand2 = operand; return;
				case 2: Operand3 = operand; return;
				default:
					{
						Operand current = instructionSet.Data[index].GetAdditionalOperand(opIndex);
						if (current != null) current.Uses.Remove(index);
						if (operand != null) operand.Uses.Add(index);
						instructionSet.Data[index].SetAdditionalOperand(opIndex, operand);
						return;
					}
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format("L_{0:X4}: {1}", Label, Instruction.ToString(this));
		}

		#endregion // Methods
	};

}
