// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Reflection;
using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MDTypeDefinition
	{
		private Ptr _name;
		private Ptr _customAttributes;
		private uint _attributes;
		private uint _size;
		private Ptr _assembly;
		private Ptr _parentType;
		private Ptr _declaringType;
		private Ptr _elementType;
		private Ptr _defaultConstructor;
		private Ptr _properties;
		private Ptr _fields;
		private Ptr _slotTable;
		private Ptr _bitmap;
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

		public Ptr Properties => _properties;

		public Ptr Fields => _fields;

		public Ptr SlotTable => _slotTable;

		public Ptr Bitmap => _bitmap;

		public uint NumberOfMethods => _numberOfMethods;

		public MDMethodDefinition* GetMethodDefinition(uint slot)
		{
			fixed (MDTypeDefinition* _this = &this)
			{
				Ptr pThis = _this;
				return (MDMethodDefinition*)(pThis + sizeof(MDTypeDefinition) + (Ptr.Size * slot))[0];
			}
		}
	}
}
