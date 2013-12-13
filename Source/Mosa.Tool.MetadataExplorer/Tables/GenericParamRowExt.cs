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
using Mosa.Compiler.Metadata.Loader;

namespace Mosa.Tool.MetadataExplorer.Tables
{
	/// <summary>
	///
	/// </summary>
	public class GenericParamRowExt : TableRow
	{
		protected GenericParamRow row;

		public GenericParamRowExt(IMetadataModule metadataModule, GenericParamRow row)
			: base(metadataModule)
		{
			this.row = row;
		}

		public override string Name { get { return Metadata.ReadString(row.NameString); } }

		public override IEnumerable GetValues()
		{
			yield return TokenString("Name", row.NameString);
			yield return Value("OwnerTable #", row.Owner);
			yield return Value("Flags", row.Flags.ToString());
			yield return Value("Number", row.Number);
		}
	}
}