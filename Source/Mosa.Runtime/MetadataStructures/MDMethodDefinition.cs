// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MDMethodDefinition
	{
		private Ptr _name;
		private Ptr _customAttributes;
		private uint _attributes;
		private uint _stackSize;
		private Ptr _method;
		private Ptr _returnType;
		private Ptr _protectedRegionTable;
		private Ptr _gcTrackingInformation;
		private uint _numberOfParameters;

		public string Name => (string)Intrinsic.GetObjectFromAddress(_name);

		public MDCustomAttributeTable* CustomAttributes => (MDCustomAttributeTable*)_customAttributes;

		public uint Attributes => _attributes;

		public uint StackSize => _stackSize;

		/// <summary>
		/// Points to the entry point of the method
		/// </summary>
		public void* Method => _method;

		public MDTypeDefinition* ReturnType => (MDTypeDefinition*)_returnType;

		public MDProtectedRegionTable* ProtectedRegionTable => (MDProtectedRegionTable*)_protectedRegionTable;

		public Ptr GCTrackingInformation => _gcTrackingInformation;

		public uint NumberOfParameters => _numberOfParameters;

		public Ptr GetParameterDefinition(uint slot)
		{
			fixed (MDMethodDefinition* _this = &this)
			{
				Ptr pThis = _this;
				return (Ptr)(pThis + sizeof(MDMethodDefinition) + (Ptr.Size * slot))[0];
			}
		}
	}
}
