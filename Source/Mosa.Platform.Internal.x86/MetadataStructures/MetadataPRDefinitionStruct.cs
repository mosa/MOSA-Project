// Copyright (c) MOSA Project. Licensed under the New BSD License.

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