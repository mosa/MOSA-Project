// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.Metadata
{
	public struct AssemblyDefinition
	{
		#region layout

		// UIntPtr _name;
		// UIntPtr _customAttributes;
		// uint _attributes;
		// uint _numberOfTypes;

		#endregion layout

		public UIntPtr Ptr;

		public AssemblyDefinition(UIntPtr ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr == UIntPtr.Zero;

		public string Name => (string)Intrinsic.GetObjectFromAddress(Intrinsic.LoadPointer(Ptr));

		public CustomAttributeTable CustomAttributeTable => new CustomAttributeTable(Intrinsic.LoadPointer(Ptr, UIntPtr.Size));

		public uint Attributes => Intrinsic.Load32(Ptr, UIntPtr.Size * 2);

		public uint NumberOfTypes => Intrinsic.Load32(Ptr, UIntPtr.Size * 3);

		public TypeDefinition GetTypeDefinition(uint slot)
		{
			return new TypeDefinition(Intrinsic.LoadPointer(Ptr, 4 + (UIntPtr.Size * 4) + (UIntPtr.Size * (int)slot)));
		}
	}
}
