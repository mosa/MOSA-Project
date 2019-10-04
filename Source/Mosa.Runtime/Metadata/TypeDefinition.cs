// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Reflection;

namespace Mosa.Runtime.Metadata
{
	public struct TypeDefinition
	{
		#region layout

		// 0: Pointer name;
		// 1: Pointer customAttributes;
		// 2: uint attributes;
		// 3: uint size;
		// 4: Pointer assembly;
		// 5: Pointer parentType;
		// 6: Pointer declaringType;
		// 7: Pointer elementType;
		// 8: Pointer defaultConstructor;
		// 9: Pointer properties;
		// 10:Pointer fields;
		// 11:Pointer slotTable;
		// 12:Pointer bitmap;
		// 13:uint numberOfMethods;

		#endregion layout

		public readonly Pointer Ptr;

		public TypeDefinition(Pointer ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr.IsNull;

		public long Handle => Ptr.ToInt64();

		public string Name => (string)Intrinsic.GetObjectFromAddress(Ptr.LoadPointer());

		public CustomAttributeTable CustomAttributes => new CustomAttributeTable(Ptr.LoadPointer(Pointer.Size));

		public TypeCode TypeCode => (TypeCode)(Ptr.Load32(Pointer.Size * 2) >> 24);

		public TypeAttributes Attributes => (TypeAttributes)(Ptr.Load32(Pointer.Size * 2) & 0x00FFFFFF);

		public uint Size => Ptr.Load32(Pointer.Size * 3);

		public AssemblyDefinition Assembly => new AssemblyDefinition(Ptr.LoadPointer(Pointer.Size * 4));

		public TypeDefinition ParentType => new TypeDefinition(Ptr.LoadPointer(Pointer.Size * 5));

		public TypeDefinition DeclaringType => new TypeDefinition(Ptr.LoadPointer(Pointer.Size * 6));

		public TypeDefinition ElementType => new TypeDefinition(Ptr.LoadPointer(Pointer.Size * 7));

		public MethodDefinition DefaultConstructor => new MethodDefinition(Ptr.LoadPointer(Pointer.Size * 8));

		public Pointer Properties => Ptr.LoadPointer(Pointer.Size * 9);

		public Pointer Fields => Ptr.LoadPointer(Pointer.Size * 10);

		public Pointer SlotTable => Ptr.LoadPointer(Pointer.Size * 11);

		public Pointer Bitmap => Ptr.LoadPointer(Pointer.Size * 12);

		public uint NumberOfMethods => Ptr.Load32(Pointer.Size * 13);

		public MethodDefinition GetMethodDefinition(uint slot)
		{
			return new MethodDefinition(Ptr.LoadPointer((Pointer.Size * 14) + (Pointer.Size * (int)slot)));
		}
	}
}
