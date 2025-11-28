namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public sealed class DynamicDependencyAttribute : Attribute
{
	public string? AssemblyName
	{
		get
		{
			throw null;
		}
	}

	public string? Condition
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? MemberSignature
	{
		get
		{
			throw null;
		}
	}

	public DynamicallyAccessedMemberTypes MemberTypes
	{
		get
		{
			throw null;
		}
	}

	public Type? Type
	{
		get
		{
			throw null;
		}
	}

	public string? TypeName
	{
		get
		{
			throw null;
		}
	}

	public DynamicDependencyAttribute(DynamicallyAccessedMemberTypes memberTypes, string typeName, string assemblyName)
	{
	}

	public DynamicDependencyAttribute(DynamicallyAccessedMemberTypes memberTypes, Type type)
	{
	}

	public DynamicDependencyAttribute(string memberSignature)
	{
	}

	public DynamicDependencyAttribute(string memberSignature, string typeName, string assemblyName)
	{
	}

	public DynamicDependencyAttribute(string memberSignature, Type type)
	{
	}
}
