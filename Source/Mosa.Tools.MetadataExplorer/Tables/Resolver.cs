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
		public static TableRow GetTableRow(IMetadataProvider metadata, Token token)
		{

			switch (token.Table)
			{
				case TableTypes.File: return new FileRowExt(metadata, metadata.ReadFileRow(token));
				case TableTypes.TypeDef: return new TypeDefRowExt(metadata, metadata.ReadTypeDefRow(token));
				case TableTypes.TypeSpec: return new TypeSpecRowExt(metadata, metadata.ReadTypeSpecRow(token));
				case TableTypes.TypeRef: return new TypeRefRowExt(metadata, metadata.ReadTypeRefRow(token));
				case TableTypes.Field: return new FieldRowExt(metadata, metadata.ReadFieldRow(token));
				case TableTypes.MethodDef: return new MethodDefRowExt(metadata, metadata.ReadMethodDefRow(token));
				case TableTypes.ImplMap: return new ImplMapRowExt(metadata, metadata.ReadImplMapRow(token));
				case TableTypes.MemberRef: return new MemberRefRowExt(metadata, metadata.ReadMemberRefRow(token));
				case TableTypes.InterfaceImpl: return new InterfaceImplRowExt(metadata, metadata.ReadInterfaceImplRow(token));
				case TableTypes.CustomAttribute: return new CustomAttributeRowExt(metadata, metadata.ReadCustomAttributeRow(token));
				case TableTypes.Assembly: return new AssemblyRowExt(metadata, metadata.ReadAssemblyRow(token));
				case TableTypes.AssemblyRef: return new AssemblyRefRowExt(metadata, metadata.ReadAssemblyRefRow(token));
				case TableTypes.GenericParam: return new GenericParamRowExt(metadata, metadata.ReadGenericParamRow(token));
				case TableTypes.Param: return new ParamRowExt(metadata, metadata.ReadParamRow(token));

				default: return null;
			}
		}
	}
}
