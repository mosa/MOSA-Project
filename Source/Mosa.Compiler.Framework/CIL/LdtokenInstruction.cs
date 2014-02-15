/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class LdtokenInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdtokenInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdtokenInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion Construction

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

			var token = decoder.DecodeTokenType();

            if (token.Table == Metadata.TableType.Field)
            {
                var field = decoder.TypeSystem.Resolver.GetFieldByToken(decoder.Method.CodeAssembly, token, decoder.Method.DeclaringType.GenericArguments);

                ctx.Operand1 = decoder.Compiler.CreateVirtualRegister(decoder.TypeSystem.BuiltIn.TypedByRef);
                ctx.Result = LoadInstruction.CreateResultOperand(decoder, field.Type);
            }
            else
            {
			    //TODO
			    throw new NotImplementedException();
            }
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldtoken(context);
		}

		#endregion Methods
	}
}