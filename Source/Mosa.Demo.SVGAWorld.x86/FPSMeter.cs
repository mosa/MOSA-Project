// Copyright (c) MOSA Project. Licensed under the New BSD License.

using RTC = Mosa.Kernel.x86.RTC;

namespace Mosa.Demo.SVGAWorld.x86
{
	public static class FPSMeter
	{
		public static int FPS = 0;

		private static int LastS = -1;
		private static int Ticken = 0;

		public static void Update()
		{
			if (LastS == -1)
				LastS = RTC.Second;

			if (RTC.Second - LastS != 0)
			{
				if (RTC.Second > LastS)
					FPS = Ticken / (RTC.Second - LastS);

				LastS = RTC.Second;
				Ticken = 0;
			}

			Ticken++;
		}
	}
}
