// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.InteropServices;

namespace Mosa.Runtime
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MDMethodDefinition
	{
		private IntPtr _name;
		private IntPtr _customAttributes;
		private uint _attributes;
		private uint _stackSize;
		private IntPtr _method;
		private IntPtr _returnType;
		private IntPtr _protectedRegionTable;
		private IntPtr _gcTrackingInformation;
		private uint _numberOfParameters;

		public string Name => (string)Intrinsic.GetObjectFromAddress(_name);

		public MDCustomAttributeTable* CustomAttributes => (MDCustomAttributeTable*)_customAttributes;

		public uint Attributes => _attributes;

		public uint StackSize => _stackSize;

		/// <summary>
		/// Points to the entry point of the method
		/// </summary>
		public IntPtr Method => _method;

		public MDTypeDefinition* ReturnType => (MDTypeDefinition*)_returnType;

		public MDProtectedRegionTable* ProtectedRegionTable => (MDProtectedRegionTable*)_protectedRegionTable;

		public IntPtr GCTrackingInformation => _gcTrackingInformation;

		public uint NumberOfParameters => _numberOfParameters;

		public IntPtr GetParameterDefinition(uint slot)
		{
			fixed (MDMethodDefinition* _this = &this)
			{
				return Intrinsic.LoadPointer(new IntPtr(_this) + sizeof(MDMethodDefinition) + (IntPtr.Size * (int)slot));
			}
		}
	}
}
