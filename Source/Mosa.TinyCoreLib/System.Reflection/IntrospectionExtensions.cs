using System.ComponentModel;

namespace System.Reflection;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class IntrospectionExtensions
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static TypeInfo GetTypeInfo(this Type type) => throw new NotImplementedException();
}
