/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class LdargaInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdargaInstruction"/> class.
		/// </summary>
		public LdargaInstruction(OpCode opCode)
			: base(opCode)
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

			ushort argIdx;

			// Opcode specific handling 
			if (opcode == OpCode.Ldarga_s)
			{
				argIdx = decoder.DecodeByte();
			}
			else
			{
				argIdx = decoder.DecodeUShort();
			}

			Operand parameterOperand = decoder.Compiler.GetParameterOperand(argIdx);
			ctx.Operand1 = parameterOperand;
			ctx.Result = decoder.Compiler.CreateVirtualRegister(new RefSigType(parameterOperand.Type));
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldarga(context);
		}

		#endregion Methods

	}
}
