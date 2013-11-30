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

namespace Mosa.Tool.MetadataExplorer.Tables
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

		public override string Name { get { return Metadata.ReadString(row.NameString); } }

		public override IEnumerable GetValues()
		{
			yield return TokenString("Name", row.NameString);
			yield return Value("ClassTable #", row.Class);

			var signature = Signature.GetSignatureFromMemberRef(Metadata, row.SignatureBlob);
			yield return Value("Signature", signature.ToString());
		}
	}
}