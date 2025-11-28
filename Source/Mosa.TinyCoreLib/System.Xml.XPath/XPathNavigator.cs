using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Schema;

namespace System.Xml.XPath;

public abstract class XPathNavigator : XPathItem, ICloneable, IXmlNamespaceResolver, IXPathNavigable
{
	public abstract string BaseURI { get; }

	public virtual bool CanEdit
	{
		get
		{
			throw null;
		}
	}

	public virtual bool HasAttributes
	{
		get
		{
			throw null;
		}
	}

	public virtual bool HasChildren
	{
		get
		{
			throw null;
		}
	}

	public virtual string InnerXml
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public abstract bool IsEmptyElement { get; }

	public sealed override bool IsNode
	{
		get
		{
			throw null;
		}
	}

	public abstract string LocalName { get; }

	public abstract string Name { get; }

	public abstract string NamespaceURI { get; }

	public abstract XmlNameTable NameTable { get; }

	public static IEqualityComparer NavigatorComparer
	{
		get
		{
			throw null;
		}
	}

	public abstract XPathNodeType NodeType { get; }

	public virtual string OuterXml
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public abstract string Prefix { get; }

	public virtual IXmlSchemaInfo? SchemaInfo
	{
		get
		{
			throw null;
		}
	}

	public override object TypedValue
	{
		get
		{
			throw null;
		}
	}

	public virtual object? UnderlyingObject
	{
		get
		{
			throw null;
		}
	}

	public override bool ValueAsBoolean
	{
		get
		{
			throw null;
		}
	}

	public override DateTime ValueAsDateTime
	{
		get
		{
			throw null;
		}
	}

	public override double ValueAsDouble
	{
		get
		{
			throw null;
		}
	}

	public override int ValueAsInt
	{
		get
		{
			throw null;
		}
	}

	public override long ValueAsLong
	{
		get
		{
			throw null;
		}
	}

	public override Type ValueType
	{
		get
		{
			throw null;
		}
	}

	public virtual string XmlLang
	{
		get
		{
			throw null;
		}
	}

	public override XmlSchemaType? XmlType
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlWriter AppendChild()
	{
		throw null;
	}

	public virtual void AppendChild(string newChild)
	{
	}

	public virtual void AppendChild(XmlReader newChild)
	{
	}

	public virtual void AppendChild(XPathNavigator newChild)
	{
	}

	public virtual void AppendChildElement(string? prefix, string localName, string? namespaceURI, string? value)
	{
	}

	public virtual bool CheckValidity(XmlSchemaSet schemas, ValidationEventHandler validationEventHandler)
	{
		throw null;
	}

	public abstract XPathNavigator Clone();

	public virtual XmlNodeOrder ComparePosition(XPathNavigator? nav)
	{
		throw null;
	}

	public virtual XPathExpression Compile(string xpath)
	{
		throw null;
	}

	public virtual void CreateAttribute(string? prefix, string localName, string? namespaceURI, string? value)
	{
	}

	public virtual XmlWriter CreateAttributes()
	{
		throw null;
	}

	public virtual XPathNavigator CreateNavigator()
	{
		throw null;
	}

	public virtual void DeleteRange(XPathNavigator lastSiblingToDelete)
	{
	}

	public virtual void DeleteSelf()
	{
	}

	public virtual object Evaluate(string xpath)
	{
		throw null;
	}

	public virtual object Evaluate(string xpath, IXmlNamespaceResolver? resolver)
	{
		throw null;
	}

	public virtual object Evaluate(XPathExpression expr)
	{
		throw null;
	}

	public virtual object Evaluate(XPathExpression expr, XPathNodeIterator? context)
	{
		throw null;
	}

	public virtual string GetAttribute(string localName, string namespaceURI)
	{
		throw null;
	}

	public virtual string GetNamespace(string name)
	{
		throw null;
	}

	public virtual IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
	{
		throw null;
	}

	public virtual XmlWriter InsertAfter()
	{
		throw null;
	}

	public virtual void InsertAfter(string newSibling)
	{
	}

	public virtual void InsertAfter(XmlReader newSibling)
	{
	}

	public virtual void InsertAfter(XPathNavigator newSibling)
	{
	}

	public virtual XmlWriter InsertBefore()
	{
		throw null;
	}

	public virtual void InsertBefore(string newSibling)
	{
	}

	public virtual void InsertBefore(XmlReader newSibling)
	{
	}

	public virtual void InsertBefore(XPathNavigator newSibling)
	{
	}

	public virtual void InsertElementAfter(string? prefix, string localName, string? namespaceURI, string? value)
	{
	}

	public virtual void InsertElementBefore(string? prefix, string localName, string? namespaceURI, string? value)
	{
	}

	public virtual bool IsDescendant([NotNullWhen(true)] XPathNavigator? nav)
	{
		throw null;
	}

