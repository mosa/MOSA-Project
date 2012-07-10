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
	/// Represents a unary instruction, which performs an operation on the operand and places
	/// the result on the stack.
	/// </summary>
	public class UnaryArithmeticInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="UnaryArithmeticInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public UnaryArithmeticInstruction(OpCode opcode)
			: base(opcode, 1)
		{
		}

		#endregion // Construction

		#region Methods Overrides

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Validate(Context ctx, BaseMethodCompiler compiler)
		{
			base.Validate(ctx, compiler);

			// Simple result is the same type as the unary argument
			ctx.Result = compiler.CreateVirtualRegister(ctx.Operand1.Type);
		}


		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.UnaryArithmetic(context);
		}

		#endregion // Methods Overrides
	}
}
