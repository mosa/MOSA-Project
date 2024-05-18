using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace System.Xml.Serialization;

public class XmlSerializer
{
	public event XmlAttributeEventHandler UnknownAttribute
	{
		add
		{
		}
		remove
		{
		}
	}

	public event XmlElementEventHandler UnknownElement
	{
		add
		{
		}
		remove
		{
		}
	}

	public event XmlNodeEventHandler UnknownNode
	{
		add
		{
		}
		remove
		{
		}
	}

	public event UnreferencedObjectEventHandler UnreferencedObject
	{
		add
		{
		}
		remove
		{
		}
	}

	protected XmlSerializer()
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlSerializer(Type type)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlSerializer(Type type, string? defaultNamespace)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlSerializer(Type type, Type[]? extraTypes)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlSerializer(Type type, XmlAttributeOverrides? overrides)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlSerializer(Type type, XmlAttributeOverrides? overrides, Type[]? extraTypes, XmlRootAttribute? root, string? defaultNamespace)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlSerializer(Type type, XmlAttributeOverrides? overrides, Type[]? extraTypes, XmlRootAttribute? root, string? defaultNamespace, string? location)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlSerializer(Type type, XmlRootAttribute? root)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public XmlSerializer(XmlTypeMapping xmlTypeMapping)
	{
	}

	public virtual bool CanDeserialize(XmlReader xmlReader)
	{
		throw null;
	}

	protected virtual XmlSerializationReader CreateReader()
	{
		throw null;
	}

	protected virtual XmlSerializationWriter CreateWriter()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from deserialized types may be trimmed if not referenced directly")]
	public object? Deserialize(Stream stream)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from deserialized types may be trimmed if not referenced directly")]
	public object? Deserialize(TextReader textReader)
	{
		throw null;
	}

	protected virtual object Deserialize(XmlSerializationReader reader)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from deserialized types may be trimmed if not referenced directly")]
	public object? Deserialize(XmlReader xmlReader)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from deserialized types may be trimmed if not referenced directly")]
	public object? Deserialize(XmlReader xmlReader, string? encodingStyle)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from deserialized types may be trimmed if not referenced directly")]
	public object? Deserialize(XmlReader xmlReader, string? encodingStyle, XmlDeserializationEvents events)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from deserialized types may be trimmed if not referenced directly")]
	public object? Deserialize(XmlReader xmlReader, XmlDeserializationEvents events)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public static XmlSerializer?[] FromMappings(XmlMapping[]? mappings)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public static XmlSerializer?[] FromMappings(XmlMapping[]? mappings, Type? type)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public static XmlSerializer?[] FromTypes(Type[]? types)
	{
		throw null;
	}

	public static string GetXmlSerializerAssemblyName(Type type)
	{
		throw null;
	}

	public static string GetXmlSerializerAssemblyName(Type type, string? defaultNamespace)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public void Serialize(Stream stream, object? o)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public void Serialize(Stream stream, object? o, XmlSerializerNamespaces? namespaces)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public void Serialize(TextWriter textWriter, object? o)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public void Serialize(TextWriter textWriter, object? o, XmlSerializerNamespaces? namespaces)
	{
	}

	protected virtual void Serialize(object? o, XmlSerializationWriter writer)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public void Serialize(XmlWriter xmlWriter, object? o)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public void Serialize(XmlWriter xmlWriter, object? o, XmlSerializerNamespaces? namespaces)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public void Serialize(XmlWriter xmlWriter, object? o, XmlSerializerNamespaces? namespaces, string? encodingStyle)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public void Serialize(XmlWriter xmlWriter, object? o, XmlSerializerNamespaces? namespaces, string? encodingStyle, string? id)
	{
	}
}
