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

namespace Mosa.Tool.MetadataExplorer.Tables
{
	/// <summary>
	///
	/// </summary>
	public class AssemblyRowExt : TableRow
	{
		protected AssemblyRow row;

		public AssemblyRowExt(IMetadataProvider metadata, AssemblyRow row)
			: base(metadata)
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
			yield return Value("CultureIdx", row.Culture);
			yield return Value("PublicKeyIdx", row.PublicKey);
		}
	}
}