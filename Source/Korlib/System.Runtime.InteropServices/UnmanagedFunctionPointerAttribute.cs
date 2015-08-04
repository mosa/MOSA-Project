// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Runtime.InteropServices
{
	[AttributeUsage(AttributeTargets.Delegate, Inherited = false, AllowMultiple = false), ComVisible(true)]
	public sealed class UnmanagedFunctionPointerAttribute : Attribute
	{
		private CallingConvention call_conv;

		public CallingConvention CallingConvention { get { return call_conv; } }

		public UnmanagedFunctionPointerAttribute(CallingConvention callingConvention)
		{
			this.call_conv = callingConvention;
		}
	}
}
