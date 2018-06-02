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
		// IntPtr	_exceptionType;

		#endregion layout

		private IntPtr Ptr;

		public ProtectedRegionDefinition(IntPtr ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr == IntPtr.Zero;

		public uint StartOffset => Intrinsic.Load32(Ptr);

		public uint EndOffset => Intrinsic.Load32(Ptr, IntPtr.Size);

		public uint HandlerOffset => Intrinsic.Load32(Ptr, IntPtr.Size * 2);

		public ExceptionHandlerType HandlerType => (ExceptionHandlerType)Intrinsic.Load32(Ptr, IntPtr.Size * 3);

		public TypeDefinition ExceptionType => new TypeDefinition(Intrinsic.LoadPointer(Ptr, IntPtr.Size * 4));
	}
}
