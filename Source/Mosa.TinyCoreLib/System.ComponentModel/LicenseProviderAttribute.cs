using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class LicenseProviderAttribute : Attribute
{
	public static readonly LicenseProviderAttribute Default;

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
	public Type? LicenseProvider
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

	public LicenseProviderAttribute()
	{
	}

	public LicenseProviderAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] string? typeName)
	{
	}

	public LicenseProviderAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type type)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}
}
