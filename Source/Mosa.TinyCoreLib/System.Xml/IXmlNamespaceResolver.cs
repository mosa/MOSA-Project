using System.Collections.Generic;

namespace System.Xml;

public interface IXmlNamespaceResolver
{
	IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope);

	string? LookupNamespace(string prefix);

	string? LookupPrefix(string namespaceName);
}
