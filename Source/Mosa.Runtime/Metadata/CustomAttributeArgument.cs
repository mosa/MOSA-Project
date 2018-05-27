// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.Metadata
{
	public struct CustomAttributeArgument
	{
		#region layout

		// UIntPtr _name;
		// bool _isField;
		// UIntPtr _argumentType;
		// int _argumentSize;

		#endregion layout

		public UIntPtr Ptr;

		public CustomAttributeArgument(UIntPtr ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr == UIntPtr.Zero;

		public string Name => (string)Intrinsic.GetObjectFromAddress(Intrinsic.LoadPointer(Ptr));

		public bool IsField => Intrinsic.Load8(Ptr, UIntPtr.Size) == 0;

		public TypeDefinition TypeDefinition => new TypeDefinition(Intrinsic.LoadPointer(Ptr, UIntPtr.Size * 2));

		public uint ArgumentSize => Intrinsic.Load32(Ptr, UIntPtr.Size * 3);

		public UIntPtr GetArgumentValue()
		{
			return Intrinsic.LoadPointer(Ptr, UIntPtr.Size * 4);
		}
	}
}
