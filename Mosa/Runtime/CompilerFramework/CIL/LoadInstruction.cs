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
	public class LoadInstruction : CILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LoadInstruction"/> class.
		/// </summary>
		public LoadInstruction(OpCode opCode)
			: base(opCode, 0, 1)
		{
		}

		#endregion // Construction

	}
}
