// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MetadataAssemblyStruct
	{
		public uint* Name;
		public uint* CustomAttributes;
		public uint Attributes;
		public uint NumberOfTypes;
		public const uint TypesOffset = 4;

		public static MetadataTypeStruct* GetTypeDefinitionAddress(MetadataAssemblyStruct* data, uint slot)
		{
			return (MetadataTypeStruct*)*((uint*)data + MetadataAssemblyStruct.TypesOffset + slot);
		}
	}
}
