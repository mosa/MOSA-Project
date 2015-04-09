/*
 * (c) 2015 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Sebastian Loncar (Arakis) <sebastian.loncar@gmail.com>
 */

namespace System.Diagnostics
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	public sealed class ConditionalAttribute : Attribute
	{
		private string conditionString;

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