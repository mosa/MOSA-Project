// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Diagnostics
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	public sealed class ConditionalAttribute : Attribute
	{
		private readonly string conditionString;

		public string ConditionString
		{
			get { return conditionString; }
		}

		public ConditionalAttribute(string conditionString)
		{
			this.conditionString = ConditionString;
		}
	}
}
