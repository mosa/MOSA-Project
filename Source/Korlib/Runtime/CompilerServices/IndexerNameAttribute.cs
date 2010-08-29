/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael "grover" Fröhlich (<mailto:sharpos@michaelruck.de>)
 */

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
