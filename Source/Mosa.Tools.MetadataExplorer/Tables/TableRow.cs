/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

using Mono.Cecil;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;

namespace Mosa.Tools.MetadataExplorer.Tables
{

	public abstract class TableRow
	{
		protected IMetadataProvider metadata;

		public abstract string Name { get; }

		public abstract IEnumerable GetValues();

		protected KeyValuePair<string, string> Create(string name, string value)
		{
			return new KeyValuePair<string, string>(name, value);
		}
	}
}
