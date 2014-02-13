/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
    public static class RuntimeHelpers
    {
        [DllImportAttribute(@"Mosa.Compiler.Framework.Intrinsics.InternalAllocateString, Mosa.Compiler.Framework")]
        public extern static void InitializeArray(Array array, IntPtr fldHandle);

        public static void InitializeArray(Array array, RuntimeFieldHandle fldHandle)
        {
            if ((array == null) || fldHandle.Value.Equals(IntPtr.Zero))
                throw new ArgumentNullException();

            InitializeArray(array, fldHandle.Value);
        }
    }
}
