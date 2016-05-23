// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MDCustomAttribute
	{
		private Ptr _attributeType;
		private Ptr _constructorMethod;
		private int _numberOfArguments;

		public MDTypeDefinition* AttributeType => (MDTypeDefinition*)_attributeType;

		public MDMethodDefinition* ConstructorMethod => (MDMethodDefinition*)_constructorMethod;

		public int NumberOfArguments => _numberOfArguments;

		public MDCustomAttributeArgument* GetCustomAttributeArgument(uint slot)
		{
			fixed (MDCustomAttribute* _this = &this)
			{
				Ptr pThis = _this;
				return (MDCustomAttributeArgument*)(pThis + sizeof(MDCustomAttribute) + (Ptr.Size * slot))[0];
			}
		}
	}
}
