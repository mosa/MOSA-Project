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
	/// Intermediate representation for the CIL stfld opcode.
	/// </summary>
	public class StfldInstruction : BinaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StfldInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public StfldInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction

		#region Methods Overrides

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
			 */

			// FIXME: Verification
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
			return String.Format("{2} ; {0} = {1}", instruction.Operand1, instruction.Operand2, base.ToString());
		}

		#endregion // Methods Overrides
	}
}
