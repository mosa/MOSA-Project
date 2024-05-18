using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

public static class FormattableStringFactory
{
	public static FormattableString Create([StringSyntax("CompositeFormat")] string format, params object?[] arguments)
	{
		throw null;
	}
}
