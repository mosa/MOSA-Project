// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System;

[Flags]
public enum ConsoleModifiers
{
	None = 0,
	Alt = 1 << 0,
	Shift = 1 << 1,
	Control = 1 << 2
}
