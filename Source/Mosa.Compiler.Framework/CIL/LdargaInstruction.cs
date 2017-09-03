// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Ldarga Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.LoadInstruction" />
	public sealed class LdargaInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdargaInstruction" /> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		public LdargaInstruction(OpCode opCode)
			: base(opCode)
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
			int index = (int)decoder.Instruction.Operand;

			var parameterOperand = decoder.MethodCompiler.Parameters[index];
			node.Operand1 = parameterOperand;
			node.Result = decoder.MethodCompiler.CreateVirtualRegister(parameterOperand.Type.ToManagedPointer());
		}

		#endregion Methods
	}
}
