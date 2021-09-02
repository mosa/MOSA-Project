// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Runtime.x64.Boot
{
	internal static class LongMode
	{
		[DllImport("Boot/Asm/EnterLongMode.o", EntryPoint = "EnterLongMode")]
		public static extern void EnterLongMode();
	}
}
