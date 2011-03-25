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
	public class MethodDefRowExt : TableRow
	{
		protected MethodDefRow row;

		public MethodDefRowExt(IMetadataProvider metadata, MethodDefRow row)
		{
			this.metadata = metadata;
			this.row = row;
		}

		public override string Name { get { return metadata.ReadString(row.NameStringIdx); } }

		public override IEnumerable GetValues()
		{
			yield return TokenString("Name", row.NameStringIdx);
			yield return TokenValue("NameStringIdx", row.NameStringIdx);
			yield return Value("Flags", row.Flags.ToString());
			yield return Value("ImplFlags", row.ImplFlags.ToString());
			yield return TokenValue("ParamList", row.ParamList);
			yield return Value("Rva", row.Rva.ToString()); 
			yield return TokenValue("SignatureBlobIdx", row.SignatureBlobIdx);
		}
	}
}
