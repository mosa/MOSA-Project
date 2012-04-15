/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.Compiler.Framework.Platform
{
	/// <summary>
	/// This enum represents the address mode of an operand 
	/// </summary>
	[Flags]
	public enum OperandAddressMode 
	{
		Register = 1,
		Memory = 2,
		Immediate = 4,
		All = Register | Memory | Immediate
	}
}
