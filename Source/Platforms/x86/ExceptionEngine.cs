using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Platforms.x86
{
    public static class ExceptionEngine
    {
        public static bool compiled = false;
        public static void ThrowException(uint eax, uint ecx, uint edx, uint ebx, uint esi, uint edi, uint ebp, Exception exception, uint eip, uint esp)
        {
            Mosa.Platforms.x86.RegisterContext registerContext = new RegisterContext ();
            registerContext.Esp = esp + 8;
            registerContext.Eip = eip;
            registerContext.Ebp = ebp;
            registerContext.Edi = edi;
            registerContext.Esi = esi;
            registerContext.Ebx = ebx;
            registerContext.Edx = edx;
            registerContext.Ecx = ecx;
            registerContext.Eax = eax;

            registerContext.Eip -= 1;

            RestoreContext(registerContext);
        }

        public static void RestoreContext(RegisterContext context)
        {
            Native.RestoreContext();
        }
    }
}
