using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
public sealed class EditorAttribute : Attribute
{
	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
	public string? EditorBaseTypeName
	{
		get
		{
			throw null;
		}
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
	public string EditorTypeName
	{
		get
		{
			throw null;
		}
	}

	public override object TypeId
	{
		get
		{
			throw null;
		}
	}

	public EditorAttribute()
	{
	}

	public EditorAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] string typeName, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] string? baseTypeName)
	{
	}

	public EditorAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] string typeName, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type baseType)
	{
	}

	public EditorAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type baseType)
	{
	}

	public override bool Equals(object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}
}
