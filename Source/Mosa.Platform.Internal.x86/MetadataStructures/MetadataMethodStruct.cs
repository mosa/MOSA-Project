/*
 * (c) 2015 MOSA - The Managed Operating System Alliance
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
	public unsafe struct MetadataMethodStruct
	{
		public uint* Name;
		public uint* CustomAttributes;
		public uint Attributes;
		public uint StackSize;
		public void* Method;
		public MetadataTypeStruct* ReturnType;
		public MetadataPRTableStruct* ProtectedRegionTable;
		public uint* GCTrackingInformation;
		public uint NumberOfParameters;
		public const uint ParametersOffset = 9;

		public static uint* GetParameterDefinitionAddress(MetadataMethodStruct* data, uint slot)
		{
			return (uint*)*((uint*)data + MetadataMethodStruct.ParametersOffset + slot);
		}
	}
}