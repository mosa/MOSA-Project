// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MDMethodDefinition
	{
		private UIntPtr _name;
		private UIntPtr _customAttributes;
		private uint _attributes;
		private uint _stackSize;
		private UIntPtr _method;
		private UIntPtr _returnType;
		private UIntPtr _protectedRegionTable;
		private UIntPtr _gcTrackingInformation;
		private uint _numberOfParameters;

		public string Name => (string)Intrinsic.GetObjectFromAddress(_name);

		public MDCustomAttributeTable* CustomAttributes => (MDCustomAttributeTable*)_customAttributes;

		public uint Attributes => _attributes;

		public uint StackSize => _stackSize;

		/// <summary>
		/// Points to the entry point of the method
		/// </summary>
		public UIntPtr Method => _method;

		public MDTypeDefinition* ReturnType => (MDTypeDefinition*)_returnType;

		public MDProtectedRegionTable* ProtectedRegionTable => (MDProtectedRegionTable*)_protectedRegionTable;

		public UIntPtr GCTrackingInformation => _gcTrackingInformation;

		public uint NumberOfParameters => _numberOfParameters;

		public UIntPtr GetParameterDefinition(uint slot)
		{
			fixed (MDMethodDefinition* _this = &this)
			{
				return Intrinsic.LoadPointer(new UIntPtr(_this) + sizeof(MDMethodDefinition) + (UIntPtr.Size * (int)slot));
			}
		}
	}
}
