using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public class ToolboxItemAttribute : Attribute
{
	public static readonly ToolboxItemAttribute Default;

	public static readonly ToolboxItemAttribute None;

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
	public Type? ToolboxItemType
	{
		get
		{
			throw null;
		}
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
	public string ToolboxItemTypeName
	{
		get
		{
			throw null;
		}
	}

	public ToolboxItemAttribute(bool defaultType)
	{
	}

	public ToolboxItemAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] string toolboxItemTypeName)
	{
	}

	public ToolboxItemAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type toolboxItemType)
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
