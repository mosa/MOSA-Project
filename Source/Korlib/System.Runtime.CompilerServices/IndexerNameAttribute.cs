// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Runtime.CompilerServices
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true)]
	[Serializable]
	public sealed class IndexerNameAttribute : Attribute
	{
		public IndexerNameAttribute(string indexerName)
		{
		}
	}
}
