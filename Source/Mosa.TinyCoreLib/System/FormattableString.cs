using System.Diagnostics.CodeAnalysis;

namespace System;

public abstract class FormattableString : IFormattable
{
	public abstract int ArgumentCount { get; }

	[StringSyntax("CompositeFormat")]
	public abstract string Format { get; }

	public static string CurrentCulture(FormattableString formattable)
	{
		throw null;
	}

	public abstract object? GetArgument(int index);

	public abstract object?[] GetArguments();

	public static string Invariant(FormattableString formattable)
	{
		throw null;
	}

	string IFormattable.ToString(string? ignored, IFormatProvider? formatProvider)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public abstract string ToString(IFormatProvider? formatProvider);
}
