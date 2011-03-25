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
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Tools.MetadataExplorer.Tables
{

	/// <summary>
	/// 
	/// </summary>
	public class GenericParamRowExt : TableRow
	{
		protected GenericParamRow row;

		public GenericParamRowExt(IMetadataProvider metadata, GenericParamRow row)
		{
			this.metadata = metadata;
			this.row = row;
		}

		public override string Name { get { return metadata.ReadString(row.NameStringIdx); } }

		public override IEnumerable GetValues()
		{
			yield return TokenString("Name", row.NameStringIdx);
			yield return TokenValue("NameStringIdx", row.NameStringIdx);
			yield return TokenValue("OwnerTableIdx", row.OwnerTableIdx);
			yield return Value("Flags", row.Flags.ToString());
			yield return Value("Number", row.Number.ToString());
		}
	}
}
