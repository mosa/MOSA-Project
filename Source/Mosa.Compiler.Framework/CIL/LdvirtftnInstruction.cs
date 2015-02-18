/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.MosaTypeSystem;
using System;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class LdvirtftnInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdvirtftnInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdvirtftnInstruction(OpCode opcode)
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

			var type = (MosaMethod)decoder.Instruction.Operand;

			//TODO
			throw new NotImplementedException();
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldvirtftn(context);
		}

		#endregion Methods
	}
}