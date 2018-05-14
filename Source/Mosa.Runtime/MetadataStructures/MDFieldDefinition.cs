// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MDFieldDefinition
	{
		private UIntPtr _name;
		private UIntPtr _customAttributes;
		private uint _attributes;
		private UIntPtr _fieldType;
		private UIntPtr _fieldData;
		private uint _offsetOrSize;

		public string Name => (string)Intrinsic.GetObjectFromAddress(_name);

		public MDCustomAttributeTable* CustomAttributes => (MDCustomAttributeTable*)_customAttributes;

		public uint Attributes => _attributes;

		public MDTypeDefinition* FieldType => (MDTypeDefinition*)_fieldType;

		public byte* FieldData => (byte*)_fieldData;

		public uint OffsetOrSize => _offsetOrSize;
	}
}
