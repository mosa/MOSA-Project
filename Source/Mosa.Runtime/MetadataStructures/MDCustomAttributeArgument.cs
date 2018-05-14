// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MDCustomAttributeArgument
	{
		private UIntPtr _name;
		private bool _isField;
		private UIntPtr _argumentType;
		private int _argumentSize;

		public string Name => (string)Intrinsic.GetObjectFromAddress(_name);

		public bool IsField => _isField;

		public MDTypeDefinition* ArgumentType => (MDTypeDefinition*)_argumentType;

		public int ArgumentSize => _argumentSize;

		public UIntPtr GetArgumentValue()
		{
			fixed (MDCustomAttributeArgument* _this = &this)
			{
				return new UIntPtr(_this) + sizeof(MDCustomAttributeArgument);
			}
		}
	}
}
