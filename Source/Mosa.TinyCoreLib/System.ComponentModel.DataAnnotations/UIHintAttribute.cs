using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class UIHintAttribute : Attribute
{
	public IDictionary<string, object?> ControlParameters
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

	public string UIHint
	{
		get
		{
			throw null;
		}
	}

	public UIHintAttribute(string uiHint)
	{
	}

	public UIHintAttribute(string uiHint, string? presentationLayer)
	{
	}

	public UIHintAttribute(string uiHint, string? presentationLayer, params object?[]? controlParameters)
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
