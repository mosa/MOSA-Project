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
		// UIntPtr	_exceptionType;

		#endregion layout

		private UIntPtr Ptr;

		public ProtectedRegionDefinition(UIntPtr ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr == UIntPtr.Zero;

		public uint StartOffset => Intrinsic.Load32(Ptr);

		public uint EndOffset => Intrinsic.Load32(Ptr, UIntPtr.Size);

		public uint HandlerOffset => Intrinsic.Load32(Ptr, UIntPtr.Size * 2);

		public ExceptionHandlerType HandlerType => (ExceptionHandlerType)Intrinsic.Load32(Ptr, UIntPtr.Size * 3);

		public TypeDefinition ExceptionType => new TypeDefinition(Intrinsic.LoadPointer(Ptr, UIntPtr.Size * 4));
	}
}
