/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;

namespace Mosa.Tools.MetadataExplorer.Tables
{

	public static class Resolver
	{
		public static TableRow GetTableRow(IMetadataProvider metadata, MetadataToken token)
		{

			switch (token.Table)
			{
				case MetadataTable.File: return new FileRowExt(metadata, metadata.ReadFileRow(token));
				case MetadataTable.TypeDef: return new TypeDefRowExt(metadata, metadata.ReadTypeDefRow(token));
				case MetadataTable.TypeSpec: return new TypeSpecRowExt(metadata, metadata.ReadTypeSpecRow(token));
				case MetadataTable.TypeRef: return new TypeRefRowExt(metadata, metadata.ReadTypeRefRow(token));
				case MetadataTable.Field: return new FieldRowExt(metadata, metadata.ReadFieldRow(token));
				case MetadataTable.MethodDef: return new MethodDefRowExt(metadata, metadata.ReadMethodDefRow(token));
				case MetadataTable.ImplMap: return new ImplMapRowExt(metadata, metadata.ReadImplMapRow(token));
				case MetadataTable.MemberRef: return new MemberRefRowExt(metadata, metadata.ReadMemberRefRow(token));
				case MetadataTable.InterfaceImpl: return new InterfaceImplRowExt(metadata, metadata.ReadInterfaceImplRow(token));
				case MetadataTable.CustomAttribute: return new CustomAttributeRowExt(metadata, metadata.ReadCustomAttributeRow(token));
				case MetadataTable.Assembly: return new AssemblyRowExt(metadata, metadata.ReadAssemblyRow(token));
				case MetadataTable.AssemblyRef: return new AssemblyRefRowExt(metadata, metadata.ReadAssemblyRefRow(token));
				case MetadataTable.GenericParam: return new GenericParamRowExt(metadata, metadata.ReadGenericParamRow(token));
				case MetadataTable.Param: return new ParamRowExt(metadata, metadata.ReadParamRow(token));

				default: return null;
			}
		}
	}
}
