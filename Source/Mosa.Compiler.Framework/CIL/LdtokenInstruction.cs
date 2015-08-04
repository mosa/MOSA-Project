// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class LdtokenInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdtokenInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdtokenInstruction(OpCode opcode)
			: base(opcode)
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

			// See Partition III, 4.17 (ldtoken)

			if (decoder.Instruction.Operand is MosaType)
			{
				ctx.MosaType = (MosaType)decoder.Instruction.Operand;
				ctx.Result = decoder.Compiler.CreateVirtualRegister(decoder.TypeSystem.GetTypeByName("System", "RuntimeTypeHandle"));
			}
			else if (decoder.Instruction.Operand is MosaMethod)
			{
				ctx.InvokeMethod = (MosaMethod)decoder.Instruction.Operand;
				ctx.Result = decoder.Compiler.CreateVirtualRegister(decoder.TypeSystem.GetTypeByName("System", "RuntimeMethodHandle"));
			}
			else if (decoder.Instruction.Operand is MosaField)
			{
				ctx.MosaField = (MosaField)decoder.Instruction.Operand;
				ctx.Result = decoder.Compiler.CreateVirtualRegister(decoder.TypeSystem.GetTypeByName("System", "RuntimeFieldHandle"));
			}
			ctx.OperandCount = 0;
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldtoken(context);
		}

		#endregion Methods
	}
}
