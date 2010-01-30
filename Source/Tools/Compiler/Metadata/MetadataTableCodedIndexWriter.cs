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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Tools.Compiler.Metadata
{
    using System;
    using System.IO;

    using Mosa.Runtime.Metadata;
    using Mosa.Runtime.Metadata.Tables;
    using System.Diagnostics;

    /// <summary>
    /// Contains methods to write coded index values.
    /// </summary>
    public sealed partial class MetadataBuilderStage
    {
        private void WriteIndex(TokenTypes token, TokenTypes[] tables)
        {
            int bits = (int)Math.Log(tables.Length, 2);
            int encodingWidthBoundary = (int)Math.Pow(2, 17 - bits) - 1;
            int tableIndex = -1;
            bool wide = false;

            int index = 0;
            foreach (TokenTypes table in tables)
            {
                if (table != TokenTypes.MaxTable)
                {
                    TokenTypes lastToken = this._metadataSource.GetMaxTokenValue(table);
                    if (lastToken - token > encodingWidthBoundary)
                    {
                        wide = true;
                    }

                    if ((TokenTypes.TableMask & token) == table)
                    {
                        tableIndex = index;
                    }
                }

                index++;
            }

            // Encode the token
            Debug.Assert(tableIndex != -1, @"Failed to find indexed table.");
            uint encodedToken = (uint)((int)(TokenTypes.RowIndexMask & token) << bits | tableIndex);

            if (wide == true)
            {
                this._metadataWriter.Write(encodedToken);
            }
            else
            {
                this._metadataWriter.Write((ushort)encodedToken);
            }
        }

        /// <summary>
        /// Writes a coded index value in a metadata table.
        /// </summary>
        /// <param name="token">The indexed token.</param>
        public void WriteTypeOrMethodDefIndex(TokenTypes token)
        {
            WriteIndex(token, new[] { TokenTypes.TypeDef, TokenTypes.MethodDef });
        }

        /// <summary>
        /// Writes a coded index value in a metadata table.
        /// </summary>
        /// <param name="token">The indexed token.</param>
        public void WriteResolutionScopeIndex(TokenTypes token)
        {
            WriteIndex(token, new[] { TokenTypes.Module, TokenTypes.ModuleRef, TokenTypes.AssemblyRef, TokenTypes.TypeRef });
        }

        /// <summary>
        /// Writes a coded index value in a metadata table.
        /// </summary>
        /// <param name="token">The indexed token.</param>
        public void WriteCustomAttributeTypeIndex(TokenTypes token)
        {
            WriteIndex(token, new[] { TokenTypes.MaxTable, TokenTypes.MaxTable, TokenTypes.MethodDef, TokenTypes.MemberRef, TokenTypes.MaxTable });
        }

        /// <summary>
        /// Writes a coded index value in a metadata table.
        /// </summary>
        /// <param name="token">The indexed token.</param>
        public void WriteImplementationIndex(TokenTypes token)
        {
            WriteIndex(token, new[]
            {
                TokenTypes.File,
                TokenTypes.AssemblyRef,
                TokenTypes.ExportedType
            });
        }

        /// <summary>
        /// Writes a coded index value in a metadata table.
        /// </summary>
        /// <param name="token">The indexed token.</param>
        public void WriteMemberForwardedIndex(TokenTypes token)
        {
            WriteIndex(token, new[]
            {
                TokenTypes.Field,
                TokenTypes.MethodDef
            });
        }

        /// <summary>
        /// Writes a coded index value in a metadata table.
        /// </summary>
        /// <param name="token">The indexed token.</param>
        public void WriteMethodDefOrRefIndex(TokenTypes token)
        {
            WriteIndex(token, new[]
            {
                TokenTypes.MethodDef,
                TokenTypes.MemberRef
            });
        }

        /// <summary>
        /// Writes a coded index value in a metadata table.
        /// </summary>
        /// <param name="token">The indexed token.</param>
        public void WriteHasSemanticsIndex(TokenTypes token)
        {
            WriteIndex(token, new[]
            {
                TokenTypes.Event,
                TokenTypes.Property
            });
        }

        /// <summary>
        /// Writes a coded index value in a metadata table.
        /// </summary>
        /// <param name="token">The indexed token.</param>
        public void WriteMemberRefParentIndex(TokenTypes token)
        {
            WriteIndex(token, new[]
            {
                TokenTypes.TypeDef,
                TokenTypes.TypeRef,
                TokenTypes.ModuleRef,
                TokenTypes.MethodDef,
                TokenTypes.TypeSpec
            });
        }

        /// <summary>
        /// Writes a coded index value in a metadata table.
        /// </summary>
        /// <param name="token">The indexed token.</param>
        public void WriteHasDeclSecurityIndex(TokenTypes token)
        {
            WriteIndex(token, new[]
            {
                TokenTypes.TypeDef,
                TokenTypes.MethodDef,
                TokenTypes.Assembly
            });
        }

        /// <summary>
        /// Writes a coded index value in a metadata table.
        /// </summary>
        /// <param name="token">The indexed token.</param>
        public void WriteHasFieldMarshalIndex(TokenTypes token)
        {
            WriteIndex(token, new[]
            {
                TokenTypes.Field,
                TokenTypes.Param
            });
        }

        /// <summary>
        /// Writes a coded index value in a metadata table.
        /// </summary>
        /// <param name="token">The indexed token.</param>
        public void WriteHasCustomAttributeIndex(TokenTypes token)
        {
            WriteIndex(token, new[]
            {
                TokenTypes.MethodDef,
                TokenTypes.Field,
                TokenTypes.TypeRef,
                TokenTypes.TypeDef,
                TokenTypes.Param,
                TokenTypes.InterfaceImpl,
                TokenTypes.MemberRef,
                TokenTypes.Module,
                TokenTypes.DeclSecurity,
                TokenTypes.Property,
                TokenTypes.Event,
                TokenTypes.StandAloneSig,
                TokenTypes.ModuleRef,
                TokenTypes.TypeSpec,
                TokenTypes.Assembly,
                TokenTypes.AssemblyRef,
                TokenTypes.File,
                TokenTypes.ExportedType,
                TokenTypes.ManifestResource
            });
        }

        /// <summary>
        /// Writes a coded index value in a metadata table.
        /// </summary>
        /// <param name="token">The indexed token.</param>
        public void WriteHasConstantIndex(TokenTypes token)
        {
            WriteIndex(token, new[]
            {
                TokenTypes.Field,
                TokenTypes.Param,
                TokenTypes.Property
            });
        }

        /// <summary>
        /// Writes a coded index value in a metadata table.
        /// </summary>
        /// <param name="token">The indexed token.</param>
        public void WriteTypeDefOrRefIndex(TokenTypes token)
        {
            WriteIndex(token, new[]
            {
                TokenTypes.TypeDef,
                TokenTypes.TypeRef,
                TokenTypes.TypeSpec
            });
        }
    }
}
