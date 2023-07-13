// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct MADTEntry
{
	private readonly Pointer Entry;

	public readonly Pointer Pointer => Entry;

	public readonly uint Size => Offset.Size;

	internal static class Offset
	{
		public const int Type = 0;
		public const int Length = Type + 1;
		public const int Size = Length + 1;
	}

	public MADTEntry(Pointer entry) => Entry = entry;

	public readonly byte Type => Entry.Load8(Offset.Type);

	public readonly byte Length => Entry.Load8(Offset.Length);
}
