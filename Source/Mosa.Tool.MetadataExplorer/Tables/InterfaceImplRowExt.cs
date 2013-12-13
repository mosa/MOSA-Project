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
	public class InterfaceImplRowExt : TableRow
	{
		protected InterfaceImplRow row;

		public InterfaceImplRowExt(IMetadataModule metadataModule, InterfaceImplRow row)
			: base(metadataModule)
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