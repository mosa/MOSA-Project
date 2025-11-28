// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal;

namespace Mosa.BareMetal.CoolWorld.Graphical;

public static class FPSMeter
{
	public static int FPS;

	private static int LastS = -1;
	private static int Ticken;

	public static void Update(ref Time time)
	{
		if (LastS == -1)
			LastS = time.Second;

		if (time.Second - LastS != 0)
		{
			if (time.Second > LastS)
				FPS = Ticken / (time.Second - LastS);

			LastS = time.Second;
			Ticken = 0;
		}

		Ticken++;
	}
}
