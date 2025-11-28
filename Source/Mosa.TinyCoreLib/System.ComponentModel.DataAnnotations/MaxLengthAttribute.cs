using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class MaxLengthAttribute : ValidationAttribute
{
	public int Length
	{
		get
		{
			throw null;
		}
	}

	[RequiresUnreferencedCode("Uses reflection to get the 'Count' property on types that don't implement ICollection. This 'Count' property may be trimmed. Ensure it is preserved.")]
	public MaxLengthAttribute()
	{
	}

	[RequiresUnreferencedCode("Uses reflection to get the 'Count' property on types that don't implement ICollection. This 'Count' property may be trimmed. Ensure it is preserved.")]
	public MaxLengthAttribute(int length)
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
