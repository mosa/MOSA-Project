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
	public class FieldRowExt : TableRow
	{
		protected FieldRow row;

		public FieldRowExt(IMetadataProvider metadata, FieldRow row)
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
			yield return TokenValue("SignatureBlobIdx", row.SignatureBlobIdx);

			FieldSignature signature = new FieldSignature(metadata, row.SignatureBlobIdx);
			yield return TokenValue("Signature Token", signature.Token);
			yield return Value("Signature Modifier", signature.Modifier.ToString());
			yield return Value("Signature Type", signature.Type.ToString());
		}
	}
}
