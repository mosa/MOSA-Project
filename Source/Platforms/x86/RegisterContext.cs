using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Mosa.Platforms.x86
{
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public class RegisterContext
    {
        public uint Eax;
        public uint Ebx;
        public uint Ecx;
        public uint Edx;
        public uint Esi;
        public uint Edi;
        public uint Ebp;
        public uint Eip;
        public uint Esp;

        public RegisterContext(uint eax, uint ebx, uint ecx, uint edx, uint esi, uint edi, uint ebp, uint eip, uint esp)
        {
            Native.BochsDebug();
            Eax = eax;
            Ebx = ebx;
            Ecx = ecx;
            Edx = edx;
            Esi = esi;
            Edi = edi;
            Ebp = ebp;
            Eip = eip;
            Esp = esp;
        }
    }
}
