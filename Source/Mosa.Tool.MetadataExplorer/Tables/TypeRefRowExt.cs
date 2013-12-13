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
	public class TypeRefRowExt : TableRow
	{
		protected TypeRefRow row;

		public TypeRefRowExt(IMetadataModule metadataModule, TypeRefRow row)
			: base(metadataModule)
		{
			this.row = row;
		}

		public override string Name { get { return Metadata.ReadString(row.TypeName); } }

		public override IEnumerable GetValues()
		{
			yield return TokenString("Name", row.TypeName);
			yield return TokenString("Namespace", row.TypeNamespace);
			yield return Value("ResolutionScope #", row.ResolutionScope);
		}
	}
}