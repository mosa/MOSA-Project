/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using Mosa.Kernel.Memory;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// Implements a mock memory page manager.
    /// </summary>
    /// <remarks>
    /// The mock does not perform any allocation or freeing of memory.
    /// </remarks>
    sealed class MockMemoryPageManager : IMemoryPageManager
    {
        #region IMemoryPageManager Members

        public IntPtr Allocate(IntPtr address, ulong size, PageProtectionFlags protectionFlags)
        {
            return IntPtr.Zero;
        }

        public void Free(IntPtr address, ulong size)
        {
        }

        public PageProtectionFlags Protect(IntPtr address, ulong size, PageProtectionFlags protectionFlags)
        {
            return PageProtectionFlags.NoAccess;
        }

        public ulong PageSize
        {
            get { return 4 * 1024; }
        }

        public ulong TotalMemory
        {
            get { return 1024 * 1024 * 1024; }
        }

        public ulong TotalMemoryInUse
        {
            get { return 0; }
        }

        #endregion
    }
}
