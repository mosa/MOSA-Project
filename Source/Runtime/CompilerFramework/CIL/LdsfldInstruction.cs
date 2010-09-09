/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;

using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class LdsfldInstruction : BaseInstruction
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

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		/// <param name="typeSystem">The type system.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder, ITypeSystem typeSystem)
		{
			// Decode base classes first
			base.Decode(ctx, decoder, typeSystem);

			TokenTypes token = decoder.DecodeTokenType();
			ctx.RuntimeField = typeSystem.GetField(decoder.Method, decoder.Method.Module, token);

			SigType sigType = ctx.RuntimeField.SignatureType;

			Debug.Assert((ctx.RuntimeField.Attributes & FieldAttributes.Static) == FieldAttributes.Static, @"Static field access on non-static field.");
			ctx.Result = LoadInstruction.CreateResultOperand(decoder, Operand.StackTypeFromSigType(sigType), sigType);
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
