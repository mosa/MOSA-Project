/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// 
	/// </summary>
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

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Validate(Context ctx, BaseMethodCompiler compiler)
		{
			base.Validate(ctx, compiler);

			SZArraySigType arrayType = ctx.Operand1.Type as SZArraySigType;
			if (arrayType == null)
				throw new InvalidProgramException(@"Operand to ldlen is not a vector.");

			ctx.Result = compiler.CreateVirtualRegister(BuiltInSigType.IntPtr);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldlen(context);
		}

		#endregion Methods

	}
}
