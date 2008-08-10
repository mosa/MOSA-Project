/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IL
{
    [Flags]
    public enum MethodDataSectionType
    {
        EHTable = 0x01,
        OptIL = 0x02,
        FatFormat = 0x40,
        MoreSections = 0x80
    }
}
