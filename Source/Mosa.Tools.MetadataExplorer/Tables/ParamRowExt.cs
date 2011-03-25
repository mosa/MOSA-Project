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
	public class ParamRowExt : TableRow
	{
		protected ParamRow row;

		public ParamRowExt(IMetadataProvider metadata, ParamRow row)
		{
			this.metadata = metadata;
			this.row = row;
		}

		public override string Name { get { return metadata.ReadString(row.NameIdx); } }

		public override IEnumerable GetValues()
		{
			yield return TokenString("Name", row.NameIdx);
			yield return TokenValue("NameIdx", row.NameIdx);
			yield return Value("NameIdx", row.Flags.ToString());
			yield return Value("Sequence", row.Sequence.ToString());
		}
	}
}
