using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System.Xml.Serialization;

public abstract class XmlSerializationWriter : XmlSerializationGeneratedCode
{
	protected bool EscapeName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected ArrayList? Namespaces
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected XmlWriter Writer
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected void AddWriteCallback(Type type, string typeName, string? typeNs, XmlSerializationWriteCallback callback)
	{
	}

	protected Exception CreateChoiceIdentifierValueException(string value, string identifier, string name, string ns)
	{
		throw null;
	}

	protected Exception CreateInvalidAnyTypeException(object o)
	{
		throw null;
	}

	protected Exception CreateInvalidAnyTypeException(Type type)
	{
		throw null;
	}

	protected Exception CreateInvalidChoiceIdentifierValueException(string type, string identifier)
	{
		throw null;
	}

	protected Exception CreateInvalidEnumValueException(object value, string typeName)
	{
		throw null;
	}

	protected Exception CreateMismatchChoiceException(string value, string elementName, string enumValue)
	{
		throw null;
	}

	protected Exception CreateUnknownAnyElementException(string name, string ns)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	protected Exception CreateUnknownTypeException(object o)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	protected Exception CreateUnknownTypeException(Type type)
	{
		throw null;
	}

	protected static byte[] FromByteArrayBase64(byte[] value)
	{
		throw null;
	}

	[return: NotNullIfNotNull("value")]
	protected static string? FromByteArrayHex(byte[]? value)
	{
		throw null;
	}

	protected static string FromChar(char value)
	{
		throw null;
	}

	protected static string FromDate(DateTime value)
	{
		throw null;
	}

	protected static string FromDateTime(DateTime value)
	{
		throw null;
	}

	protected static string FromEnum(long value, string[] values, long[] ids)
	{
		throw null;
	}

	protected static string FromEnum(long value, string[] values, long[] ids, string typeName)
	{
		throw null;
	}

	protected static string FromTime(DateTime value)
	{
		throw null;
	}

	[return: NotNullIfNotNull("name")]
	protected static string? FromXmlName(string? name)
	{
		throw null;
	}

	[return: NotNullIfNotNull("ncName")]
	protected static string? FromXmlNCName(string? ncName)
	{
		throw null;
	}

	[return: NotNullIfNotNull("nmToken")]
	protected static string? FromXmlNmToken(string? nmToken)
	{
		throw null;
	}

	[return: NotNullIfNotNull("nmTokens")]
	protected static string? FromXmlNmTokens(string? nmTokens)
	{
		throw null;
	}

	protected string? FromXmlQualifiedName(XmlQualifiedName? xmlQualifiedName)
	{
		throw null;
	}

	protected string? FromXmlQualifiedName(XmlQualifiedName? xmlQualifiedName, bool ignoreEmpty)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	protected abstract void InitCallbacks();

	protected static Assembly? ResolveDynamicAssembly(string assemblyFullName)
	{
		throw null;
	}

	protected void TopLevelElement()
	{
	}

	protected void WriteAttribute(string localName, byte[]? value)
	{
	}

	protected void WriteAttribute(string localName, string? value)
	{
	}

	protected void WriteAttribute(string localName, string ns, byte[]? value)
	{
	}

	protected void WriteAttribute(string localName, string? ns, string? value)
	{
	}

	protected void WriteAttribute(string? prefix, string localName, string? ns, string? value)
	{
	}

	protected void WriteElementEncoded(XmlNode? node, string name, string? ns, bool isNullable, bool any)
	{
	}

	protected void WriteElementLiteral(XmlNode? node, string name, string? ns, bool isNullable, bool any)
	{
	}

	protected void WriteElementQualifiedName(string localName, string? ns, XmlQualifiedName? value)
	{
	}

	protected void WriteElementQualifiedName(string localName, string? ns, XmlQualifiedName? value, XmlQualifiedName? xsiType)
	{
	}

	protected void WriteElementQualifiedName(string localName, XmlQualifiedName? value)
	{
	}

	protected void WriteElementQualifiedName(string localName, XmlQualifiedName? value, XmlQualifiedName? xsiType)
	{
	}

	protected void WriteElementString(string localName, string? value)
	{
	}

	protected void WriteElementString(string localName, string? ns, string? value)
	{
	}

	protected void WriteElementString(string localName, string? ns, string? value, XmlQualifiedName? xsiType)
	{
	}

	protected void WriteElementString(string localName, string? value, XmlQualifiedName? xsiType)
	{
	}

