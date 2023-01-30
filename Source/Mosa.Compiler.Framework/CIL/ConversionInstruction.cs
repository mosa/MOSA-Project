// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL;

/// <summary>
/// Implements the internal representation for the IL conversion instructions.
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.CIL.UnaryArithmeticInstruction" />
public sealed class ConversionInstruction : UnaryArithmeticInstruction
{
	#region Construction

	/// <summary>
	/// Initializes a new instance of the <see cref="ConversionInstruction"/> class.
	/// </summary>
	/// <param name="opcode">The opcode.</param>
	public ConversionInstruction(OpCode opcode)
		: base(opcode)
	{
	}

	#endregion Construction

	#region Methods

	/// <summary>
	/// Validates the instruction operands and creates a matching variable for the result.
	/// </summary>
	/// <param name="context"></param>
	/// <param name="methodCompiler">The compiler.</param>
	public override void Resolve(Context context, MethodCompiler methodCompiler)
	{
		base.Resolve(context, methodCompiler);

		// Validate the typecode & determine the resulting stack type

		MosaType resultType = opcode switch
		{
			OpCode.Conv_u => methodCompiler.TypeSystem.BuiltIn.U,
			OpCode.Conv_i => methodCompiler.TypeSystem.BuiltIn.I,
			OpCode.Conv_i1 => methodCompiler.TypeSystem.BuiltIn.I1,
			OpCode.Conv_i2 => methodCompiler.TypeSystem.BuiltIn.I2,
			OpCode.Conv_i4 => methodCompiler.TypeSystem.BuiltIn.I4,
			OpCode.Conv_i8 => methodCompiler.TypeSystem.BuiltIn.I8,
			OpCode.Conv_r4 => methodCompiler.TypeSystem.BuiltIn.R4,
			OpCode.Conv_r8 => methodCompiler.TypeSystem.BuiltIn.R8,
			OpCode.Conv_u1 => methodCompiler.TypeSystem.BuiltIn.U1,
			OpCode.Conv_u2 => methodCompiler.TypeSystem.BuiltIn.U2,
			OpCode.Conv_u4 => methodCompiler.TypeSystem.BuiltIn.U4,
			OpCode.Conv_u8 => methodCompiler.TypeSystem.BuiltIn.U8,
			OpCode.Conv_ovf_u => methodCompiler.TypeSystem.BuiltIn.U,
			OpCode.Conv_ovf_i => methodCompiler.TypeSystem.BuiltIn.I,
			OpCode.Conv_ovf_i1 => methodCompiler.TypeSystem.BuiltIn.I1,
			OpCode.Conv_ovf_i2 => methodCompiler.TypeSystem.BuiltIn.I2,
			OpCode.Conv_ovf_i4 => methodCompiler.TypeSystem.BuiltIn.I4,
			OpCode.Conv_ovf_i8 => methodCompiler.TypeSystem.BuiltIn.I8,
			OpCode.Conv_ovf_u1 => methodCompiler.TypeSystem.BuiltIn.U1,
			OpCode.Conv_ovf_u2 => methodCompiler.TypeSystem.BuiltIn.U2,
			OpCode.Conv_ovf_u4 => methodCompiler.TypeSystem.BuiltIn.U4,
			OpCode.Conv_ovf_u8 => methodCompiler.TypeSystem.BuiltIn.U8,
			OpCode.Conv_ovf_u_un => methodCompiler.TypeSystem.BuiltIn.U,
			OpCode.Conv_ovf_i_un => methodCompiler.TypeSystem.BuiltIn.I,
			OpCode.Conv_ovf_i1_un => methodCompiler.TypeSystem.BuiltIn.I1,
			OpCode.Conv_ovf_i2_un => methodCompiler.TypeSystem.BuiltIn.I2,
			OpCode.Conv_ovf_i4_un => methodCompiler.TypeSystem.BuiltIn.I4,
			OpCode.Conv_ovf_i8_un => methodCompiler.TypeSystem.BuiltIn.I8,
			OpCode.Conv_ovf_u1_un => methodCompiler.TypeSystem.BuiltIn.U1,
			OpCode.Conv_ovf_u2_un => methodCompiler.TypeSystem.BuiltIn.U2,
			OpCode.Conv_ovf_u4_un => methodCompiler.TypeSystem.BuiltIn.U4,
			OpCode.Conv_ovf_u8_un => methodCompiler.TypeSystem.BuiltIn.U8,
			OpCode.Conv_r_un => methodCompiler.TypeSystem.BuiltIn.R8,
			_ => throw new CompilerException("Unknown conversion")
		};

		var result = methodCompiler.Compiler.GetStackType(resultType);

		context.Result = methodCompiler.CreateVirtualRegister(resultType);
		context.MosaType = resultType;
	}

	#endregion Methods
}
