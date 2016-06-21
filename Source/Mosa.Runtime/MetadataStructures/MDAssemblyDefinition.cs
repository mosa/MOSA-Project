// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MDAssemblyDefinition
	{
		private Ptr _name;
		private Ptr _customAttributes;
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
				Ptr pThis = _this;
				return (MDTypeDefinition*)(pThis + sizeof(MDAssemblyDefinition) + (Ptr.Size * slot)).Dereference(0);
			}
		}
	}
}
