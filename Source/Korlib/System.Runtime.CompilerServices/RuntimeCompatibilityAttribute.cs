// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Runtime.CompilerServices
{
	[SerializableAttribute]
	[AttributeUsageAttribute(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
	public sealed class RuntimeCompatibilityAttribute : Attribute
	{
		public RuntimeCompatibilityAttribute()
		{
		}

		public bool WrapNonExceptionThrows
		{
			get { return false; }
			set { }
		}
	}
}
