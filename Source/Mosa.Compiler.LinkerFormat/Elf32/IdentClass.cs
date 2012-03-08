/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

namespace Mosa.Compiler.LinkerFormat.Elf32
{
	/// <summary>
	/// Identifies the file's class, or capacity.
	/// </summary>
	public enum IdentClass : byte
	{
		/// <summary>
		/// Invalid class
		/// </summary>
		ClassNone = 0x00,
		/// <summary>
		/// 32-bit objects
		/// </summary>
		Class32 = 0x01,
		/// <summary>
		/// 64-bit objects
		/// </summary>
		Class64 = 0x02,
	}
}
