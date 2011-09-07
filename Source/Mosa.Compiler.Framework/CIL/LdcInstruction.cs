/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class LdcInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdcInstruction"/> class.
		/// </summary>
		public LdcInstruction(OpCode opCode)
			: base(opCode, 1)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			ConstantOperand constantValueOperand;

			// Opcode specific handling
			switch (opcode)
			{
				case OpCode.Ldc_i4:
					{
						int i = decoder.DecodeInt();
						constantValueOperand = new ConstantOperand(new SigType(CilElementType.I4), i);
					}
					break;

				case OpCode.Ldc_i4_s:
					{
						sbyte sb = decoder.DecodeSByte();
						constantValueOperand = new ConstantOperand(new SigType(CilElementType.I4), sb);
					}
					break;

				case OpCode.Ldc_i8:
					{
						long l = decoder.DecodeLong();
						constantValueOperand = new ConstantOperand(new SigType(CilElementType.I8), l);
					}
					break;

				case OpCode.Ldc_r4:
					{
						float f = decoder.DecodeFloat();
						constantValueOperand = new ConstantOperand(new SigType(CilElementType.R4), f);
					}
					break;

				case OpCode.Ldc_r8:
					{
						double d = decoder.DecodeDouble();
						constantValueOperand = new ConstantOperand(new SigType(CilElementType.R8), d);
					}
					break;

				case OpCode.Ldnull:
					constantValueOperand = ConstantOperand.GetNull();
					break;

				case OpCode.Ldc_i4_0:
					constantValueOperand = ConstantOperand.FromValue(0);
					break;

				case OpCode.Ldc_i4_1:
					constantValueOperand = ConstantOperand.FromValue(1);
					break;

				case OpCode.Ldc_i4_2:
					constantValueOperand = ConstantOperand.FromValue(2);
					break;

				case OpCode.Ldc_i4_3:
					constantValueOperand = ConstantOperand.FromValue(3);
					break;

				case OpCode.Ldc_i4_4:
					constantValueOperand = ConstantOperand.FromValue(4);
					break;

				case OpCode.Ldc_i4_5:
					constantValueOperand = ConstantOperand.FromValue(5);
					break;

				case OpCode.Ldc_i4_6:
					constantValueOperand = ConstantOperand.FromValue(6);
					break;

				case OpCode.Ldc_i4_7:
					constantValueOperand = ConstantOperand.FromValue(7);
					break;

				case OpCode.Ldc_i4_8:
					constantValueOperand = ConstantOperand.FromValue(8);
					break;

				case OpCode.Ldc_i4_m1:
					constantValueOperand = ConstantOperand.FromValue(-1);
					break;

				default:
					throw new System.NotImplementedException();
			}

			ctx.Operand1 = constantValueOperand;
			ctx.Result = decoder.Compiler.CreateTemporary(constantValueOperand.Type);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldc(context);
		}

		#endregion Methods

	}
}
