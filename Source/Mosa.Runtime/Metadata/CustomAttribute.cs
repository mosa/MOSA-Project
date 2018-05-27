// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.Metadata
{
	public struct CustomAttribute
	{
		#region layout

		// UIntPtr _attributeType;
		// UIntPtr _constructorMethod;
		// int _numberOfArguments;

		#endregion layout

		public UIntPtr Ptr;

		public CustomAttribute(UIntPtr ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr == UIntPtr.Zero;

		public TypeDefinition TypeDefinition => new TypeDefinition(Ptr);

		public MethodDefinition MethodDefinition => new MethodDefinition(Ptr + UIntPtr.Size);

		public uint NumberOfArguments => Intrinsic.Load32(Ptr, UIntPtr.Size * 2);

		public CustomAttributeArgument GetCustomAttributeArgument(uint slot)
		{
			return new CustomAttributeArgument(Intrinsic.LoadPointer(Ptr, (UIntPtr.Size * 3) + (UIntPtr.Size * (int)slot)));
		}
	}
}
