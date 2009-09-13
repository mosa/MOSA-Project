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
	/// Represents a basic jump instruction.
	/// </summary>
	/// <remarks>
	/// Other more complex method invocation instructions derive From this class, specifically the CallInstruction,
	/// the CalliInstruction and CallvirtInstruction classes. They share the features provided by the JumpInstruction.
	/// </remarks>
	public class JumpInstruction : InvokeInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="JumpInstruction"/> class.
		/// </summary>
		public JumpInstruction(OpCode opCode)
			: base(opCode)
		{
		}

		#endregion // Construction

	}
}
