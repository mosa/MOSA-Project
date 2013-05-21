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
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public sealed class DllImportAttribute : Attribute
	{
		public CallingConvention CallingConvention;
		private string Dll;
		public string EntryPoint;

		public string Value { get { return Dll; } }

		public DllImportAttribute(string dllName)
		{
			Dll = dllName;
		}
	}
}