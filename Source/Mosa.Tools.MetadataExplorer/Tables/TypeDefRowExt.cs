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
	public class TypeDefRowExt : TableRow
	{
		protected TypeDefRow row;

		public TypeDefRowExt(IMetadataProvider metadata, TypeDefRow row)
			: base(metadata)
		{
			this.row = row;
		}

		public override string Name { get { return Metadata.ReadString(row.TypeName); } }

		public override IEnumerable GetValues()
		{
			yield return TokenString("Name", row.TypeName);
			yield return TokenString("Namespace", row.TypeNamespace);
			yield return Value("TypeNameIdx", row.TypeName);
			yield return Value("TypeNamespaceIdx", row.TypeNamespace);
			yield return Value("Extends", row.Extends);
			yield return Value("FieldList", row.FieldList);
			yield return Value("Flags", row.Flags.ToString());
			yield return Value("MethodList", row.MethodList);
		}
	}
}
