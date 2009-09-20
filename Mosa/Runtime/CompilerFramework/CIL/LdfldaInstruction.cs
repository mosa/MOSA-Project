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
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public class LdfldaInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdfldaInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdfldaInstruction(OpCode opcode)
			: base(opcode, 1)
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


			// Load the _stackFrameIndex token From the immediate
			TokenTypes token;
			decoder.Decode(out token);
			throw new NotImplementedException();
			/*
				Debug.Assert(TokenTypes.Field == (TokenTypes.TableMask & token) ||
							 TokenTypes.MemberRef == (TokenTypes.TableMask & token), @"Invalid token type.");
				MemberDefinition memberDef = MetadataMemberReference.FromToken(decoder.Metadata, token).Resolve();

				_field = memberDef as FieldDefinition;
				_results[0] = CreateResultOperand(new ReferenceTypeSpecification(_field.Type));
			 */
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(CILVisitor vistor, Context context)
		{
			vistor.Ldflda(context);
		}

		#endregion Methods
	}
}
