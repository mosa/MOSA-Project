/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;

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
						int i = (int)decoder.Instruction.Operand;
						constantValueOperand = Operand.CreateConstantSignedInt(decoder.TypeSystem, i);
					}
					break;

				case OpCode.Ldc_i4_s:
					{
						sbyte sb = (sbyte)decoder.Instruction.Operand;
						constantValueOperand = Operand.CreateConstantSignedInt(decoder.TypeSystem, sb);
					}
					break;

				case OpCode.Ldc_i8:
					{
						long l = (long)decoder.Instruction.Operand;
						constantValueOperand = Operand.CreateConstantSignedLong(decoder.TypeSystem, l);
					}
					break;

				case OpCode.Ldc_r4:
					{
						float f = (float)decoder.Instruction.Operand;
						constantValueOperand = Operand.CreateConstantSingle(decoder.TypeSystem, f);
					}
					break;

				case OpCode.Ldc_r8:
					{
						double d = (double)decoder.Instruction.Operand;
						constantValueOperand = Operand.CreateConstantDouble(decoder.TypeSystem, d);
					}
					break;

				case OpCode.Ldnull: constantValueOperand = Operand.GetNull(decoder.TypeSystem); break;
				case OpCode.Ldc_i4_0: constantValueOperand = Operand.CreateConstantSignedInt(decoder.TypeSystem, 0); break;
				case OpCode.Ldc_i4_1: constantValueOperand = Operand.CreateConstantSignedInt(decoder.TypeSystem, 1); break;
				case OpCode.Ldc_i4_2: constantValueOperand = Operand.CreateConstantSignedInt(decoder.TypeSystem, 2); break;
				case OpCode.Ldc_i4_3: constantValueOperand = Operand.CreateConstantSignedInt(decoder.TypeSystem, 3); break;
				case OpCode.Ldc_i4_4: constantValueOperand = Operand.CreateConstantSignedInt(decoder.TypeSystem, 4); break;
				case OpCode.Ldc_i4_5: constantValueOperand = Operand.CreateConstantSignedInt(decoder.TypeSystem, 5); break;
				case OpCode.Ldc_i4_6: constantValueOperand = Operand.CreateConstantSignedInt(decoder.TypeSystem, 6); break;
				case OpCode.Ldc_i4_7: constantValueOperand = Operand.CreateConstantSignedInt(decoder.TypeSystem, 7); break;
				case OpCode.Ldc_i4_8: constantValueOperand = Operand.CreateConstantSignedInt(decoder.TypeSystem, 8); break;
				case OpCode.Ldc_i4_m1: constantValueOperand = Operand.CreateConstantSignedInt(decoder.TypeSystem, -1); break;
				default: throw new NotImplementCompilerException();
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