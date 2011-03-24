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

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;

namespace Mosa.Tools.MetadataExplorer.Tables
{

	/// <summary>
	/// 
	/// </summary>
	public class TypeDefRowExt : TableRow
	{
		protected TypeDefRow row;

		public TypeDefRowExt(IMetadataProvider metadata, TypeDefRow row)
		{
			this.metadata = metadata;
			this.row = row;
		}

		public override string Name { get { return metadata.ReadString(row.TypeNameIdx); } }

		public override IEnumerable GetValues()
		{
			//yield return Create("Name IDX", row.TypeNameIdx.FormatToString());
			yield return Create("Name", Name);
			yield return Create("Namespace", metadata.ReadString(row.TypeNamespaceIdx));
		}
	}
}
