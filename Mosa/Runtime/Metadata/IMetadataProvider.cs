/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata.Tables;

namespace Mosa.Runtime.Metadata
{
    /// <summary>
    /// Metadata root interface.
    /// </summary>
    /// <remarks>
    /// This interface provides clients with the capability to traverse
    /// all provider contained in an assembly.
    /// </remarks>
    public interface IMetadataProvider
    {
        /// <summary>
        /// Retrieves the assembly, which contains this provider.
        /// </summary>
        IMetadataModule Assembly { get; }

		/// <summary>
		/// Retrieves the metadata binary byte array .
		/// </summary>	
		byte[] Metadata { get; }

        /// <summary>
        /// Returns the number of rows for the specified provider table.
        /// </summary>
        /// <param name="table">The token type, whose maximum value is returned.</param>
        /// <exception cref="System.ArgumentException">Invalid token type specified.</exception>
        TokenTypes GetMaxTokenValue(TokenTypes table);

        /// <summary>
        /// Reads a string heap or user string heap entry.
        /// </summary>
        /// <param name="token">The token of the string to read.</param>
        /// <param name="result">Receives the read string.</param>
        TokenTypes Read(TokenTypes token, out string result);

        /// <summary>
        /// Reads a guid heap entry.
        /// </summary>
        /// <param name="token">The token of the guid heap entry to read.</param>
        /// <param name="guid">Receives the read guid.</param>
        TokenTypes Read(TokenTypes token, out Guid guid);

        /// <summary>
        /// Reads a blob heap entry.
        /// </summary>
        /// <param name="token">The token of the blob heap entry to read.</param>
        /// <param name="blob">Receives the read blob.</param>
        TokenTypes Read(TokenTypes token, out byte[] blob);

        /// <summary>
        /// Reads a module row from provider.
        /// </summary>
        /// <param name="token">The module row token.</param>
        /// <param name="result">Receives the read module row.</param>
        void Read(TokenTypes token, out ModuleRow result);

        /// <summary>
        /// Reads a type reference row from provider.
        /// </summary>
        /// <param name="token">The type reference row token.</param>
        /// <param name="result">Receives the read type reference row.</param>
        void Read(TokenTypes token, out TypeRefRow result);

        /// <summary>
        /// Reads a type definition row from provider.
        /// </summary>
        /// <param name="token">The type definition row token.</param>
        /// <param name="result">Receives the read type definition row.</param>
        void Read(TokenTypes token, out TypeDefRow result);

        /// <summary>
        /// Reads a _stackFrameIndex definition row from provider.
        /// </summary>
        /// <param name="token">The _stackFrameIndex definition row token.</param>
        /// <param name="result">Receives the read _stackFrameIndex definition row.</param>
        void Read(TokenTypes token, out FieldRow result);

        /// <summary>
        /// Reads a method definition row from provider.
        /// </summary>
        /// <param name="token">The method definition row token.</param>
        /// <param name="result">Receives the read method definition row.</param>
        void Read(TokenTypes token, out MethodDefRow result);

        /// <summary>
        /// Reads a parameter row from provider.
        /// </summary>
        /// <param name="token">The parameter row token.</param>
        /// <param name="result">Receives the read parameter row.</param>
        void Read(TokenTypes token, out ParamRow result);

        /// <summary>
        /// Reads an interface implementation row from provider.
        /// </summary>
        /// <param name="token">The interface implementation row token.</param>
        /// <param name="result">Receives the read interface implementation row.</param>
        void Read(TokenTypes token, out InterfaceImplRow result);

        /// <summary>
        /// Reads an member reference row from provider.
        /// </summary>
        /// <param name="token">The member reference row token.</param>
        /// <param name="result">Receives the read member reference row.</param>
        void Read(TokenTypes token, out MemberRefRow result);

