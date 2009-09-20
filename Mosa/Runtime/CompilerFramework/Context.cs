using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework.IR;
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
		public BasicBlock Block
		{
			get { return _block; }
			set { _block = value; }
		}

		/// <summary>
		/// Gets or sets the instruction set.
		/// </summary>
		public InstructionSet Instructions
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
			get { return _instructionSet.instructions[_index].Instruction; }
			set { _instructionSet.instructions[_index].Instruction = value; }
		}

		/// <summary>
		/// Gets the offset.
		/// </summary>
		/// <value>The offset.</value>
		public int Offset
		{
			get { return _instructionSet.instructions[_index].Offset; }
			set { _instructionSet.instructions[_index].Offset = value; }
		}

		/// <summary>
		/// Gets the result operand.
		/// </summary>
		/// <value>The result operand.</value>
		public IBranch Branch
		{
			get { return _instructionSet.instructions[_index].Branch; }
			set { _instructionSet.instructions[_index].Branch = value; }
		}

		/// <summary>
		/// Gets the result operand.
		/// </summary>
		/// <value>The result operand.</value>
		public Operand Result
		{
			get { return _instructionSet.instructions[_index].Result; }
		}

		/// <summary>
		/// Gets the second result operand.
		/// </summary>
		/// <value>The second result operand.</value>
		public Operand Result2
		{
			get { return _instructionSet.instructions[_index].Result2; }
		}

		/// <summary>
		/// Gets the first operand.
		/// </summary>
		/// <value>The first operand.</value>
		public Operand Operand1
		{
			get { return _instructionSet.instructions[_index].Operand1; }
		}

		/// <summary>
		/// Gets the first operand.
		/// </summary>
		/// <value>The first operand.</value>
		public Operand Operand2
		{
			get { return _instructionSet.instructions[_index].Operand2; }
		}

		/// <summary>
		/// Gets the first operand.
		/// </summary>
		/// <value>The first operand.</value>
		public Operand Operand3
		{
			get { return _instructionSet.instructions[_index].Operand3; }
		}

		/// <summary>
		/// Gets the operand count.
		/// </summary>
		/// <value>The operand count.</value>
		public byte OperandCount
		{
			get { return _instructionSet.instructions[_index].OperandCount; }
		}

		/// <summary>
		/// Gets the result count.
		/// </summary>
		/// <value>The result count.</value>
		public byte ResultCount
		{
			get { return _instructionSet.instructions[_index].ResultCount; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Context"/> is ignore.
		/// </summary>
		/// <value><c>true</c> if ignore; otherwise, <c>false</c>.</value>
		public bool Ignore
		{
			get { return _instructionSet.instructions[_index].Ignore; }
			set { _instructionSet.instructions[_index].Ignore = value; }
		}

		/// <summary>
		/// Holds the function being called.
		/// </summary>
		public RuntimeMethod InvokeTarget
		{
			get { return _instructionSet.instructions[_index].InvokeTarget; }
		}

		/// <summary>
		/// Holds the string.
		/// </summary>
		public string String
		{
			get { return _instructionSet.instructions[_index].String; }
		}

		/// <summary>
		/// Holds the field of the load instruction.
		/// </summary>
		public RuntimeField RuntimeField
		{
			get { return _instructionSet.instructions[_index].RuntimeField; }
		}

		/// <summary>
		/// Holds the token type.
		/// </summary>
		/// <value>The token.</value>
		public TokenTypes Token
		{
			get { return _instructionSet.instructions[_index].Token; }
		}

		/// <summary>
		/// Gets a value indicating whether [end of instructions].
		/// </summary>
		/// <value><c>true</c> if [end of instructions]; otherwise, <c>false</c>.</value>
		public bool EndOfInstructions
		{
			get { return _index < 0; }
		}

		/// <summary>
		/// Gets a value indicating whether [last instruction].
		/// </summary>
		/// <value><c>true</c> if [last instruction]; otherwise, <c>false</c>.</value>
		public bool LastInstruction
		{
			get { return _instructionSet.Next(_index) < 0; }
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
		/// <param name="block">The basic block.</param>
		public Context(BasicBlock block)
		{
			_block = block;
			_index = block.Index;
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
		/// Inserts an instruction the after the current instruction.
		/// </summary>
		/// <returns></returns>
		public Context InsertAfter()
		{
			Context ctx = new Context(_instructionSet, _instructionSet.InsertAfter(_index));
			ctx.Block = _block;
			ctx.Instruction = null;
			ctx.Ignore = true;
			return ctx;
		}

		/// <summary>
		/// Slices this instance.
		/// </summary>
		public void SliceAfter()
		{
			_instructionSet.SliceAfter(_index);
		}

		#endregion // Methods
	};

}
