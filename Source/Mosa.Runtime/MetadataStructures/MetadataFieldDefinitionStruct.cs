// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MetadataFieldDefinitionStruct
	{
		public uint* Name;
		public uint* CustomAttributes;
		public uint Attributes;
		public MetadataTypeStruct* FieldType;
		public byte* FieldData;
		public uint OffsetOrSize;
	}
}
