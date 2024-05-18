using System.Runtime.Versioning;

namespace System.Runtime.InteropServices.JavaScript;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
[SupportedOSPlatform("browser")]
public sealed class JSExportAttribute : Attribute
{
	public JSExportAttribute()
	{
		throw null;
	}
}
