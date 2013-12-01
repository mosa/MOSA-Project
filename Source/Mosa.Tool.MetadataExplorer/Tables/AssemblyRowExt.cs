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
	public class AssemblyRowExt : TableRow
	{
		protected AssemblyRow row;

		public AssemblyRowExt(IMetadataModule metadataModule, AssemblyRow row)
			: base(metadataModule)
		{
			this.row = row;
		}

		public override string Name { get { return Metadata.ReadString(row.Name); } }

		public override IEnumerable GetValues()
		{
			yield return TokenString("Name", row.Name);
			yield return Value("Flags", row.Flags.ToString());
			yield return Value("BuildNumber", row.BuildNumber);
			yield return Value("MajorVersion", row.MajorVersion);
			yield return Value("MinorVersion", row.MinorVersion);
			yield return Value("HashAlgId", row.HashAlgId.ToString());
			yield return Value("Revision", row.Revision);
			yield return Value("Culture #", row.Culture);
			yield return Value("PublicKey #", row.PublicKey);
		}
	}
}