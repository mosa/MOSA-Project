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
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public class LdstrInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdstrInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		public LdstrInstruction(OpCode opCode)
			: base(opCode)
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

			// Load the string value, it's a token
			IMetadataProvider metadata = decoder.Compiler.Assembly.Metadata;
			TokenTypes token;
			decoder.Decode(out token);
			token |= TokenTypes.UserString;

			string value;
			metadata.Read(token, out value);

			// Set the result
			instruction.Result = decoder.Compiler.CreateTemporary(new SigType(CilElementType.String));
			instruction.String = value;
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
			return String.Format("{0} = \"{1}\"", instruction.Result, instruction.String);
		}

		#endregion // CILInstruction Overrides


	}
}
