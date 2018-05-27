// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.Metadata
{
	public struct MethodDefinition
	{
		#region layout

		// IntPtr		_name;
		// IntPtr		_customAttributes;
		// uint			_attributes;
		// uint			_stackSize;
		// IntPtr		_method;
		// IntPtr		_returnType;
		// IntPtr		_protectedRegionTable;
		// IntPtr		_gcTrackingInformation;
		// uint			_numberOfParameters;

		#endregion layout

		public IntPtr Ptr;

		public MethodDefinition(IntPtr ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr == IntPtr.Zero;

		public string Name => (string)Intrinsic.GetObjectFromAddress(Intrinsic.LoadPointer(Ptr));

		public uint StackSize => Intrinsic.Load32(Ptr, IntPtr.Size * 3);

		public IntPtr Method => Intrinsic.LoadPointer(Ptr, IntPtr.Size * 4);

		public ProtectedRegionTable ProtectedRegionTable => new ProtectedRegionTable(Intrinsic.LoadPointer(Ptr, IntPtr.Size * 6));
	}
}
