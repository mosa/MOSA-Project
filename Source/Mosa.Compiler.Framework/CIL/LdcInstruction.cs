// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.Framework.CIL;

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

		// Opcode specific handling
		Operand constantValueOperand = opcode switch
		{
			OpCode.Ldc_i4 => decoder.MethodCompiler.CreateConstant((int)decoder.Instruction.Operand),
			OpCode.Ldc_i4_s => decoder.MethodCompiler.CreateConstant((sbyte)decoder.Instruction.Operand),
			OpCode.Ldc_i8 => decoder.MethodCompiler.CreateConstant((long)decoder.Instruction.Operand),
			OpCode.Ldc_r4 => decoder.MethodCompiler.CreateConstant((float)decoder.Instruction.Operand),
			OpCode.Ldc_r8 => decoder.MethodCompiler.CreateConstant((double)decoder.Instruction.Operand),
			OpCode.Ldnull => Operand.GetNullObject(decoder.TypeSystem),
			OpCode.Ldc_i4_0 => decoder.MethodCompiler.CreateConstant(0),
			OpCode.Ldc_i4_1 => decoder.MethodCompiler.CreateConstant(1),
			OpCode.Ldc_i4_2 => decoder.MethodCompiler.CreateConstant(2),
			OpCode.Ldc_i4_3 => decoder.MethodCompiler.CreateConstant(3),
			OpCode.Ldc_i4_4 => decoder.MethodCompiler.CreateConstant(4),
			OpCode.Ldc_i4_5 => decoder.MethodCompiler.CreateConstant(5),
			OpCode.Ldc_i4_6 => decoder.MethodCompiler.CreateConstant(6),
			OpCode.Ldc_i4_7 => decoder.MethodCompiler.CreateConstant(7),
			OpCode.Ldc_i4_8 => decoder.MethodCompiler.CreateConstant(8),
			OpCode.Ldc_i4_m1 => decoder.MethodCompiler.CreateConstant(-1),
			_ => throw new NotImplementCompilerException()
		};

		node.Operand1 = constantValueOperand;
		node.Result = decoder.MethodCompiler.CreateVirtualRegister(constantValueOperand.Type);
	}

	#endregion Methods
}
