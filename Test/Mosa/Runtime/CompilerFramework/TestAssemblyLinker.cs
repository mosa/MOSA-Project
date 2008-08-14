using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.CompilerFramework;
using System.Diagnostics;

namespace Test.Mosa.Runtime.CompilerFramework
{
    public class TestAssemblyLinker : AssemblyLinkerStageBase
    {
        protected unsafe override void ApplyPatch(long position, long value)
        {
            // Position is a raw memory address, we're just storing value there
            Debug.Assert(0 != value && value == (value & 0xFFFFFFFF));
            int* pAddress = (int*)position;
            *pAddress = (int)value;
        }
    }
}
