/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon "Kintaro" Wollwage (<mailto:kintaro@think-in-co.de>)
 */

namespace Mosa.Runtime.Linker.Elf32
{
    /// <summary>
    /// 
    /// </summary>
    public enum Elf32Version : uint
    {
        /// <summary>
        /// Invalid version
        /// </summary>
        None    = 0x00,
        /// <summary>
        /// Currrent version
        /// </summary>
        Current = 0x01,
    }
}
