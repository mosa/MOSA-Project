/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Mosa.Kernel.Memory;

namespace cltester
{
    /// <summary>
    /// Provides a stream around virtual memory.
    /// </summary>
    /// <remarks>
    /// The virtual memory allocated by this stream is readable, writable and executable to be able to run 
    /// the compiled native code.
    /// </remarks>
    public sealed class VirtualMemoryStream : UnmanagedMemoryStream
    {
        #region Data members

        /// <summary>
        /// Pointer to the allocated virtual memory to be able to free it later on.
        /// </summary>
        private IntPtr _memory;

        private uint _allocationSize;

        private IMemoryPageManager _pageManager;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new <see cref="VirtualMemoryStream"/> and allocates the requested amount of virtual memory to back it.
        /// </summary>
        /// <param name="allocationSize">The number of bytes to allocate from virtual memory.</param>
        public unsafe VirtualMemoryStream(IMemoryPageManager pageManager, uint allocationSize)
        {
            _memory = pageManager.Allocate(IntPtr.Zero, allocationSize, PageProtectionFlags.Read | PageProtectionFlags.Write | PageProtectionFlags.Execute);
            if (IntPtr.Zero == _memory)
                throw new OutOfMemoryException();

            base.Initialize((byte*)_memory.ToPointer(), allocationSize, allocationSize, FileAccess.Write);
            _allocationSize = allocationSize;
            _pageManager = pageManager;
        }

        #endregion // Construction

        #region Disposal

        /// <summary>
        /// Disposes the virtual memory allocated for the stream.
        /// </summary>
        /// <param name="disposing">Determines the disposal reason.</param>
        protected unsafe override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (null != _memory)
            {
                _pageManager.Free(_memory, _allocationSize);
                _memory = IntPtr.Zero;
            }
        }

        #endregion // Disposal

        #region Properties

        /// <summary>
        /// Retrieves the stream position as a physical address.
        /// </summary>
        public override unsafe long Position
        {
            get { return (long)_memory + base.Position; }
            set { base.Position = value - (long)_memory; }
        }

        #endregion // Properties
    }
}
