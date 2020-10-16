// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Represents a unary instruction, which performs an operation on the operand and places
	/// the result on the stack.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.UnaryInstruction" />
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

		#endregion Construction

		#region Methods Overrides

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The compiler.</param>
		public override void Resolve(Context context, MethodCompiler methodCompiler)
		{
			base.Resolve(context, methodCompiler);

			// Simple result is the same type as the unary argument
			context.Result = methodCompiler.CreateVirtualRegister(context.Operand1.Type);
		}

		#endregion Methods Overrides
	}
}
