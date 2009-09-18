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
		/// <param name="instruction">The instruction.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(ref InstructionData instruction, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ref instruction, decoder);
			
			// Retrieve the provider token to check against
			TokenTypes token;
			decoder.Decode(out token);
			
			throw new NotImplementedException();

			//TypeReference targetType = MetadataTypeReference.FromToken(decoder.Metadata, token);
			// FIXME: TypeReference targetType = decoder.Metadata.GetRow<TypeReference>(decoder.Metadata, token);

			// Push the result
			//_results[0] = CreateResultOperand(targetType);
		}

		#endregion Methods

	}
}
