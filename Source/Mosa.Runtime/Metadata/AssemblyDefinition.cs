// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Runtime.Metadata
{
	public struct AssemblyDefinition
	{
		#region layout

		// Pointer _name;
		// Pointer _customAttributes;
		// uint _attributes;
		// uint _numberOfTypes;

		#endregion layout

		public Pointer Ptr;

		public AssemblyDefinition(Pointer ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr.IsNull;

		public string Name => (string)Intrinsic.GetObjectFromAddress(Intrinsic.LoadPointer(Ptr));

		public CustomAttributeTable CustomAttributes => new CustomAttributeTable(Intrinsic.LoadPointer(Ptr, Pointer.Size));

		public uint Attributes => Intrinsic.Load32(Ptr, Pointer.Size * 2);

		public uint NumberOfTypes => Intrinsic.Load32(Ptr, Pointer.Size * 3);

		public TypeDefinition GetTypeDefinition(uint slot)
		{
			return new TypeDefinition(Intrinsic.LoadPointer(Ptr, (Pointer.Size * 4) + (Pointer.Size * (int)slot)));
		}
	}
}
