/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Provides context for transformations.
	/// </summary>
	public sealed class Context
	{
		#region Data members

		/// <summary>
		/// Holds the index of the instruction being operated on.
		/// </summary>
		private int index;

		/// <summary>
		/// Holds the list of instructions
		/// </summary>
		private InstructionSet instructionSet;

		/// <summary>
		/// Holds the basic block of the instruction being operated on.
		/// </summary>
		private BasicBlock block;

		#endregion Data members

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
		/// Gets the instruction.
		/// </summary>
		/// <value>The instruction at the current index.</value>
		public BaseInstruction Instruction
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
		/// The order slot number (initalized by some stage)
		/// </summary>
		/// <value>The label.</value>
		public int SlotNumber
		{
			get { return instructionSet.Data[index].SlotNumber; }
			set { instructionSet.Data[index].SlotNumber = value; }
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
		/// Gets or sets a value indicating whether [update status flags].
		/// </summary>
		/// <value>
		///   <c>true</c> if [update status flags]; otherwise, <c>false</c>.
		/// </value>
		public bool UpdateStatus
		{
			get { return instructionSet.Data[index].UpdateStatus; }
			set { instructionSet.Data[index].UpdateStatus = value; }
		}

		/// <summary>
		///  Holds the cil branch targets
		/// </summary>
		public List<int> CILTargets
		{
			get { return instructionSet.Data[index].CILTargets; }
			set { instructionSet.Data[index].CILTargets = value; }
		}

		/// <summary>
		/// Gets the branch targets.
		/// </summary>
		/// <value>
		/// The targets.
		/// </value>
		public List<BasicBlock> Targets
		{
			get { return instructionSet.Data[index].Targets; }
			private set { instructionSet.Data[index].Targets = value; }
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
				if (current == value) return;
				if (current != null)
				{
					current.Uses.Remove(index);
					if (current.IsMemoryAddress && current.OffsetBase != null)
					{
						current.OffsetBase.Uses.Remove(index);
					}
				}
				if (value != null)
				{
					if (!value.IsCPURegister)
					{
						value.Uses.Add(index);
					}
					if (value.IsMemoryAddress && value.OffsetBase != null)
					{
						value.OffsetBase.Uses.Add(index);
					}
				}

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
				if (current == value) return;
				if (current != null)
				{
					current.Uses.Remove(index);
					if (current.IsMemoryAddress && current.OffsetBase != null)
					{
						current.OffsetBase.Uses.Remove(index);
					}
				}
				if (value != null)
				{
					if (!value.IsCPURegister)
					{
						value.Uses.Add(index);
					}
					if (value.IsMemoryAddress && value.OffsetBase != null)
					{
						value.OffsetBase.Uses.Add(index);
					}
				}
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
				if (current == value) return;
				if (current != null)
				{
					current.Uses.Remove(index);
					if (current.IsMemoryAddress && current.OffsetBase != null)
					{
						current.OffsetBase.Uses.Remove(index);
					}
				}
				if (value != null)
				{
					if (!value.IsCPURegister)
					{
						value.Uses.Add(index);
					}
					if (value.IsMemoryAddress && value.OffsetBase != null)
					{
						value.OffsetBase.Uses.Add(index);
					}
				}
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
		/// Gets all results.
		/// </summary>
		/// <value>The operands.</value>
		public IEnumerable<Operand> Results
		{
			get
			{
				if (Result != null)
					yield return Result;
				if (Result2 != null)
					yield return Result2;
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
				if (current != null)
				{
					current.Definitions.Remove(index);
					if (current.IsMemoryAddress)
					{
						if (current.OffsetBase != null)
						{
							current.OffsetBase.Uses.Remove(index);
						}
						if (current.SSAParent != null)
						{
							current.SSAParent.Uses.Remove(index);
						}
					}
				}
				if (value != null)
				{
					if (!value.IsCPURegister)
					{
						value.Definitions.Add(index);
					}
					if (value.IsMemoryAddress)
					{
						if (value.OffsetBase != null)
						{
							value.OffsetBase.Uses.Add(index);
						}
						if (value.SSAParent != null)
						{
							value.SSAParent.Uses.Add(index);
						}
					}
				}
				instructionSet.Data[index].Result = value;
			}
		}

		/// <summary>
		/// Gets or sets the result operand.
		/// </summary>
		/// <value>The result operand.</value>
		public Operand Result2
		{
			get { return instructionSet.Data[index].Result2; }
			set
			{
				Operand current = instructionSet.Data[index].Result2;
				if (current != null)
				{
					current.Definitions.Remove(index);
					if (!value.IsCPURegister)
					{
						value.Definitions.Add(index);
					}
					if (current.IsMemoryAddress)
					{
						if (current.OffsetBase != null)
						{
							current.OffsetBase.Uses.Remove(index);
						}
						if (current.SSAParent != null)
						{
							current.SSAParent.Uses.Remove(index);
						}
					}
				}
				if (value != null)
				{
					value.Definitions.Add(index);
					if (value.IsMemoryAddress)
					{
						if (value.OffsetBase != null)
						{
							value.OffsetBase.Uses.Add(index);
						}
						if (value.SSAParent != null)
						{
							value.SSAParent.Uses.Add(index);
						}
					}
				}
				instructionSet.Data[index].Result2 = value;
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
		/// Gets or sets the instruction size.
		/// </summary>
		/// <value>
		/// The instruction size.
		/// </value>
		public InstructionSize Size
		{
			get { return instructionSet.Data[index].Size; }
			set { instructionSet.Data[index].Size = value; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is empty.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
		/// </value>
		public bool IsEmpty
		{
			get { return instructionSet.Data[index].Instruction == null; }
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
			set { instructionSet.Data[index].BranchHint = value; }
		}

		/// <summary>
		/// Gets or sets the runtime method.
		/// </summary>
		public MosaMethod MosaMethod
		{
			get { return instructionSet.Data[index].InvokeMethod; }
			set { instructionSet.Data[index].InvokeMethod = value; }
		}

		/// <summary>
		/// Gets or sets the runtime field.
		/// </summary>
		/// <value>The runtime field.</value>
		public MosaField MosaField
		{
			get { return instructionSet.Data[index].MosaField; }
			set { instructionSet.Data[index].MosaField = value; }
		}

		/// <summary>
		/// Gets or sets the runtime field.
		/// </summary>
		/// <value>The runtime field.</value>
		public MosaType MosaType
		{
			get { return instructionSet.Data[index].MosaType; }
			set { instructionSet.Data[index].MosaType = value; }
		}

		/// <summary>
		/// Gets or sets the condition code.
		/// </summary>
		/// <value>The condition code.</value>
		public ConditionCode ConditionCode
		{
			get { return instructionSet.Data[index].ConditionCode; }
			set { instructionSet.Data[index].ConditionCode = value; }
		}

		/// <summary>
		/// Gets or sets the phi blocks.
		/// </summary>
		/// <value>
		/// The phi blocks.
		/// </value>
		public List<BasicBlock> PhiBlocks
		{
			get { return instructionSet.Data[index].PhiBlocks; }
			set { instructionSet.Data[index].PhiBlocks = value; }
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

		#endregion Properties

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
			: this(instructionSet, basicBlock, basicBlock.StartIndex)
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
			return new Context(instructionSet, block, index);
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
		/// Clears this context.
		/// </summary>
		private void Clear()
		{
			Operand1 = null;
			Operand2 = null;
			Operand3 = null;
			Result = null;
			Size = InstructionSize.None;
			if (OperandCount >= 3)
				for (int i = 3; i < OperandCount; i++)
					SetOperand(i, null);

			instructionSet.Data[index].Clear();
		}

		/// <summary>
		/// Inserts an instruction before the current instruction.
		/// </summary>
		/// <returns></returns>
		public Context InsertBefore()
		{
			Debug.Assert(!IsBlockStartInstruction);

			int label = Label;
			int beforeIndex = instructionSet.InsertBefore(index);

			Context ctx = new Context(instructionSet, beforeIndex);
			ctx.Clear();
			ctx.Label = label; // assign label to this new node
			return ctx;
		}

		/// <summary>
		/// Remove this instruction (to be replaced shortly)
		/// </summary>
		public void Remove()
		{
			int label = Label;
			Clear();

			Label = label; // maintain label for this node
		}

		/// <summary>
		/// delete this instruction (not to be replaced)
		/// </summary>
		/// <param name="remove">also remove in instruction set</param>
		public void Delete(bool remove)
		{
			Clear();
			if (remove)
			{
				int next = instructionSet.Next(index);
				instructionSet.Remove(index);
				this.index = instructionSet.Previous(next);
				Debug.Assert(this.index != -1);
			}
		}

		/// <summary>
		/// Replaces the instruction only.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		public void ReplaceInstructionOnly(BaseInstruction instruction)
		{
			Instruction = instruction;
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="operandCount">The operand count.</param>
		/// <param name="resultCount">The result count.</param>
		public void SetInstruction(BaseInstruction instruction, byte operandCount, byte resultCount)
		{
			int label = -1;

			if (index == -1)
			{
				index = instructionSet.CreateRoot();
			}
			else
			{
				label = Label;
				Clear();
			}

			Instruction = instruction;
			OperandCount = operandCount;
			ResultCount = resultCount;
			Label = label;
			Operand1 = null;
			Operand2 = null;
			Operand3 = null;
			Result = null;
			Result2 = null;
			Size = InstructionSize.None;
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
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="target">The target.</param>
		public void SetInstruction(BaseInstruction instruction, MosaMethod target)
		{
			SetInstruction(instruction);
			MosaMethod = target;
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="block">The block.</param>
		public void SetInstruction(BaseInstruction instruction, BasicBlock block)
		{
			SetInstruction(instruction);
			AddBranch(block);
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
			AddBranch(block1, block2);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="condition">The code.</param>
		public void SetInstruction(BaseInstruction instruction, ConditionCode condition)
		{
			SetInstruction(instruction);
			ConditionCode = condition;
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="condition">The code.</param>
		/// <param name="block">The block.</param>
		public void SetInstruction(BaseInstruction instruction, ConditionCode condition, BasicBlock block)
		{
			SetInstruction(instruction);
			ConditionCode = condition;
			AddBranch(block);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="code">The code.</param>
		/// <param name="block">The block.</param>
		/// <param name="branchHint">if set to <c>true</c> [branch hint].</param>
		public void SetInstruction(BaseInstruction instruction, ConditionCode code, BasicBlock block, bool branchHint)
		{
			SetInstruction(instruction, code, block);
			BranchHint = branchHint;
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
		public void SetInstruction(BaseInstruction instruction, bool updateStatus, Operand result)
		{
			SetInstruction(instruction, 0, 1);
			Result = result;
			UpdateStatus = updateStatus;
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
		/// <param name="size">The size.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		public void SetInstruction(BaseInstruction instruction, InstructionSize size, Operand result, Operand operand1)
		{
			SetInstruction(instruction, result, operand1);
			Size = size;
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		public void SetInstruction(BaseInstruction instruction, bool updateStatus, Operand result, Operand operand1)
		{
			SetInstruction(instruction, 1, (byte)((result == null) ? 0 : 1));
			Result = result;
			Operand1 = operand1;
			UpdateStatus = updateStatus;
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
		/// <param name="updateStatus">if set to <c>true</c> [update status].</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		public void SetInstruction(BaseInstruction instruction, bool updateStatus, Operand result, Operand operand1, Operand operand2)
		{
			SetInstruction(instruction, 2, (byte)((result == null) ? 0 : 1));
			Result = result;
			Operand1 = operand1;
			Operand2 = operand2;
			UpdateStatus = updateStatus;
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
		public void SetInstruction(BaseInstruction instruction, ConditionCode condition, bool updateStatus, Operand result, Operand operand1)
		{
			SetInstruction(instruction, 1, (byte)((result == null) ? 0 : 1));
			Result = result;
			Operand1 = operand1;
			ConditionCode = condition;
			UpdateStatus = updateStatus;
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
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
		/// <param name="updateStatus">if set to <c>true</c> [update status].</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		public void SetInstruction(BaseInstruction instruction, ConditionCode condition, bool updateStatus, Operand result, Operand operand1, Operand operand2)
		{
			SetInstruction(instruction, 2, (byte)((result == null) ? 0 : 1));
			Result = result;
			Operand1 = operand1;
			Operand2 = operand2;
			ConditionCode = condition;
			UpdateStatus = updateStatus;
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
		/// <param name="size">The size.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		public void SetInstruction(BaseInstruction instruction, InstructionSize size, Operand result, Operand operand1, Operand operand2)
		{
			SetInstruction(instruction, result, operand1, operand2);
			Size = size;
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
			SetInstruction(instruction, result, operand1, operand2, operand3);
			Size = size;
		}

		/// <summary>
		/// Appends an (empty) instruction after the current index.
		/// </summary>
		private void AppendInstruction()
		{
			int label = -1;

			if (index == -1)
			{
				index = instructionSet.InsertAfter(index);
			}
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
		/// <param name="condition">The condition code.</param>
		public void AppendInstruction(BaseInstruction instruction, ConditionCode condition)
		{
			AppendInstruction();
			SetInstruction(instruction);
			ConditionCode = condition;
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="condition">The condition code.</param>
		/// <param name="result">The result operand.</param>
		public void AppendInstruction(BaseInstruction instruction, ConditionCode condition, Operand result)
		{
			AppendInstruction();
			SetInstruction(instruction, result);
			ConditionCode = condition;
			Result = result;
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		public void AppendInstruction(BaseInstruction instruction)
		{
			AppendInstruction();
			SetInstruction(instruction);
		}

		/// <summary>
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="target">The invoke target.</param>
		public void AppendInstruction(BaseInstruction instruction, MosaMethod target)
		{
			AppendInstruction(instruction);
			MosaMethod = target;
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="block">The basic block.</param>
		public void AppendInstruction(BaseInstruction instruction, BasicBlock block)
		{
			AppendInstruction();
			SetInstruction(instruction, block);
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="block1">The block1.</param>
		/// <param name="block2">The block2.</param>
		public void AppendInstruction(BaseInstruction instruction, BasicBlock block1, BasicBlock block2)
		{
			AppendInstruction();
			SetInstruction(instruction, block1, block2);
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="code">The condition code.</param>
		/// <param name="block">The basic block.</param>
		public void AppendInstruction(BaseInstruction instruction, ConditionCode code, BasicBlock block)
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
		public void AppendInstruction(BaseInstruction instruction, ConditionCode code, BasicBlock block, bool branchHint)
		{
			AppendInstruction(instruction, code, block);
			BranchHint = branchHint;
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="result">The operand result.</param>
		public void AppendInstruction(BaseInstruction instruction, Operand result)
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
		public void AppendInstruction(BaseInstruction instruction, Operand result, Operand operand1)
		{
			AppendInstruction();
			SetInstruction(instruction, result, operand1);
		}

		/// <summary>
		/// Appends the instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="size">The size.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		public void AppendInstruction(BaseInstruction instruction, InstructionSize size, Operand result, Operand operand1)
		{
			AppendInstruction(instruction, result, operand1);
			Size = size;
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="result">The result operand.</param>
		/// <param name="result2">The result2.</param>
		public void AppendInstruction2(BaseInstruction instruction, Operand result, Operand result2)
		{
			AppendInstruction();
			SetInstruction2(instruction, result, result2);
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="result">The result operand.</param>
		/// <param name="result2">The result2.</param>
		/// <param name="operand1">The first operand.</param>
		public void AppendInstruction2(BaseInstruction instruction, Operand result, Operand result2, Operand operand1)
		{
			AppendInstruction();
			SetInstruction2(instruction, result, result2, operand1);
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="result">The result operand.</param>
		/// <param name="result2">The result2.</param>
		/// <param name="operand1">The first operand.</param>
		/// <param name="operand2">The operand2.</param>
		public void AppendInstruction2(BaseInstruction instruction, Operand result, Operand result2, Operand operand1, Operand operand2)
		{
			AppendInstruction();
			SetInstruction2(instruction, result, result2, operand1, operand2);
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="result">The result operand.</param>
		/// <param name="result2">The result2.</param>
		/// <param name="operand1">The first operand.</param>
		/// <param name="operand2">The operand2.</param>
		/// <param name="operand3">The operand3.</param>
		public void AppendInstruction2(BaseInstruction instruction, Operand result, Operand result2, Operand operand1, Operand operand2, Operand operand3)
		{
			AppendInstruction();
			SetInstruction2(instruction, result, result2, operand1, operand2, operand3);
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="condition">The condition.</param>
		/// <param name="result">The result operand.</param>
		/// <param name="operand1">The first operand.</param>
		public void AppendInstruction(BaseInstruction instruction, ConditionCode condition, Operand result, Operand operand1)
		{
			AppendInstruction();
			SetInstruction(instruction, condition, result, operand1);
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="condition">The condition.</param>
		/// <param name="result">The result operand.</param>
		/// <param name="operand1">The first operand.</param>
		/// <param name="operand2">The second operand.</param>
		public void AppendInstruction(BaseInstruction instruction, ConditionCode condition, Operand result, Operand operand1, Operand operand2)
		{
			AppendInstruction();
			SetInstruction(instruction, condition, result, operand1, operand2);
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="result">The result operand.</param>
		/// <param name="operand1">The first operand.</param>
		/// <param name="operand2">The second operand.</param>
		public void AppendInstruction(BaseInstruction instruction, Operand result, Operand operand1, Operand operand2)
		{
			AppendInstruction();
			SetInstruction(instruction, result, operand1, operand2);
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
			SetInstruction(instruction, size, result, operand1, operand2);
		}

		/// <summary>
		/// Appends an instruction after the current index.
		/// </summary>
		/// <param name="instruction">The instruction to append.</param>
		/// <param name="result">The result operand.</param>
		/// <param name="operand1">The first operand.</param>
		/// <param name="operand2">The second operand.</param>
		/// <param name="operand3">The third operand.</param>
		public void AppendInstruction(BaseInstruction instruction, Operand result, Operand operand1, Operand operand2, Operand operand3)
		{
			AppendInstruction();
			SetInstruction(instruction, result, operand1, operand2, operand3);
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
			SetInstruction(instruction, size, result, operand1, operand2, operand3);
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
		public void AddBranch(BasicBlock block)
		{
			Debug.Assert(block != null);

			if (Targets == null)
			{
				Targets = new List<BasicBlock>(1);
			}

			Targets.Add(block);
		}

		/// <summary>
		/// Sets the branch targets.
		/// </summary>
		/// <param name="block1">The block1.</param>
		/// <param name="block2">The block2.</param>
		public void AddBranch(BasicBlock block1, BasicBlock block2)
		{
			Debug.Assert(block1 != null);
			Debug.Assert(block2 != null);

			if (Targets == null)
			{
				Targets = new List<BasicBlock>(2);
			}

			Targets.Add(block1);
			Targets.Add(block2);
		}

		/// <summary>
		/// Sets the branch target.
		/// </summary>
		/// <param name="target">The first target.</param>
		public void AddCILBranch(int target)
		{
			if (CILTargets == null)
			{
				CILTargets = new List<int>(1);
			}

			CILTargets.Add(target);
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
		/// Gets the result.
		/// </summary>
		/// <param name="opIndex">The index.</param>
		/// <returns></returns>
		public Operand GetResult(int opIndex)
		{
			switch (opIndex)
			{
				case 0: return Result;
				case 1: return Result2;
				default: throw new System.IndexOutOfRangeException();
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
						if (current == operand) return;
						if (current != null)
						{
							current.Uses.Remove(index);
							if (current.IsMemoryAddress && current.OffsetBase != null)
							{
								current.OffsetBase.Uses.Remove(index);
							}
						}

						if (operand != null)
						{
							if (!operand.IsCPURegister)
							{
								operand.Uses.Add(index);
							}
							if (operand.IsMemoryAddress && operand.OffsetBase != null)
							{
								operand.OffsetBase.Uses.Remove(index);
							}
						}

						instructionSet.Data[index].SetAdditionalOperand(opIndex, operand);
						return;
					}
			}
		}

		/// <summary>
		/// Sets the result by index.
		/// </summary>
		/// <param name="opIndex">The index.</param>
		/// <param name="operand">The operand.</param>
		public void SetResult(int opIndex, Operand operand)
		{
			switch (opIndex)
			{
				case 0: Result = operand; return;
				case 1: Result2 = operand; return;
				default: throw new System.IndexOutOfRangeException();
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

		#endregion Methods
	};
}
