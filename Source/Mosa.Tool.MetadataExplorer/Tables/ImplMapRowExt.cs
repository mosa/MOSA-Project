/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Tables;
using System.Collections;

namespace Mosa.Tool.MetadataExplorer.Tables
{
	/// <summary>
	///
	/// </summary>
	public class ImplMapRowExt : TableRow
	{
		protected ImplMapRow row;

		public ImplMapRowExt(IMetadataProvider metadata, ImplMapRow row)
			: base(metadata)
		{
			this.row = row;
		}

		public override string Name { get { return Metadata.ReadString(row.ImportNameString); } }

		public override IEnumerable GetValues()
		{
			yield return TokenString("ImportName", row.ImportNameString);
			yield return Value("MappingFlags", row.MappingFlags.ToString());

			yield return Value("ImportScopeTable #", row.ImportScopeTable);
			yield return Value("MemberForwardedTable #", row.MemberForwarded);
		}
	}
}