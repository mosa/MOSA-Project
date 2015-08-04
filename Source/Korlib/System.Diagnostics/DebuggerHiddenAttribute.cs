// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Diagnostics
{
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
	public sealed class DebuggerHiddenAttribute : Attribute
	{
		public DebuggerHiddenAttribute()
			: base()
		{
		}
	}
}
