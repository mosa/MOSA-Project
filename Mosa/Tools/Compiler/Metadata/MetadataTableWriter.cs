/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;

namespace Mosa.Tools.Compiler.Metadata
{
    /// <summary>
    /// Holds the table writers for the MetadataBuilderStage.
    /// </summary>
    public sealed partial class MetadataBuilderStage
    {
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
        /// Writes the module table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteModuleTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.Module;
            ModuleRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.Generation);
                metadataWriter.Write(row.NameStringIdx);
                metadataWriter.Write(row.MvidGuidIdx);
                metadataWriter.Write(row.EncIdGuidIdx);
                metadataWriter.Write(row.EncBaseIdGuidIdx);
            }
        }

        /// <summary>
        /// Writes the typeref table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteTypeRefTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.TypeRef;
            TypeRefRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.ResolutionScopeIdx);
                metadataWriter.Write(row.TypeNameIdx);
                metadataWriter.Write(row.TypeNamespaceIdx);
            }
        }

        /// <summary>
        /// Writes the typedef table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteTypeDefTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.TypeDef;
            TypeDefRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write((uint)row.Flags);
                metadataWriter.Write(row.TypeNameIdx);
                metadataWriter.Write(row.TypeNamespaceIdx);
                metadataWriter.Write(row.Extends);
                metadataWriter.Write(row.FieldList);
                metadataWriter.Write(row.MethodList);
            }
        }

        /// <summary>
        /// Writes the field table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteFieldTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.Field;
            FieldRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write((ushort)row.Flags);
                metadataWriter.Write(row.NameStringIdx);
                metadataWriter.Write(row.SignatureBlobIdx);
            }
        }

        /// <summary>
        /// Writes the methoddef table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteMethodDefTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.MethodDef;
            MethodDefRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.Rva);
                metadataWriter.Write((ushort)row.ImplFlags);
                metadataWriter.Write((ushort)row.Flags);
                metadataWriter.Write(row.NameStringIdx);
                metadataWriter.Write(row.SignatureBlobIdx);
                metadataWriter.Write(row.ParamList);
            }
        }

        /// <summary>
        /// Writes the param table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteParamTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.Param;
            ParamRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write((ushort)row.Flags);
                metadataWriter.Write((ushort)row.Sequence);
                metadataWriter.Write(row.NameIdx);
            }
        }

        /// <summary>
        /// Writes the interfaceimpl table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteInterfaceImplTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.InterfaceImpl;
            InterfaceImplRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.ClassTableIdx);
                metadataWriter.Write(row.InterfaceTableIdx);
            }
        }

        /// <summary>
        /// Writes the member ref table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteMemberRefTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.MemberRef;
            MemberRefRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.ClassTableIdx);
                metadataWriter.Write(row.NameStringIdx);
                metadataWriter.Write(row.SignatureBlobIdx);
            }
        }

        /// <summary>
        /// Writes the constant table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteConstantTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.Constant;
            ConstantRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write((byte)row.Type);
                metadataWriter.Write((byte)0);
                metadataWriter.Write(row.Parent);
                metadataWriter.Write(row.ValueBlobIdx);
            }
        }

        /// <summary>
        /// Writes the custom attribute table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteCustomAttributeTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.CustomAttribute;
            CustomAttributeRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.ParentTableIdx);
                metadataWriter.Write(row.TypeIdx);
                metadataWriter.Write(row.ValueBlobIdx);
            }
        }

        /// <summary>
        /// Writes the field marshal table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteFieldMarshalTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.FieldMarshal;
            FieldMarshalRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.ParentTableIdx);
                metadataWriter.Write(row.NativeTypeBlobIdx);
            }
        }

        /// <summary>
        /// Writes the decl security table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteDeclSecurityTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.DeclSecurity;
            DeclSecurityRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write((ushort)row.Action);
                metadataWriter.Write(row.ParentTableIdx);
                metadataWriter.Write(row.PermissionSetBlobIdx);
            }
        }

        /// <summary>
        /// Writes the class layout table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteClassLayoutTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.ClassLayout;
            ClassLayoutRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write((ushort)row.PackingSize);
                metadataWriter.Write((uint)row.ClassSize);
                metadataWriter.Write(row.ParentTypeDefIdx);
            }
        }

        /// <summary>
        /// Writes the field layout table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteFieldLayoutTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.FieldLayout;
            FieldLayoutRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.Offset);
                metadataWriter.Write(row.Field);
            }
        }

        /// <summary>
        /// Writes the standalone sig table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteStandaloneSigTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.StandAloneSig;
            StandAloneSigRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.SignatureBlobIdx);
            }
        }

        /// <summary>
        /// Writes the event map table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteEventMapTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.EventMap;
            EventMapRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.TypeDefTableIdx);
                metadataWriter.Write(row.EventListTableIdx);
            }
        }

        /// <summary>
        /// Writes the event table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteEventTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.Event;
            EventRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write((ushort)row.Flags);
                metadataWriter.Write(row.NameStringIdx);
                metadataWriter.Write(row.EventTypeTableIdx);
            }
        }

        /// <summary>
        /// Writes the property map table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WritePropertyMapTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.PropertyMap;
            PropertyMapRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.ParentTableIdx);
                metadataWriter.Write(row.PropertyTableIdx);
            }
        }

        /// <summary>
        /// Writes the property table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WritePropertyTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.Property;
            PropertyRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write((ushort)row.Flags);
                metadataWriter.Write(row.NameStringIdx);
                metadataWriter.Write(row.TypeBlobIdx);
            }
        }

        /// <summary>
        /// Writes the method semantics table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteMethodSemanticsTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.MethodSemantics;
            MethodSemanticsRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write((ushort)row.Semantics);
                metadataWriter.Write(row.MethodTableIdx);
                metadataWriter.Write(row.AssociationTableIdx);
            }
        }

        /// <summary>
        /// Writes the method impl table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteMethodImplTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.MethodImpl;
            MethodImplRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.ClassTableIdx);
                metadataWriter.Write(row.MethodBodyTableIdx);
                metadataWriter.Write(row.MethodDeclarationTableIdx);
            }
        }

        /// <summary>
        /// Writes the module ref table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteModuleRefTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.ModuleRef;
            ModuleRefRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.NameStringIdx);
            }
        }

        /// <summary>
        /// Writes the type spec table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteTypeSpecTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.TypeSpec;
            TypeSpecRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.SignatureBlobIdx);
            }
        }

        /// <summary>
        /// Writes the impl map table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteImplMapTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.ImplMap;
            ImplMapRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write((ushort)row.MappingFlags);
                metadataWriter.Write(row.MemberForwardedTableIdx);
                metadataWriter.Write(row.ImportNameStringIdx);
                metadataWriter.Write(row.ImportScopeTableIdx);
            }
        }

        /// <summary>
        /// Writes the field RVA table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteFieldRVATable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.FieldRVA;
            FieldRVARow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.Rva);
                metadataWriter.Write(row.FieldTableIdx);
            }
        }

        /// <summary>
        /// Writes the assembly metadata table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteAssemblyTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.Assembly;
            AssemblyRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write((uint)row.HashAlgId);
                metadataWriter.Write((ushort)row.MajorVersion);
                metadataWriter.Write((ushort)row.MinorVersion);
                metadataWriter.Write((ushort)row.BuildNumber);
                metadataWriter.Write((ushort)row.Revision);
                metadataWriter.Write((uint)row.Flags);
                metadataWriter.Write(row.PublicKeyIdx);
                metadataWriter.Write(row.NameIdx);
                metadataWriter.Write(row.CultureIdx);
            }
        }

        /// <summary>
        /// Writes the assembly processor table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteAssemblyProcessorTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.AssemblyProcessor;
            AssemblyProcessorRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.Processor);
            }
        }

        /// <summary>
        /// Writes the assembly OS table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteAssemblyOSTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.AssemblyOS;
            AssemblyOSRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write((uint)row.PlatformId);
                metadataWriter.Write((uint)row.MajorVersion);
                metadataWriter.Write((uint)row.MinorVersion);
            }
        }

        /// <summary>
        /// Writes the assembly ref table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteAssemblyRefTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.AssemblyRef;
            AssemblyRefRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write((ushort)row.MajorVersion);
                metadataWriter.Write((ushort)row.MinorVersion);
                metadataWriter.Write((ushort)row.BuildNumber);
                metadataWriter.Write((ushort)row.Revision);

                metadataWriter.Write((uint)row.Flags);
                metadataWriter.Write(row.PublicKeyOrTokenIdx);
                metadataWriter.Write(row.NameIdx);
                metadataWriter.Write(row.CultureIdx);
                metadataWriter.Write(row.HashValueIdx);
            }
        }

        /// <summary>
        /// Writes the assembly ref processor table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteAssemblyRefProcessorTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.AssemblyRefProcessor;
            AssemblyRefProcessorRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.Processor);
                metadataWriter.Write(row.AssemblyRef);
            }
        }

        /// <summary>
        /// Writes the assembly ref OS table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteAssemblyRefOSTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.AssemblyRefOS;
            AssemblyRefOSRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.PlatformId);
                metadataWriter.Write(row.MajorVersion);
                metadataWriter.Write(row.MinorVersion);
                metadataWriter.Write(row.AssemblyRefIdx);
            }
        }

        /// <summary>
        /// Writes the file table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteFileTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.File;
            FileRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write((uint)row.Flags);
                metadataWriter.Write(row.NameStringIdx);
                metadataWriter.Write(row.HashValueBlobIdx);
            }
        }

        /// <summary>
        /// Writes the exported type table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteExportedTypeTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.ExportedType;
            ExportedTypeRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write((uint)row.Flags);
                metadataWriter.Write(row.TypeDefTableIdx);
                metadataWriter.Write(row.TypeNameStringIdx);
                metadataWriter.Write(row.TypeNamespaceStringIdx);
                metadataWriter.Write(row.ImplementationTableIdx);
            }
        }

        /// <summary>
        /// Writes the manifest resource table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteManifestResourceTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.ManifestResource;
            ManifestResourceRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);
            }
        }

        /// <summary>
        /// Writes the nested class table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteNestedClassTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.NestedClass;
            NestedClassRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.NestedClassTableIdx);
                metadataWriter.Write(row.EnclosingClassTableIdx);
            }
        }

        /// <summary>
        /// Writes the generic param table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteGenericParamTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.GenericParam;
            GenericParamRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write((ushort)row.Number);
                metadataWriter.Write((ushort)row.Flags);
                metadataWriter.Write(row.OwnerTableIdx);
                metadataWriter.Write(row.NameStringIdx);
            }
        }

        /// <summary>
        /// Writes the method spec table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteMethodSpecTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.MethodSpec;
            MethodSpecRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.MethodTableIdx);
                metadataWriter.Write(row.InstantiationBlobIdx);
            }
        }

        /// <summary>
        /// Writes the generic param constraint table.
        /// </summary>
        /// <param name="metadataSource">The metadata source.</param>
        /// <param name="metadataWriter">The metadata writer.</param>
        private static void WriteGenericParamConstraintTable(IMetadataProvider metadataSource, MetadataBuilderStage metadataWriter)
        {
            const TokenTypes table = TokenTypes.GenericParamConstraint;
            GenericParamConstraintRow row;
            TokenTypes lastToken = metadataSource.GetMaxTokenValue(table);
            for (TokenTypes token = table; token < lastToken; token++)
            {
                metadataSource.Read(token, out row);

                metadataWriter.Write(row.OwnerTableIdx);
                metadataWriter.Write(row.ConstraintTableIdx);
            }
        }
    }
}
