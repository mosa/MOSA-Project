// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Ldlen Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.UnaryInstruction" />
	public sealed class LdlenInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdlenInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdlenInstruction(OpCode opcode)
			: base(opcode, 1)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The compiler.</param>
		public override void Resolve(Context context, MethodCompiler methodCompiler)
		{
			if (context == null)
				throw new System.ArgumentNullException(nameof(context));

			base.Resolve(context, methodCompiler);

			context.Result = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.U);
		}

		#endregion Methods
	}
}
