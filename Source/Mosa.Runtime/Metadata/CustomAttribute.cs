// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.Metadata
{
	public struct CustomAttribute
	{
		#region layout

		// Pointer _attributeType;
		// Pointer _constructorMethod;
		// int _numberOfArguments;

		#endregion layout

		public Pointer Ptr;

		public CustomAttribute(Pointer ptr)
		{
			Ptr = ptr;
		}

		public bool IsNull => Ptr.IsNull;

		public TypeDefinition AttributeType => new TypeDefinition(Ptr);

		public MethodDefinition ConstructorMethod => new MethodDefinition(Ptr + Pointer.Size);

		public uint NumberOfArguments => Intrinsic.Load32(Ptr, Pointer.Size * 2);

		public CustomAttributeArgument GetCustomAttributeArgument(uint slot)
		{
			return new CustomAttributeArgument(Intrinsic.LoadPointer(Ptr, (Pointer.Size * 3) + (Pointer.Size * (int)slot)));
		}
	}
}
