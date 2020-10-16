// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// IsInst Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.UnaryInstruction" />
	public sealed class IsInstInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="IsInstInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public IsInstInstruction(OpCode opcode)
			: base(opcode)
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

			// result must be a reference
			var resultType = (type.IsReferenceType) ? type : type.ToManagedPointer();

			node.Result = decoder.MethodCompiler.AllocateVirtualRegisterOrStackSlot(resultType);
			node.MosaType = type;
			node.ResultCount = 1;
		}

		#endregion Methods
	}
}
