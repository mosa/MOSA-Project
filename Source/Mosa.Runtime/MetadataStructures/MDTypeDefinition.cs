// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MDTypeDefinition
	{
		private UIntPtr _name;
		private UIntPtr _customAttributes;
		private uint _attributes;
		private uint _size;
		private UIntPtr _assembly;
		private UIntPtr _parentType;
		private UIntPtr _declaringType;
		private UIntPtr _elementType;
		private UIntPtr _defaultConstructor;
		private UIntPtr _properties;
		private UIntPtr _fields;
		private UIntPtr _slotTable;
		private UIntPtr _bitmap;
		private uint _numberOfMethods;

		public string Name => (string)Intrinsic.GetObjectFromAddress(_name);

		public MDCustomAttributeTable* CustomAttributes => (MDCustomAttributeTable*)_customAttributes;

		public TypeCode TypeCode => (TypeCode)(_attributes >> 24);

		public TypeAttributes Attributes => (TypeAttributes)(_attributes & 0x00FFFFFF);

		public uint Size => _size;

		public MDAssemblyDefinition* Assembly => (MDAssemblyDefinition*)_assembly;

		public MDTypeDefinition* ParentType => (MDTypeDefinition*)_parentType;

		public MDTypeDefinition* DeclaringType => (MDTypeDefinition*)_declaringType;

		public MDTypeDefinition* ElementType => (MDTypeDefinition*)_elementType;

		public MDMethodDefinition* DefaultConstructor => (MDMethodDefinition*)_defaultConstructor;

		public UIntPtr Properties => _properties;

		public UIntPtr Fields => _fields;

		public UIntPtr SlotTable => _slotTable;

		public UIntPtr Bitmap => _bitmap;

		public uint NumberOfMethods => _numberOfMethods;

		public MDMethodDefinition* GetMethodDefinition(uint slot)
		{
			fixed (MDTypeDefinition* _this = &this)
			{
				return (MDMethodDefinition*)Intrinsic.Load(new UIntPtr(_this) + sizeof(MDTypeDefinition) + (UIntPtr.Size * (int)slot));
			}
		}
	}
}
