// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class LdlocInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdlocInstruction"/> class.
		/// </summary>
		public LdlocInstruction(OpCode opCode)
			: base(opCode, 1)
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
			var local = decoder.Compiler.GetLocalOperand(index);
			var result = LoadInstruction.CreateResultOperand(decoder, local.Type);

			ctx.Operand1 = local;
			ctx.Result = result;
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldloc(context);
		}

		#endregion Methods
	}
}
