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
	public class MkrefanyInstruction : CILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MkrefanyInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public MkrefanyInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction

	}
}
