/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class CastclassInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CastclassInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public CastclassInstruction(OpCode opcode)
			: base(opcode)
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

			Token token = decoder.DecodeTokenType();

			RuntimeType type = decoder.TypeModule.GetType(token);

			ctx.Result = decoder.Compiler.CreateTemporary(new ClassSigType(token));
		}

		#endregion Methods

	}
}
