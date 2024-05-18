using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;

namespace System.ComponentModel;

public class ComponentResourceManager : ResourceManager
{
	public ComponentResourceManager()
	{
	}

	public ComponentResourceManager(Type t)
	{
	}

	[RequiresUnreferencedCode("The Type of value cannot be statically discovered.")]
	public void ApplyResources(object value, string objectName)
	{
	}

	[RequiresUnreferencedCode("The Type of value cannot be statically discovered.")]
	public virtual void ApplyResources(object value, string objectName, CultureInfo? culture)
	{
	}
}