        /// <summary>
        /// Reads a constant row from provider.
        /// </summary>
        /// <param name="token">The constant row token.</param>
        /// <param name="result">Receives the read constant row.</param>
        void Read(TokenTypes token, out ConstantRow result);

        /// <summary>
        /// Reads a constant row from provider.
        /// </summary>
        /// <param name="token">The constant row token.</param>
        /// <param name="result">Receives the read constant row.</param>
        void Read(TokenTypes token, out CustomAttributeRow result);

        /// <summary>
        /// Reads a _stackFrameIndex marshal row from provider.
        /// </summary>
        /// <param name="token">The _stackFrameIndex marshal row token.</param>
        /// <param name="result">Receives the read _stackFrameIndex marshal row.</param>
        void Read(TokenTypes token, out FieldMarshalRow result);

        /// <summary>
        /// Reads a declarative security row from provider.
        /// </summary>
        /// <param name="token">The declarative security row token.</param>
        /// <param name="result">Receives the read declarative security row.</param>
        void Read(TokenTypes token, out DeclSecurityRow result);

        /// <summary>
        /// Reads a class layout row from provider.
        /// </summary>
        /// <param name="token">The class layout row token.</param>
        /// <param name="result">Receives the read class layout row.</param>
        void Read(TokenTypes token, out ClassLayoutRow result);

        /// <summary>
        /// Reads a _stackFrameIndex layout row from provider.
        /// </summary>
        /// <param name="token">The _stackFrameIndex layout row token.</param>
        /// <param name="result">Receives the read _stackFrameIndex layout row.</param>
        void Read(TokenTypes token, out FieldLayoutRow result);

        /// <summary>
        /// Reads a standalone signature row from provider.
        /// </summary>
        /// <param name="token">The standalone signature row token.</param>
        /// <param name="result">Receives the read standalone signature row.</param>
        void Read(TokenTypes token, out StandAloneSigRow result);

        /// <summary>
        /// Reads a event map row from provider.
        /// </summary>
        /// <param name="token">The event map row token.</param>
        /// <param name="result">Receives the read event map row.</param>
        void Read(TokenTypes token, out EventMapRow result);

        /// <summary>
        /// Reads a event row from provider.
        /// </summary>
        /// <param name="token">The event row token.</param>
        /// <param name="result">Receives the read event row.</param>
        void Read(TokenTypes token, out EventRow result);

        /// <summary>
        /// Reads a property map row from provider.
        /// </summary>
        /// <param name="token">The property map row token.</param>
        /// <param name="result">Receives the read property map row.</param>
        void Read(TokenTypes token, out PropertyMapRow result);

        /// <summary>
        /// Reads a property row from provider.
        /// </summary>
        /// <param name="token">The property row token.</param>
        /// <param name="result">Receives the read property row.</param>
        void Read(TokenTypes token, out PropertyRow result);

        /// <summary>
        /// Reads a method semantics row from provider.
        /// </summary>
        /// <param name="token">The method semantics row token.</param>
        /// <param name="result">Receives the read method semantics row.</param>
        void Read(TokenTypes token, out MethodSemanticsRow result);

        /// <summary>
        /// Reads a method impl row from provider.
        /// </summary>
        /// <param name="token">The method impl row token.</param>
        /// <param name="result">Receives the read method impl row.</param>
        void Read(TokenTypes token, out MethodImplRow result);

        /// <summary>
        /// Reads a module ref row from provider.
        /// </summary>
        /// <param name="token">The module ref row token.</param>
        /// <param name="result">Receives the read module ref row.</param>
        void Read(TokenTypes token, out ModuleRefRow result);

        /// <summary>
        /// Reads a typespec row from provider.
        /// </summary>
        /// <param name="token">The typespec row token.</param>
        /// <param name="result">Receives the read typespec row.</param>
        void Read(TokenTypes token, out TypeSpecRow result);

        /// <summary>
        /// Reads a implementation map row from provider.
        /// </summary>
        /// <param name="token">The implementation map row token.</param>
        /// <param name="result">Receives the read implementation map row.</param>
        void Read(TokenTypes token, out ImplMapRow result);

