// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Framework;

public static class Address
{
	public const uint UnitTestStack = 0x00004000;  // 4KB (stack grows down)
	public const uint UnitTestQueue = 0x01E00000;  // 30MB [Size=2MB] - previous: 5KB [Size=1KB] 0x00005000
	public const uint DebuggerBuffer = 0x00010000;  // 16KB [Size=64KB]
}
