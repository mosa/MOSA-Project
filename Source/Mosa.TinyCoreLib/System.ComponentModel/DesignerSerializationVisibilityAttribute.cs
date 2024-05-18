using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event)]
public sealed class DesignerSerializationVisibilityAttribute : Attribute
{
	public static readonly DesignerSerializationVisibilityAttribute Content;

	public static readonly DesignerSerializationVisibilityAttribute Default;

	public static readonly DesignerSerializationVisibilityAttribute Hidden;

	public static readonly DesignerSerializationVisibilityAttribute Visible;

	public DesignerSerializationVisibility Visibility
	{
		get
		{
			throw null;
		}
	}

	public DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility visibility)
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
