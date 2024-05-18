using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Property)]
public sealed class SettingsBindableAttribute : Attribute
{
	public static readonly SettingsBindableAttribute No;

	public static readonly SettingsBindableAttribute Yes;

	public bool Bindable
	{
		get
		{
			throw null;
		}
	}

	public SettingsBindableAttribute(bool bindable)
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
}
