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
using System.Text;
using System.Diagnostics;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;
using IR2 = Mosa.Runtime.CompilerFramework;

namespace Mosa.Runtime.CompilerFramework
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
		private int _index;

		/// <summary>
		/// Holds the list of instructions
		/// </summary>
		private InstructionSet _instructionSet;

		/// <summary>
		/// Holds the block being operated on.
		/// </summary>
		private BasicBlock _block;

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Gets or sets the basic block currently processed.
		/// </summary>
		public BasicBlock BasicBlock
		{
			get { return _block; }
			set { _block = value; }
		}

		/// <summary>
		/// Gets or sets the instruction set.
		/// </summary>
		public InstructionSet InstructionSet
		{
			get { return _instructionSet; }
			set { _instructionSet = value; }
		}

		/// <summary>
		/// Gets or sets the instruction index currently processed.
		/// </summary>
		public int Index
		{
			get { return _index; }
			set { _index = value; }
		}

		/// <summary>
		/// Gets the instruction.
		/// </summary>
		/// <value>The result operand.</value>
		public IInstruction Instruction
		{
			get { return _instructionSet.Data[_index].Instruction; }
			//set { _instructionSet.Data[_index].Instruction = value; }
		}

		/// <summary>
		/// Sets the new instruction.
		/// </summary>
		/// <value>The result operand.</value>
		private IInstruction NewInstruction
		{
			set { _instructionSet.Data[_index].Instruction = value; }
		}

		/// <summary>
		/// Gets the offset.
		/// </summary>
		/// <value>The offset.</value>
		public int Label
		{
			get { return _instructionSet.Data[_index].Label; }
			set { _instructionSet.Data[_index].Label = value; }
		}

		/// <summary>
		/// Gets the offset.
		/// </summary>
		/// <value>The offset.</value>
		public int Offset
		{
			get { return _instructionSet.Data[_index].Offset; }
			set { _instructionSet.Data[_index].Offset = value; }
		}

		/// <summary>
		/// Gets the result operand.
		/// </summary>
		/// <value>The result operand.</value>
		public IBranch Branch
		{
			get { return _instructionSet.Data[_index].Branch; }
			set { _instructionSet.Data[_index].Branch = value; }
		}

		/// <summary>
		/// Gets the first operand.
		/// </summary>
		/// <value>The first operand.</value>
		public Operand Operand1
		{
			get { return _instructionSet.Data[_index].Operand1; }
			set
			{
				Operand current = _instructionSet.Data[_index].Operand1;
				if (current != null) current.Uses.Remove(_index);
				if (value != null) value.Uses.Add(_index);
				_instructionSet.Data[_index].Operand1 = value;
			}
		}

		/// <summary>
		/// Gets the first operand.
		/// </summary>
		/// <value>The first operand.</value>
		public Operand Operand2
		{
			get { return _instructionSet.Data[_index].Operand2; }
			set
			{
				Operand current = _instructionSet.Data[_index].Operand2;
				if (current != null) current.Uses.Remove(_index);
				if (value != null) value.Uses.Add(_index);
				_instructionSet.Data[_index].Operand2 = value;
			}
		}

		/// <summary>
		/// Gets the first operand.
		/// </summary>
		/// <value>The first operand.</value>
		public Operand Operand3
		{
			get { return _instructionSet.Data[_index].Operand3; }
			set
			{
				Operand current = _instructionSet.Data[_index].Operand3;
				if (current != null) current.Uses.Remove(_index);
				if (value != null) value.Uses.Add(_index);
				_instructionSet.Data[_index].Operand3 = value;
			}
		}

		/// <summary>
		/// Gets the operands.
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
					for (int index = 3; index < OperandCount; index++)
						yield return _instructionSet.Data[_index].GetAdditionalOperand(index);
			}
		}

		/// <summary>
		/// Gets the result operand.
		/// </summary>
		/// <value>The result operand.</value>
		public Operand Result
		{
			get { return _instructionSet.Data[_index].Result; }
			set
			{
				Operand current = _instructionSet.Data[_index].Result;
				if (current != null) current.Definitions.Remove(_index);
				if (value != null) value.Definitions.Add(_index);
				_instructionSet.Data[_index].Result = value;
			}
		}

		/// <summary>
		/// Gets the second result operand.
		/// </summary>
		/// <value>The second result operand.</value>
		public Operand Result2
		{
			get
			{
				if (ResultCount == 2)
					return Result;
				else
					return null;
			}
			set
			{
				Debug.Assert(Result == value);
			}
		}

		/// <summary>
		/// Gets the operands.
		/// </summary>
		/// <value>The operands.</value>
		public IEnumerable<Operand> Results
		{
			get
			{
				if (Result != null)
					yield return Result;
				if (ResultCount == 2)
					yield return Result;
			}
		}

		/// <summary>
		/// Gets the operand count.
		/// </summary>
		/// <value>The operand count.</value>
		public byte OperandCount
		{
			get { return _instructionSet.Data[_index].OperandCount; }
			set { _instructionSet.Data[_index].OperandCount = value; }
		}

		/// <summary>
		/// Gets the result count.
		/// </summary>
		/// <value>The result count.</value>
		public byte ResultCount
		{
			get { return _instructionSet.Data[_index].ResultCount; }
			set { _instructionSet.Data[_index].ResultCount = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Context"/> is ignore.
		/// </summary>
		/// <value><c>true</c> if ignore; otherwise, <c>false</c>.</value>
		public bool Ignore
		{
			get { return _instructionSet.Data[_index].Ignore; }
			set { _instructionSet.Data[_index].Ignore = value; }
		}

		/// <summary>
		/// Holds the function being called.
		/// </summary>
		public RuntimeMethod InvokeTarget
		{
			get { return _instructionSet.Data[_index].InvokeTarget; }
			set { _instructionSet.Data[_index].InvokeTarget = value; }
		}

		/// <summary>
		/// Gets or sets the string.
		/// </summary>
		/// <value>The string.</value>
		public string String
		{
			get { return _instructionSet.Data[_index].String; }
			set { _instructionSet.Data[_index].String = value; }
		}

		/// <summary>
		/// Gets or sets the runtime field.
		/// </summary>
		/// <value>The runtime field.</value>
		public RuntimeField RuntimeField
		{
			get { return _instructionSet.Data[_index].RuntimeField; }
			set { _instructionSet.Data[_index].RuntimeField = value; }
		}

		/// <summary>
		/// Gets or sets the token.
		/// </summary>
		/// <value>The token.</value>
		public TokenTypes Token
		{
			get { return _instructionSet.Data[_index].Token; }
			set { _instructionSet.Data[_index].Token = value; }
		}

		/// <summary>
		/// Gets or sets the condition code.
		/// </summary>
		/// <value>The condition code.</value>
		public IR.ConditionCode ConditionCode
		{
			get { return _instructionSet.Data[_index].ConditionCode; }
			set { _instructionSet.Data[_index].ConditionCode = value; }
		}

		/// <summary>
		/// Gets or sets the literal data.
		/// </summary>
		/// <value>The token.</value>
		public IR.LiteralData LiteralData
		{
			get { return (IR.LiteralData)Other; }
			set { Other = value; }
		}

		/// <summary>
		/// Gets the other object.
		/// </summary>
		/// <value>The other.</value>
		public object Other
		{
			get { return _instructionSet.Data[_index].Other; }
			set { _instructionSet.Data[_index].Other = value; }
		}

		/// <summary>
		/// Gets a value indicating whether [last instruction].
		/// </summary>
		/// <value><c>true</c> if [last instruction]; otherwise, <c>false</c>.</value>
		public bool IsLastInstruction
		{
			get { return _instructionSet.Next(_index) < 0; }
		}

		/// <summary>
		/// Gets a value indicating whether [end of instruction].
		/// </summary>
		/// <value><c>true</c> if [end of instruction]; otherwise, <c>false</c>.</value>
		public bool EndOfInstruction
		{
			get { return _index < 0; }
		}

		/// <summary>
		/// Returns the contexts of the next instruction
		/// </summary>
		/// <value>The next.</value>
		/// <returns></returns>
		public Context Next
		{
			get { return new Context(_instructionSet, _instructionSet.Next(_index)); }
		}

		/// <summary>
		/// Returns the contexts of the previous instruction
		/// </summary>
		/// <value>The next.</value>
		/// <returns></returns>
		public Context Previous
		{
			get { return new Context(_instructionSet, _instructionSet.Previous(_index)); }
		}

		#endregion // Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Context"/> class.
		/// </summary>
		/// <param name="instructionSet">The instruction set.</param>
		/// <param name="block">The block.</param>
		/// <param name="index">The index.</param>
		public Context(InstructionSet instructionSet, BasicBlock block, int index)
		{
			_instructionSet = instructionSet;
			_index = index;
			_block = block;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Context"/> class.
		/// </summary>
		/// <param name="instructionSet">The instruction set.</param>
		/// <param name="index">The index.</param>
		public Context(InstructionSet instructionSet, int index)
		{
			_instructionSet = instructionSet;
			_index = index;
			_block = null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Context"/> class.
		/// </summary>
		/// <param name="instructionSet">The instruction set.</param>
		/// <param name="basicBlock">The basic block.</param>
		public Context(InstructionSet instructionSet, BasicBlock basicBlock)
		{
			_instructionSet = instructionSet;
			_index = basicBlock.Index;
			_block = basicBlock;
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Visits the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		public void Visit(IVisitor visitor)
		{
			Instruction.Visit(visitor, this);
		}

		/// <summary>
		/// Clones this instance.
		/// </summary>
		/// <returns></returns>
		public Context Clone()
		{
			return new Context(_instructionSet, _block, _index);
		}

		/// <summary>
		/// Nexts this instance.
		/// </summary>
		public void GotoNext()
		{
			_index = _instructionSet.Next(_index);
		}

		/// <summary>
		/// Gotos the previous instruction.
		/// </summary>
		public void GotoPrevious()
		{
			_index = _instructionSet.Previous(_index);
		}

		/// <summary>
		/// Gotos the last instruction.
		/// </summary>
		public void GotoLast()
		{
			if (_index < 0)
				return;

			int next = _instructionSet.Next(_index);

			for (; next < 0; )
				_index = next;
		}

		/// <summary>
		/// Clears the specified context.
		/// </summary>
		public void Clear()
		{
			Operand1 = null;
			Operand2 = null;
			Operand3 = null;
			Result = null;
			_instructionSet.Data[_index].Clear();

			if (OperandCount >= 3)
				for (int i = 3; i < OperandCount; i++)
					SetOperand(i, null);
		}

		/// <summary>
		/// Inserts an instruction the before the current instruction.
		/// </summary>
		/// <returns></returns>
		public Context InsertBefore()
		{
			int offset = Offset;

			if (_instructionSet.Previous(_index) == -1)
				_index = _index + 1 - 1; // TEST

			Context ctx = new Context(_instructionSet, _instructionSet.InsertBefore(_index));
			ctx.Clear();
			ctx.Offset = offset;
			return ctx;
		}

		/// <summary>
		/// Slices the instruction flow before the current instruction
		/// </summary>
		public void SliceBefore()
		{
			_instructionSet.SliceBefore(_index);
		}

		/// <summary>
		/// Slices the instruction flow after the current instruction
		/// </summary>
		public void SliceAfter()
		{
			_instructionSet.SliceAfter(_index);
		}

		/// <summary>
		/// Remove this instance.
		/// </summary>
		public void Remove()
		{
			Clear();

			NewInstruction = null;
			Ignore = true;
			return;

			//int prev = _instructionSet.Previous(_index);

			//// if this instruction was part of the head of the block, then don't remove it 
			//if (prev <= 0) {
			//    NewInstruction = null;
			//    Ignore = true;
			//    return;
			//}

			//_instructionSet.Remove(_index);
			//_index = prev;
		}

		/// <summary>
		/// Replaces the instruction only.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		public void ReplaceInstructionOnly(IInstruction instruction)
		{
			NewInstruction = instruction;
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		public void SetInstruction(IInstruction instruction)
		{
			int offset = Offset;

			if (instruction != null)
				SetInstruction(instruction, instruction.DefaultOperandCount, instruction.DefaultResultCount);
			else
				SetInstruction(null, 0, 0);

			Offset = offset;
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
		/// Inserts the instruction after.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="code">The code.</param>
		public void InsertInstructionAfter(IInstruction instruction, IR.ConditionCode code)
		{
			_index = _instructionSet.InsertAfter(_index);
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
		/// Inserts the instruction after.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		public void InsertInstructionAfter(IInstruction instruction)
		{
			// replace dummy instruction, if possible
			if (_index < 0)
				_index = _instructionSet.InsertAfter(_index);
			else if (instruction != null)
				_index = _instructionSet.InsertAfter(_index);

			SetInstruction(instruction);
		}

		/// <summary>
		/// Inserts the instruction after.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="block">The block.</param>
		public void InsertInstructionAfter(IInstruction instruction, BasicBlock block)
		{
			_index = _instructionSet.InsertAfter(_index);
			SetInstruction(instruction, block);
		}

		/// <summary>
		/// Inserts the instruction after.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="code">The code.</param>
		/// <param name="block">The block.</param>
		public void InsertInstructionAfter(IInstruction instruction, IR.ConditionCode code, BasicBlock block)
		{
			_index = _instructionSet.InsertAfter(_index);
			SetInstruction(instruction, code, block);
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
		public void InsertInstructionAfter(IInstruction instruction, Operand result)
		{
			_index = _instructionSet.InsertAfter(_index);
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
		public void InsertInstructionAfter(IInstruction instruction, Operand result, Operand operand1)
		{
			if (Instruction != null)
				_index = _instructionSet.InsertAfter(_index);

			SetInstruction(instruction, result, operand1);
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
		public void InsertInstructionAfter(IInstruction instruction, Operand result, Operand operand1, Operand operand2)
		{
			_index = _instructionSet.InsertAfter(_index);
			SetInstruction(instruction, result, operand1, operand2);
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
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		/// <param name="operand3">The operand3.</param>
		public void InsertInstructionAfter(IInstruction instruction, Operand result, Operand operand1, Operand operand2, Operand operand3)
		{
			_index = _instructionSet.InsertAfter(_index);
			SetInstruction(instruction, result, operand1, operand2, operand3);
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="operandCount">The operand count.</param>
		/// <param name="resultCount">The result count.</param>
		public void SetInstruction(IInstruction instruction, byte operandCount, byte resultCount)
		{
			if (_index == -1)
				_index = _instructionSet.CreateRoot();
			else
				Clear();

			NewInstruction = instruction;
			OperandCount = operandCount;
			ResultCount = resultCount;
			Ignore = false;
		}

		/// <summary>
		/// Sets the result.
		/// </summary>
		/// <param name="result">The result.</param>
		public void SetResult(Operand result)
		{
			Result = result;
			ResultCount = 1;
		}

		/// <summary>
		/// Sets the results.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="result2">The result2.</param>
		public void SetResults(Operand result, Operand result2)
		{
			Result = result;
			Result2 = result2;
			ResultCount = 2;
		}

		/// <summary>
		/// Sets the operands.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
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
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
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
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		/// <param name="operand3">The operand3.</param>
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
		/// Sets the branch.
		/// </summary>
		/// <param name="block">The block.</param>
		public void SetBranch(BasicBlock block)
		{
			SetBranch(block.Label);
		}

		/// <summary>
		/// Sets the branch.
		/// </summary>
		/// <param name="target1">The target1.</param>
		public void SetBranch(int target1)
		{
			if (Branch == null)
				Branch = new Branch(1);

			Branch.Targets[0] = target1;
		}

		/// <summary>
		/// Sets the branch.
		/// </summary>
		/// <param name="target1">The target1.</param>
		/// <param name="target2">The target2.</param>
		public void SetBranch(int target1, int target2)
		{
			if (Branch == null)
				Branch = new Branch(2);
			else
				if (Branch.Targets.Length < 2) {
					Branch newBranch = new Branch(2);
					newBranch.Targets[0] = Branch.Targets[0];
					Branch = newBranch;
				}

			Branch.Targets[0] = target1;
			Branch.Targets[1] = target2;
		}

		/// <summary>
		/// Gets the operand by index
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public Operand GetOperand(int index)
		{
			switch (index) {
				case 0: return Operand1;
				case 1: return Operand2;
				case 2: return Operand3;
				default: return _instructionSet.Data[_index].GetAdditionalOperand(index);
			}
		}

		/// <summary>
		/// Sets the operand by index
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="operand">The operand.</param>
		public void SetOperand(int index, Operand operand)
		{
			switch (index) {
				case 0: Operand1 = operand; return;
				case 1: Operand2 = operand; return;
				case 2: Operand3 = operand; return;
				default: {
						Operand current = _instructionSet.Data[_index].GetAdditionalOperand(index);
						if (current != null) current.Uses.Remove(_index);
						if (operand != null) operand.Uses.Add(_index);
						_instructionSet.Data[_index].SetAdditionalOperand(index, operand);
						return;
					}
			}
		}

		/// <summary>
		/// Gets the result by index
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public Operand GetResult(int index)
		{
			switch (index) {
				case 0: return Result;
				case 1: return Result2;
				default: break;
			}

			System.Diagnostics.Debug.Assert(false, @"No index");
			return null;
		}
		/// <summary>
		/// Sets the result by index
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="result">The result operand.</param>
		public void SetResult(int index, Operand result)
		{
			switch (index) {
				case 0: Result = result; break;
				case 1: Result2 = result; break;
				default: break;
			}

			System.Diagnostics.Debug.Assert(false, @"No index");
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return Instruction.ToString(this);
		}

		#endregion // Methods
	};

}
