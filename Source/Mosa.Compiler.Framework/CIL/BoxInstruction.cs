// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Box Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.UnaryInstruction" />
	public class BoxInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BoxInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public BoxInstruction(OpCode opcode)
			: base(opcode, 1)
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

			var type = (MosaType)decoder.Instruction.Operand;
			node.Result = decoder.MethodCompiler.CreateVirtualRegister(decoder.TypeSystem.BuiltIn.Object);
			node.MosaType = type;
		}

		#endregion Methods
	}
}
