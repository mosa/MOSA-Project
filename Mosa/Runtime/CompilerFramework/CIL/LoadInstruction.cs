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
	public class LoadInstruction : BaseInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LoadInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		public LoadInstruction(OpCode opCode)
			: base(opCode, 0, 1)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LoadInstruction"/> class.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="operandCount">The number of operands of the load.</param>
		protected LoadInstruction(OpCode code, byte operandCount)
			: base(code, operandCount, 1)
		{
		}

		#endregion // Construction

	}
}
