using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.VisualBasic.CompilerServices;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class LikeOperator
{
	internal LikeOperator()
	{
	}

	[RequiresUnreferencedCode("The types of source and pattern cannot be statically analyzed so the like operator may be trimmed")]
	public static object LikeObject(object? Source, object? Pattern, CompareMethod CompareOption)
	{
		throw null;
	}

	public static bool LikeString(string? Source, string? Pattern, CompareMethod CompareOption)
	{
		throw null;
	}
}
