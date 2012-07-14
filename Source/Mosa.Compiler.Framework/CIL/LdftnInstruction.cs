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
	public sealed class LdftnInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdftnInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdftnInstruction(OpCode opcode)
			: base(opcode, 0)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			// Read the fn token
			Token token = decoder.DecodeTokenType();
			ctx.Result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.IntPtr);
			ctx.InvokeTarget = decoder.TypeModule.GetMethod(token);
			
			decoder.Compiler.Scheduler.TrackMethodInvoked(ctx.InvokeTarget);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldftn(context);
		}

		#endregion Methods

	}
}
