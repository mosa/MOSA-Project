// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class UnboxAnyInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="UnboxAnyInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public UnboxAnyInstruction(OpCode opcode)
			: base(opcode, 1)
		{
		}

		#endregion Construction

		#region Methods

		public override void Decode(InstructionNode ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			var type = (MosaType)decoder.Instruction.Operand;

			//Operand result = decoder.Compiler.CreateVirtualRegister(type);
			//ctx.Result = result;
			ctx.Result = LoadInstruction.CreateResultOperand(decoder, type);
			ctx.MosaType = type;
		}

		#endregion Methods
	}
}
