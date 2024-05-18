using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public sealed class RefreshPropertiesAttribute : Attribute
{
	public static readonly RefreshPropertiesAttribute All;

	public static readonly RefreshPropertiesAttribute Default;

	public static readonly RefreshPropertiesAttribute Repaint;

	public RefreshProperties RefreshProperties
	{
		get
		{
			throw null;
		}
	}

	public RefreshPropertiesAttribute(RefreshProperties refresh)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override bool IsDefaultAttribute()
	{
		throw null;
	}
}
