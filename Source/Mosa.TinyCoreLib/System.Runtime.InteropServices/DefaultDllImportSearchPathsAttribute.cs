namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Method, AllowMultiple = false)]
public sealed class DefaultDllImportSearchPathsAttribute : Attribute
{
	public DllImportSearchPath Paths
	{
		get
		{
			throw null;
		}
	}

	public DefaultDllImportSearchPathsAttribute(DllImportSearchPath paths)
	{
	}
}
