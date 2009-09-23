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
	public sealed class LdsfldInstruction : BaseInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdsfldInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdsfldInstruction(OpCode opcode)
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

			// Read the _stackFrameIndex From the code
			TokenTypes token;
			decoder.Decode(out token);
			instruction.RuntimeField = RuntimeBase.Instance.TypeLoader.GetField(decoder.Compiler.Assembly, token);

			Debug.Assert((instruction.RuntimeField.Attributes & FieldAttributes.Static) == FieldAttributes.Static, @"Static field access on non-static field.");
			instruction.Result = decoder.Compiler.CreateTemporary(instruction.RuntimeField.Type);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor vistor, Context context)
		{
			vistor.Ldsfld(context);
		}

		#endregion Methods

	}
}
