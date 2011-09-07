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
	public class CustomAttributeRowExt : TableRow
	{
		protected CustomAttributeRow row;

		public CustomAttributeRowExt(IMetadataProvider metadata, CustomAttributeRow row)
			: base(metadata)
		{
			this.row = row;
		}

		public override string Name { get { return string.Empty; } }

		public override IEnumerable GetValues()
		{
			yield return Value("ParentTableIdx", row.Parent);
			yield return Value("TypeIdx", row.Type);
			yield return Value("ValueBlobIdx", row.Value);
		}
	}
}
