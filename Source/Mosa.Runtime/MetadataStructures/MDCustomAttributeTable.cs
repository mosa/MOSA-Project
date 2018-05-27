// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	// Protected Region Table
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MDCustomAttributeTable
	{
		private int _numberOfAttributes;

		public int NumberOfAttributes => _numberOfAttributes;

		public MDCustomAttribute* GetCustomAttribute(uint slot)
		{
			fixed (MDCustomAttributeTable* _this = &this)
			{
				return (MDCustomAttribute*)Intrinsic.LoadPointer(new IntPtr(_this) + sizeof(MDCustomAttributeTable) + (IntPtr.Size * (int)slot));
			}
		}
	}
}
