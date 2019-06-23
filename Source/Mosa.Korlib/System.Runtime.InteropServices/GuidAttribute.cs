// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Runtime.InteropServices
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Delegate, Inherited = false)]
	public sealed class GuidAttribute : Attribute
	{
		public GuidAttribute(string guid)
		{
			Value = guid;
		}

		public string Value { get; }
	}
}
