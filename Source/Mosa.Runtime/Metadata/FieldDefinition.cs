// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.Metadata
{
	public struct FieldDefinition
	{
		#region layout

		// UIntPtr _name;
		// UIntPtr _customAttributes;
		// uint _attributes;
		// UIntPtr _fieldType;
		// UIntPtr _fieldData;
		// uint _offsetOrSize;

		#endregion layout

		public UIntPtr Ptr;

		public FieldDefinition(UIntPtr ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr == UIntPtr.Zero;

		public string Name => (string)Intrinsic.GetObjectFromAddress(Intrinsic.LoadPointer(Ptr));

		public CustomAttributeTable CustomAttributes => new CustomAttributeTable(Intrinsic.LoadPointer(Ptr, UIntPtr.Size));

		public uint Attributes => Intrinsic.Load32(Ptr, UIntPtr.Size * 2);

		public TypeDefinition FieldType => new TypeDefinition(Intrinsic.LoadPointer(Ptr, UIntPtr.Size * 3));

		public UIntPtr FieldData => Intrinsic.LoadPointer(Ptr, UIntPtr.Size * 4);

		public uint OffsetOrSize => Intrinsic.Load32(Ptr, UIntPtr.Size * 5);
	}
}
