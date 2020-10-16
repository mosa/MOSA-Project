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

		public string Name => (string)Intrinsic.GetObjectFromAddress(Ptr.LoadPointer());

		public CustomAttributeTable CustomAttributes => new CustomAttributeTable(Ptr.LoadPointer(Pointer.Size));

		public uint Attributes => Ptr.Load32(Pointer.Size * 2);

		public uint NumberOfTypes => Ptr.Load32(Pointer.Size * 3);

		public TypeDefinition GetTypeDefinition(uint slot)
		{
			return new TypeDefinition(Ptr.LoadPointer((Pointer.Size * 4) + (Pointer.Size * (int)slot)));
		}
	}
}
