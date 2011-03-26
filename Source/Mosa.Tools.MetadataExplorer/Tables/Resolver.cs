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
				case TableTypes.File: return new FileRowExt(metadata, metadata.ReadFileRow((TokenTypes)token.ToInt32()));
				case TableTypes.TypeDef: return new TypeDefRowExt(metadata, metadata.ReadTypeDefRow((TokenTypes)token.ToInt32()));
				case TableTypes.TypeSpec: return new TypeSpecRowExt(metadata, metadata.ReadTypeSpecRow((TokenTypes)token.ToInt32()));
				case TableTypes.TypeRef: return new TypeRefRowExt(metadata, metadata.ReadTypeRefRow((TokenTypes)token.ToInt32()));
				case TableTypes.Field: return new FieldRowExt(metadata, metadata.ReadFieldRow((TokenTypes)token.ToInt32()));
				case TableTypes.MethodDef: return new MethodDefRowExt(metadata, metadata.ReadMethodDefRow((TokenTypes)token.ToInt32()));
				case TableTypes.ImplMap: return new ImplMapRowExt(metadata, metadata.ReadImplMapRow((TokenTypes)token.ToInt32()));
				case TableTypes.MemberRef: return new MemberRefRowExt(metadata, metadata.ReadMemberRefRow((TokenTypes)token.ToInt32()));
				case TableTypes.InterfaceImpl: return new InterfaceImplRowExt(metadata, metadata.ReadInterfaceImplRow((TokenTypes)token.ToInt32()));
				case TableTypes.CustomAttribute: return new CustomAttributeRowExt(metadata, metadata.ReadCustomAttributeRow((TokenTypes)token.ToInt32()));
				case TableTypes.Assembly: return new AssemblyRowExt(metadata, metadata.ReadAssemblyRow((TokenTypes)token.ToInt32()));
				case TableTypes.AssemblyRef: return new AssemblyRefRowExt(metadata, metadata.ReadAssemblyRefRow((TokenTypes)token.ToInt32()));
				case TableTypes.GenericParam: return new GenericParamRowExt(metadata, metadata.ReadGenericParamRow((TokenTypes)token.ToInt32()));
				case TableTypes.Param: return new ParamRowExt(metadata, metadata.ReadParamRow((TokenTypes)token.ToInt32()));

				default: return null;
			}
		}
	}
}
