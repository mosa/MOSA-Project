/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using Mosa.Runtime.Memory;

namespace Test.Mosa.Runtime.CompilerFramework.BaseCode
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
		/// <param name="pageManager"></param>
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
		/// Gets the memory base pointer.
		/// </summary>
		/// <value>The memory base pointer.</value>
		public IntPtr Base
		{
			get { return this._memory; }
		}

		#endregion // Properties
	}
}
