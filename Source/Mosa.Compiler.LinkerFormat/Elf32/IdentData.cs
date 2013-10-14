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
	/// Specifies the data encoding of the
	/// processor-specific data in the object file.
	/// </summary>
	public enum IdentData : byte
	{
		/// <summary>
		/// Invalid data encoding
		/// </summary>
		DataNone = 0x00,

		/// <summary>
		/// Encoding Data2LSB specifies 2's complement values, with the least significant byte
		/// occupying the lowest virtualAddress.
		/// </summary>
		Data2LSB = 0x01,

		/// <summary>
		/// Encoding Data2MSB specifies 2's complement values, with the most significant byte
		/// occupying the lowest virtualAddress.
		/// </summary>
		Data2MSB = 0x02,
	}
}