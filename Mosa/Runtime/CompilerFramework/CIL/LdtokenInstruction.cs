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

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public class LdtokenInstruction : LoadInstruction
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
		
		#endregion // Construction

		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(ref InstructionData instruction, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ref instruction, decoder);
			
			TokenTypes token;
			 // Retrieve the token From the code stream
			decoder.Decode(out token);

			instruction.Token = token;

            throw new NotImplementedException();
			/*
						TypeReference typeRef;

						// Determine the result type...
						switch (TokenTypes.TableMask & token)
						{
							case TokenTypes.TypeDef:
								n = @"RuntimeTypeHandle";
								break;

							case TokenTypes.TypeRef:
								n = @"RuntimeTypeHandle";
								break;

							case TokenTypes.TypeSpec:
								n = @"RuntimeTypeHandle";
								break;

							case TokenTypes.MethodDef:
								n = @"RuntimeMethodHandle";
								break;

							case TokenTypes.MemberRef:
								// Field or Method
								{
									MemberReference memberRef = MetadataMemberReference.FromToken(decoder.Metadata, _token);
									MemberDefinition memberDef = memberRef.Resolve();
									if (memberDef is MethodDefinition)
									{
										n = @"RuntimeMethodHandle";
									}
									else if (memberDef is FieldDefinition)
									{
										n = @"RuntimeFieldHandle";
									}
									else
									{
										Debug.Assert(false, @"Failed to determine member reference type in ldtoken.");
										throw new InvalidOperationException();
									}
								}
								break;

							case TokenTypes.MethodSpec:
								n = @"RuntimeMethodHandle";
								break;

							case TokenTypes.Field:
								n = @"RuntimeFieldHandle";
								break;

							default:
								throw new NotImplementedException();
						}

						typeRef = MetadataTypeReference.FromName(decoder.Metadata, @"System", n);
						if (null == typeRef)
							typeRef = MetadataTypeDefinition.FromName(decoder.Metadata, @"System", n);

						// Set the result
						Debug.Assert(null != typeRef, @"ldtoken: Failed to retrieve type reference.");
						_results[0] = CreateResultOperand(typeRef);
			 */
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(ref InstructionData instruction)
		{
			return String.Format("{0} = ldtoken({1})", instruction.Result, instruction.Token);
		}

		#endregion Methods
	}
}
