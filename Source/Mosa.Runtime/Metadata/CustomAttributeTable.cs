// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.Metadata
{
	/// <summary>
	/// Protected Region Table
	/// </summary>
	public struct CustomAttributeTable
	{
		#region layout

		// int _numberOfAttributes;

		#endregion layout

		public Pointer Ptr;

		public CustomAttributeTable(Pointer ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr.IsNull;

		public uint NumberOfAttributes => Intrinsic.Load32(Ptr);

		public CustomAttribute GetCustomAttribute(uint slot)
		{
			return new CustomAttribute(Intrinsic.LoadPointer(Ptr, Pointer.Size + (Pointer.Size * (int)slot)));
		}
	}
}
