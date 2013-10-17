/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Linker
{
	/// <summary>
	/// Specifies the type of linker.
	/// </summary>
	public enum LinkerType
	{
		PE,
		Elf32,
		Elf64,
		None
	}
}