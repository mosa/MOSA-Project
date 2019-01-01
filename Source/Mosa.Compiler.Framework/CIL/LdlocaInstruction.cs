// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Ldloca Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.LoadInstruction" />
	public sealed class LdlocaInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdlocaInstruction" /> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		public LdlocaInstruction(OpCode opCode)
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
			var local = decoder.MethodCompiler.LocalVariables[(int)decoder.Instruction.Operand];

			local = decoder.ConvertVirtualRegisterToStackLocal(local);

			node.Operand1 = local;
			node.Result = decoder.MethodCompiler.CreateVirtualRegister(local.Type.ToManagedPointer());
		}

		#endregion Methods
	}
}
