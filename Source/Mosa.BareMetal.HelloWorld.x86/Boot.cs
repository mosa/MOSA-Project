// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.x86;

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
		VGAText.Write((byte)'+');

		while (true)
		{ }
	}
}
