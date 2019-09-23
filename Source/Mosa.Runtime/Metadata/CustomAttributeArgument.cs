// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.Metadata
{
	public struct CustomAttributeArgument
	{
		#region layout

		// Pointer _name;
		// bool _isField;
		// Pointer _argumentType;
		// int _argumentSize;

		#endregion layout

		public Pointer Ptr;

		public CustomAttributeArgument(Pointer ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr.IsNull;

		public string Name => (string)Intrinsic.GetObjectFromAddress(Intrinsic.LoadPointer(Ptr));

		public bool IsField => Intrinsic.Load8(Ptr, Pointer.Size) == 0;

		public TypeDefinition ArgumentType => new TypeDefinition(Intrinsic.LoadPointer(Ptr, Pointer.Size * 2));

		public uint ArgumentSize => Intrinsic.Load32(Ptr, Pointer.Size * 3);

		public Pointer GetArgumentValue()
		{
			return Intrinsic.LoadPointer(Ptr, Pointer.Size * 4);
		}
	}
}
