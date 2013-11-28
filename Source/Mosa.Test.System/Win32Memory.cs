﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */

using System;
using System.Runtime.InteropServices;

namespace Mosa.Test.System
{
	internal static class Win32Memory
	{
		internal static long Allocate(long address, uint size, PageProtectionFlags accessMode)
		{
			IntPtr memory = VirtualAlloc(new IntPtr(address), size, VirtualAllocTypes.MEM_COMMIT | VirtualAllocTypes.MEM_RESERVE, AccessProtectionFlags.PAGE_EXECUTE_READWRITE);

			return memory.ToInt64();
		}

		internal static void Free(long address, uint size)
		{
			VirtualFree(new IntPtr(address), size, VirtualAllocTypes.MEM_RELEASE);
		}

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

		[DllImport("kernel32.dll", SetLastError = true, PreserveSig = true)]
		private static extern unsafe bool VirtualFree(IntPtr lpAddress, uint dwSize, VirtualAllocTypes dwFreeType);

		[DllImport("kernel32.dll", SetLastError = true, PreserveSig = true)]
		private static unsafe extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, VirtualAllocTypes flAllocationType, AccessProtectionFlags flProtect);

		#endregion Win32 P/Invoke
	}
}