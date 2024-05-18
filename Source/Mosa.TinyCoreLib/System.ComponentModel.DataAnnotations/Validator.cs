using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.DataAnnotations;

public static class Validator
{
	[RequiresUnreferencedCode("The Type of instance cannot be statically discovered and the Type's properties can be trimmed.")]
	public static bool TryValidateObject(object instance, ValidationContext validationContext, ICollection<ValidationResult>? validationResults)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of instance cannot be statically discovered and the Type's properties can be trimmed.")]
	public static bool TryValidateObject(object instance, ValidationContext validationContext, ICollection<ValidationResult>? validationResults, bool validateAllProperties)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of validationContext.ObjectType cannot be statically discovered.")]
	public static bool TryValidateProperty(object? value, ValidationContext validationContext, ICollection<ValidationResult>? validationResults)
	{
		throw null;
	}

	public static bool TryValidateValue(object? value, ValidationContext validationContext, ICollection<ValidationResult>? validationResults, IEnumerable<ValidationAttribute> validationAttributes)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of instance cannot be statically discovered and the Type's properties can be trimmed.")]
	public static void ValidateObject(object instance, ValidationContext validationContext)
	{
	}

	[RequiresUnreferencedCode("The Type of instance cannot be statically discovered and the Type's properties can be trimmed.")]
	public static void ValidateObject(object instance, ValidationContext validationContext, bool validateAllProperties)
	{
	}

	[RequiresUnreferencedCode("The Type of validationContext.ObjectType cannot be statically discovered.")]
	public static void ValidateProperty(object? value, ValidationContext validationContext)
	{
	}

	public static void ValidateValue(object? value, ValidationContext validationContext, IEnumerable<ValidationAttribute> validationAttributes)
	{
	}
}
