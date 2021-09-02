// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Runtime.Boot
{
	internal static class LongMode
	{
		[DllImport("Asm/EnterLongMode.o", EntryPoint = "EnterLongMode")]
		public static extern void EnterLongMode();
	}
}
