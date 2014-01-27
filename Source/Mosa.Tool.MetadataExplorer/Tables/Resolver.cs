/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Loader;

namespace Mosa.Tool.MetadataExplorer.Tables
{
	public static class Resolver
	{
		public static TableRow GetTableRow(IMetadataModule metadataModule, Token token)
		{
			switch (token.Table)
			{
				case TableType.File: return new FileRowExt(metadataModule, metadataModule.Metadata.ReadFileRow(token));
				case TableType.TypeDef: return new TypeDefRowExt(metadataModule, metadataModule.Metadata.ReadTypeDefRow(token));
				case TableType.TypeSpec: return new TypeSpecRowExt(metadataModule, metadataModule.Metadata.ReadTypeSpecRow(token));
				case TableType.TypeRef: return new TypeRefRowExt(metadataModule, metadataModule.Metadata.ReadTypeRefRow(token));
				case TableType.Field: return new FieldRowExt(metadataModule, metadataModule.Metadata.ReadFieldRow(token));
				case TableType.MethodDef: return new MethodDefRowExt(metadataModule, metadataModule.Metadata.ReadMethodDefRow(token));
				case TableType.ImplMap: return new ImplMapRowExt(metadataModule, metadataModule.Metadata.ReadImplMapRow(token));
				case TableType.MemberRef: return new MemberRefRowExt(metadataModule, metadataModule.Metadata.ReadMemberRefRow(token));
				case TableType.InterfaceImpl: return new InterfaceImplRowExt(metadataModule, metadataModule.Metadata.ReadInterfaceImplRow(token));
				case TableType.CustomAttribute: return new CustomAttributeRowExt(metadataModule, metadataModule.Metadata.ReadCustomAttributeRow(token));
				case TableType.Assembly: return new AssemblyRowExt(metadataModule, metadataModule.Metadata.ReadAssemblyRow(token));
				case TableType.AssemblyRef: return new AssemblyRefRowExt(metadataModule, metadataModule.Metadata.ReadAssemblyRefRow(token));
				case TableType.GenericParam: return new GenericParamRowExt(metadataModule, metadataModule.Metadata.ReadGenericParamRow(token));
				case TableType.Param: return new ParamRowExt(metadataModule, metadataModule.Metadata.ReadParamRow(token));
				case TableType.StandAloneSig: return new StandAloneSigExt(metadataModule, metadataModule.Metadata.ReadStandAloneSigRow(token));
				case TableType.MethodSpec: return new MethodSpecExt(metadataModule, metadataModule.Metadata.ReadMethodSpecRow(token));
				case TableType.NestedClass: return new NestedClassExt(metadataModule, metadataModule.Metadata.ReadNestedClassRow(token));
				default: return null;
			}
		}
	}
}