	protected void WriteElementStringRaw(string localName, byte[]? value)
	{
	}

	protected void WriteElementStringRaw(string localName, byte[]? value, XmlQualifiedName? xsiType)
	{
	}

	protected void WriteElementStringRaw(string localName, string? value)
	{
	}

	protected void WriteElementStringRaw(string localName, string? ns, byte[]? value)
	{
	}

	protected void WriteElementStringRaw(string localName, string? ns, byte[]? value, XmlQualifiedName? xsiType)
	{
	}

	protected void WriteElementStringRaw(string localName, string? ns, string? value)
	{
	}

	protected void WriteElementStringRaw(string localName, string? ns, string? value, XmlQualifiedName? xsiType)
	{
	}

	protected void WriteElementStringRaw(string localName, string? value, XmlQualifiedName? xsiType)
	{
	}

	protected void WriteEmptyTag(string? name)
	{
	}

	protected void WriteEmptyTag(string? name, string? ns)
	{
	}

	protected void WriteEndElement()
	{
	}

	protected void WriteEndElement(object? o)
	{
	}

	protected void WriteId(object o)
	{
	}

	protected void WriteNamespaceDeclarations(XmlSerializerNamespaces? xmlns)
	{
	}

	protected void WriteNullableQualifiedNameEncoded(string name, string? ns, XmlQualifiedName? value, XmlQualifiedName? xsiType)
	{
	}

	protected void WriteNullableQualifiedNameLiteral(string name, string? ns, XmlQualifiedName? value)
	{
	}

	protected void WriteNullableStringEncoded(string name, string? ns, string? value, XmlQualifiedName? xsiType)
	{
	}

	protected void WriteNullableStringEncodedRaw(string name, string? ns, byte[]? value, XmlQualifiedName? xsiType)
	{
	}

	protected void WriteNullableStringEncodedRaw(string name, string? ns, string? value, XmlQualifiedName? xsiType)
	{
	}

	protected void WriteNullableStringLiteral(string name, string? ns, string? value)
	{
	}

	protected void WriteNullableStringLiteralRaw(string name, string? ns, byte[]? value)
	{
	}

	protected void WriteNullableStringLiteralRaw(string name, string? ns, string? value)
	{
	}

	protected void WriteNullTagEncoded(string? name)
	{
	}

	protected void WriteNullTagEncoded(string? name, string? ns)
	{
	}

	protected void WriteNullTagLiteral(string? name)
	{
	}

	protected void WriteNullTagLiteral(string? name, string? ns)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	protected void WritePotentiallyReferencingElement(string? n, string? ns, object? o)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	protected void WritePotentiallyReferencingElement(string? n, string? ns, object? o, Type? ambientType)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	protected void WritePotentiallyReferencingElement(string n, string? ns, object? o, Type? ambientType, bool suppressReference)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	protected void WritePotentiallyReferencingElement(string? n, string? ns, object? o, Type? ambientType, bool suppressReference, bool isNullable)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	protected void WriteReferencedElements()
	{
	}

	protected void WriteReferencingElement(string n, string? ns, object? o)
	{
	}

	protected void WriteReferencingElement(string n, string? ns, object? o, bool isNullable)
	{
	}

	protected void WriteRpcResult(string name, string? ns)
	{
	}

	protected void WriteSerializable(IXmlSerializable? serializable, string name, string ns, bool isNullable)
	{
	}

	protected void WriteSerializable(IXmlSerializable? serializable, string name, string? ns, bool isNullable, bool wrapped)
	{
	}

	protected void WriteStartDocument()
	{
	}

	protected void WriteStartElement(string name)
	{
	}

	protected void WriteStartElement(string name, string? ns)
	{
	}

	protected void WriteStartElement(string name, string? ns, bool writePrefixed)
	{
	}

	protected void WriteStartElement(string name, string? ns, object? o)
	{
	}

	protected void WriteStartElement(string name, string? ns, object? o, bool writePrefixed)
	{
	}

	protected void WriteStartElement(string name, string? ns, object? o, bool writePrefixed, XmlSerializerNamespaces? xmlns)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	protected void WriteTypedPrimitive(string? name, string? ns, object o, bool xsiType)
	{
	}

	protected void WriteValue(byte[]? value)
	{
	}

	protected void WriteValue(string? value)
	{
	}

	protected void WriteXmlAttribute(XmlNode node)
	{
	}

	protected void WriteXmlAttribute(XmlNode node, object? container)
	{
	}

	protected void WriteXsiType(string name, string? ns)
	{
	}
}
