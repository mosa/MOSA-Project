/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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