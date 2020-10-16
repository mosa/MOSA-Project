// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Linker.Elf
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
