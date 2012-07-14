/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class LdsfldaInstruction : BaseCILInstruction
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LdsfldaInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdsfldaInstruction(OpCode opcode)
			: base(opcode, 0, 1)
		{
		}

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			// Read the _stackFrameIndex From the code
			Token token = decoder.DecodeTokenType();
			ctx.RuntimeField = decoder.TypeModule.GetField(token);

			if (ctx.RuntimeField.ContainsGenericParameter)
			{
				//TODO
			}

			ctx.Result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.Ptr);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldsflda(context);
		}
	}
}
