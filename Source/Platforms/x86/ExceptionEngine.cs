using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Platforms.x86
{
    public static class ExceptionEngine
    {
        public static void ThrowException(uint eax, uint ebx, uint ecx, uint edx, uint esi, uint edi, uint ebp, Exception exception, uint eip, uint esp)
        {
            eip = Native.GetEip(Native.Pop()); // Fixme: This value has to be recomputed each time the method is changed!
            Native.Push(0x00);
            RegisterContext registerContext = new RegisterContext(eax, ebx, ecx, edx, esi, edi, ebp, eip, esp + 8);
            //registerContext.Eip -= 1;
            RestoreContext(registerContext);
        }

        public static void RestoreContext(RegisterContext context)
        {
            Native.RestoreContext();
        }
    }
}
