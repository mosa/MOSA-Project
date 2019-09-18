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

		public string Name => (string)Intrinsic.GetObjectFromAddress(Intrinsic.LoadPointer(Ptr));

		public CustomAttributeTable CustomAttributes => new CustomAttributeTable(Intrinsic.LoadPointer(Ptr, Pointer.Size));

		public TypeCode TypeCode => (TypeCode)(Intrinsic.Load32(Ptr, Pointer.Size * 2) >> 24);

		public TypeAttributes Attributes => (TypeAttributes)(Intrinsic.Load32(Ptr, Pointer.Size * 2) & 0x00FFFFFF);

		public uint Size => Intrinsic.Load32(Ptr, Pointer.Size * 3);

		public AssemblyDefinition Assembly => new AssemblyDefinition(Intrinsic.LoadPointer(Ptr, Pointer.Size * 4));

		public TypeDefinition ParentType => new TypeDefinition(Intrinsic.LoadPointer(Ptr, Pointer.Size * 5));

		public TypeDefinition DeclaringType => new TypeDefinition(Intrinsic.LoadPointer(Ptr, Pointer.Size * 6));

		public TypeDefinition ElementType => new TypeDefinition(Intrinsic.LoadPointer(Ptr, Pointer.Size * 7));

		public MethodDefinition DefaultConstructor => new MethodDefinition(Intrinsic.LoadPointer(Ptr, Pointer.Size * 8));

		public Pointer Properties => Intrinsic.LoadPointer(Ptr, Pointer.Size * 9);

		public Pointer Fields => Intrinsic.LoadPointer(Ptr, Pointer.Size * 10);

		public Pointer SlotTable => Intrinsic.LoadPointer(Ptr, Pointer.Size * 11);

		public Pointer Bitmap => Intrinsic.LoadPointer(Ptr, Pointer.Size * 12);

		public uint NumberOfMethods => Intrinsic.Load32(Ptr, Pointer.Size * 13);

		public MethodDefinition GetMethodDefinition(uint slot)
		{
			return new MethodDefinition(Intrinsic.LoadPointer(Ptr, (Pointer.Size * 14) + (Pointer.Size * (int)slot)));
		}

		//public static bool operator ==(TypeDefinition a, TypeDefinition b)
		//{
		//	return a.Ptr == b.Ptr;
		//}

		//public static bool operator !=(TypeDefinition a, TypeDefinition b)
		//{
		//	return a.Ptr != b.Ptr;
		//}

		//public override bool Equals(object obj)
		//{
		//	if (obj == null)
		//	{
		//		return false;
		//	}

		//	if (!(obj is TypeDefinition))
		//		return false;

		//	return ((TypeDefinition)obj).Ptr == this.Ptr;
		//}

		//public override int GetHashCode()
		//{
		//	return Ptr.GetHashCode();
		//}
	}
}
