/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.Metadata.Tables;
using System.Collections;

namespace Mosa.Tool.MetadataExplorer.Tables
{
	/// <summary>
	///
	/// </summary>
	public class MethodDefRowExt : TableRow
	{
		protected MethodDefRow row;

		public MethodDefRowExt(IMetadataProvider metadata, MethodDefRow row)
			: base(metadata)
		{
			this.row = row;
		}

		public override string Name { get { return Metadata.ReadString(row.NameString); } }

		public override IEnumerable GetValues()
		{
			yield return TokenString("Name", row.NameString);
			yield return Value("Flags", row.Flags.ToString());
			yield return Value("ImplFlags", row.ImplFlags.ToString());
			yield return Value("ParamList", row.ParamList);
			yield return Value("Rva", row.Rva.ToString());

			MethodSignature signature = new MethodSignature(Metadata, row.SignatureBlob);
			yield return Value("Signature", signature.ToString());
			yield return Value("Signature Generic Parameters", signature.GenericParameterCount.ToString());
		}
	}
}