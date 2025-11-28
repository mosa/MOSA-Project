using System.Runtime.Versioning;

namespace System.Runtime.InteropServices.JavaScript;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false, AllowMultiple = false)]
[SupportedOSPlatform("browser")]
public sealed class JSMarshalAsAttribute<T> : Attribute where T : JSType
{
	public JSMarshalAsAttribute()
	{
		throw null;
	}
}
