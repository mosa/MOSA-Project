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
	public class NestedClassExt : TableRow
	{
		protected NestedClassRow row;

		public NestedClassExt(IMetadataModule metadataModule, NestedClassRow row)
			: base(metadataModule)
		{
			this.row = row;
		}

		public override string Name { get { return row.NestedClass.FormatToString(); } }

		public override IEnumerable GetValues()
		{
			yield return Value("EnclosingClass", row.EnclosingClass);
			yield return Value("NestedClass", row.NestedClass);
		}
	}
}