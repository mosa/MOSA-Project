namespace Mosa.Demo.VBEWorld.x86
{
	public static class FPSMeter
	{
		public static int FPS = 0;

		public static int LastS = -1;
		public static int Ticken = 0;

		public static void Update()
		{
			if (LastS == -1)
				LastS = Mosa.Kernel.x86.CMOS.Second;

			if (Mosa.Kernel.x86.CMOS.Second - LastS != 0)
			{
				if (Mosa.Kernel.x86.CMOS.Second > LastS)
					FPS = Ticken / (Mosa.Kernel.x86.CMOS.Second - LastS);

				LastS = Mosa.Kernel.x86.CMOS.Second;
				Ticken = 0;
			}

			Ticken++;
		}
	}
}
