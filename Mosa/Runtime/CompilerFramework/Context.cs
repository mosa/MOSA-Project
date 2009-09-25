using System;
using System.Collections.Generic;
using System.Text;

using IR = Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{

	/// <summary>
	/// Provides context for transformations.
	/// </summary>
	public class Context
	{
		#region Data members

		/// <summary>
		/// Holds the block being operated on.
		/// </summary>
		private BasicBlock _block;

		/// <summary>
		/// Holds the instruction index operated on.
		/// </summary>
		private int _index;

		/// <summary>
		/// Holds the list of instructions
		/// </summary>
		private InstructionSet _instructionSet;

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
		/// Gets the result operand.
		/// </summary>
		/// <value>The result operand.</value>
		public IInstruction Instruction
		{
			get { return _instructionSet.Data[_index].Instruction; }
			set { _instructionSet.Data[_index].Instruction = value; }
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
		/// Gets or sets the block index
		/// </summary>
		public int Block
		{
			get { return _instructionSet.Data[_index].Block; }
			set { _instructionSet.Data[_index].Block = value; }
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
		/// Gets the result operand.
		/// </summary>
		/// <value>The result operand.</value>
		public Operand Result
		{
			get { return _instructionSet.Data[_index].Result; }
			set { _instructionSet.Data[_index].Result = value; }
		}

		/// <summary>
		/// Gets the second result operand.
		/// </summary>
		/// <value>The second result operand.</value>
		public Operand Result2
		{
			get { return _instructionSet.Data[_index].Result2; }
			set { _instructionSet.Data[_index].Result2 = value; }
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
				if (Result2 != null)
					yield return Result2;
			}
		}

		/// <summary>
		/// Gets the first operand.
		/// </summary>
		/// <value>The first operand.</value>
		public Operand Operand1
		{
			get { return _instructionSet.Data[_index].Operand1; }
			set { _instructionSet.Data[_index].Operand1 = value; }
		}

		/// <summary>
		/// Gets the first operand.
		/// </summary>
		/// <value>The first operand.</value>
		public Operand Operand2
		{
			get { return _instructionSet.Data[_index].Operand2; }
			set { _instructionSet.Data[_index].Operand2 = value; }
		}

		/// <summary>
		/// Gets the first operand.
		/// </summary>
		/// <value>The first operand.</value>
		public Operand Operand3
		{
			get { return _instructionSet.Data[_index].Operand3; }
			set { _instructionSet.Data[_index].Operand3 = value; }
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
		/// Gets or sets the instruction prefix.
		/// </summary>
		/// <value>The prefix.</value>
		public IInstruction Prefix
		{
			get { return _instructionSet.Data[_index].Prefix; }
			set { _instructionSet.Data[_index].Prefix = value; }
		}

		/// <summary>
		/// Holds the function being called.
		/// </summary>
		public RuntimeMethod InvokeTarget
		{
			get { return _instructionSet.Data[_index].InvokeTarget; }
		}

		/// <summary>
		/// Holds the string.
		/// </summary>
		public string String
		{
			get { return _instructionSet.Data[_index].String; }
		}

		/// <summary>
		/// Holds the field of the load instruction.
		/// </summary>
		public RuntimeField RuntimeField
		{
			get { return _instructionSet.Data[_index].RuntimeField; }
		}

		/// <summary>
		/// Holds the token type.
		/// </summary>
		/// <value>The token.</value>
		public TokenTypes Token
		{
			get { return _instructionSet.Data[_index].Token; }
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
		/// Nexts this instance.
		/// </summary>
		/// <value>The next.</value>
		/// <returns></returns>
		public Context Next
		{
			get { return new Context(_instructionSet, _instructionSet.Next(_index)); }
		}

		#endregion // Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Context"/> class.
		/// </summary>
		/// <param name="instructionSet">The instruction set.</param>
		/// <param name="index">The index.</param>
		public Context(InstructionSet instructionSet, int index)
		{
			_block = null;
			_index = index;
			_instructionSet = instructionSet;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Context"/> class.
		/// </summary>
		/// <param name="basicBlock">The basic block.</param>
		public Context(BasicBlock basicBlock)
		{
			_block = basicBlock;
			_index = basicBlock.Index;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Context"/> class.
		/// </summary>
		/// <param name="instructionSet">The instruction set.</param>
		/// <param name="basicBlock">The basic block.</param>
		public Context(InstructionSet instructionSet, BasicBlock basicBlock)
		{
			_instructionSet = instructionSet;
			_block = basicBlock;
			_index = basicBlock.Index;
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Moves context to the next instruction
		/// </summary>
		public void Forward()
		{
			_index = _instructionSet.Next(_index);
		}

		/// <summary>
		/// Moves context to the previous instruction
		/// </summary>
		public void Backwards()
		{
			_index = _instructionSet.Previous(_index);
		}

		/// <summary>
		/// Gotos the last.
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
			_instructionSet.Data[_index].Clear();
		}

		/// <summary>
		/// Inserts an instruction the after the current instruction.
		/// </summary>
		/// <returns></returns>
		public Context InsertAfter()
		{
			Context ctx = new Context(_instructionSet, _instructionSet.InsertAfter(_index));
			ctx.Clear();
			return ctx;
		}

		/// <summary>
		/// Inserts an instruction the before the current instruction.
		/// </summary>
		/// <returns></returns>
		public Context InsertBefore()
		{
			Context ctx = new Context(_instructionSet, _instructionSet.InsertBefore(_index));
			ctx.Clear();
			return ctx;
		}

		/// <summary>
		/// Slices this instance.
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
			int prev = _instructionSet.Previous(_index);

			if (prev <= 0)
				prev = _instructionSet.Next(_index);

			_instructionSet.Remove(_index);

			_index = prev;
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		public void SetInstruction(IInstruction instruction)
		{
			if (instruction is CIL.ICILInstruction)
				SetInstruction(instruction,
					(instruction as CIL.ICILInstruction).DefaultOperandCount,
					(instruction as CIL.ICILInstruction).DefaultResultCount);
			else
				SetInstruction(instruction, 0, 0);
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
		/// Inserts the instruction after.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		public void InsertInstructionAfter(IInstruction instruction)
		{
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
		}

		/// <summary>
		/// Sets the instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		public void SetInstruction(IInstruction instruction, Operand result, Operand operand1)
		{
			SetInstruction(instruction, 1, 1);
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
			SetInstruction(instruction, 2, 1);
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
			SetInstruction(instruction, 3, 1);
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
			Clear();
			Instruction = instruction;
			Ignore = false;
			OperandCount = operandCount;
			ResultCount = resultCount;
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
			ResultCount = 1;
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
			ResultCount = 1;
			OperandCount = 3;
		}

		/// <summary>
		/// Sets the branch.
		/// </summary>
		/// <param name="block">The block.</param>
		public void SetBranch(BasicBlock block)
		{
			if (Branch == null)
				Branch = new Branch(1);

			Branch.Targets[0] = block.Label;
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
			Branch.Targets[0] = target2;
		}

		#endregion // Methods
	};

}
