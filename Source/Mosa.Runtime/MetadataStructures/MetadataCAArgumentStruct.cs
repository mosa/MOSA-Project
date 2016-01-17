// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MetadataCAArgumentStruct
	{
		public uint* Name;
		public bool IsField;
		public MetadataTypeStruct* ArgumentType;
		public int ArgumentSize;
		public const uint ArgumentOffset = 4;

		public static uint* GetArgumentAddress(MetadataCAArgumentStruct* data)
		{
			return (uint*)data + MetadataCAArgumentStruct.ArgumentOffset;
		}
	}
}
