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
	/// Implements the internal representation for the IL conversion instructions.
	/// </summary>
	public sealed class ConversionInstruction : UnaryArithmeticInstruction
	{
		#region Data members

		// FIXME
		private static StackTypeCode[] _conversionTable = new StackTypeCode[] {

		};

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ConversionInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public ConversionInstruction(OpCode opcode)
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

			// Validate the typecode & determine the resulting stack type
			SigType resultType;

			switch (opcode)
			{
				case OpCode.Conv_u: goto case OpCode.Conv_i;
				case OpCode.Conv_i:
					resultType = compiler.Architecture.NativeType;
					break;

				case OpCode.Conv_i1:
					resultType = BuiltInSigType.SByte;
					break;

				case OpCode.Conv_i2:
					resultType = BuiltInSigType.Int16;
					break;

				case OpCode.Conv_i4:
					resultType = BuiltInSigType.Int32;
					break;

				case OpCode.Conv_i8:
					resultType = BuiltInSigType.Int64;
					break;

				case OpCode.Conv_r4:
					resultType = BuiltInSigType.Single;
					break;

				case OpCode.Conv_r8:
					resultType = BuiltInSigType.Double;
					break;

				case OpCode.Conv_u1:
					resultType = BuiltInSigType.Byte;
					break;

				case OpCode.Conv_u2:
					resultType = BuiltInSigType.UInt16;
					break;

				case OpCode.Conv_u4:
					resultType = BuiltInSigType.UInt32;
					break;

				case OpCode.Conv_u8:
					resultType = BuiltInSigType.UInt64;
					break;

				case OpCode.Conv_ovf_i: goto case OpCode.Conv_i;
				case OpCode.Conv_ovf_u: goto case OpCode.Conv_i;

				case OpCode.Conv_ovf_i_un: goto case OpCode.Conv_i;
				case OpCode.Conv_ovf_u_un: goto case OpCode.Conv_i;

				default:
					throw new NotSupportedException(@"Overflow checking conversions not supported.");
			}

			ctx.Result = compiler.CreateVirtualRegister(resultType);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Conversion(context);
		}

		#endregion Methods

	}
}
