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

namespace Mosa.Tools.MetadataExplorer.Tables
{

	/// <summary>
	/// 
	/// </summary>
	public class ParamRowExt : TableRow
	{
		protected ParamRow row;

		public ParamRowExt(IMetadataProvider metadata, ParamRow row)
			: base(metadata)
		{
			this.row = row;
		}

		public override string Name { get { return Metadata.ReadString(row.NameIdx); } }

		public override IEnumerable GetValues()
		{
			yield return TokenString("Name", row.NameIdx);
			yield return Value("NameIdx", row.NameIdx);
			yield return Value("NameIdx", row.Flags.ToString());
			yield return Value("Sequence", row.Sequence.ToString());
		}
	}
}
