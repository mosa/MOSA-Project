using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class ToolboxItemFilterAttribute : Attribute
{
	public string FilterString
	{
		get
		{
			throw null;
		}
	}

	public ToolboxItemFilterType FilterType
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

	public ToolboxItemFilterAttribute(string filterString)
	{
	}

	public ToolboxItemFilterAttribute(string filterString, ToolboxItemFilterType filterType)
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

	public override bool Match([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
