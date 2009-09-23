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
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.IR2
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class Instruction
	{
		/// <summary>
		/// 
		/// </summary>
		AddressOfInstruction _addressOfInstruction = new AddressOfInstruction();

		/// <summary>
		/// Gets the address of instruction.
		/// </summary>
		/// <value>The address of instruction.</value>
		AddressOfInstruction AddressOfInstruction { get { return _addressOfInstruction; } }
	}
}
