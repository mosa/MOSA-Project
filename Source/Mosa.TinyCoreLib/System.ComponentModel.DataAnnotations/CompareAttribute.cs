using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class CompareAttribute : ValidationAttribute
{
	public string OtherProperty
	{
		get
		{
			throw null;
		}
	}

	public string? OtherPropertyDisplayName
	{
		get
		{
			throw null;
		}
	}

	public override bool RequiresValidationContext
	{
		get
		{
			throw null;
		}
	}

	[RequiresUnreferencedCode("The property referenced by 'otherProperty' may be trimmed. Ensure it is preserved.")]
	public CompareAttribute(string otherProperty)
	{
	}

	public override string FormatErrorMessage(string name)
	{
		throw null;
	}

	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		throw null;
	}
}
