// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

		public string Name => (string)Intrinsic.GetObjectFromAddress(Ptr.LoadPointer());

		public uint StackSize => Ptr.Load32(Pointer.Size * 3);

		public Pointer Method => Ptr.LoadPointer(Pointer.Size * 4);

		public ProtectedRegionTable ProtectedRegionTable => new ProtectedRegionTable(Ptr.LoadPointer(Pointer.Size * 6));
	}
}
