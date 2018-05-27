// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.Metadata
{
	public struct FieldDefinition
	{
		#region layout

		// IntPtr _name;
		// IntPtr _customAttributes;
		// uint _attributes;
		// IntPtr _fieldType;
		// IntPtr _fieldData;
		// uint _offsetOrSize;

		#endregion layout

		public IntPtr Ptr;

		public FieldDefinition(IntPtr ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr == IntPtr.Zero;

		public string Name => (string)Intrinsic.GetObjectFromAddress(Intrinsic.LoadPointer(Ptr));

		public CustomAttributeTable CustomAttributes => new CustomAttributeTable(Intrinsic.LoadPointer(Ptr, IntPtr.Size));

		public uint Attributes => Intrinsic.Load32(Ptr, IntPtr.Size * 2);

		public TypeDefinition FieldType => new TypeDefinition(Intrinsic.LoadPointer(Ptr, IntPtr.Size * 3));

		public IntPtr FieldData => Intrinsic.LoadPointer(Ptr, IntPtr.Size * 4);

		public uint OffsetOrSize => Intrinsic.Load32(Ptr, IntPtr.Size * 5);
	}
}
