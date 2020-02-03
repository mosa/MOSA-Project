// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;

namespace $safeprojectname$
{
    public static class Boot
    {
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
