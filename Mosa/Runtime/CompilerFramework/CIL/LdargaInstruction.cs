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
	public class LdargaInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdargaInstruction"/> class.
		/// </summary>
		public LdargaInstruction(OpCode opCode)
			: base(opCode, 1)
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

			ushort argIdx;

			// Opcode specific handling 
			if (_opcode == OpCode.Ldarga_s) {
				byte arg;
				decoder.Decode(out arg);
				argIdx = arg;
			}
			else {
				decoder.Decode(out argIdx);
			}

			instruction.Operand1 = decoder.Compiler.GetParameterOperand(argIdx);
			instruction.Result = decoder.Compiler.CreateTemporary(new RefSigType(instruction.Operand1.Type));
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor vistor, Context context)
		{
			vistor.Ldarga(context);
		}

		#endregion Methods

	}
}
