/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework
{
	public enum InstructionSize : byte
	{
		None = 0,
		Size8 = 8,
		Size16 = 16,
		Size32 = 32,
		Size64 = 64,
		Native = 0xFF,
	}
}