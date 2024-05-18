using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
[Obsolete("FilterUIHintAttribute has been deprecated and is not supported.")]
public sealed class FilterUIHintAttribute : Attribute
{
	public IDictionary<string, object?> ControlParameters
	{
		get
		{
			throw null;
		}
	}

	public string FilterUIHint
	{
		get
		{
			throw null;
		}
	}

	public string? PresentationLayer
	{
		get
		{
			throw null;
		}
	}

	public FilterUIHintAttribute(string filterUIHint)
	{
	}

	public FilterUIHintAttribute(string filterUIHint, string? presentationLayer)
	{
	}

	public FilterUIHintAttribute(string filterUIHint, string? presentationLayer, params object?[] controlParameters)
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
