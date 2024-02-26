// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.DeviceSystem;

[Flags]
public enum PlatformArchitecture
{
	None = 0,
	X86,
	X64,
	X86AndX64 = X86 | X64,
	ARM32
}
