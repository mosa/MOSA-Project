using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System.Xml.Serialization;

public abstract class XmlSerializationReader : XmlSerializationGeneratedCode
{
	protected class CollectionFixup
	{
		public XmlSerializationCollectionFixupCallback Callback
		{
			get
			{
				throw null;
			}
		}

		public object? Collection
		{
			get
			{
				throw null;
			}
		}

		public object CollectionItems
		{
			get
			{
				throw null;
			}
		}

		public CollectionFixup(object? collection, XmlSerializationCollectionFixupCallback callback, object collectionItems)
		{
		}
	}

	protected class Fixup
	{
		public XmlSerializationFixupCallback Callback
		{
			get
			{
				throw null;
			}
		}

		public string?[]? Ids
		{
			get
			{
				throw null;
			}
		}

		public object? Source
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}

		public Fixup(object? o, XmlSerializationFixupCallback callback, int count)
		{
		}

		public Fixup(object? o, XmlSerializationFixupCallback callback, string?[]? ids)
		{
		}
	}

	protected bool DecodeName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected XmlDocument Document
	{
		get
		{
			throw null;
		}
	}

	protected bool IsReturnValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected XmlReader Reader
	{
		get
		{
			throw null;
		}
	}

	protected int ReaderCount
	{
		get
		{
			throw null;
		}
	}

	protected void AddFixup(CollectionFixup? fixup)
	{
	}

	protected void AddFixup(Fixup? fixup)
	{
	}

	protected void AddReadCallback(string name, string ns, Type type, XmlSerializationReadCallback read)
	{
	}

	protected void AddTarget(string? id, object? o)
	{
	}

	protected void CheckReaderCount(ref int whileIterations, ref int readerCount)
	{
	}

	[return: NotNullIfNotNull("value")]
	protected string? CollapseWhitespace(string? value)
	{
		throw null;
	}

	protected Exception CreateAbstractTypeException(string name, string? ns)
	{
		throw null;
	}

	protected Exception CreateBadDerivationException(string? xsdDerived, string? nsDerived, string? xsdBase, string? nsBase, string? clrDerived, string? clrBase)
	{
		throw null;
	}

	protected Exception CreateCtorHasSecurityException(string typeName)
	{
		throw null;
	}

	protected Exception CreateInaccessibleConstructorException(string typeName)
	{
		throw null;
	}

	protected Exception CreateInvalidCastException(Type type, object? value)
	{
		throw null;
	}

	protected Exception CreateInvalidCastException(Type type, object? value, string? id)
	{
		throw null;
	}

	protected Exception CreateMissingIXmlSerializableType(string? name, string? ns, string? clrType)
	{
		throw null;
	}

	protected Exception CreateReadOnlyCollectionException(string name)
	{
		throw null;
	}

	protected Exception CreateUnknownConstantException(string? value, Type enumType)
	{
		throw null;
	}

	protected Exception CreateUnknownNodeException()
	{
		throw null;
	}

	protected Exception CreateUnknownTypeException(XmlQualifiedName type)
	{
		throw null;
	}

	protected Array EnsureArrayIndex(Array? a, int index, Type elementType)
	{
		throw null;
	}

	protected void FixupArrayRefs(object fixup)
	{
	}

	protected int GetArrayLength(string name, string ns)
	{
		throw null;
	}

	protected bool GetNullAttr()
	{
		throw null;
	}

	protected object GetTarget(string id)
	{
		throw null;
	}

	protected XmlQualifiedName? GetXsiType()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	protected abstract void InitCallbacks();

	protected abstract void InitIDs();

	protected bool IsXmlnsAttribute(string name)
	{
		throw null;
	}

	protected void ParseWsdlArrayType(XmlAttribute attr)
	{
	}

	protected XmlQualifiedName ReadElementQualifiedName()
	{
		throw null;
	}

	protected void ReadEndElement()
	{
	}

	protected bool ReadNull()
	{
		throw null;
	}

	protected XmlQualifiedName? ReadNullableQualifiedName()
	{
		throw null;
	}

	protected string? ReadNullableString()
	{
		throw null;
	}

	protected bool ReadReference([NotNullWhen(true)] out string? fixupReference)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	protected object? ReadReferencedElement()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	protected object? ReadReferencedElement(string? name, string? ns)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	protected void ReadReferencedElements()
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	protected object? ReadReferencingElement(string? name, string? ns, bool elementCanBeType, out string? fixupReference)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	protected object? ReadReferencingElement(string? name, string? ns, out string? fixupReference)
	{
		throw null;
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	protected object? ReadReferencingElement(out string? fixupReference)
	{
		throw null;
	}

	protected IXmlSerializable ReadSerializable(IXmlSerializable serializable)
	{
		throw null;
	}

	protected IXmlSerializable ReadSerializable(IXmlSerializable serializable, bool wrappedAny)
	{
		throw null;
	}

	[return: NotNullIfNotNull("value")]
	protected string? ReadString(string? value)
	{
		throw null;
	}

	[return: NotNullIfNotNull("value")]
	protected string? ReadString(string? value, bool trim)
	{
		throw null;
	}

	protected object? ReadTypedNull(XmlQualifiedName type)
	{
		throw null;
	}

	protected object? ReadTypedPrimitive(XmlQualifiedName type)
	{
		throw null;
	}

	protected XmlDocument? ReadXmlDocument(bool wrapped)
	{
		throw null;
	}

	protected XmlNode? ReadXmlNode(bool wrapped)
	{
		throw null;
	}

	protected void Referenced(object? o)
	{
	}

	protected static Assembly? ResolveDynamicAssembly(string assemblyFullName)
	{
		throw null;
	}

	protected Array? ShrinkArray(Array? a, int length, Type elementType, bool isNullable)
	{
		throw null;
	}

	protected byte[]? ToByteArrayBase64(bool isNull)
	{
		throw null;
	}

	[return: NotNullIfNotNull("value")]
	protected static byte[]? ToByteArrayBase64(string? value)
	{
		throw null;
	}

	protected byte[]? ToByteArrayHex(bool isNull)
	{
		throw null;
	}

	[return: NotNullIfNotNull("value")]
	protected static byte[]? ToByteArrayHex(string? value)
	{
		throw null;
	}

	protected static char ToChar(string value)
	{
		throw null;
	}

	protected static DateTime ToDate(string value)
	{
		throw null;
	}

	protected static DateTime ToDateTime(string value)
	{
		throw null;
	}

	protected static long ToEnum(string value, Hashtable h, string typeName)
	{
		throw null;
	}

	protected static DateTime ToTime(string value)
	{
		throw null;
	}

	[return: NotNullIfNotNull("value")]
	protected static string? ToXmlName(string? value)
	{
		throw null;
	}

	[return: NotNullIfNotNull("value")]
	protected static string? ToXmlNCName(string? value)
	{
		throw null;
	}

	[return: NotNullIfNotNull("value")]
	protected static string? ToXmlNmToken(string? value)
	{
		throw null;
	}

	[return: NotNullIfNotNull("value")]
	protected static string? ToXmlNmTokens(string? value)
	{
		throw null;
	}

	protected XmlQualifiedName ToXmlQualifiedName(string? value)
	{
		throw null;
	}

	protected void UnknownAttribute(object? o, XmlAttribute attr)
	{
	}

	protected void UnknownAttribute(object? o, XmlAttribute attr, string? qnames)
	{
	}

	protected void UnknownElement(object? o, XmlElement elem)
	{
	}

	protected void UnknownElement(object? o, XmlElement elem, string? qnames)
	{
	}

	protected void UnknownNode(object? o)
	{
	}

	protected void UnknownNode(object? o, string? qnames)
	{
	}

	protected void UnreferencedObject(string? id, object? o)
	{
	}
}
