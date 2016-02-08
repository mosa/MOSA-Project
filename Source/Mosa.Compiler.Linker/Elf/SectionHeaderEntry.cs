// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Linker.Elf
{
	/// <summary>
	///
	/// </summary>
	public class Section
	{
		public string Name;

		public SectionType Type;

		public SectionAttribute Flags;

		public ulong Address;

		public long Offset;

		public ulong Size;

		public Section Link;

		public Section Info;

		public ulong AddressAlignment;

		public ulong EntrySize;
	}
}
