/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class BaseCILInstruction : BaseInstruction
	{
		#region Data members

		/// <summary>
		/// Holds the CIL opcode
		/// </summary>
		protected readonly OpCode opcode;

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Gets the op code.
		/// </summary>
		/// <value>The op code.</value>
		public OpCode OpCode { get { return opcode; } }

		#endregion // Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseCILInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		public BaseCILInstruction(OpCode opCode)
			: base()
		{
			this.opcode = opCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseCILInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="operandCount">The operand count.</param>
		public BaseCILInstruction(OpCode opCode, byte operandCount)
			: base(operandCount, 0)
		{
			this.opcode = opCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseCILInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="operandCount">The operand count.</param>
		/// <param name="resultCount">The result count.</param>
		public BaseCILInstruction(OpCode opCode, byte operandCount, byte resultCount)
			: base(operandCount, resultCount)
		{
			this.opcode = opCode;
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public virtual void Decode(Context ctx, IInstructionDecoder decoder)
		{
			ctx.SetInstruction(this, DefaultOperandCount, DefaultResultCount);
		}

		/// <summary>
		/// Determines if the IL decoder pushes the results of this instruction onto the IL operand stack.
		/// </summary>
		/// <value><c>true</c> if [push result]; otherwise, <c>false</c>.</value>
		public virtual bool PushResult
		{
			get { return true; }
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public virtual void Visit(ICILVisitor visitor, Context context)
		{
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IVisitor visitor, Context context)
		{
			if (visitor is ICILVisitor)
				Visit(visitor as ICILVisitor, context);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A string representation of the operand.
		/// </returns>
		public override string ToString()
		{
			string code = opcode.ToString();
			int index = code.IndexOf("Instruction");

			if (index > 0)
				code = code.Substring(0, index);

			return "CIL." + code;
		}

		#endregion //  Overrides
	}
}
