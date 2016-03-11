// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class LdargInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdargInstruction"/> class.
		/// </summary>
		public LdargInstruction(OpCode opCode)
			: base(opCode, 1)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Decodes the specified CIL instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		/// <remarks>
		/// This method is used by instructions to retrieve immediate operands
		/// From the instruction stream.
		/// </remarks>
		public override void Decode(InstructionNode ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			int index;

			// Opcode specific handling
			switch (opcode)
			{
				case OpCode.Ldarg:
				case OpCode.Ldarg_s: index = (int)decoder.Instruction.Operand; break;
				case OpCode.Ldarg_0: index = 0; break;
				case OpCode.Ldarg_1: index = 1; break;
				case OpCode.Ldarg_2: index = 2; break;
				case OpCode.Ldarg_3: index = 3; break;
				default: throw new System.NotImplementedException();
			}

			// Push the loaded value onto the evaluation stack
			var parameterOperand = decoder.Compiler.GetParameterOperand(index);
			var result = LoadInstruction.CreateResultOperand(decoder, parameterOperand.Type);

			ctx.Operand1 = parameterOperand;
			ctx.Result = result;
		}

		#endregion Methods
	}
}
