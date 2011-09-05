/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


using Mosa.Runtime.Metadata;

namespace Mosa.Tools.MetadataExplorer
{

	public static class Extension
	{

		public static string FormatToString(this HeapIndexToken token)
		{
			return ((int)token).ToString("X8");
		}

		public static string FormatToString(this TableType token)
		{
			return ((int)token).ToString("X8");
		}

		public static string FormatToString(this Token token)
		{
			return token.ToInt32().ToString("X8");
		}

	}
}
