using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.VisualBasic.CompilerServices;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class Versioned
{
	internal Versioned()
	{
	}

	[RequiresUnreferencedCode("The method name cannot and type cannot be statically analyzed so it may be trimmed")]
	public static object? CallByName(object? Instance, string MethodName, CallType UseCallType, params object?[]? Arguments)
	{
		throw null;
	}

	public static bool IsNumeric(object? Expression)
	{
		throw null;
	}

	public static string? SystemTypeName(string? VbName)
	{
		throw null;
	}

	public static string TypeName(object? Expression)
	{
		throw null;
	}

	public static string? VbTypeName(string? SystemName)
	{
		throw null;
	}
}
