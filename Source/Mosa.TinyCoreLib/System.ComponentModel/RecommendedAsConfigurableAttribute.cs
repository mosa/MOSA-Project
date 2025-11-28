using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Property)]
[Obsolete("RecommendedAsConfigurableAttribute has been deprecated. Use System.ComponentModel.SettingsBindableAttribute instead.")]
public class RecommendedAsConfigurableAttribute : Attribute
{
	public static readonly RecommendedAsConfigurableAttribute Default;

	public static readonly RecommendedAsConfigurableAttribute No;

	public static readonly RecommendedAsConfigurableAttribute Yes;

	public bool RecommendedAsConfigurable
	{
		get
		{
			throw null;
		}
	}

	public RecommendedAsConfigurableAttribute(bool recommendedAsConfigurable)
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
