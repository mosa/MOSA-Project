// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;

namespace Mosa.Starter.x86
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
            #region Initialization

            Kernel.Setup();
            IDT.SetInterruptHandler(ProcessInterrupt);

            #endregion

            Program.Setup();

            for (; ; )
                Program.Loop();
        }

        public static void ProcessInterrupt(uint interrupt, uint errorCode)
        {
            if (interrupt >= 0x20 && interrupt < 0x30)
                HAL.ProcessInterrupt((byte)(interrupt - 0x20));
        }
    }
}
