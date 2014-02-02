/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace System.Runtime.CompilerServices
{
    public static class RuntimeHelpers
    {
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public extern static void InitializeArray(Array array, RuntimeFieldHandle fldHandle);
    }
}
