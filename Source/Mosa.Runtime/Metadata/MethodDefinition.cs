// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.Metadata
{
	public struct MethodDefinition
	{
		#region layout

		// Pointer		_name;
		// Pointer		_customAttributes;
		// uint			_attributes;
		// uint			_stackSize;
		// Pointer		_method;
		// Pointer		_returnType;
		// Pointer		_protectedRegionTable;
		// Pointer		_gcTrackingInformation;
		// uint			_numberOfParameters;

		#endregion layout

		public Pointer Ptr;

		public MethodDefinition(Pointer ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr.IsNull;

		public string Name => (string)Intrinsic.GetObjectFromAddress(Intrinsic.LoadPointer(Ptr));

		public uint StackSize => Intrinsic.Load32(Ptr, Pointer.Size * 3);

		public Pointer Method => Intrinsic.LoadPointer(Ptr, Pointer.Size * 4);

		public ProtectedRegionTable ProtectedRegionTable => new ProtectedRegionTable(Intrinsic.LoadPointer(Ptr, Pointer.Size * 6));
	}
}
