/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Compiler.Metadata;

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

			// See Partition III, 4.17 (ldtoken)
			// ldtoken can produce three kinds of token depending on the operand.
			var token = decoder.DecodeTokenType();

			const int MethodToken = 1;
			const int FieldToken = 2;
			const int TypeToken = 3;
			int tokenType = 0;

			var assembly = decoder.Method.CodeAssembly;
			switch (token.Table)
			{
				case TableType.MethodDef:
				case TableType.MethodSpec:
					tokenType = MethodToken;
					break;

				case TableType.TypeDef:
				case TableType.TypeRef:
				case TableType.TypeSpec:
					tokenType = TypeToken;
					break;

				case TableType.Field:
					tokenType = FieldToken;
					break;

				case TableType.MemberRef:
					// MemberRef can only be field or method.
					if (decoder.TypeSystem.Resolver.CheckFieldExists(assembly, token))
					{
						tokenType = FieldToken;
					}
					else
					{
						tokenType = MethodToken;
					}
					break;
			}

			// Set the result according to token type
			switch (tokenType)
			{
				case TypeToken:
					var type = decoder.TypeSystem.Resolver.GetTypeByToken(assembly, token, decoder.Method);
					ctx.MosaType = type;
					ctx.Result = decoder.Compiler.CreateVirtualRegister(decoder.TypeSystem.GetTypeByName("System", "RuntimeTypeHandle"));
					break;

				case MethodToken:
					var method = decoder.TypeSystem.Resolver.GetMethodByToken(assembly, token, decoder.Method.DeclaringType.GenericArguments);
					ctx.InvokeMethod = method;      // Since there isn't way to store as 'referenced method' rather than 'invoked method', this will do.
					ctx.Result = decoder.Compiler.CreateVirtualRegister(decoder.TypeSystem.GetTypeByName("System", "RuntimeMethodHandle"));
					break;

				case FieldToken:
					var field = decoder.TypeSystem.Resolver.GetFieldByToken(assembly, token, decoder.Method.DeclaringType.GenericArguments);
					ctx.MosaField = field;
					ctx.Result = decoder.Compiler.CreateVirtualRegister(decoder.TypeSystem.GetTypeByName("System", "RuntimeFieldHandle"));
					break;
			}
			ctx.OperandCount = 0;
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