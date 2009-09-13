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

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// Intermediate representation of an instruction, which takes two stack arguments.
	/// </summary>
	public class BinaryInstruction : CILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public BinaryInstruction(OpCode opcode)
			: base(opcode,2)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		/// <param name="resultCount">The result count.</param>
		public BinaryInstruction(OpCode opcode, int resultCount)
			: base(opcode, 2, resultCount)
		{
		}

		#endregion // Construction

	}
}