        /// <summary>
        /// Reads a _stackFrameIndex rva row from provider.
        /// </summary>
        /// <param name="token">The _stackFrameIndex rva row token.</param>
        /// <param name="result">Receives the read _stackFrameIndex rva row.</param>
        void Read(TokenTypes token, out FieldRVARow result);

        /// <summary>
        /// Reads a assembly row from provider.
        /// </summary>
        /// <param name="token">The assembly row token.</param>
        /// <param name="result">Receives the read assembly row.</param>
        void Read(TokenTypes token, out AssemblyRow result);

        /// <summary>
        /// Reads a assembly processor row from provider.
        /// </summary>
        /// <param name="token">The assembly processor row token.</param>
        /// <param name="result">Receives the read assembly processor row.</param>
        void Read(TokenTypes token, out AssemblyProcessorRow result);

        /// <summary>
        /// Reads a assembly os row from provider.
        /// </summary>
        /// <param name="token">The assembly os row token.</param>
        /// <param name="result">Receives the read assembly os row.</param>
        void Read(TokenTypes token, out AssemblyOSRow result);

        /// <summary>
        /// Reads a assembly reference row from provider.
        /// </summary>
        /// <param name="token">The assembly reference row token.</param>
        /// <param name="result">Receives the read assembly reference row.</param>
        void Read(TokenTypes token, out AssemblyRefRow result);

        /// <summary>
        /// Reads a assembly reference processor row from provider.
        /// </summary>
        /// <param name="token">The assembly reference processor row token.</param>
        /// <param name="result">Receives the read assembly reference processor row.</param>
        void Read(TokenTypes token, out AssemblyRefProcessorRow result);

        /// <summary>
        /// Reads a assembly reference os row from provider.
        /// </summary>
        /// <param name="token">The assembly reference os row token.</param>
        /// <param name="result">Receives the read assembly reference os row.</param>
        void Read(TokenTypes token, out AssemblyRefOSRow result);

        /// <summary>
        /// Reads a file row from provider.
        /// </summary>
        /// <param name="token">The file row token.</param>
        /// <param name="result">Receives the read file row.</param>
        void Read(TokenTypes token, out FileRow result);

        /// <summary>
        /// Reads an exported type row from provider.
        /// </summary>
        /// <param name="token">The exported type row token.</param>
        /// <param name="result">Receives the read exported type row.</param>
        void Read(TokenTypes token, out ExportedTypeRow result);

        /// <summary>
        /// Reads a manifest resource row from provider.
        /// </summary>
        /// <param name="token">The manifest resource row token.</param>
        /// <param name="result">Receives the read manifest resource row.</param>
        void Read(TokenTypes token, out ManifestResourceRow result);

        /// <summary>
        /// Reads a manifest resource row from provider.
        /// </summary>
        /// <param name="token">The manifest resource row token.</param>
        /// <param name="result">Receives the read manifest resource row.</param>
        void Read(TokenTypes token, out NestedClassRow result);

        /// <summary>
        /// Reads a generic parameter row from provider.
        /// </summary>
        /// <param name="token">The generic parameter row token.</param>
        /// <param name="result">Receives the read generic parameter row.</param>
        void Read(TokenTypes token, out GenericParamRow result);

        /// <summary>
        /// Reads a method specification row from provider.
        /// </summary>
        /// <param name="token">The method specification row token.</param>
        /// <param name="result">Receives the read method specification row.</param>
        void Read(TokenTypes token, out MethodSpecRow result);

        /// <summary>
        /// Reads a generic parameter constraint row from provider.
        /// </summary>
        /// <param name="token">The generic parameter constraint row token.</param>
        /// <param name="result">Receives the read generic parameter constraint row.</param>
        void Read(TokenTypes token, out GenericParamConstraintRow result);
    }
}
