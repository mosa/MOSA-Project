/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Vm
{
    [InternalCallType]
    sealed class RuntimeSupport
    {
        /// <summary>
        /// Allocates the type.
        /// </summary>
        /// <returns></returns>
        [VmCallAttribute(VmCall.Allocate)]
        public static object AllocateType(RuntimeType type, int additionalSize)
        {
            return null;
        }
    }
}
