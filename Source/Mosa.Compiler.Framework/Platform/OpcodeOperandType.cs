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
	/// This enum represents the type of operand type
	/// </summary>
	[Flags]
	public enum OpcodeOperandType 
	{
		Register,
		Memory,
		Immediate
	}
}
