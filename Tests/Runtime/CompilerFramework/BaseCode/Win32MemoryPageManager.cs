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
using System.Text;
using System.Runtime.InteropServices;

using Mosa.Runtime.Memory;

namespace Test.Mosa.Runtime.CompilerFramework.BaseCode
{
	/// <summary>
	/// Provides implementation of Mosa.Kernel.Memory.IMemoryPageManager based on Win32 virtual memory.
	/// </summary>
	sealed class Win32MemoryPageManager : IMemoryPageManager
	{
		#region IMemoryPageManager Members

		IntPtr IMemoryPageManager.Allocate(IntPtr address, ulong size, PageProtectionFlags accessMode)
		{
			return VirtualAlloc(address, (uint)size, VirtualAllocTypes.MEM_COMMIT | VirtualAllocTypes.MEM_RESERVE, AccessProtectionFlags.PAGE_EXECUTE_READWRITE);
		}

		void IMemoryPageManager.Free(IntPtr address, ulong size)
		{
			if (size > UInt32.MaxValue)
				throw new ArgumentOutOfRangeException(@"size", size, @"Can't exceed " + UInt32.MaxValue);

			VirtualFree(address, (uint)size, VirtualAllocTypes.MEM_RELEASE);
		}

		PageProtectionFlags IMemoryPageManager.Protect(IntPtr address, ulong size, PageProtectionFlags accessMode)
		{
			throw new NotImplementedException();
		}

		ulong IMemoryPageManager.PageSize
		{
			get { return 4096; }
		}

		ulong IMemoryPageManager.TotalMemory
		{
			get { return 1024 * 1024 * 1024; }
		}

		ulong IMemoryPageManager.TotalMemoryInUse
		{
			get { return 0; }
		}

		#endregion // IMemoryPageManager Members

		#region Win32 P/Invoke

		[Flags]
		private enum VirtualAllocTypes
		{
			MEM_COMMIT = 0x00001000,
			MEM_RESERVE = 0x00002000,
			MEM_RELEASE = 0x00008000,
			MEM_RESET = 0x00080000, // Win2K only
			MEM_TOP_DOWN = 0x00100000,
			MEM_WRITE_WATCH = 0x00200000, // Win98 only
			MEM_PHYSICAL = 0x00400000, // Win2K only
			MEM_4MB_PAGES = 0x20000000, // ??
		}

		[Flags]
		private enum AccessProtectionFlags
		{
			None = 0,
			PAGE_NOACCESS = 0x001,
			PAGE_READONLY = 0x002,
			PAGE_READWRITE = 0x004,
			PAGE_WRITECOPY = 0x008,
			PAGE_EXECUTE = 0x010,
			PAGE_EXECUTE_READ = 0x020,
			PAGE_EXECUTE_READWRITE = 0x040,
			PAGE_EXECUTE_WRITECOPY = 0x080,
			PAGE_GUARD = 0x100,
			PAGE_NOCACHE = 0x200,
			PAGE_WRITECOMBINE = 0x400
		}

		[DllImport(@"kernel32.dll", SetLastError = true, PreserveSig = true)]
		private static extern unsafe bool VirtualFree(IntPtr lpAddress, uint dwSize, VirtualAllocTypes dwFreeType);

		[DllImport("kernel32.dll", SetLastError = true, PreserveSig = true)]
		private static unsafe extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, VirtualAllocTypes flAllocationType, AccessProtectionFlags flProtect);

		#endregion // Win32 P/Invoke
	}
}
