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
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.Metadata.Tables;

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

		public override string Name { get { return Metadata.ReadString(row.NameStringIdx); } }

		public override IEnumerable GetValues()
		{
			yield return TokenString("Name", row.NameStringIdx);
			yield return Value("NameStringIdx", row.NameStringIdx);
			yield return Value("Flags", row.Flags.ToString());
			yield return Value("ImplFlags", row.ImplFlags.ToString());
			yield return Value("ParamList", row.ParamList);
			yield return Value("Rva", row.Rva.ToString());
			yield return Value("SignatureBlobIdx", row.SignatureBlobIdx);

			MethodSignature signature = new MethodSignature(Metadata, row.SignatureBlobIdx);
			//yield return Value("Signature Token", signature.Token);
			yield return Value("Signature Generic Parameters", signature.GenericParameterCount.ToString());
		}
	}
}
