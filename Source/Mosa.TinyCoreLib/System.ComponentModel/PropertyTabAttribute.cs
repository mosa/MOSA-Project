using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public class PropertyTabAttribute : Attribute
{
	public Type[] TabClasses
	{
		get
		{
			throw null;
		}
	}

	protected string[]? TabClassNames
	{
		get
		{
			throw null;
		}
	}

	public PropertyTabScope[] TabScopes
	{
		get
		{
			throw null;
		}
	}

	public PropertyTabAttribute()
	{
	}

	public PropertyTabAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] string tabClassName)
	{
	}

	public PropertyTabAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] string tabClassName, PropertyTabScope tabScope)
	{
	}

	public PropertyTabAttribute(Type tabClass)
	{
	}

	public PropertyTabAttribute(Type tabClass, PropertyTabScope tabScope)
	{
	}

	public bool Equals([NotNullWhen(true)] PropertyTabAttribute? other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Types referenced by tabClassNames may be trimmed.")]
	protected void InitializeArrays(string[]? tabClassNames, PropertyTabScope[]? tabScopes)
	{
	}

	protected void InitializeArrays(Type[]? tabClasses, PropertyTabScope[]? tabScopes)
	{
	}
}
