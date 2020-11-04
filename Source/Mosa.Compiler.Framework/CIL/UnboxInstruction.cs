// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Unbox Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.UnaryInstruction" />
	public sealed class UnboxInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="UnboxInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public UnboxInstruction(OpCode opcode)
			: base(opcode, 1)
		{
		}

		#endregion Construction

		#region Methods

		public override void Decode(InstructionNode node, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(node, decoder);

			var type = (MosaType)decoder.Instruction.Operand;

			//Operand result = decoder.Compiler.CreateVirtualRegister(type);
			//ctx.Result = result;
			node.Result = decoder.MethodCompiler.AllocateVirtualRegisterOrStackSlot(type);
			node.MosaType = type;
		}

		#endregion Methods
	}
}
