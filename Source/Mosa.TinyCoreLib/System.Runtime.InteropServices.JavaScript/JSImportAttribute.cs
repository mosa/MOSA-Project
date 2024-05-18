using System.Runtime.Versioning;

namespace System.Runtime.InteropServices.JavaScript;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
[SupportedOSPlatform("browser")]
public sealed class JSImportAttribute : Attribute
{
	public string FunctionName
	{
		get
		{
			throw null;
		}
	}

	public string? ModuleName
	{
		get
		{
			throw null;
		}
	}

	public JSImportAttribute(string functionName)
	{
		throw null;
	}

	public JSImportAttribute(string functionName, string moduleName)
	{
		throw null;
	}
}
