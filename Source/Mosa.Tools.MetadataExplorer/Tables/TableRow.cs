/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections;
using System.Collections.Generic;
using Mosa.Runtime.Metadata;

namespace Mosa.Tools.MetadataExplorer.Tables
{

	public abstract class TableRow
	{
		private IMetadataProvider metadata;

		public abstract string Name { get; }
		public IMetadataProvider Metadata { get { return metadata; } }

		public TableRow(IMetadataProvider metadata)
		{
			this.metadata = metadata;
		}

		public abstract IEnumerable GetValues();

		protected KeyValuePair<string, string> Value(string name, string value)
		{
			return new KeyValuePair<string, string>(name, value);
		}

		protected KeyValuePair<string, string> Value(string name, int value)
		{
			return new KeyValuePair<string, string>(name, value.ToString());
		}

		protected KeyValuePair<string, string> Value(string name, Token token)
		{
			return Value(name, token.FormatToString());
		}

		protected KeyValuePair<string, string> Value(string name, HeapIndexToken token)
		{
			return Value(name, token.FormatToString());
		}

		protected KeyValuePair<string, string> TokenString(string name, HeapIndexToken token)
		{
			return Value(name, "[" + token.ToString("X") + "] " + ReadString(token));
		}

		private string ReadString(HeapIndexToken token)
		{
			return metadata.ReadString(token);
		}
	}
}
