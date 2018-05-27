// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.Metadata
{
	public struct CustomAttribute
	{
		#region layout

		// IntPtr _attributeType;
		// IntPtr _constructorMethod;
		// int _numberOfArguments;

		#endregion layout

		public IntPtr Ptr;

		public CustomAttribute(IntPtr ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr == IntPtr.Zero;

		public TypeDefinition TypeDefinition => new TypeDefinition(Ptr);

		public MethodDefinition MethodDefinition => new MethodDefinition(Ptr + IntPtr.Size);

		public uint NumberOfArguments => Intrinsic.Load32(Ptr, IntPtr.Size * 2);

		public CustomAttributeArgument GetCustomAttributeArgument(uint slot)
		{
			return new CustomAttributeArgument(Intrinsic.LoadPointer(Ptr, (IntPtr.Size * 3) + (IntPtr.Size * (int)slot)));
		}
	}
}
