/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;

namespace Mosa.Tools.MetadataExplorer.Tables
{

	/// <summary>
	/// 
	/// </summary>
	public class MemberRefRowExt : TableRow
	{
		protected MemberRefRow row;

		public MemberRefRowExt(IMetadataProvider metadata, MemberRefRow row)
			: base(metadata)
		{
			this.row = row;
		}

		public override string Name { get { return Metadata.ReadString(row.NameStringIdx); } }

		public override IEnumerable GetValues()
		{
			yield return TokenString("Name", row.NameStringIdx);
			yield return Value("NameStringIdx", row.NameStringIdx);
			yield return Value("SignatureBlobIdx", row.SignatureBlobIdx);
			yield return Value("ClassTableIdx", row.Class);

			//FieldSignature signature = new FieldSignature(metadata, row.SignatureBlobIdx);
			//yield return TokenValue("Signature Token", signature.Token);
			//yield return Value("Signature Modifier", signature.Modifier.ToString());
			//yield return Value("Signature Type", signature.Type.ToString());
		}
	}
}
