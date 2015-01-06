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
	public unsafe struct MetadataCAStruct
	{
		public MetadataTypeStruct* AttributeType;
		public uint* ConstructorMethod;
		public int NumberOfArguments;
		public const uint ArgumentsOffset = 3;

		public static uint* GetCAArgumentAddress(MetadataCAStruct* data, uint slot)
		{
			return (uint*)*((uint*)data + MetadataCAStruct.ArgumentsOffset + slot);
		}
	}
}