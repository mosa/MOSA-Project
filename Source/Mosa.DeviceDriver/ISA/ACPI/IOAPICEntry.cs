// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct IOAPICEntry
{
	public readonly Pointer Pointer;

	public readonly uint Size = Offset.Size;

	internal static class Offset
	{
		public const int MADTEntry = 0;
		public const int ApicID = MADTEntry + 2;
		public const int Reserved = ApicID + 1;
		public const int ApicAddress = Reserved + 1;
		public const int GlobalSystemInterruptBase = ApicAddress + 4;
		public const int Size = GlobalSystemInterruptBase + 4;
	}

	public IOAPICEntry(Pointer entry) => Pointer = entry;

	public readonly MADTEntry MADTEntry { get { return new MADTEntry(Pointer + Offset.MADTEntry); } }

	public readonly byte ApicID => Pointer.Load8(Offset.ApicID);

	public readonly byte Reserved => Pointer.Load8(Offset.Reserved);

	public readonly uint ApicAddress => Pointer.Load8(Offset.ApicAddress);

	public readonly uint GlobalSystemInterruptBase => Pointer.Load8(Offset.GlobalSystemInterruptBase);
}
