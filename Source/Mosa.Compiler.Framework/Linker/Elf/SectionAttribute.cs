// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework.Linker.Elf
{
	/// <summary>
	/// Describes miscellaneous attributes for a section.
	/// </summary>
	[Flags]
	public enum SectionAttribute : uint
	{
		/// <summary>
		/// The section contains data that should be writable during process execution.
		/// </summary>
		Write = 0x00000001,

		/// <summary>
		/// The section occupies memory during process execution. Some control
		/// sections do not reside in the memory image of an object file; this attribute
		/// is off for those sections.
		/// </summary>
		Alloc = 0x00000002,

		/// <summary>
		/// The section contains executable machine instructions.
		/// </summary>
		ExecuteInstructions = 0x00000004,

		/// <summary>
		/// The alloc execute
		/// </summary>
		AllocExecute = Alloc | ExecuteInstructions,

		/// <summary>
		/// All bits included in this mask are reserved for processor-specific semantics.
		/// </summary>
		ProcessorMask = 0xF0000000,
	}
}
