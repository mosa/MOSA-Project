// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.Metadata
{
	public struct FieldDefinition
	{
		#region layout

		// Pointer _name;
		// Pointer _customAttributes;
		// uint _attributes;
		// Pointer _fieldType;
		// Pointer _fieldData;
		// uint _offsetOrSize;

		#endregion layout

		public Pointer Ptr;

		public FieldDefinition(Pointer ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr.IsNull;

		public string Name => (string)Intrinsic.GetObjectFromAddress(Intrinsic.LoadPointer(Ptr));

		public CustomAttributeTable CustomAttributes => new CustomAttributeTable(Intrinsic.LoadPointer(Ptr, Pointer.Size));

		public uint Attributes => Intrinsic.Load32(Ptr, Pointer.Size * 2);

		public TypeDefinition FieldType => new TypeDefinition(Intrinsic.LoadPointer(Ptr, Pointer.Size * 3));

		public Pointer FieldData => Intrinsic.LoadPointer(Ptr, Pointer.Size * 4);

		public uint OffsetOrSize => Intrinsic.Load32(Ptr, Pointer.Size * 5);
	}
}
