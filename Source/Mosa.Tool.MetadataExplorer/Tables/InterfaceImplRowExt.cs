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
	public class InterfaceImplRowExt : TableRow
	{
		protected InterfaceImplRow row;

		public InterfaceImplRowExt(IMetadataProvider metadata, InterfaceImplRow row)
			: base(metadata)
		{
			this.row = row;
		}

		public override string Name { get { return string.Empty; } }

		public override IEnumerable GetValues()
		{
			yield return Value("ClassTable #", row.Class);
			yield return Value("InterfaceTable #", row.Interface);
		}
	}
}