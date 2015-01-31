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
	// Protected Region Definition Struct
	public unsafe struct MetadataPRDefinitionStruct
	{
		public uint StartOffset;
		public uint EndOffset;
		public uint HandlerOffset;
		public ExceptionHandlerType HandlerType;
		public MetadataTypeStruct* ExceptionType;
	}
}