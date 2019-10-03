// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

		public uint NumberOfArguments => Ptr.Load32(Pointer.Size * 2);

		public CustomAttributeArgument GetCustomAttributeArgument(uint slot)
		{
			return new CustomAttributeArgument(Ptr.LoadPointer((Pointer.Size * 3) + (Pointer.Size * (int)slot)));
		}
	}
}
