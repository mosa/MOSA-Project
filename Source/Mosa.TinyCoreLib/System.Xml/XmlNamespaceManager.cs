using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Xml;

public class XmlNamespaceManager : IEnumerable, IXmlNamespaceResolver
{
	public virtual string DefaultNamespace
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlNameTable? NameTable
	{
		get
		{
			throw null;
		}
	}

	public XmlNamespaceManager(XmlNameTable nameTable)
	{
	}

	public virtual void AddNamespace(string prefix, [StringSyntax("Uri")] string uri)
	{
	}

	public virtual IEnumerator GetEnumerator()
	{
		throw null;
	}

	public virtual IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
	{
		throw null;
	}

	public virtual bool HasNamespace(string prefix)
	{
		throw null;
	}

	public virtual string? LookupNamespace(string prefix)
	{
		throw null;
	}

	public virtual string? LookupPrefix([StringSyntax("Uri")] string uri)
	{
		throw null;
	}

	public virtual bool PopScope()
	{
		throw null;
	}

	public virtual void PushScope()
	{
	}

	public virtual void RemoveNamespace(string prefix, [StringSyntax("Uri")] string uri)
	{
	}
}
