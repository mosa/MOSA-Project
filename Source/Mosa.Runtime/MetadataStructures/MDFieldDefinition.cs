// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MDFieldDefinition
	{
		private Ptr _name;
		private Ptr _customAttributes;
		private uint _attributes;
		private Ptr _fieldType;
		private Ptr _fieldData;
		private uint _offsetOrSize;

		public string Name => (string)Intrinsic.GetObjectFromAddress(_name);

		public MDCustomAttributeTable* CustomAttributes => (MDCustomAttributeTable*)_customAttributes;

		public uint Attributes => _attributes;

		public MDTypeDefinition* FieldType => (MDTypeDefinition*)_fieldType;

		public byte* FieldData => (byte*)_fieldData;

		public uint OffsetOrSize => _offsetOrSize;
	}
}
