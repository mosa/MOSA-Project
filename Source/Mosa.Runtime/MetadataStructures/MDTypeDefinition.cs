// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MDTypeDefinition
	{
		private IntPtr _name;
		private IntPtr _customAttributes;
		private uint _attributes;
		private uint _size;
		private IntPtr _assembly;
		private IntPtr _parentType;
		private IntPtr _declaringType;
		private IntPtr _elementType;
		private IntPtr _defaultConstructor;
		private IntPtr _properties;
		private IntPtr _fields;
		private IntPtr _slotTable;
		private IntPtr _bitmap;
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

		public IntPtr Properties => _properties;

		public IntPtr Fields => _fields;

		public IntPtr SlotTable => _slotTable;

		public IntPtr Bitmap => _bitmap;

		public uint NumberOfMethods => _numberOfMethods;

		public MDMethodDefinition* GetMethodDefinition(uint slot)
		{
			fixed (MDTypeDefinition* _this = &this)
			{
				return (MDMethodDefinition*)Intrinsic.LoadPointer(new IntPtr(_this) + sizeof(MDTypeDefinition) + (IntPtr.Size * (int)slot));
			}
		}
	}
}
