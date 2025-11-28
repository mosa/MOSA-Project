using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
public sealed class CustomValidationAttribute : ValidationAttribute
{
	public string Method
	{
		get
		{
			throw null;
		}
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
	public Type ValidatorType
	{
		get
		{
			throw null;
		}
	}

	public CustomValidationAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] Type validatorType, string method)
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
