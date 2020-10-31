// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Runtime.Metadata
{
	public struct ProtectedRegionDefinition
	{
		#region layout

		// uint		_startOffset;
		// uint		_endOffset;
		// uint		_handlerOffset;
		// uint		_handlerType;
		// Pointer	_exceptionType;

		#endregion layout

		private readonly Pointer Ptr;

		public ProtectedRegionDefinition(Pointer ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr.IsNull;

		public uint StartOffset => Ptr.Load32();

		public uint EndOffset => Ptr.Load32(Pointer.Size);

		public uint HandlerOffset => Ptr.Load32(Pointer.Size * 2);

		public ExceptionHandlerType HandlerType => (ExceptionHandlerType)Ptr.Load32(Pointer.Size * 3);

		public TypeDefinition ExceptionType => new TypeDefinition(Ptr.LoadPointer(Pointer.Size * 4));
	}
}
