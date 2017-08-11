// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// NotI nstruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.UnaryArithmeticInstruction" />
	public sealed class NotInstruction : UnaryArithmeticInstruction
	{
		#region Operand Table

		/// <summary>
		/// Operand table according to ISO/IEC 23271:2006 (E), Partition III, 1.5, Table 5.
		/// </summary>
		private static readonly StackTypeCode[] opTable = new StackTypeCode[] {
			StackTypeCode.Unknown,
			StackTypeCode.Int32,
			StackTypeCode.Int64,
			StackTypeCode.N,
			StackTypeCode.Unknown,
			StackTypeCode.Unknown,
			StackTypeCode.Unknown,
			StackTypeCode.Unknown
		};

		#endregion Operand Table

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="NotInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public NotInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="compiler">The compiler.</param>
		public override void Resolve(Context context, BaseMethodCompiler compiler)
		{
			base.Resolve(context, compiler);

			// Validate the operand
			var result = opTable[(int)context.Operand1.Type.GetStackTypeCode()];
			if (StackTypeCode.Unknown == result)
				throw new InvalidOperationException("Invalid operand to Not instruction.");

			context.Result = compiler.CreateVirtualRegister(context.Operand1.Type);
		}

		#endregion Methods
	}
}
