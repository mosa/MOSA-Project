// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
				Ptr pThis = _this;
				return (MDCustomAttribute*)(pThis + sizeof(MDCustomAttributeTable) + (Ptr.Size * slot)).Dereference(0);
			}
		}
	}
}
