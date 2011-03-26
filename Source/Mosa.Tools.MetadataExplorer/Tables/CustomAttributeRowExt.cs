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
	public class CustomAttributeRowExt : TableRow
	{
		protected CustomAttributeRow row;

		public CustomAttributeRowExt(IMetadataProvider metadata, CustomAttributeRow row)
			: base(metadata)
		{
			this.row = row;
		}

		public override string Name { get { return string.Empty; } }

		public override IEnumerable GetValues()
		{
			yield return Value("ParentTableIdx", row.ParentTableIdx);
			yield return Value("TypeIdx", row.TypeIdx);
			yield return Value("SignatureBlobIdx", row.ValueBlobIdx);
		}
	}
}
