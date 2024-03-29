﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ACPI;

/// <summary>
/// A generic entry in the ACPI MADT, which is the base of all other entries (like the <see cref="IOAPICEntry"/> for example).
/// </summary>
public readonly struct MADTEntry
{
	public readonly Pointer Pointer;

	public const uint Size = Offset.Size;

	internal static class Offset
	{
		public const int Type = 0;
		public const int Length = Type + 1;
		public const int Size = Length + 1;
	}

	public MADTEntry(Pointer entry) => Pointer = entry;

	public byte Type => Pointer.Load8(Offset.Type);

	public byte Length => Pointer.Load8(Offset.Length);
}
