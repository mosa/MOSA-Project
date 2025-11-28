using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.DataAnnotations;

public abstract class ValidationAttribute : Attribute
{
	public string? ErrorMessage
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? ErrorMessageResourceName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)]
	public Type? ErrorMessageResourceType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected string ErrorMessageString
	{
		get
		{
			throw null;
		}
	}

	public virtual bool RequiresValidationContext
	{
		get
		{
			throw null;
		}
	}

	protected ValidationAttribute()
	{
	}

	protected ValidationAttribute(Func<string> errorMessageAccessor)
	{
	}

	protected ValidationAttribute(string errorMessage)
	{
	}

	public virtual string FormatErrorMessage(string name)
	{
		throw null;
	}

	public ValidationResult? GetValidationResult(object? value, ValidationContext validationContext)
	{
		throw null;
	}

	public virtual bool IsValid(object? value)
	{
		throw null;
	}

	protected virtual ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		throw null;
	}

	public void Validate(object? value, ValidationContext validationContext)
	{
	}

	public void Validate(object? value, string name)
	{
	}
}
