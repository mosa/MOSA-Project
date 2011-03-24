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
	public class TypeSpecRowExt : TableRow
	{
		protected TypeSpecRow row;

		public TypeSpecRowExt(IMetadataProvider metadata, TypeSpecRow row)
		{
			this.metadata = metadata;
			this.row = row;
		}

		public override string Name { get { return row.SignatureBlobIdx.FormatToString(); } }

		public override IEnumerable GetValues()
		{
			//yield return Create("Name IDX", row.TypeNameIdx.ToString());
			yield return Create("SignatureBlobIdx", row.SignatureBlobIdx.FormatToString());
			
			//TODO: parse the signature and convert to a list of values for display
			
			//yield return Create("Signature", signature);
		}
	}
}
