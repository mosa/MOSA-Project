// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Linker
{
	/// <summary>
	/// Linker Section
	/// </summary>
	public sealed class LinkerSection
	{
		public string Name { get { return SectionKind.ToString(); } }

		public SectionKind SectionKind { get; }

		public ulong VirtualAddress { get; set; }

		public uint Size { get; set; }

		public LinkerSection(SectionKind sectionKind)
		{
			SectionKind = sectionKind;
			Size = 0;
		}
	}
}
