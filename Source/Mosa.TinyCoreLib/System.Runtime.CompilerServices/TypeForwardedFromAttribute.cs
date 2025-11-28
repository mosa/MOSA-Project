namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false, AllowMultiple = false)]
public sealed class TypeForwardedFromAttribute : Attribute
{
	public string AssemblyFullName
	{
		get
		{
			throw null;
		}
	}

	public TypeForwardedFromAttribute(string assemblyFullName)
	{
	}
}
