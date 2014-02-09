/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.Metadata.Tables;
using System.Collections;
using Mosa.Compiler.Metadata.Loader;

namespace Mosa.Tool.MetadataExplorer.Tables
{
	/// <summary>
	///
	/// </summary>
	public class TypeSpecRowExt : TableRow
	{
		protected TypeSpecRow row;

		public TypeSpecRowExt(IMetadataModule metadataModule, TypeSpecRow row)
			: base(metadataModule)
		{
			this.row = row;
		}

		public override string Name { get { return new TypeSpecSignature(Metadata, row.SignatureBlob).ToString(); } }

		public override IEnumerable GetValues()
		{
			yield break;
		}
	}
}