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

		public IntPtr Ptr;

		public CustomAttributeTable(IntPtr ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr == IntPtr.Zero;

		public uint NumberOfAttributes => Intrinsic.Load32(Ptr);

		public CustomAttribute GetCustomAttribute(uint slot)
		{
			return new CustomAttribute(Intrinsic.LoadPointer(Ptr, IntPtr.Size + (IntPtr.Size * (int)slot)));
		}
	}
}
