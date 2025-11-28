using System.Collections.Generic;

namespace System.ComponentModel.DataAnnotations;

public class ValidationResult
{
	public static readonly ValidationResult? Success;

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

	public IEnumerable<string> MemberNames
	{
		get
		{
			throw null;
		}
	}

	protected ValidationResult(ValidationResult validationResult)
	{
	}

	public ValidationResult(string? errorMessage)
	{
	}

	public ValidationResult(string? errorMessage, IEnumerable<string>? memberNames)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
