// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Internal
{
	[StructLayout(LayoutKind.Sequential)]

	// Protected Region Table
	public unsafe struct MetadataPRTableStruct
	{
		public int NumberOfRegions;
		public const uint ProtectedRegionDefintionOffset = 1;

		public static MetadataPRDefinitionStruct* GetProtectedRegionDefinitionAddress(MetadataPRTableStruct* data, uint slot)
		{
			return (MetadataPRDefinitionStruct*)*((uint*)data + MetadataPRTableStruct.ProtectedRegionDefintionOffset + slot);
		}
	}
}
