// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Ldobj Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.UnaryInstruction" />
	public sealed class LdobjInstruction : UnaryInstruction
	{
		/// <summary>
		/// A fixed typeref for ldind.* instructions.
		/// </summary>
		private readonly MosaTypeCode? elementType;

		/// <summary>
		/// Initializes a new instance of the <see cref="LdobjInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdobjInstruction(OpCode opcode)
			: base(opcode, 1)
		{
			switch (opcode)
			{
				case OpCode.Ldind_i1: elementType = MosaTypeCode.I1; break;
				case OpCode.Ldind_i2: elementType = MosaTypeCode.I2; break;
				case OpCode.Ldind_i4: elementType = MosaTypeCode.I4; break;
				case OpCode.Ldind_i8: elementType = MosaTypeCode.I8; break;
				case OpCode.Ldind_u1: elementType = MosaTypeCode.U1; break;
				case OpCode.Ldind_u2: elementType = MosaTypeCode.U2; break;
				case OpCode.Ldind_u4: elementType = MosaTypeCode.U4; break;
				case OpCode.Ldind_i: elementType = MosaTypeCode.I; break;
				case OpCode.Ldind_r4: elementType = MosaTypeCode.R4; break;
				case OpCode.Ldind_r8: elementType = MosaTypeCode.R8; break;
				case OpCode.Ldind_ref: elementType = MosaTypeCode.Object; break;
				case OpCode.Ldobj: elementType = null; break;
				default: throw new NotImplementCompilerException();
			}
		}

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(InstructionNode node, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(node, decoder);

			var type = (elementType == null)
				? (MosaType)decoder.Instruction.Operand
				: decoder.MethodCompiler.Compiler.GetTypeFromTypeCode(elementType.Value);

			// Push the loaded value
			node.Result = decoder.MethodCompiler.AllocateVirtualRegisterOrStackSlot(type);
			node.MosaType = type;

			//System.Diagnostics.Debug.WriteLine(decoder.Method.FullName); //temp - remove me
		}

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The compiler.</param>
		public override void Resolve(Context context, MethodCompiler methodCompiler)
		{
			base.Resolve(context, methodCompiler);

			// If we're ldind.i8, fix an IL deficiency that the result may be U8
			if (opcode == OpCode.Ldind_i8 && elementType.Value == MosaTypeCode.I8)
			{
				if (context.Operand1.Type.ElementType?.IsU8 == true)
				{
					context.Result = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.U8);
				}
			}
		}
	}
}
