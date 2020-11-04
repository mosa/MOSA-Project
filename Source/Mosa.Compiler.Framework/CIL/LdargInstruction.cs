// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Ldarg Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.LoadInstruction" />
	public sealed class LdargInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdargInstruction" /> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		public LdargInstruction(OpCode opCode)
			: base(opCode, 1)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Decodes the specified CIL instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		/// <remarks>
		/// This method is used by instructions to retrieve immediate operands
		/// From the instruction stream.
		/// </remarks>
		public override void Decode(InstructionNode node, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(node, decoder);

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
				default: throw new CompilerException();
			}

			var parameterOperand = decoder.MethodCompiler.Parameters[index];

			node.Operand1 = parameterOperand;
			node.Result = decoder.MethodCompiler.AllocateVirtualRegisterOrStackSlot(parameterOperand.Type);
		}

		#endregion Methods
	}
}
