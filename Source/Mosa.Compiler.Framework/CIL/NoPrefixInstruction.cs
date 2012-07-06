/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class NoPrefixInstruction : PrefixInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="NoPrefixInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public NoPrefixInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction

		#region Methods Overrides

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			byte nocheck = decoder.DecodeByte();

			ctx.Other = nocheck;
		}

		#endregion // Methods Overrides


	}
}
