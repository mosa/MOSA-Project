// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MDAssemblyDefinition
	{
		private UIntPtr _name;
		private UIntPtr _customAttributes;
		private uint _attributes;
		private uint _numberOfTypes;

		public string Name => (string)Intrinsic.GetObjectFromAddress(_name);

		public MDCustomAttributeTable* CustomAttributes => (MDCustomAttributeTable*)_customAttributes;

		public uint Attributes => _attributes;

		public uint NumberOfTypes => _numberOfTypes;

		public MDTypeDefinition* GetTypeDefinition(uint slot)
		{
			fixed (MDAssemblyDefinition* _this = &this)
			{
				return (MDTypeDefinition*)Intrinsic.Load(new UIntPtr(_this) + sizeof(MDAssemblyDefinition) + (UIntPtr.Size * (int)slot));
			}
		}
	}
}
