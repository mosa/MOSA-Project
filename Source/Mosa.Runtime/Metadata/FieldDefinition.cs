// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

		public string Name => (string)Intrinsic.GetObjectFromAddress(Ptr.LoadPointer());

		public CustomAttributeTable CustomAttributes => new CustomAttributeTable(Ptr.LoadPointer(Pointer.Size));

		public uint Attributes => Ptr.Load32(Pointer.Size * 2);

		public TypeDefinition FieldType => new TypeDefinition(Ptr.LoadPointer(Pointer.Size * 3));

		public Pointer FieldData => Ptr.LoadPointer(Pointer.Size * 4);

		public uint OffsetOrSize => Ptr.Load32(Pointer.Size * 5);
	}
}
