/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.IO;
using System.Collections.Generic;

using Mosa.Runtime.Metadata;

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
		/// Returns the number of rows for the specified provider table.
		/// </summary>
		/// <param name="table">The token type, whose maximum value is returned.</param>
		/// <exception cref="System.ArgumentException">Invalid token type specified.</exception>
		MetadataToken GetMaxTokenValue(MetadataTable table);
		
		/// <summary>
		/// Gets the row count.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <returns></returns>
		int GetRowCount(TokenTypes table);

		/// <summary>
		/// Gets the row count.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <returns></returns>
		int GetRowCount(MetadataTable table);

		/// <summary>
		/// Reads a string heap or user string heap entry.
		/// </summary>
		/// <param name="token">The token of the string to read.</param>
		/// <returns></returns>
		string ReadString(TokenTypes token);

		/// <summary>
		/// Reads a guid heap entry.
		/// </summary>
		/// <param name="token">The token of the guid heap entry to read.</param>
		Guid ReadGuid(TokenTypes token);

		/// <summary>
		/// Reads a blob heap entry.
		/// </summary>
		/// <param name="token">The token of the blob heap entry to read.</param>
		byte[] ReadBlob(TokenTypes token);

		/// <summary>
		/// Reads a module row from provider.
		/// </summary>
		/// <param name="token">The module row token.</param>
		/// <returns></returns>
		ModuleRow ReadModuleRow(MetadataToken token);

		/// <summary>
		/// Reads a type reference row from provider.
		/// </summary>
		/// <param name="token">The type reference row token.</param>
		/// <returns></returns>
		TypeRefRow ReadTypeRefRow(MetadataToken token);

		/// <summary>
		/// Reads a type definition row from provider.
		/// </summary>
		/// <param name="token">The type definition row token.</param>
		/// <returns></returns>
		TypeDefRow ReadTypeDefRow(MetadataToken token);

		/// <summary>
		/// Reads a _stackFrameIndex definition row from provider.
		/// </summary>
		/// <param name="token">The _stackFrameIndex definition row token.</param>
		/// <returns></returns>
		FieldRow ReadFieldRow(MetadataToken token);

		/// <summary>
		/// Reads a method definition row from provider.
		/// </summary>
		/// <param name="token">The method definition row token.</param>
		/// <returns></returns>
		MethodDefRow ReadMethodDefRow(MetadataToken token);

		/// <summary>
		/// Reads a parameter row from provider.
		/// </summary>
		/// <param name="token">The parameter row token.</param>
		/// <returns></returns>
		ParamRow ReadParamRow(MetadataToken token);

		/// <summary>
		/// Reads an interface implementation row from provider.
		/// </summary>
		/// <param name="token">The interface implementation row token.</param>
		/// <returns></returns>
		InterfaceImplRow ReadInterfaceImplRow(MetadataToken token);

		/// <summary>
		/// Reads an member reference row from provider.
		/// </summary>
		/// <param name="token">The member reference row token.</param>
		/// <returns></returns>
		MemberRefRow ReadMemberRefRow(MetadataToken token);

		/// <summary>
		/// Reads a constant row from provider.
		/// </summary>
		/// <param name="token">The constant row token.</param>
		/// <returns></returns>
		ConstantRow ReadConstantRow(MetadataToken token);

		/// <summary>
		/// Reads a constant row from provider.
		/// </summary>
		/// <param name="token">The constant row token.</param>
		/// <returns></returns>
		CustomAttributeRow ReadCustomAttributeRow(MetadataToken token);

		/// <summary>
		/// Reads a _stackFrameIndex marshal row from provider.
		/// </summary>
		/// <param name="token">The _stackFrameIndex marshal row token.</param>
		/// <returns></returns>
		FieldMarshalRow ReadFieldMarshalRow(MetadataToken token);

		/// <summary>
		/// Reads a declarative security row from provider.
		/// </summary>
		/// <param name="token">The declarative security row token.</param>
		/// <returns></returns>
		DeclSecurityRow ReadDeclSecurityRow(MetadataToken token);

		/// <summary>
		/// Reads a class layout row from provider.
		/// </summary>
		/// <param name="token">The class layout row token.</param>
		/// <returns></returns>
		ClassLayoutRow ReadClassLayoutRow(MetadataToken token);

		/// <summary>
		/// Reads a _stackFrameIndex layout row from provider.
		/// </summary>
		/// <param name="token">The field layout row token.</param>
		/// <returns></returns>
		FieldLayoutRow ReadFieldLayoutRow(MetadataToken token);

		/// <summary>
		/// Reads a standalone signature row from provider.
		/// </summary>
		/// <param name="token">The standalone signature row token.</param>
		/// <returns></returns>
		StandAloneSigRow ReadStandAloneSigRow(MetadataToken token);

		/// <summary>
		/// Reads a event map row from provider.
		/// </summary>
		/// <param name="token">The event map row token.</param>
		/// <returns></returns>
		EventMapRow ReadEventMapRow(MetadataToken token);

		/// <summary>
		/// Reads a event row from provider.
		/// </summary>
		/// <param name="token">The event row token.</param>
		/// <returns></returns>
		EventRow ReadEventRow(MetadataToken token);

		/// <summary>
		/// Reads a property map row from provider.
		/// </summary>
		/// <param name="token">The property map row token.</param>
		/// <returns></returns>
		PropertyMapRow ReadPropertyMapRow(MetadataToken token);

		/// <summary>
		/// Reads a property row from provider.
		/// </summary>
		/// <param name="token">The property row token.</param>
		/// <returns></returns>
		PropertyRow ReadPropertyRow(MetadataToken token);

		/// <summary>
		/// Reads a method semantics row from provider.
		/// </summary>
		/// <param name="token">The method semantics row token.</param>
		/// <returns></returns>
		MethodSemanticsRow ReadMethodSemanticsRow(MetadataToken token);

		/// <summary>
		/// Reads a method impl row from provider.
		/// </summary>
		/// <param name="token">The method impl row token.</param>
		/// <returns></returns>
		MethodImplRow ReadMethodImplRow(MetadataToken token);

		/// <summary>
		/// Reads a module ref row from provider.
		/// </summary>
		/// <param name="token">The module ref row token.</param>
		/// <returns></returns>
		ModuleRefRow ReadModuleRefRow(MetadataToken token);

		/// <summary>
		/// Reads a typespec row from provider.
		/// </summary>
		/// <param name="token">The typespec row token.</param>
		/// <returns></returns>
		TypeSpecRow ReadTypeSpecRow(MetadataToken token);

		/// <summary>
		/// Reads a implementation map row from provider.
		/// </summary>
		/// <param name="token">The implementation map row token.</param>
		/// <returns></returns>
		ImplMapRow ReadImplMapRow(MetadataToken token);

		/// <summary>
		/// Reads a field rva row from provider.
		/// </summary>
		/// <param name="token">The field rva row token.</param>
		/// <returns></returns>
		FieldRVARow ReadFieldRVARow(MetadataToken token);

		/// <summary>
		/// Reads a assembly row from provider.
		/// </summary>
		/// <param name="token">The assembly row token.</param>
		/// <returns></returns>
		AssemblyRow ReadAssemblyRow(MetadataToken token);

		/// <summary>
		/// Reads a assembly processor row from provider.
		/// </summary>
		/// <param name="token">The assembly processor row token.</param>
		/// <returns></returns>
		AssemblyProcessorRow ReadAssemblyProcessorRow(MetadataToken token);

		/// <summary>
		/// Reads a assembly os row from provider.
		/// </summary>
		/// <param name="token">The assembly os row token.</param>
		/// <returns></returns>
		AssemblyOSRow ReadAssemblyOSRow(MetadataToken token);

		/// <summary>
		/// Reads a assembly reference row from provider.
		/// </summary>
		/// <param name="token">The assembly reference row token.</param>
		/// <returns></returns>
		AssemblyRefRow ReadAssemblyRefRow(MetadataToken token);

		/// <summary>
		/// Reads a assembly reference processor row from provider.
		/// </summary>
		/// <param name="token">The assembly reference processor row token.</param>
		/// <returns></returns>
		AssemblyRefProcessorRow ReadAssemblyRefProcessorRow(MetadataToken token);

		/// <summary>
		/// Reads a assembly reference os row from provider.
		/// </summary>
		/// <param name="token">The assembly reference os row token.</param>
		/// <returns></returns>
		AssemblyRefOSRow ReadAssemblyRefOSRow(MetadataToken token);

		/// <summary>
		/// Reads a file row from provider.
		/// </summary>
		/// <param name="token">The file row token.</param>
		/// <returns></returns>
		FileRow ReadFileRow(MetadataToken token);

		/// <summary>
		/// Reads an exported type row from provider.
		/// </summary>
		/// <param name="token">The exported type row token.</param>
		/// <returns></returns>
		ExportedTypeRow ReadExportedTypeRow(MetadataToken token);

		/// <summary>
		/// Reads a manifest resource row from provider.
		/// </summary>
		/// <param name="token">The manifest resource row token.</param>
		/// <returns></returns>
		ManifestResourceRow ReadManifestResourceRow(MetadataToken token);

		/// <summary>
		/// Reads a manifest resource row from provider.
		/// </summary>
		/// <param name="token">The manifest resource row token.</param>
		/// <returns></returns>
		NestedClassRow ReadNestedClassRow(MetadataToken token);

		/// <summary>
		/// Reads a generic parameter row from provider.
		/// </summary>
		/// <param name="token">The generic parameter row token.</param>
		/// <returns></returns>
		GenericParamRow ReadGenericParamRow(MetadataToken token);

		/// <summary>
		/// Reads a method specification row from provider.
		/// </summary>
		/// <param name="token">The method specification row token.</param>
		/// <returns></returns>
		MethodSpecRow ReadMethodSpecRow(MetadataToken token);

		/// <summary>
		/// Reads a generic parameter constraint row from provider.
		/// </summary>
		/// <param name="token">The generic parameter constraint row token.</param>
		/// <returns></returns>
		GenericParamConstraintRow ReadGenericParamConstraintRow(MetadataToken token);

		/// <summary>
		/// Gets the heaps of a specified type
		/// </summary>
		/// <param name="heapType">Type of the heap.</param>
		/// <returns></returns>
		IList<Heap> GetHeaps(HeapType heapType);
	}
}
