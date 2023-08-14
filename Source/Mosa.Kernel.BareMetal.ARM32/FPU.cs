// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.ARM32;

namespace Mosa.Kernel.BareMetal.ARM32;

/// <summary>
/// GDT
/// </summary>
public static class FPU
{
	public static void Setup()
	{
		Native.Nop();
	}
}
