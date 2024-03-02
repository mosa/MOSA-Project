// Copyright (c) MOSA Project. Licensed under the New BSD License.
using System;

namespace Mosa.DeviceSystem.Framework.PCI;

[Flags]
public enum PCIField : byte
{
	DeviceID = 1,
	VendorID = 2,
	SubSystemVendorID = 4,
	SubSystemID = 8,
	RevisionID = 16,
	ProgIF = 32,
	ClassCode = 64,
	SubClassCode = 128
}
