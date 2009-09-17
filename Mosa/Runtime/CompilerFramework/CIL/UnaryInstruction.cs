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
	/// 
	/// </summary>
	public class UnaryInstruction : CILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="UnaryInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public UnaryInstruction(OpCode opcode)
			: base(opcode, 1)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UnaryInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		/// <param name="resultCount">The result count.</param>
		public UnaryInstruction(OpCode opcode, byte resultCount)
			: base(opcode, 1, resultCount)
		{
		}

		#endregion // Construction
	}
}
