// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Diagnostics
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module)]
	public sealed class DebuggableAttribute : Attribute
	{
		public enum DebuggingModes
		{
			None = 0,
			Default = 1,
			IgnoreSymbolStoreSequencePoints = 2,
			EnableEditAndContinue = 4,
			DisableOptimizations = 256
		}

		public DebuggableAttribute(DebuggingModes modes)
		{
		}
	}
}
