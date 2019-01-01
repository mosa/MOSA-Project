// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Runtime.InteropServices
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public sealed class DllImportAttribute : Attribute
	{
		public DllImportAttribute(string dllName)
		{
			Value = dllName;
		}

		public string Value { get; }

		public string EntryPoint;
		public bool SetLastError;
		public bool ExactSpelling;
		public CallingConvention CallingConvention;
		public bool BestFitMapping;
		public bool PreserveSig;
		public bool ThrowOnUnmappableChar;
	}
}
