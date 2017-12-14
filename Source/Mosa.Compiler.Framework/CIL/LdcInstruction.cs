// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Ldc Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.LoadInstruction" />
	public sealed class LdcInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdcInstruction" /> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		public LdcInstruction(OpCode opCode)
			: base(opCode, 1)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(InstructionNode node, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(node, decoder);

			Operand constantValueOperand;

			// Opcode specific handling
			switch (opcode)
			{
				case OpCode.Ldc_i4:
					{
						int i = (int)decoder.Instruction.Operand;
						constantValueOperand = Operand.CreateConstant(i, decoder.TypeSystem);
					}
					break;

				case OpCode.Ldc_i4_s:
					{
						sbyte sb = (sbyte)decoder.Instruction.Operand;
						constantValueOperand = Operand.CreateConstant(sb, decoder.TypeSystem);
					}
					break;

				case OpCode.Ldc_i8:
					{
						long l = (long)decoder.Instruction.Operand;
						constantValueOperand = Operand.CreateConstant(l, decoder.TypeSystem);
					}
					break;

				case OpCode.Ldc_r4:
					{
						float f = (float)decoder.Instruction.Operand;
						constantValueOperand = Operand.CreateConstant(f, decoder.TypeSystem);
					}
					break;

				case OpCode.Ldc_r8:
					{
						double d = (double)decoder.Instruction.Operand;
						constantValueOperand = Operand.CreateConstant(d, decoder.TypeSystem);
					}
					break;

				case OpCode.Ldnull: constantValueOperand = Operand.GetNullObject(decoder.TypeSystem); break;
				case OpCode.Ldc_i4_0: constantValueOperand = Operand.CreateConstant(0, decoder.TypeSystem); break;
				case OpCode.Ldc_i4_1: constantValueOperand = Operand.CreateConstant(1, decoder.TypeSystem); break;
				case OpCode.Ldc_i4_2: constantValueOperand = Operand.CreateConstant(2, decoder.TypeSystem); break;
				case OpCode.Ldc_i4_3: constantValueOperand = Operand.CreateConstant(3, decoder.TypeSystem); break;
				case OpCode.Ldc_i4_4: constantValueOperand = Operand.CreateConstant(4, decoder.TypeSystem); break;
				case OpCode.Ldc_i4_5: constantValueOperand = Operand.CreateConstant(5, decoder.TypeSystem); break;
				case OpCode.Ldc_i4_6: constantValueOperand = Operand.CreateConstant(6, decoder.TypeSystem); break;
				case OpCode.Ldc_i4_7: constantValueOperand = Operand.CreateConstant(7, decoder.TypeSystem); break;
				case OpCode.Ldc_i4_8: constantValueOperand = Operand.CreateConstant(8, decoder.TypeSystem); break;
				case OpCode.Ldc_i4_m1: constantValueOperand = Operand.CreateConstant(-1, decoder.TypeSystem); break;
				default: throw new NotImplementCompilerException();
			}

			node.Operand1 = constantValueOperand;
			node.Result = decoder.MethodCompiler.CreateVirtualRegister(constantValueOperand.Type);
		}

		#endregion Methods
	}
}
