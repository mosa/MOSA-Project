// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Ldloc Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.LoadInstruction" />
	public sealed class LdlocInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdlocInstruction" /> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		public LdlocInstruction(OpCode opCode)
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
			int index;
			switch (opcode)
			{
				case OpCode.Ldloc:
				case OpCode.Ldloc_s: index = (int)decoder.Instruction.Operand; break;
				case OpCode.Ldloc_0: index = 0; break;
				case OpCode.Ldloc_1: index = 1; break;
				case OpCode.Ldloc_2: index = 2; break;
				case OpCode.Ldloc_3: index = 3; break;
				default: throw new InvalidMetadataException();
			}

			// Push the loaded value onto the evaluation stack
			var local = decoder.MethodCompiler.LocalVariables[index];
			var result = decoder.MethodCompiler.AllocateVirtualRegisterOrStackSlot(local.Type);

			node.Operand1 = local;
			node.Result = result;
		}

		#endregion Methods
	}
}
