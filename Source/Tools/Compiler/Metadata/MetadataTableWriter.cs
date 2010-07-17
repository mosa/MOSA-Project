/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Tools.Compiler.Metadata
{
	/// <summary>
	/// Holds the table writers for the MetadataBuilderStage.
	/// </summary>
	public sealed partial class MetadataBuilderStage
	{

		#region IPipelineStage members

		string IPipelineStage.Name { get { return @"MetadataBuilderStage"; } }

		#endregion // IPipelineStage members

		/// <summary>
		/// Holds the token types of all tables supported.
		/// </summary>
		private static readonly TokenTypes[] MetadataTableTokens = new[]
        {
            TokenTypes.Module,
            TokenTypes.TypeRef,
            TokenTypes.TypeDef,
            TokenTypes.Field,
            TokenTypes.MethodDef,
            TokenTypes.Param,
            TokenTypes.InterfaceImpl,
            TokenTypes.MemberRef,
            TokenTypes.Constant,
            TokenTypes.CustomAttribute,
            TokenTypes.FieldMarshal,
            TokenTypes.DeclSecurity,
            TokenTypes.ClassLayout,
            TokenTypes.FieldLayout,
            TokenTypes.StandAloneSig,
            TokenTypes.EventMap,
            TokenTypes.Event,
            TokenTypes.PropertyMap,
            TokenTypes.Property,
            TokenTypes.MethodSemantics,
            TokenTypes.MethodImpl,
            TokenTypes.ModuleRef,
            TokenTypes.TypeSpec,
            TokenTypes.ImplMap,
            TokenTypes.FieldRVA,
            TokenTypes.Assembly,
            TokenTypes.AssemblyProcessor,
            TokenTypes.AssemblyOS,
            TokenTypes.AssemblyRef,
            TokenTypes.AssemblyRefProcessor,
            TokenTypes.AssemblyRefOS,
            TokenTypes.File,
            TokenTypes.ExportedType,
            TokenTypes.ManifestResource,
            TokenTypes.NestedClass,
            TokenTypes.GenericParam,
            TokenTypes.MethodSpec,
            TokenTypes.GenericParamConstraint
        };

		/// <summary>
		/// Holds all metadata table handlers in order of execution.
		/// </summary>
		private static readonly Action<IMetadataProvider, MetadataBuilderStage>[] MetadataTableHandlers = new Action<IMetadataProvider, MetadataBuilderStage>[]
        {
            WriteModuleTable,                   // 0x00
            WriteTypeRefTable,                  // 0x01
            WriteTypeDefTable,                  // 0x02
            WriteFieldTable,                    // 0x04
            WriteMethodDefTable,                // 0x06
            WriteParamTable,                    // 0x08
            WriteInterfaceImplTable,            // 0x09
            WriteMemberRefTable,                // 0x0A
            WriteConstantTable,                 // 0x0B
            WriteCustomAttributeTable,          // 0x0C
            WriteFieldMarshalTable,             // 0x0D
            WriteDeclSecurityTable,             // 0x0E
            WriteClassLayoutTable,              // 0x0F
            WriteFieldLayoutTable,              // 0x10
            WriteStandaloneSigTable,            // 0x11
            WriteEventMapTable,                 // 0x12
            WriteEventTable,                    // 0x14
            WritePropertyMapTable,              // 0x15
            WritePropertyTable,                 // 0x17
            WriteMethodSemanticsTable,          // 0x18
            WriteMethodImplTable,               // 0x19
            WriteModuleRefTable,                // 0x1A
            WriteTypeSpecTable,                 // 0x1B
            WriteImplMapTable,                  // 0x1C
            WriteFieldRVATable,                 // 0x1D
            WriteAssemblyTable,                 // 0x20
            WriteAssemblyProcessorTable,        // 0x21
            WriteAssemblyOSTable,               // 0x22
            WriteAssemblyRefTable,              // 0x23
            WriteAssemblyRefProcessorTable,     // 0x24
            WriteAssemblyRefOSTable,            // 0x25
            WriteFileTable,                     // 0x26
            WriteExportedTypeTable,             // 0x27
            WriteManifestResourceTable,         // 0x28
            WriteNestedClassTable,              // 0x29
            WriteGenericParamTable,             // 0x2A
            WriteMethodSpecTable,               // 0x2B
            WriteGenericParamConstraintTable,   // 0x2C
        };

		/// <summary>
		/// Writes an entire metadata table.
		/// </summary>
		/// <param name="table">The token of the table to write.</param>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="writer">The writer lambda.</param>
		private static void WriteTable(TokenTypes table, IMetadataProvider metadataSource, Action<TokenTypes> writer)
		{
			TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
			for (TokenTypes token = table + 1; token <= lastToken; token++)
			{
				writer(token);
			}
		}

		/// <summary>
		/// Writes the module table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteModuleTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.Module, metadataSource, token =>
			{
				ModuleRow row = metadataSource.ReadModuleRow(token);
				metadataWriter.Write(row.Generation);
				metadataWriter.Write(row.NameStringIdx);
				metadataWriter.Write(row.MvidGuidIdx);
				metadataWriter.Write(row.EncIdGuidIdx);
				metadataWriter.Write(row.EncBaseIdGuidIdx);
			});
		}

		/// <summary>
		/// Writes the typeref table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteTypeRefTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.TypeRef, metadataSource, token =>
			{
				TypeRefRow row = metadataSource.ReadTypeRefRow(token);

				metadataWriter.WriteResolutionScopeIndex(row.ResolutionScopeIdx);
				metadataWriter.Write(row.TypeNameIdx);
				metadataWriter.Write(row.TypeNamespaceIdx);
			});
		}

		/// <summary>
		/// Writes the typedef table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteTypeDefTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.TypeDef, metadataSource, token =>
			{
				TypeDefRow row = metadataSource.ReadTypeDefRow(token);

				metadataWriter.Write((uint)row.Flags);
				metadataWriter.Write(row.TypeNameIdx);
				metadataWriter.Write(row.TypeNamespaceIdx);
				metadataWriter.WriteTypeDefOrRefIndex(row.Extends);
				metadataWriter.Write(row.FieldList);
				metadataWriter.Write(row.MethodList);
			});
		}

		/// <summary>
		/// Writes the field table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteFieldTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.Field, metadataSource, token =>
			{
				FieldRow row = metadataSource.ReadFieldRow(token);

				metadataWriter.Write((ushort)row.Flags);
				metadataWriter.Write(row.NameStringIdx);
				metadataWriter.Write(row.SignatureBlobIdx);
			});
		}

		/// <summary>
		/// Writes the methoddef table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteMethodDefTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.MethodDef, metadataSource, token =>
			{
				MethodDefRow row = metadataSource.ReadMethodDefRow(token);

				metadataWriter.Write((uint)row.Rva);
				metadataWriter.Write((ushort)row.ImplFlags);
				metadataWriter.Write((ushort)row.Flags);
				metadataWriter.Write(row.NameStringIdx);
				metadataWriter.Write(row.SignatureBlobIdx);
				metadataWriter.Write(row.ParamList);
			});
		}

		/// <summary>
		/// Writes the param table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteParamTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.Param, metadataSource, token =>
			{
				ParamRow row = metadataSource.ReadParamRow(token);

				metadataWriter.Write((ushort)row.Flags);
				metadataWriter.Write((ushort)row.Sequence);
				metadataWriter.Write(row.NameIdx);
			});
		}

		/// <summary>
		/// Writes the interfaceimpl table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteInterfaceImplTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.InterfaceImpl, metadataSource, token =>
			{
				InterfaceImplRow row = metadataSource.ReadInterfaceImplRow(token);

				metadataWriter.Write(row.ClassTableIdx);
				metadataWriter.WriteTypeDefOrRefIndex(row.InterfaceTableIdx);
			});
		}

		/// <summary>
		/// Writes the member ref table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteMemberRefTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.MemberRef, metadataSource, token =>
			{
				MemberRefRow row = metadataSource.ReadMemberRefRow(token);

				metadataWriter.WriteMemberRefParentIndex(row.ClassTableIdx);
				metadataWriter.Write(row.NameStringIdx);
				metadataWriter.Write(row.SignatureBlobIdx);
			});
		}

		/// <summary>
		/// Writes the constant table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteConstantTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.Constant, metadataSource, token =>
			{
				ConstantRow row = metadataSource.ReadConstantRow(token);

				metadataWriter.Write((byte)row.Type);
				metadataWriter.Write((byte)0);
				metadataWriter.WriteHasConstantIndex(row.Parent);
				metadataWriter.Write(row.ValueBlobIdx);
			});
		}

		/// <summary>
		/// Writes the custom attribute table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteCustomAttributeTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.CustomAttribute, metadataSource, token =>
			{
				CustomAttributeRow row = metadataSource.ReadCustomAttributeRow(token);

				metadataWriter.WriteHasCustomAttributeIndex(row.ParentTableIdx);
				metadataWriter.WriteCustomAttributeTypeIndex(row.TypeIdx);
				metadataWriter.Write(row.ValueBlobIdx);
			});
		}

		/// <summary>
		/// Writes the field marshal table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteFieldMarshalTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.FieldMarshal, metadataSource, token =>
			{
				FieldMarshalRow row = metadataSource.ReadFieldMarshalRow(token);

				metadataWriter.WriteHasFieldMarshalIndex(row.ParentTableIdx);
				metadataWriter.Write(row.NativeTypeBlobIdx);
			});
		}

		/// <summary>
		/// Writes the decl security table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteDeclSecurityTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.DeclSecurity, metadataSource, token =>
			{
				DeclSecurityRow row = metadataSource.ReadDeclSecurityRow(token);

				metadataWriter.Write((ushort)row.Action);
				metadataWriter.WriteHasDeclSecurityIndex(row.ParentTableIdx);
				metadataWriter.Write(row.PermissionSetBlobIdx);
			});
		}

		/// <summary>
		/// Writes the class layout table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteClassLayoutTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.ClassLayout, metadataSource, token =>
			{
				ClassLayoutRow row = metadataSource.ReadClassLayoutRow(token);

				metadataWriter.Write((ushort)row.PackingSize);
				metadataWriter.Write((uint)row.ClassSize);
				metadataWriter.Write(row.ParentTypeDefIdx);
			});
		}

		/// <summary>
		/// Writes the field layout table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteFieldLayoutTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.FieldLayout, metadataSource, token =>
			{
				FieldLayoutRow row = metadataSource.ReadFieldLayoutRow(token);

				metadataWriter.Write(row.Offset);
				metadataWriter.Write(row.Field);
			});
		}

		/// <summary>
		/// Writes the standalone sig table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteStandaloneSigTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.StandAloneSig, metadataSource, token =>
			{
				StandAloneSigRow row = metadataSource.ReadStandAloneSigRow(token);

				metadataWriter.Write(row.SignatureBlobIdx);
			});
		}

		/// <summary>
		/// Writes the event map table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteEventMapTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.EventMap, metadataSource, token =>
			{
				EventMapRow row = metadataSource.ReadEventMapRow(token);

				metadataWriter.Write(row.TypeDefTableIdx);
				metadataWriter.Write(row.EventListTableIdx);
			});
		}

		/// <summary>
		/// Writes the event table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteEventTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.Event, metadataSource, token =>
			{
				EventRow row = metadataSource.ReadEventRow(token);

				metadataWriter.Write((ushort)row.Flags);
				metadataWriter.Write(row.NameStringIdx);
				metadataWriter.WriteTypeDefOrRefIndex(row.EventTypeTableIdx);
			});
		}

		/// <summary>
		/// Writes the property map table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WritePropertyMapTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.PropertyMap, metadataSource, token =>
			{
				PropertyMapRow row = metadataSource.ReadPropertyMapRow(token);

				metadataWriter.Write(row.ParentTableIdx);
				metadataWriter.Write(row.PropertyTableIdx);
			});
		}

		/// <summary>
		/// Writes the property table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WritePropertyTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.Property, metadataSource, token =>
			{
				PropertyRow row = metadataSource.ReadPropertyRow(token);

				metadataWriter.Write((ushort)row.Flags);
				metadataWriter.Write(row.NameStringIdx);
				metadataWriter.Write(row.TypeBlobIdx);
			});
		}

		/// <summary>
		/// Writes the method semantics table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteMethodSemanticsTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.MethodSemantics, metadataSource, token =>
			{
				MethodSemanticsRow row = metadataSource.ReadMethodSemanticsRow(token);

				metadataWriter.Write((ushort)row.Semantics);
				metadataWriter.Write(row.MethodTableIdx);
				metadataWriter.WriteHasSemanticsIndex(row.AssociationTableIdx);
			});
		}

		/// <summary>
		/// Writes the method impl table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteMethodImplTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.MethodImpl, metadataSource, token =>
			{
				MethodImplRow row = metadataSource.ReadMethodImplRow(token);

				metadataWriter.Write(row.ClassTableIdx);
				metadataWriter.WriteMethodDefOrRefIndex(row.MethodBodyTableIdx);
				metadataWriter.WriteMethodDefOrRefIndex(row.MethodDeclarationTableIdx);
			});
		}

		/// <summary>
		/// Writes the module ref table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteModuleRefTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.ModuleRef, metadataSource, token =>
			{
				ModuleRefRow row = metadataSource.ReadModuleRefRow(token);

				metadataWriter.Write(row.NameStringIdx);
			});
		}

		/// <summary>
		/// Writes the type spec table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteTypeSpecTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.TypeSpec, metadataSource, token =>
			{
				TypeSpecRow row = metadataSource.ReadTypeSpecRow(token);

				metadataWriter.Write(row.SignatureBlobIdx);
			});
		}

		/// <summary>
		/// Writes the impl map table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteImplMapTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.ImplMap, metadataSource, token =>
			{
				ImplMapRow row = metadataSource.ReadImplMapRow(token);

				metadataWriter.Write((ushort)row.MappingFlags);
				metadataWriter.WriteMemberForwardedIndex(row.MemberForwardedTableIdx);
				metadataWriter.Write(row.ImportNameStringIdx);
				metadataWriter.Write(row.ImportScopeTableIdx);
			});
		}

		/// <summary>
		/// Writes the field RVA table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteFieldRVATable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.FieldRVA, metadataSource, token =>
			{
				FieldRVARow row = metadataSource.ReadFieldRVARow(token);

				metadataWriter.Write(row.Rva);
				metadataWriter.Write(row.FieldTableIdx);
			});
		}

		/// <summary>
		/// Writes the assembly metadata table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteAssemblyTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.Assembly, metadataSource, token =>
			{
				AssemblyRow row = metadataSource.ReadAssemblyRow(token);

				metadataWriter.Write((uint)row.HashAlgId);
				metadataWriter.Write((ushort)row.MajorVersion);
				metadataWriter.Write((ushort)row.MinorVersion);
				metadataWriter.Write((ushort)row.BuildNumber);
				metadataWriter.Write((ushort)row.Revision);
				metadataWriter.Write((uint)row.Flags);
				metadataWriter.Write(row.PublicKeyIdx);
				metadataWriter.Write(row.NameIdx);
				metadataWriter.Write(row.CultureIdx);
			});
		}

		/// <summary>
		/// Writes the assembly processor table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteAssemblyProcessorTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.AssemblyProcessor, metadataSource, token =>
			{
				AssemblyProcessorRow row = metadataSource.ReadAssemblyProcessorRow(token);

				metadataWriter.Write(row.Processor);
			});
		}

		/// <summary>
		/// Writes the assembly OS table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteAssemblyOSTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.AssemblyOS, metadataSource, token =>
			{
				AssemblyOSRow row = metadataSource.ReadAssemblyOSRow(token);

				metadataWriter.Write((uint)row.PlatformId);
				metadataWriter.Write((uint)row.MajorVersion);
				metadataWriter.Write((uint)row.MinorVersion);
			});
		}

		/// <summary>
		/// Writes the assembly ref table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteAssemblyRefTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.AssemblyRef, metadataSource, token =>
			{
				AssemblyRefRow row = metadataSource.ReadAssemblyRefRow(token);

				metadataWriter.Write((ushort)row.MajorVersion);
				metadataWriter.Write((ushort)row.MinorVersion);
				metadataWriter.Write((ushort)row.BuildNumber);
				metadataWriter.Write((ushort)row.Revision);

				metadataWriter.Write((uint)row.Flags);
				metadataWriter.Write(row.PublicKeyOrTokenIdx);
				metadataWriter.Write(row.NameIdx);
				metadataWriter.Write(row.CultureIdx);
				metadataWriter.Write(row.HashValueIdx);
			});
		}

		/// <summary>
		/// Writes the assembly ref processor table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteAssemblyRefProcessorTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.AssemblyRefProcessor, metadataSource, token =>
			{
				AssemblyRefProcessorRow row = metadataSource.ReadAssemblyRefProcessorRow(token);

				metadataWriter.Write(row.Processor);
				metadataWriter.Write(row.AssemblyRef);
			});
		}

		/// <summary>
		/// Writes the assembly ref OS table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteAssemblyRefOSTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.AssemblyRefOS, metadataSource, token =>
			{
				AssemblyRefOSRow row = metadataSource.ReadAssemblyRefOSRow(token);

				metadataWriter.Write(row.PlatformId);
				metadataWriter.Write(row.MajorVersion);
				metadataWriter.Write(row.MinorVersion);
				metadataWriter.Write(row.AssemblyRefIdx);
			});
		}

		/// <summary>
		/// Writes the file table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteFileTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.File, metadataSource, token =>
			{
				FileRow row = metadataSource.ReadFileRow(token);

				metadataWriter.Write((uint)row.Flags);
				metadataWriter.Write(row.NameStringIdx);
				metadataWriter.Write(row.HashValueBlobIdx);
			});
		}

		/// <summary>
		/// Writes the exported type table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteExportedTypeTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.ExportedType, metadataSource, token =>
			{
				ExportedTypeRow row = metadataSource.ReadExportedTypeRow(token);

				metadataWriter.Write((uint)row.Flags);
				metadataWriter.Write(row.TypeDefTableIdx);
				metadataWriter.Write(row.TypeNameStringIdx);
				metadataWriter.Write(row.TypeNamespaceStringIdx);
				metadataWriter.WriteImplementationIndex(row.ImplementationTableIdx);
			});
		}

		/// <summary>
		/// Writes the manifest resource table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteManifestResourceTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.ManifestResource, metadataSource, token =>
			{
				ManifestResourceRow row = metadataSource.ReadManifestResourceRow(token);

				metadataWriter.Write(row.Offset);
				metadataWriter.Write((uint)row.Flags);
				metadataWriter.Write(row.NameStringIdx);
				metadataWriter.WriteImplementationIndex(row.ImplementationTableIdx);
			});
		}

		/// <summary>
		/// Writes the nested class table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteNestedClassTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.NestedClass, metadataSource, token =>
			{
				NestedClassRow row = metadataSource.ReadNestedClassRow(token);

				metadataWriter.Write(row.NestedClassTableIdx);
				metadataWriter.Write(row.EnclosingClassTableIdx);
			});
		}

		/// <summary>
		/// Writes the generic param table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteGenericParamTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.GenericParam, metadataSource, token =>
			{
				GenericParamRow row = metadataSource.ReadGenericParamRow(token);

				metadataWriter.Write((ushort)row.Number);
				metadataWriter.Write((ushort)row.Flags);
				metadataWriter.WriteTypeOrMethodDefIndex(row.OwnerTableIdx);
				metadataWriter.Write(row.NameStringIdx);
			});
		}

		/// <summary>
		/// Writes the method spec table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteMethodSpecTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.MethodSpec, metadataSource, token =>
			{
				MethodSpecRow row = metadataSource.ReadMethodSpecRow(token);

				metadataWriter.WriteMethodDefOrRefIndex(row.MethodTableIdx);
				metadataWriter.Write(row.InstantiationBlobIdx);
			});
		}

		/// <summary>
		/// Writes the generic param constraint table.
		/// </summary>
		/// <param name="metadataSource">The metadata source.</param>
		/// <param name="metadataWriter">The metadata writer.</param>
		private static void WriteGenericParamConstraintTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
		{
			WriteTable(TokenTypes.GenericParamConstraint, metadataSource, token =>
			{
				GenericParamConstraintRow row = metadataSource.ReadGenericParamConstraintRow(token);

				metadataWriter.Write(row.OwnerTableIdx);
				metadataWriter.WriteTypeDefOrRefIndex(row.ConstraintTableIdx);
			});
		}
	}
}
