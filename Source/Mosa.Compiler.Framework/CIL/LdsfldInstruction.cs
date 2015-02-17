/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class LdsfldInstruction : BaseCILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdsfldInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdsfldInstruction(OpCode opcode)
			: base(opcode, 0, 1)
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

			var field = (MosaField)decoder.Instruction.Operand;

			decoder.Compiler.Scheduler.TrackFieldReferenced(field);

			Debug.Assert(field.IsStatic, "Static field access on non-static field.");

			ctx.MosaField = field;
			ctx.Result = LoadInstruction.CreateResultOperand(decoder, field.FieldType);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldsfld(context);
		}

		#endregion Methods
	}
}