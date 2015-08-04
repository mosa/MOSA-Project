// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Platform.Internal.x86
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MetadataCAStruct
	{
		public MetadataTypeStruct* AttributeType;
		public uint* ConstructorMethod;
		public int NumberOfArguments;
		public const uint ArgumentsOffset = 3;

		public static MetadataCAArgumentStruct* GetCAArgumentAddress(MetadataCAStruct* data, uint slot)
		{
			return (MetadataCAArgumentStruct*)*((uint*)data + MetadataCAStruct.ArgumentsOffset + slot);
		}
	}
}
