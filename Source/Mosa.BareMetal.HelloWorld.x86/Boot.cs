// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal;
using Mosa.Kernel.BareMetal.x86;
using Mosa.Runtime.Plug;

namespace Mosa.BareMetal.HelloWorld.x86;

/// <summary>
/// Boot
/// </summary>
public static class Boot
{
	/// <summary>
	/// Main
	/// </summary>
	public static void Main()
	{
		VGAText.SetColor(VGAColor.Brown);
		VGAText.Write((byte)'+');

		while (true)
		{ }
	}

	//[Plug("Mosa.Runtime.StartUp::BootOptions")]
	public static void SetBootOptions()
	{
		BootOptions.EnableDebugOutput = true;
	}
}