	public abstract bool IsSamePosition(XPathNavigator other);

	public virtual string? LookupNamespace(string prefix)
	{
		throw null;
	}

	public virtual string? LookupPrefix(string namespaceURI)
	{
		throw null;
	}

	public virtual bool Matches(string xpath)
	{
		throw null;
	}

	public virtual bool Matches(XPathExpression expr)
	{
		throw null;
	}

	public abstract bool MoveTo(XPathNavigator other);

	public virtual bool MoveToAttribute(string localName, string namespaceURI)
	{
		throw null;
	}

	public virtual bool MoveToChild(string localName, string namespaceURI)
	{
		throw null;
	}

	public virtual bool MoveToChild(XPathNodeType type)
	{
		throw null;
	}

	public virtual bool MoveToFirst()
	{
		throw null;
	}

	public abstract bool MoveToFirstAttribute();

	public abstract bool MoveToFirstChild();

	public bool MoveToFirstNamespace()
	{
		throw null;
	}

	public abstract bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope);

	public virtual bool MoveToFollowing(string localName, string namespaceURI)
	{
		throw null;
	}

	public virtual bool MoveToFollowing(string localName, string namespaceURI, XPathNavigator? end)
	{
		throw null;
	}

	public virtual bool MoveToFollowing(XPathNodeType type)
	{
		throw null;
	}

	public virtual bool MoveToFollowing(XPathNodeType type, XPathNavigator? end)
	{
		throw null;
	}

	public abstract bool MoveToId(string id);

	public virtual bool MoveToNamespace(string name)
	{
		throw null;
	}

	public abstract bool MoveToNext();

	public virtual bool MoveToNext(string localName, string namespaceURI)
	{
		throw null;
	}

	public virtual bool MoveToNext(XPathNodeType type)
	{
		throw null;
	}

	public abstract bool MoveToNextAttribute();

	public bool MoveToNextNamespace()
	{
		throw null;
	}

	public abstract bool MoveToNextNamespace(XPathNamespaceScope namespaceScope);

	public abstract bool MoveToParent();

	public abstract bool MoveToPrevious();

	public virtual void MoveToRoot()
	{
	}

	public virtual XmlWriter PrependChild()
	{
		throw null;
	}

	public virtual void PrependChild(string newChild)
	{
	}

	public virtual void PrependChild(XmlReader newChild)
	{
	}

	public virtual void PrependChild(XPathNavigator newChild)
	{
	}

	public virtual void PrependChildElement(string? prefix, string localName, string? namespaceURI, string? value)
	{
	}

	public virtual XmlReader ReadSubtree()
	{
		throw null;
	}

	public virtual XmlWriter ReplaceRange(XPathNavigator lastSiblingToReplace)
	{
		throw null;
	}

	public virtual void ReplaceSelf(string newNode)
	{
	}

	public virtual void ReplaceSelf(XmlReader newNode)
	{
	}

	public virtual void ReplaceSelf(XPathNavigator newNode)
	{
	}

	public virtual XPathNodeIterator Select(string xpath)
	{
		throw null;
	}

	public virtual XPathNodeIterator Select(string xpath, IXmlNamespaceResolver? resolver)
	{
		throw null;
	}

	public virtual XPathNodeIterator Select(XPathExpression expr)
	{
		throw null;
	}

	public virtual XPathNodeIterator SelectAncestors(string name, string namespaceURI, bool matchSelf)
	{
		throw null;
	}

	public virtual XPathNodeIterator SelectAncestors(XPathNodeType type, bool matchSelf)
	{
		throw null;
	}

	public virtual XPathNodeIterator SelectChildren(string name, string namespaceURI)
	{
		throw null;
	}

	public virtual XPathNodeIterator SelectChildren(XPathNodeType type)
	{
		throw null;
	}

	public virtual XPathNodeIterator SelectDescendants(string name, string namespaceURI, bool matchSelf)
	{
		throw null;
	}

	public virtual XPathNodeIterator SelectDescendants(XPathNodeType type, bool matchSelf)
	{
		throw null;
	}

	public virtual XPathNavigator? SelectSingleNode(string xpath)
	{
		throw null;
	}

	public virtual XPathNavigator? SelectSingleNode(string xpath, IXmlNamespaceResolver? resolver)
	{
		throw null;
	}

	public virtual XPathNavigator? SelectSingleNode(XPathExpression expression)
	{
		throw null;
	}

	public virtual void SetTypedValue(object typedValue)
	{
	}

	public virtual void SetValue(string value)
	{
	}

	object ICloneable.Clone()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public override object ValueAs(Type returnType, IXmlNamespaceResolver? nsResolver)
	{
		throw null;
	}

	public virtual void WriteSubtree(XmlWriter writer)
	{
	}
}
