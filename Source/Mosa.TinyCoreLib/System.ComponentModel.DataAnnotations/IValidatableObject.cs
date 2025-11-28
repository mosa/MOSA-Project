using System.Collections.Generic;

namespace System.ComponentModel.DataAnnotations;

public interface IValidatableObject
{
	IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
}
