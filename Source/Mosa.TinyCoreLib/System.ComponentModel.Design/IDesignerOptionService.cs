using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.Design;

public interface IDesignerOptionService
{
	[RequiresUnreferencedCode("The option value's Type cannot be statically discovered.")]
	object? GetOptionValue(string pageName, string valueName);

	[RequiresUnreferencedCode("The option value's Type cannot be statically discovered.")]
	void SetOptionValue(string pageName, string valueName, object value);
}
