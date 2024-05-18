using System.Globalization;
using System.Resources;

namespace System.ComponentModel.Design;

public interface IResourceService
{
	IResourceReader? GetResourceReader(CultureInfo info);

	IResourceWriter GetResourceWriter(CultureInfo info);
}
