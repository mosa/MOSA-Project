// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Neg Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.UnaryArithmeticInstruction" />
	public sealed class NegInstruction : UnaryArithmeticInstruction
	{
		#region Data Members

		/// <summary>
		/// Holds the typecode validation table from ISO/IEC 23271:2006 (E),
		/// Partition III, §1.5, Table 3.
		/// </summary>
		private static readonly StackTypeCode[] typeCodes = new StackTypeCode[] {
			StackTypeCode.Unknown,
			StackTypeCode.Int32,
			StackTypeCode.Int64,
			StackTypeCode.N,
			StackTypeCode.F,
			StackTypeCode.Unknown,
			StackTypeCode.Unknown,
			StackTypeCode.Unknown
		};

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="NegInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public NegInstruction(OpCode opcode)
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
		public override void Resolve(Context context, MethodCompiler compiler)
		{
			base.Resolve(context, compiler);

			// Validate the operand
			var result = typeCodes[(int)context.Operand1.Type.GetStackTypeCode()];
			if (StackTypeCode.Unknown == result)
				throw new InvalidOperationException("Invalid operand to Neg instruction [" + result + "]");

			context.Result = compiler.CreateVirtualRegister(context.Operand1.Type);
		}

		#endregion Methods
	}
}
