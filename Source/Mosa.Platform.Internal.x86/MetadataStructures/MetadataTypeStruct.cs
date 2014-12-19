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
	public unsafe struct MetadataTypeStruct
	{
		public uint* Name;
		public uint* CustomAttributes;
		public uint Attributes;
		public uint Size;
		public uint* Assembly;
		public MetadataTypeStruct* ParentType;
		public MetadataTypeStruct* DeclaringType;
		public MetadataTypeStruct* ElementType;
		public uint* DefaultConstructor;
		public uint* Properties;
		public uint* Fields;
		public uint* SlotTable;
		public uint* Bitmap;
		public uint NumberOfMethods;
		public const uint MethodsOffset = 14;

		public static uint* GetMethodDefinitionAddress(MetadataTypeStruct* data, uint slot)
		{
			return (uint*)*((uint*)data + MetadataTypeStruct.MethodsOffset + slot);
		}
	}
}