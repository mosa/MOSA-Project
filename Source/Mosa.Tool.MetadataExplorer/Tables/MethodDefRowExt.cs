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
using System.IO;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Common;

namespace Mosa.Tool.MetadataExplorer.Tables
{
	/// <summary>
	///
	/// </summary>
	public class MethodDefRowExt : TableRow
	{
		protected MethodDefRow row;

		public MethodDefRowExt(IMetadataModule metadataModule, MethodDefRow row)
			: base(metadataModule)
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
			//yield return Value("Signature Generic Parameters", signature.GenericParameterCount.ToString());

			var code = MetadataModule.GetInstructionStream((long)row.Rva);
			var codeReader = new EndianAwareBinaryReader(code, Endianness.Little);
			var header = new MethodHeader(codeReader);

			if (header.LocalVarSigTok.RID != 0)
			{
				StandAloneSigRow standAlongSigRow = Metadata.ReadStandAloneSigRow(header.LocalVarSigTok);
				var local = new LocalVariableSignature(Metadata, standAlongSigRow.SignatureBlob);
				yield return Value("Method Header", local.ToString());
			}
		}
	}
}