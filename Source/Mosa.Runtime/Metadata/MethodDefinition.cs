// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.Metadata
{
	public struct MethodDefinition
	{
		#region layout

		// UIntPtr		_name;
		// UIntPtr		_customAttributes;
		// uint			_attributes;
		// uint			_stackSize;
		// UIntPtr		_method;
		// UIntPtr		_returnType;
		// UIntPtr		_protectedRegionTable;
		// UIntPtr		_gcTrackingInformation;
		// uint			_numberOfParameters;

		#endregion layout

		public UIntPtr Ptr;

		public MethodDefinition(UIntPtr ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr == UIntPtr.Zero;

		public string Name => (string)Intrinsic.GetObjectFromAddress(Intrinsic.LoadPointer(Ptr));

		public uint StackSize => Intrinsic.Load32(Ptr, UIntPtr.Size * 3);

		public UIntPtr Method => Intrinsic.LoadPointer(Ptr, UIntPtr.Size * 4);

		public ProtectedRegionTable ProtectedRegionTable => new ProtectedRegionTable(Intrinsic.LoadPointer(Ptr, UIntPtr.Size * 6));
	}
}
