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
	public class FileRowExt : TableRow
	{
		protected FileRow row;

		public FileRowExt(IMetadataProvider metadata, FileRow row)
		{
			this.metadata = metadata;
			this.row = row;
		}

		public override string Name { get { return metadata.ReadString(row.NameStringIdx); } }

		public override IEnumerable GetValues()
		{
			yield return TokenValue("NameStringIdx", row.NameStringIdx);
			yield return TokenString("Name", row.NameStringIdx);
		}
	}
}
