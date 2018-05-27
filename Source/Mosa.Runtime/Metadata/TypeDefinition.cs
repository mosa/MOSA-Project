// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Reflection;

namespace Mosa.Runtime.Metadata
{
	public struct TypeDefinition
	{
		#region layout

		// IntPtr _name;
		// IntPtr _customAttributes;
		// uint _attributes;
		// uint _size;
		// IntPtr _assembly;
		// IntPtr _parentType;
		// IntPtr _declaringType;
		// IntPtr _elementType;
		// IntPtr _defaultConstructor;
		// IntPtr _properties;
		// IntPtr _fields;
		// IntPtr _slotTable;
		// IntPtr _bitmap;
		// uint _numberOfMethods;

		#endregion layout

		public readonly IntPtr Ptr;

		public TypeDefinition(IntPtr ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr == IntPtr.Zero;

		public long Handle => Ptr.ToInt64();

		public string Name => (string)Intrinsic.GetObjectFromAddress(Intrinsic.LoadPointer(Ptr));

		public CustomAttributeTable CustomAttributes => new CustomAttributeTable(Intrinsic.LoadPointer(Ptr, IntPtr.Size));

		public TypeAttributes OffsetOrSize => (TypeAttributes)(Intrinsic.Load32(Ptr, IntPtr.Size * 2) & 0x00FFFFFF);

		public uint Size => Intrinsic.Load32(Ptr, IntPtr.Size * 3);

		public AssemblyDefinition Assembly => new AssemblyDefinition(Intrinsic.LoadPointer(Ptr, IntPtr.Size * 4));

		public TypeDefinition ParentType => new TypeDefinition(Intrinsic.LoadPointer(Ptr, IntPtr.Size * 5));

		public TypeDefinition DeclaringType => new TypeDefinition(Intrinsic.LoadPointer(Ptr, IntPtr.Size * 6));

		public TypeDefinition ElementType => new TypeDefinition(Intrinsic.LoadPointer(Ptr, IntPtr.Size * 7));

		public CustomAttributeTable DefaultConstructor => new CustomAttributeTable(Intrinsic.LoadPointer(Ptr, IntPtr.Size * 8));

		public IntPtr Properties => Intrinsic.LoadPointer(Ptr, IntPtr.Size * 9);

		public IntPtr Fields => Intrinsic.LoadPointer(Ptr, IntPtr.Size * 10);

		public IntPtr SlotTable => Intrinsic.LoadPointer(Ptr, IntPtr.Size * 11);

		public IntPtr Bitmap => Intrinsic.LoadPointer(Ptr, IntPtr.Size * 12);

		public uint NumberOfMethods => Intrinsic.Load32(Ptr, IntPtr.Size * 13);

		public MethodDefinition GetMethodDefinition(uint slot)
		{
			return new MethodDefinition(Intrinsic.LoadPointer(Ptr, (IntPtr.Size * 14) + (IntPtr.Size * (int)slot)));
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
