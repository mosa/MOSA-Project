// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.Metadata
{
	public struct CustomAttributeArgument
	{
		#region layout

		// IntPtr _name;
		// bool _isField;
		// IntPtr _argumentType;
		// int _argumentSize;

		#endregion layout

		public IntPtr Ptr;

		public CustomAttributeArgument(IntPtr ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr == IntPtr.Zero;

		public string Name => (string)Intrinsic.GetObjectFromAddress(Intrinsic.LoadPointer(Ptr));

		public bool IsField => Intrinsic.Load8(Ptr, IntPtr.Size) == 0;

		public TypeDefinition ArgumentType => new TypeDefinition(Intrinsic.LoadPointer(Ptr, IntPtr.Size * 2));

		public uint ArgumentSize => Intrinsic.Load32(Ptr, IntPtr.Size * 3);

		public IntPtr GetArgumentValue()
		{
			return Intrinsic.LoadPointer(Ptr, IntPtr.Size * 4);
		}
	}
}
