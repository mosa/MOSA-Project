// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MDCustomAttribute
	{
		private UIntPtr _attributeType;
		private UIntPtr _constructorMethod;
		private int _numberOfArguments;

		public MDTypeDefinition* AttributeType => (MDTypeDefinition*)_attributeType;

		public MDMethodDefinition* ConstructorMethod => (MDMethodDefinition*)_constructorMethod;

		public int NumberOfArguments => _numberOfArguments;

		public MDCustomAttributeArgument* GetCustomAttributeArgument(uint slot)
		{
			fixed (MDCustomAttribute* _this = &this)
			{
				return (MDCustomAttributeArgument*)Intrinsic.LoadPointer(new UIntPtr(_this) + sizeof(MDCustomAttribute) + (UIntPtr.Size * (int)slot));
			}
		}
	}
}
