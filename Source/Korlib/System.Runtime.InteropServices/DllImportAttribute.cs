// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
