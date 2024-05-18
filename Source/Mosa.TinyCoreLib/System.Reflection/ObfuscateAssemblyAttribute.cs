namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
public sealed class ObfuscateAssemblyAttribute : Attribute
{
	public bool AssemblyIsPrivate
	{
		get
		{
			throw null;
		}
	}

	public bool StripAfterObfuscation
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ObfuscateAssemblyAttribute(bool assemblyIsPrivate)
	{
	}
}
