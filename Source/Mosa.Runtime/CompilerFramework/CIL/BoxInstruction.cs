/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public class BoxInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BoxInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public BoxInstruction(OpCode opcode)
			: base(opcode, 1)
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

			// Retrieve the provider token to check against
			var token = decoder.DecodeTokenType();
			var type = decoder.TypeModule.GetType(token);

			if (type == null)
			{
				var signatureType = decoder.GenericTypePatcher.PatchSignatureType(decoder.TypeModule, decoder.Method.DeclaringType, token);

				if (signatureType is BuiltInSigType)
				{
					var builtInSigType = signatureType as BuiltInSigType;
					switch (builtInSigType.Type)
					{
						case CilElementType.I4:
							{
								var int32type = decoder.TypeModule.TypeSystem.GetType("mscorlib", "System", "Int32");
								var valueTypeSignature = new ValueTypeSigType(int32type.Token);
								ctx.Result = decoder.Compiler.CreateTemporary(valueTypeSignature);
								ctx.Other = int32type;
								return;
							}
						default:
							break;
					}
				}

				ctx.Result = decoder.Compiler.CreateTemporary(signatureType);
			}
			else if (type.ContainsOpenGenericParameters)
			{
				var signatureType = decoder.GenericTypePatcher.PatchSignatureType(decoder.TypeModule, decoder.Method.DeclaringType, token);
				ctx.Result = decoder.Compiler.CreateTemporary(signatureType);
			}
			
			ctx.Other = decoder.TypeModule.GetType(token);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Box(context);
		}

		#endregion Methods

	}
}
