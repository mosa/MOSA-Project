using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.CompilerFramework;
using System.Diagnostics;
using Mosa.Runtime.Vm;

namespace Test.Mosa.Runtime.CompilerFramework
{
    public class TestAssemblyLinker : AssemblyLinkerStageBase
    {
        protected unsafe override void ApplyPatch(LinkType linkType, RuntimeMethod method, long methodOffset, long methodRelativeBase, long targetAddress)
        {
            long value;
            switch (linkType & LinkType.KindMask)
            {
                case LinkType.RelativeOffset:
                    value = targetAddress - (method.Address.ToInt64() + methodRelativeBase);
                    break;
                case LinkType.AbsoluteAddress:
                    value = targetAddress;
                    break;
                default:
                    throw new NotSupportedException();
            }
            long address = method.Address.ToInt64() + methodOffset;
            // Position is a raw memory address, we're just storing value there
            Debug.Assert(0 != value && value == (int)value);
            int* pAddress = (int*)address;
            *pAddress = (int)value;
        }
    }
}
