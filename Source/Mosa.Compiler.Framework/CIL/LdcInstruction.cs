﻿/*
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

		#endregion Construction

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

			Operand constantValueOperand;

			// Opcode specific handling
			switch (opcode)
			{
				case OpCode.Ldc_i4:
					{
						int i = decoder.DecodeInt();
						constantValueOperand = Operand.CreateConstantSignedInt(i);
					}
					break;

				case OpCode.Ldc_i4_s:
					{
						sbyte sb = decoder.DecodeSByte();
						constantValueOperand = Operand.CreateConstantSignedInt(sb);
					}
					break;

				case OpCode.Ldc_i8:
					{
						long l = decoder.DecodeLong();
						constantValueOperand = Operand.CreateConstantSignedLong(l);
					}
					break;

				case OpCode.Ldc_r4:
					{
						float f = decoder.DecodeFloat();
						constantValueOperand = Operand.CreateConstantFloat(f);
					}
					break;

				case OpCode.Ldc_r8:
					{
						double d = decoder.DecodeDouble();
						constantValueOperand = Operand.CreateConstantDouble(d);
					}
					break;

				case OpCode.Ldnull:
					constantValueOperand = Operand.GetNull();
					break;

				case OpCode.Ldc_i4_0:
					constantValueOperand = Operand.CreateConstantSignedInt(0);
					break;

				case OpCode.Ldc_i4_1:
					constantValueOperand = Operand.CreateConstantSignedInt(1);
					break;

				case OpCode.Ldc_i4_2:
					constantValueOperand = Operand.CreateConstantSignedInt(2);
					break;

				case OpCode.Ldc_i4_3:
					constantValueOperand = Operand.CreateConstantSignedInt(3);
					break;

				case OpCode.Ldc_i4_4:
					constantValueOperand = Operand.CreateConstantSignedInt(4);
					break;

				case OpCode.Ldc_i4_5:
					constantValueOperand = Operand.CreateConstantSignedInt(5);
					break;

				case OpCode.Ldc_i4_6:
					constantValueOperand = Operand.CreateConstantSignedInt(6);
					break;

				case OpCode.Ldc_i4_7:
					constantValueOperand = Operand.CreateConstantSignedInt(7);
					break;

				case OpCode.Ldc_i4_8:
					constantValueOperand = Operand.CreateConstantSignedInt(8);
					break;

				case OpCode.Ldc_i4_m1:
					constantValueOperand = Operand.CreateConstantSignedInt(-1);
					break;

				default:
					throw new System.NotImplementedException();
			}

			ctx.Operand1 = constantValueOperand;
			ctx.Result = decoder.Compiler.CreateVirtualRegister(constantValueOperand.Type);
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