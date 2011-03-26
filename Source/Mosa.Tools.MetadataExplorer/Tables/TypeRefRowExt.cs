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
	public class TypeRefRowExt : TableRow
	{
		protected TypeRefRow row;

		public TypeRefRowExt(IMetadataProvider metadata, TypeRefRow row)
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
			yield return Value("ResolutionScopeIdx", row.ResolutionScopeIdx);
		}
	}
}
