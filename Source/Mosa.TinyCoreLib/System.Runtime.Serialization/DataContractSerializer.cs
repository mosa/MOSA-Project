using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace System.Runtime.Serialization;

public sealed class DataContractSerializer : XmlObjectSerializer
{
	public DataContractResolver? DataContractResolver
	{
		get
		{
			throw null;
		}
	}

	public bool IgnoreExtensionDataObject
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyCollection<Type> KnownTypes
	{
		get
		{
			throw null;
		}
	}

	public int MaxItemsInObjectGraph
	{
		get
		{
			throw null;
		}
	}

	public bool PreserveObjectReferences
	{
		get
		{
			throw null;
		}
	}

	public bool SerializeReadOnlyTypes
	{
		get
		{
			throw null;
		}
	}

	public DataContractSerializer(Type type)
	{
	}

	public DataContractSerializer(Type type, IEnumerable<Type>? knownTypes)
	{
	}

	public DataContractSerializer(Type type, DataContractSerializerSettings? settings)
	{
	}

	public DataContractSerializer(Type type, string rootName, string rootNamespace)
	{
	}

	public DataContractSerializer(Type type, string rootName, string rootNamespace, IEnumerable<Type>? knownTypes)
	{
	}

	public DataContractSerializer(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace)
	{
	}

	public DataContractSerializer(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace, IEnumerable<Type>? knownTypes)
	{
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public override bool IsStartObject(XmlDictionaryReader reader)
	{
		throw null;
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public override bool IsStartObject(XmlReader reader)
	{
		throw null;
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public override object? ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
	{
		throw null;
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public object? ReadObject(XmlDictionaryReader reader, bool verifyObjectName, DataContractResolver? dataContractResolver)
	{
		throw null;
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public override object? ReadObject(XmlReader reader)
	{
		throw null;
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public override object? ReadObject(XmlReader reader, bool verifyObjectName)
	{
		throw null;
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public override void WriteEndObject(XmlDictionaryWriter writer)
	{
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public override void WriteEndObject(XmlWriter writer)
	{
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public void WriteObject(XmlDictionaryWriter writer, object? graph, DataContractResolver? dataContractResolver)
	{
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public override void WriteObject(XmlWriter writer, object? graph)
	{
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public override void WriteObjectContent(XmlDictionaryWriter writer, object? graph)
	{
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public override void WriteObjectContent(XmlWriter writer, object? graph)
	{
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public override void WriteStartObject(XmlDictionaryWriter writer, object? graph)
	{
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public override void WriteStartObject(XmlWriter writer, object? graph)
	{
	}
}
