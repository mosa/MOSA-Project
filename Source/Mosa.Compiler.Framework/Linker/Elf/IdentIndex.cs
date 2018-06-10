// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Linker.Elf
{
	/// <summary>
	/// ELF provides an object file framework to support multiple processors,
	/// multiple data encodings, and multiple classes of machines. To support this object file family,
	/// the initial bytes of the file specify how to interpret the file, independent of the processor on
	/// which the inquiry is made and independent of the file's remaining contents.
	/// </summary>
	public enum IdentIndex
	{
		/// <summary>
		/// File identification
		/// </summary>
		Mag0 = 0x00,

		/// <summary>
		/// File identification
		/// </summary>
		Mag1 = 0x01,

		/// <summary>
		/// File identification
		/// </summary>
		Mag2 = 0x02,

		/// <summary>
		/// File identification
		/// </summary>
		Mag3 = 0x03,

		/// <summary>
		/// File class
		/// </summary>
		Class = 0x04,

		/// <summary>
		/// Data encoding
		/// </summary>
		Data = 0x05,

		/// <summary>
		/// File version
		/// </summary>
		Version = 0x06,

		/// <summary>
		/// Start of padding bytes
		/// </summary>
		Pad = 0x07,

		/// <summary>
		/// Size of Ident[]
		/// </summary>
		nIdent = 0x10,
	}
}
