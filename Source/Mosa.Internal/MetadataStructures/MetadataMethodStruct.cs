// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Internal
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MetadataMethodStruct
	{
		public uint* Name;
		public uint* CustomAttributes;
		public uint Attributes;
		public uint StackSize;

		/// <summary>
		/// Points to the entry point of the method
		/// </summary>
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
