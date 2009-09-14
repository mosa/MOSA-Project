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

using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public class IsInstInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="IsInstInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public IsInstInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction
		#region CILInstruction Overrides

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(ref InstructionData instruction, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ref instruction, decoder);

			throw new NotImplementedException();

			//TypeReference targetType = MetadataTypeReference.FromToken(decoder.Metadata, token);
			//instruction.Result = CreateResultOperand(targetType);
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
			return String.Format(@"{1} = {2} is {3}", base.ToString(), instruction.Result, instruction.Operand1, instruction.Result.Type);
		}

		#endregion // CILInstruction Overrides
	}
}
