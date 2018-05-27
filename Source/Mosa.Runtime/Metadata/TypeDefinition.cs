// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Reflection;

namespace Mosa.Runtime.Metadata
{
	public struct TypeDefinition
	{
		#region layout

		// UIntPtr _name;
		// UIntPtr _customAttributes;
		// uint _attributes;
		// uint _size;
		// UIntPtr _assembly;
		// UIntPtr _parentType;
		// UIntPtr _declaringType;
		// UIntPtr _elementType;
		// UIntPtr _defaultConstructor;
		// UIntPtr _properties;
		// UIntPtr _fields;
		// UIntPtr _slotTable;
		// UIntPtr _bitmap;
		// uint _numberOfMethods;

		#endregion layout

		public readonly UIntPtr Ptr;

		public TypeDefinition(UIntPtr ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr == UIntPtr.Zero;

		public ulong Handle => Ptr.ToUInt64();

		public string Name => (string)Intrinsic.GetObjectFromAddress(Intrinsic.LoadPointer(Ptr));

		public CustomAttributeTable CustomAttributes => new CustomAttributeTable(Intrinsic.LoadPointer(Ptr, UIntPtr.Size));

		public TypeAttributes OffsetOrSize => (TypeAttributes)(Intrinsic.Load32(Ptr, UIntPtr.Size * 2) & 0x00FFFFFF);

		public uint Size => Intrinsic.Load32(Ptr, UIntPtr.Size * 3);

		public AssemblyDefinition Assembly => new AssemblyDefinition(Intrinsic.LoadPointer(Ptr, UIntPtr.Size * 4));

		public TypeDefinition ParentType => new TypeDefinition(Intrinsic.LoadPointer(Ptr, UIntPtr.Size * 5));

		public TypeDefinition DeclaringType => new TypeDefinition(Intrinsic.LoadPointer(Ptr, UIntPtr.Size * 6));

		public TypeDefinition ElementType => new TypeDefinition(Intrinsic.LoadPointer(Ptr, UIntPtr.Size * 7));

		public CustomAttributeTable DefaultConstructor => new CustomAttributeTable(Intrinsic.LoadPointer(Ptr, UIntPtr.Size * 8));

		public UIntPtr Properties => Intrinsic.LoadPointer(Ptr, UIntPtr.Size * 9);

		public UIntPtr Fields => Intrinsic.LoadPointer(Ptr, UIntPtr.Size * 10);

		public UIntPtr SlotTable => Intrinsic.LoadPointer(Ptr, UIntPtr.Size * 11);

		public UIntPtr Bitmap => Intrinsic.LoadPointer(Ptr, UIntPtr.Size * 12);

		public uint NumberOfMethods => Intrinsic.Load32(Ptr, UIntPtr.Size * 13);

		public MethodDefinition GetMethodDefinition(uint slot)
		{
			return new MethodDefinition(Intrinsic.LoadPointer(Ptr, (UIntPtr.Size * 14) + (UIntPtr.Size * (int)slot)));
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
