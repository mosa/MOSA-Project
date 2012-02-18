/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


using Mosa.Compiler.Metadata;

namespace Mosa.Tool.MetadataExplorer.Tables
{

	public static class Resolver
	{
		public static TableRow GetTableRow(IMetadataProvider metadata, Token token)
		{

			switch (token.Table)
			{
				case TableType.File: return new FileRowExt(metadata, metadata.ReadFileRow(token));
				case TableType.TypeDef: return new TypeDefRowExt(metadata, metadata.ReadTypeDefRow(token));
				case TableType.TypeSpec: return new TypeSpecRowExt(metadata, metadata.ReadTypeSpecRow(token));
				case TableType.TypeRef: return new TypeRefRowExt(metadata, metadata.ReadTypeRefRow(token));
				case TableType.Field: return new FieldRowExt(metadata, metadata.ReadFieldRow(token));
				case TableType.MethodDef: return new MethodDefRowExt(metadata, metadata.ReadMethodDefRow(token));
				case TableType.ImplMap: return new ImplMapRowExt(metadata, metadata.ReadImplMapRow(token));
				case TableType.MemberRef: return new MemberRefRowExt(metadata, metadata.ReadMemberRefRow(token));
				case TableType.InterfaceImpl: return new InterfaceImplRowExt(metadata, metadata.ReadInterfaceImplRow(token));
				case TableType.CustomAttribute: return new CustomAttributeRowExt(metadata, metadata.ReadCustomAttributeRow(token));
				case TableType.Assembly: return new AssemblyRowExt(metadata, metadata.ReadAssemblyRow(token));
				case TableType.AssemblyRef: return new AssemblyRefRowExt(metadata, metadata.ReadAssemblyRefRow(token));
				case TableType.GenericParam: return new GenericParamRowExt(metadata, metadata.ReadGenericParamRow(token));
				case TableType.Param: return new ParamRowExt(metadata, metadata.ReadParamRow(token));

				default: return null;
			}
		}
	}
}
