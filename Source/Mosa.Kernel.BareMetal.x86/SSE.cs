// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.x86;

namespace Mosa.Kernel.BareMetal.x86
{
	/// <summary>
	/// GDT
	/// </summary>
	public static class SSE
	{
		public static void Setup()
		{
			Native.SetCR0(Native.GetCR0() & 0xFFFB | 0x2);
			Native.SetCR4(Native.GetCR4() | 0x600);
		}
	}
}
