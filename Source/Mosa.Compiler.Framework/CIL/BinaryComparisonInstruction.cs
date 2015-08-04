// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class BinaryComparisonInstruction : BinaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryComparisonInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public BinaryComparisonInstruction(OpCode opcode)
			: base(opcode, 1)
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

			// Set the result
			ctx.Result = decoder.Compiler.CreateVirtualRegister(decoder.TypeSystem.BuiltIn.I4);
		}

		/// <summary>
		/// Gets the instruction modifier.
		/// </summary>
		/// <param name="node">The context.</param>
		/// <returns></returns>
		protected override string GetModifier(InstructionNode node)
		{
			switch (((node.Instruction) as CIL.BaseCILInstruction).OpCode)
			{
				case OpCode.Ceq: return @"==";
				case OpCode.Cgt: return @">";
				case OpCode.Cgt_un: return @"> unordered";
				case OpCode.Clt: return @"<";
				case OpCode.Clt_un: return @"< unordered";
				default: throw new InvalidOperationException(@"Invalid opcode.");
			}
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.BinaryComparison(context);
		}

		#endregion Methods
	}
}
