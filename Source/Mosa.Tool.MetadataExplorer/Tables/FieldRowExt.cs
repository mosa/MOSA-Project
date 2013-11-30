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
			yield return Value("Flags", row.Flags.ToString());

			FieldSignature signature = new FieldSignature(Metadata, row.Signature);
			yield return Value("Signature", signature.ToString());
		}
	}
}