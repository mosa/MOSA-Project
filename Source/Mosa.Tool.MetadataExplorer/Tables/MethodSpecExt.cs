/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.Metadata.Tables;
using System.Collections;

namespace Mosa.Tool.MetadataExplorer.Tables
{
	/// <summary>
	///
	/// </summary>
	public class MethodSpecExt : TableRow
	{
		protected MethodSpecRow row;

		public MethodSpecExt(IMetadataModule metadataModule, MethodSpecRow row)
			: base(metadataModule)
		{
			this.row = row;
		}

		public override string Name { get { return row.InstantiationBlob.FormatToString(); } }

		public override IEnumerable GetValues()
		{
			var signature = new MethodSpecSignature(Metadata, row.InstantiationBlob);
			yield return Value("Signature", signature.ToString());

			yield return Value("Method", row.Method);
		}
	}
}