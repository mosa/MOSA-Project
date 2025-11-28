using System.Diagnostics.CodeAnalysis;

namespace System.Xml.Xsl;

public class XsltArgumentList
{
	public event XsltMessageEncounteredEventHandler XsltMessageEncountered
	{
		add
		{
		}
		remove
		{
		}
	}

	[RequiresUnreferencedCode("The stylesheet may have calls to methods of the extension object passed in which cannot be statically analyzed by the trimmer. Ensure all methods that may be called are preserved.")]
	public void AddExtensionObject(string namespaceUri, object extension)
	{
	}

	public void AddParam(string name, string namespaceUri, object parameter)
	{
	}

	public void Clear()
	{
	}

	[RequiresUnreferencedCode("The stylesheet may have calls to methods of the extension object passed in which cannot be statically analyzed by the trimmer. Ensure all methods that may be called are preserved.")]
	public object? GetExtensionObject(string namespaceUri)
	{
		throw null;
	}

	public object? GetParam(string name, string namespaceUri)
	{
		throw null;
	}

	public object? RemoveExtensionObject(string namespaceUri)
	{
		throw null;
	}

	public object? RemoveParam(string name, string namespaceUri)
	{
		throw null;
	}
}
