// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	// Protected Region Definition Struct
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MDProtectedRegionDefinition
	{
		private uint _startOffset;
		private uint _endOffset;
		private uint _handlerOffset;
		private uint _handlerType;
		private Ptr _exceptionType;

		public uint StartOffset => _startOffset;

		public uint EndOffset => _endOffset;

		public uint HandlerOffset => _handlerOffset;

		public ExceptionHandlerType HandlerType => (ExceptionHandlerType)_handlerType;

		public MDTypeDefinition* ExceptionType => (MDTypeDefinition*)_exceptionType;
	}
}
