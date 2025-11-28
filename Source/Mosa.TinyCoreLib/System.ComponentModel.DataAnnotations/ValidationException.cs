using System.Runtime.Serialization;

namespace System.ComponentModel.DataAnnotations;

public class ValidationException : Exception
{
	public ValidationAttribute? ValidationAttribute
	{
		get
		{
			throw null;
		}
	}

	public ValidationResult ValidationResult
	{
		get
		{
			throw null;
		}
	}

	public object? Value
	{
		get
		{
			throw null;
		}
	}

	public ValidationException()
	{
	}

	public ValidationException(ValidationResult validationResult, ValidationAttribute? validatingAttribute, object? value)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected ValidationException(SerializationInfo info, StreamingContext context)
	{
	}

	public ValidationException(string? message)
	{
	}

	public ValidationException(string? errorMessage, ValidationAttribute? validatingAttribute, object? value)
	{
	}

	public ValidationException(string? message, Exception? innerException)
	{
	}
}
