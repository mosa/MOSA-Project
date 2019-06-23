// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Reflection
{
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyTrademarkAttribute : Attribute
	{
		public AssemblyTrademarkAttribute(string trademark)
		{
			Trademark = trademark;
		}

		public string Trademark { get; }
	}
}
