// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class LdargaInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdargaInstruction"/> class.
		/// </summary>
		public LdargaInstruction(OpCode opCode)
			: base(opCode)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(InstructionNode ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			int argIdx;

			// Opcode specific handling
			argIdx = (int)decoder.Instruction.Operand;

			var parameterOperand = decoder.Compiler.GetParameterOperand((int)decoder.Instruction.Operand);
			ctx.Operand1 = parameterOperand;
			ctx.Result = decoder.Compiler.CreateVirtualRegister(parameterOperand.Type.ToManagedPointer());
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldarga(context);
		}

		#endregion Methods
	}
}