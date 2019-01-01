// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Linker
{
	/// <summary>
	/// Linker Section
	/// </summary>
	public sealed class LinkerSection
	{
		public string Name { get { return SectionKind.ToString(); } }

		public SectionKind SectionKind { get; }

		public uint SectionAlignment { get; }

		public bool IsResolved { get; set; }

		public ulong VirtualAddress { get; set; }

		public uint FileOffset { get; set; }

		public uint Size { get; set; }

		public uint AlignedSize { get { return Alignment.AlignUp(Size, SectionAlignment); } }

		public LinkerSection(SectionKind sectionKind, uint alignment)
		{
			SectionKind = sectionKind;
			IsResolved = false;
			SectionAlignment = alignment;
			Size = 0;
		}
	}
}
