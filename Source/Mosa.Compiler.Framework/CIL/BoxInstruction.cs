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

namespace Mosa.Compiler.Framework.CIL
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
					RuntimeType builtInType = null;
					switch (builtInSigType.Type)
					{
						case CilElementType.I1:
							{
								builtInType = decoder.TypeModule.TypeSystem.GetType("mscorlib", "System", "SByte");
								break;
							}
						case CilElementType.I2:
							{
								builtInType = decoder.TypeModule.TypeSystem.GetType("mscorlib", "System", "Int16");
								break;
							}
						case CilElementType.I4:
							{
								builtInType = decoder.TypeModule.TypeSystem.GetType("mscorlib", "System", "Int32");
								break;
							}
						case CilElementType.I8:
							{
								builtInType = decoder.TypeModule.TypeSystem.GetType("mscorlib", "System", "Int64");
								break;
							}
						case CilElementType.U1:
							{
								builtInType = decoder.TypeModule.TypeSystem.GetType("mscorlib", "System", "Byte");
								break;
							}
						case CilElementType.U2:
							{
								builtInType = decoder.TypeModule.TypeSystem.GetType("mscorlib", "System", "UInt16");
								break;
							}
						case CilElementType.U4:
							{
								builtInType = decoder.TypeModule.TypeSystem.GetType("mscorlib", "System", "UInt32");
								break;
							}
						case CilElementType.U8:
							{
								builtInType = decoder.TypeModule.TypeSystem.GetType("mscorlib", "System", "UInt64");
								break;
							}
						case CilElementType.Boolean:
							{
								builtInType = decoder.TypeModule.TypeSystem.GetType("mscorlib", "System", "Boolean");
								break;
							}
						case CilElementType.String:
							{
								builtInType = decoder.TypeModule.TypeSystem.GetType("mscorlib", "System", "String");
								break;
							}
						default:
							break;
					}
					var valueTypeSignature = new ValueTypeSigType(builtInType.Token);
					ctx.Result = decoder.Compiler.CreateVirtualRegister(valueTypeSignature);
					ctx.RuntimeType = builtInType;
					return;
				}

				ctx.Result = decoder.Compiler.CreateVirtualRegister(signatureType);
			}
			else if (type.ContainsOpenGenericParameters)
			{
				var signatureType = decoder.GenericTypePatcher.PatchSignatureType(decoder.TypeModule, decoder.Method.DeclaringType, token);
				ctx.Result = decoder.Compiler.CreateVirtualRegister(signatureType);
			}
			else
			{
				ctx.Result = decoder.Compiler.CreateVirtualRegister(decoder.GenericTypePatcher.PatchSignatureType(decoder.TypeModule, decoder.Method.DeclaringType, token));
			}
			ctx.RuntimeType = decoder.TypeModule.GetType(token);
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
