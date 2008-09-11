using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum LinkType : byte
    {
        /// <summary>
        /// 
        /// </summary>
        KindMask = 0xF0,
        /// <summary>
        /// 
        /// </summary>
        RelativeOffset = 0x80,
        /// <summary>
        /// 
        /// </summary>
        AbsoluteAddress = 0x40,

        /// <summary>
        /// 
        /// </summary>
        SizeMask = 0x0F,
        /// <summary>
        /// 
        /// </summary>
        I1 = 0x01,
        /// <summary>
        /// 
        /// </summary>
        I2 = 0x02,
        /// <summary>
        /// 
        /// </summary>
        I4 = 0x04,
        /// <summary>
        /// 
        /// </summary>
        I8 = 0x08,
    }
}
