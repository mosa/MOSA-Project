// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MDCustomAttributeArgument
	{
		private Ptr _name;
		private bool _isField;
		private Ptr _argumentType;
		private int _argumentSize;

		public string Name => (string)Intrinsic.GetObjectFromAddress(_name);

		public bool IsField => _isField;

		public MDTypeDefinition* ArgumentType => (MDTypeDefinition*)_argumentType;

		public int ArgumentSize => _argumentSize;

		public Ptr GetArgumentValue()
		{
			fixed (MDCustomAttributeArgument* _this = &this)
			{
				Ptr pThis = _this;
				return (pThis + sizeof(MDCustomAttributeArgument));
			}
		}
	}
}
