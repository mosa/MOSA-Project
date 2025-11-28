using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;

namespace System.Runtime.Serialization.DataContracts;

public sealed class DataContractSet
{
	public Dictionary<XmlQualifiedName, DataContract> Contracts
	{
		get
		{
			throw null;
		}
	}

	public Dictionary<XmlQualifiedName, DataContract>? KnownTypesForObject
	{
		get
		{
			throw null;
		}
	}

	public Dictionary<DataContract, object> ProcessedContracts
	{
		get
		{
			throw null;
		}
	}

	public Hashtable SurrogateData
	{
		get
		{
			throw null;
		}
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public DataContractSet(DataContractSet dataContractSet)
	{
		throw null;
	}

	public DataContractSet(ISerializationSurrogateProvider? dataContractSurrogate, IEnumerable<Type>? referencedTypes, IEnumerable<Type>? referencedCollectionTypes)
	{
		throw null;
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public DataContract GetDataContract(Type type)
	{
		throw null;
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public DataContract? GetDataContract(XmlQualifiedName key)
	{
		throw null;
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public Type? GetReferencedType(XmlQualifiedName xmlName, DataContract dataContract, out DataContract? referencedContract, out object[]? genericParameters, bool? supportGenericTypes = null)
	{
		throw null;
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public void ImportSchemaSet(XmlSchemaSet schemaSet, IEnumerable<XmlQualifiedName>? typeNames, bool importXmlDataType)
	{
		throw null;
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public List<XmlQualifiedName> ImportSchemaSet(XmlSchemaSet schemaSet, IEnumerable<XmlSchemaElement> elements, bool importXmlDataType)
	{
		throw null;
	}
}
