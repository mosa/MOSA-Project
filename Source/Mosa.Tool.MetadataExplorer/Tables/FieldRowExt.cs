/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.Metadata.Tables;

namespace Mosa.Tool.MetadataExplorer.Tables
{
	/// <summary>
	///
	/// </summary>
	public class FieldRowExt : TableRow
	{
		protected FieldRow row;

		public FieldRowExt(IMetadataProvider metadata, FieldRow row)
			: base(metadata)
		{
			this.row = row;
		}

		public override string Name { get { return Metadata.ReadString(row.Name); } }

		public override IEnumerable GetValues()
		{
			yield return TokenString("Name", row.Name);
			yield return Value("NameStringIdx", row.Name);
			yield return Value("Flags", row.Flags.ToString());
			yield return Value("SignatureBlobIdx", row.Signature);

			FieldSignature signature = new FieldSignature(Metadata, row.Signature);

			//yield return Value("Signature Token", signature.Token);
			yield return Value("Signature Modifier", signature.Modifier.ToString());
			yield return Value("Signature Type", signature.Type.ToString());
		}
	}
}