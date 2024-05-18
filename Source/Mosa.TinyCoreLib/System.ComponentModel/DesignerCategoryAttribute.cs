using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class DesignerCategoryAttribute : Attribute
{
	public static readonly DesignerCategoryAttribute Component;

	public static readonly DesignerCategoryAttribute Default;

	public static readonly DesignerCategoryAttribute Form;

	public static readonly DesignerCategoryAttribute Generic;

	public string Category
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

	public DesignerCategoryAttribute()
	{
	}

	public DesignerCategoryAttribute(string category)
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
