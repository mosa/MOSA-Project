/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System.Runtime.InteropServices;

namespace Mosa.Platform.Internal.x86
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