using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.ObjectFiles.Elf32
{
    /// <summary>
    /// 
    /// </summary>
    public enum Elf32IdentIndex
    {
        /// <summary>
        /// 
        /// </summary>
        Mag0        = 0x00,
        /// <summary>
        /// 
        /// </summary>
        Mag1        = 0x01,
        /// <summary>
        /// 
        /// </summary>
        Mag2        = 0x02,
        /// <summary>
        /// 
        /// </summary>
        Mag3        = 0x03,
        /// <summary>
        /// 
        /// </summary>
        Class       = 0x04,
        /// <summary>
        /// 
        /// </summary>
        Data        = 0x05,
        /// <summary>
        /// 
        /// </summary>
        Version     = 0x06,
        /// <summary>
        /// 
        /// </summary>
        Pad         = 0x07,
        /// <summary>
        /// 
        /// </summary>
        nIdent      = 0x10,
    }
}
