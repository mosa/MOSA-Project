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
using Mosa.Compiler.TypeSystem;
using Mosa.Compiler.TypeSystem.Generic;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class ConstrainedPrefixInstruction : PrefixInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ConstrainedPrefixInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public ConstrainedPrefixInstruction(OpCode opcode)
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
			Token token = decoder.DecodeTokenType();
			ctx.RuntimeType = decoder.TypeModule.GetType(token);

			if (ctx.RuntimeType == null)
			{
				var signature = decoder.GenericTypePatcher.PatchSignatureType(decoder.TypeModule, decoder.Method.DeclaringType, token);
				if (signature is BuiltInSigType)
				{
					var builtInSigType = signature as BuiltInSigType;
					switch (builtInSigType.Type)
					{
						case CilElementType.I4:
							{
								var int32type = decoder.TypeModule.TypeSystem.GetType("mscorlib", "System", "Int32");
								ctx.RuntimeType = int32type;
								return;
							}
						case CilElementType.String:
							{
								var stringType = decoder.TypeModule.TypeSystem.GetType("mscorlib", "System", "String");
								ctx.RuntimeType = stringType;
								return;
							}
						default:
							break;
					}
				}
				else if (signature is ClassSigType)
				{
					var instantiationModule = (decoder.Method.DeclaringType as CilGenericType).InstantiationModule;
					var classSigType = signature as ClassSigType;
					ctx.RuntimeType = instantiationModule.GetType(classSigType.Token);
				}
			}

		}

		public override string ToString(Context context)
		{
			string s = base.ToString(context);

			RuntimeType type = context.Other as RuntimeType;

			if (type != null)
				s = s + " {" + type.ToString() + "}";

			return s;
		}

		#endregion Methods

	}
}
