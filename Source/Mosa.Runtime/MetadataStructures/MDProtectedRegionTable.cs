// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	// Protected Region Table
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MDProtectedRegionTable
	{
		private uint _numberOfRegions;

		public uint NumberOfRegions => _numberOfRegions;

		public MDProtectedRegionDefinition* GetProtectedRegionDefinition(uint slot)
		{
			fixed (MDProtectedRegionTable* _this = &this)
			{
				return (MDProtectedRegionDefinition*)Intrinsic.LoadPointer(new IntPtr(_this) + sizeof(MDProtectedRegionTable) + (IntPtr.Size * (int)slot));
			}
		}
	}
}
