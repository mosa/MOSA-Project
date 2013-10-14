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
using Mosa.Compiler.Metadata.Tables;

namespace Mosa.Tool.MetadataExplorer.Tables
{
	/// <summary>
	///
	/// </summary>
	public class GenericParamRowExt : TableRow
	{
		protected GenericParamRow row;

		public GenericParamRowExt(IMetadataProvider metadata, GenericParamRow row)
			: base(metadata)
		{
			this.row = row;
		}

		public override string Name { get { return Metadata.ReadString(row.NameStringIdx); } }

		public override IEnumerable GetValues()
		{
			yield return TokenString("Name", row.NameStringIdx);
			yield return Value("NameStringIdx", row.NameStringIdx);
			yield return Value("OwnerTableIdx", row.Owner);
			yield return Value("Flags", row.Flags.ToString());
			yield return Value("Number", row.Number);
		}
	}
}