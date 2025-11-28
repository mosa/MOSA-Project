using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class LengthAttribute : ValidationAttribute
{
	public int MaximumLength
	{
		get
		{
			throw null;
		}
	}

	public int MinimumLength
	{
		get
		{
			throw null;
		}
	}

	[RequiresUnreferencedCode("Uses reflection to get the 'Count' property on types that don't implement ICollection. This 'Count' property may be trimmed. Ensure it is preserved.")]
	public LengthAttribute(int minimumLength, int maximumLength)
	{
	}

	public override string FormatErrorMessage(string name)
	{
		throw null;
	}

	public override bool IsValid(object? value)
	{
		throw null;
	}
}
