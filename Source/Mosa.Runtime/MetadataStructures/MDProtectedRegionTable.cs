// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
				Ptr pThis = _this;
				return (MDProtectedRegionDefinition*)(pThis + sizeof(MDProtectedRegionTable) + (Ptr.Size * slot))[0];
			}
		}
	}
}
