/*
 * (c) 20010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExceptionEngine
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eax"></param>
        /// <param name="ebx"></param>
        /// <param name="ecx"></param>
        /// <param name="edx"></param>
        /// <param name="esi"></param>
        /// <param name="edi"></param>
        /// <param name="ebp"></param>
        /// <param name="exception"></param>
        /// <param name="eip"></param>
        /// <param name="esp"></param>
        public static void ThrowException(uint eax, uint ebx, uint ecx, uint edx, uint esi, uint edi, uint ebp, Exception exception, uint eip, uint esp)
        {
            // Read callee's EIP from method header
            eip = Native.GetEip();
            // Create context
            RegisterContext registerContext = new RegisterContext(eax, ebx, ecx, edx, esi, edi, ebp, eip, esp + 8);

            // Return after exception has been handled
            RestoreContext(registerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public static void RestoreContext(RegisterContext context)
        {
            Native.RestoreContext();
        }
    }
}
