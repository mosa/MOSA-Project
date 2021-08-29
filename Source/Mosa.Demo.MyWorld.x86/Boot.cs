// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;

namespace Mosa.Demo.MyWorld.x86
{
	public static class Boot
	{
		[Plug("Mosa.Runtime.StartUp::SetInitialMemory")]
		public static void SetInitialMemory()
		{
			KernelMemory.SetInitialMemory(Address.GCInitialMemory, 0x01000000);
		}

		public static void Main()
		{
			Mosa.Kernel.x86.Kernel.Setup();

			IDT.SetInterruptHandler(ProcessInterrupt);

			Screen.Clear();
			Screen.Goto(0, 0);
			Screen.Color = ScreenColor.White;

			Program.Setup();

			while (true)
			{
				Program.Loop();
			}
		}

		public static void ProcessInterrupt(uint interrupt, uint errorCode)
		{
			Program.OnInterrupt();
		}
	}
}
