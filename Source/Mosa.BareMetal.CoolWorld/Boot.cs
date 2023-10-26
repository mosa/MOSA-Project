// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal;
using Mosa.Runtime.Plug;

namespace Mosa.BareMetal.CoolWorld;

internal class Boot
{
	[Plug("Mosa.Runtime.StartUp::BootOptions")]
	public static void SetBootOptions()
	{
		BootSettings.EnableDebugOutput = true;
	}
}
