// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

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

		private Pointer Ptr;

		public ProtectedRegionDefinition(Pointer ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr.IsNull;

		public uint StartOffset => Intrinsic.Load32(Ptr);

		public uint EndOffset => Intrinsic.Load32(Ptr, Pointer.Size);

		public uint HandlerOffset => Intrinsic.Load32(Ptr, Pointer.Size * 2);

		public ExceptionHandlerType HandlerType => (ExceptionHandlerType)Intrinsic.Load32(Ptr, Pointer.Size * 3);

		public TypeDefinition ExceptionType => new TypeDefinition(Intrinsic.LoadPointer(Ptr, Pointer.Size * 4));
	}
}
