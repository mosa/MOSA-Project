using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
    [Flags]
    public enum LinkType : byte
    {
        KindMask = 0xF0,
        RelativeOffset = 0x80,
        AbsoluteAddress = 0x40,

        SizeMask = 0x0F,
        I1 = 0x01,
        I2 = 0x02,
        I4 = 0x04,
        I8 = 0x08,
    }
}
