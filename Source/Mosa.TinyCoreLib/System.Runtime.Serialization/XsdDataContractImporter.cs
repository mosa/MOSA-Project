using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;

namespace System.Runtime.Serialization;

public class XsdDataContractImporter
{
	public CodeCompileUnit CodeCompileUnit
	{
		get
		{
			throw null;
		}
	}

	public ImportOptions? Options
	{
		get
		{
			throw null;
		}
		set
		{
			throw null;
		}
	}

	public XsdDataContractImporter()
	{
		throw null;
	}

	public XsdDataContractImporter(CodeCompileUnit codeCompileUnit)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public bool CanImport(XmlSchemaSet schemas)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public bool CanImport(XmlSchemaSet schemas, ICollection<XmlQualifiedName> typeNames)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public bool CanImport(XmlSchemaSet schemas, XmlQualifiedName typeName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public bool CanImport(XmlSchemaSet schemas, XmlSchemaElement element)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public CodeTypeReference GetCodeTypeReference(XmlQualifiedName typeName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public CodeTypeReference GetCodeTypeReference(XmlQualifiedName typeName, XmlSchemaElement element)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public ICollection<CodeTypeReference>? GetKnownTypeReferences(XmlQualifiedName typeName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public void Import(XmlSchemaSet schemas)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public void Import(XmlSchemaSet schemas, ICollection<XmlQualifiedName> typeNames)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public void Import(XmlSchemaSet schemas, XmlQualifiedName typeName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public XmlQualifiedName? Import(XmlSchemaSet schemas, XmlSchemaElement element)
	{
		throw null;
	}
}
