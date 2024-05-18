namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
public sealed class InternalsVisibleToAttribute : Attribute
{
	public bool AllInternalsVisible
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string AssemblyName
	{
		get
		{
			throw null;
		}
	}

	public InternalsVisibleToAttribute(string assemblyName)
	{
	}
}
