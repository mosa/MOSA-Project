/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class NotInstruction : UnaryArithmeticInstruction
	{
		#region Operand Table

		/// <summary>
		/// Operand table according to ISO/IEC 23271:2006 (E), Partition III, 1.5, Table 5.
		/// </summary>
		private static readonly StackTypeCode[] _opTable = new StackTypeCode[] {
			StackTypeCode.Unknown,
			StackTypeCode.Int32,   
			StackTypeCode.Int64, 
			StackTypeCode.N,
			StackTypeCode.Unknown, 
			StackTypeCode.Unknown, 
			StackTypeCode.Unknown
		};

		#endregion // Operand Table

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="NotInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public NotInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="compiler">The compiler.</param>
		public override void Validate(Context ctx, BaseMethodCompiler compiler)
		{
			base.Validate(ctx, compiler);

			// Validate the operand
			StackTypeCode result = _opTable[(int)ctx.Operand1.StackType];
			if (StackTypeCode.Unknown == result)
				throw new InvalidOperationException(@"Invalid operand to Not instruction.");

			ctx.Result = compiler.CreateVirtualRegister(ctx.Operand1.Type);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Not(context);
		}

		#endregion Methods

	}
}
