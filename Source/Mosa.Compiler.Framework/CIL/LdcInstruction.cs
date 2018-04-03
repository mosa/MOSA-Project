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
				case OpCode.Ldc_i4: constantValueOperand = decoder.MethodCompiler.CreateConstant((int)decoder.Instruction.Operand); break;
				case OpCode.Ldc_i4_s: constantValueOperand = decoder.MethodCompiler.CreateConstant((sbyte)decoder.Instruction.Operand); break;
				case OpCode.Ldc_i8: constantValueOperand = decoder.MethodCompiler.CreateConstant((long)decoder.Instruction.Operand); break;
				case OpCode.Ldc_r4: constantValueOperand = decoder.MethodCompiler.CreateConstant((float)decoder.Instruction.Operand); break;
				case OpCode.Ldc_r8: constantValueOperand = decoder.MethodCompiler.CreateConstant((double)decoder.Instruction.Operand); break;
				case OpCode.Ldnull: constantValueOperand = Operand.GetNullObject(decoder.TypeSystem); break;
				case OpCode.Ldc_i4_0: constantValueOperand = decoder.MethodCompiler.CreateConstant(0); break;
				case OpCode.Ldc_i4_1: constantValueOperand = decoder.MethodCompiler.CreateConstant(1); break;
				case OpCode.Ldc_i4_2: constantValueOperand = decoder.MethodCompiler.CreateConstant(2); break;
				case OpCode.Ldc_i4_3: constantValueOperand = decoder.MethodCompiler.CreateConstant(3); break;
				case OpCode.Ldc_i4_4: constantValueOperand = decoder.MethodCompiler.CreateConstant(4); break;
				case OpCode.Ldc_i4_5: constantValueOperand = decoder.MethodCompiler.CreateConstant(5); break;
				case OpCode.Ldc_i4_6: constantValueOperand = decoder.MethodCompiler.CreateConstant(6); break;
				case OpCode.Ldc_i4_7: constantValueOperand = decoder.MethodCompiler.CreateConstant(7); break;
				case OpCode.Ldc_i4_8: constantValueOperand = decoder.MethodCompiler.CreateConstant(8); break;
				case OpCode.Ldc_i4_m1: constantValueOperand = decoder.MethodCompiler.CreateConstant(-1); break;
				default: throw new NotImplementCompilerException();
			}

			node.Operand1 = constantValueOperand;
			node.Result = decoder.MethodCompiler.CreateVirtualRegister(constantValueOperand.Type);
		}

		#endregion Methods
	}
}
