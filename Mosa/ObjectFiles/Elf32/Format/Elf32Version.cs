/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.ObjectFiles.Elf32.Format
{
    /// <summary>
    /// This enum identifies the object file version. 
    /// 
    /// |----------------------------------------------|
    /// |      Name     | Value |        Meaning       |
    /// |----------------------------------------------|
    /// | EV_NONE       |   0   |  Invalid version     |
    /// | EV_CURRENT    |   1   |  Current version     |
    /// |----------------------------------------------|
    /// 
    /// The value 1 signifies the original file format; extensions will create new versions 
    /// with higher numbers. The value of EV_CURRENT, though given as 1 above, will 
    /// change as necessary to reflect the current version number. 
    /// 
    /// Refer to the specification in the TIS (Tool Interface Standard) 
    /// ELF (Executable and Linking Format) Specification, 1-4, page 19, "e_version"
    /// </summary>
    enum Elf32Version
    {
        /// <summary>
        /// Invalid version
        /// </summary>
        None = 0,
        /// <summary>
        /// Current version
        /// </summary>
        Current = 1,
    }
}
