/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */

using System;
using System.IO;

namespace Mosa.Test.System
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
		private long memory;

		private uint allocationSize;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new <see cref="VirtualMemoryStream"/> and allocates the requested amount of virtual memory to back it.
		/// </summary>
		/// <param name="memoryManager"></param>
		/// <param name="allocationSize">The number of bytes to allocate from virtual memory.</param>
		public unsafe VirtualMemoryStream(uint allocationSize)
		{
			this.memory = Win32Memory.Allocate(0, allocationSize, PageProtectionFlags.Read | PageProtectionFlags.Write | PageProtectionFlags.Execute);
			
			if (this.memory == 0)
				throw new OutOfMemoryException();

			base.Initialize((byte*)memory, allocationSize, allocationSize, FileAccess.Write);
			this.allocationSize = allocationSize;
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
			if (memory != 0)
			{
				Win32Memory.Free(memory, allocationSize);
				memory = 0;
			}
		}

		#endregion // Disposal

		#region Properties

		/// <summary>
		/// Gets the memory base pointer.
		/// </summary>
		/// <value>The memory base pointer.</value>
		public long Base
		{
			get { return this.memory; }
		}

		#endregion // Properties
	}
}
