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
			yield return TokenString("Name", row.TypeNameIdx);
			yield return TokenString("Namespace", row.TypeNamespaceIdx);
			yield return TokenValue("TypeNameIdx", row.TypeNameIdx);
			yield return TokenValue("TypeNamespaceIdx", row.TypeNamespaceIdx);
			yield return TokenValue("Extends", row.Extends);
			yield return TokenValue("FieldList", row.FieldList);
			yield return Value("Flags", row.Flags.ToString());
			yield return TokenValue("MethodList", row.MethodList);
		}
	}
}
