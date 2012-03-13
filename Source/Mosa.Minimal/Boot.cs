/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;
using Mosa.Kernel;
using Mosa.Kernel.AVR32;

namespace Mosa.Minimal
{
    class Boot
    {
        static void Main()
        {
            Mosa.Kernel.AVR32.Kernel.Setup();
        }
    }
}
