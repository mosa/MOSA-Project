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
			: base(metadata)
		{
			this.row = row;
		}

		public override string Name { get { return Metadata.ReadString(row.TypeNameIdx); } }

		public override IEnumerable GetValues()
		{
			yield return TokenString("Name", row.TypeNameIdx);
			yield return TokenString("Namespace", row.TypeNamespaceIdx);
			yield return Value("TypeNameIdx", row.TypeNameIdx);
			yield return Value("TypeNamespaceIdx", row.TypeNamespaceIdx);
			yield return Value("Extends", row.Extends);
			yield return Value("FieldList", row.FieldList);
			yield return Value("Flags", row.Flags.ToString());
			yield return Value("MethodList", row.MethodList);
		}
	}
}
