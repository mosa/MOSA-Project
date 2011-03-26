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
	public class TypeSpecRowExt : TableRow
	{
		protected TypeSpecRow row;

		public TypeSpecRowExt(IMetadataProvider metadata, TypeSpecRow row)
			: base(metadata)
		{
			this.row = row;
		}

		public override string Name { get { return row.SignatureBlobIdx.FormatToString(); } }

		public override IEnumerable GetValues()
		{
			yield return Value("SignatureBlobIdx", row.SignatureBlobIdx);

			TypeSpecSignature signature = new TypeSpecSignature(Metadata, row.SignatureBlobIdx);
			yield return Value("Signature Token", signature.Token);
			yield return Value("Signature Type", signature.Type.ToString());

		}
	}
}
