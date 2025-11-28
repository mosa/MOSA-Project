using System.Diagnostics.CodeAnalysis;

namespace System.Text;

public sealed class CompositeFormat
{
	public string Format
	{
		get
		{
			throw null;
		}
	}

	public int MinimumArgumentCount
	{
		get
		{
			throw null;
		}
	}

	internal CompositeFormat()
	{
	}

	public static CompositeFormat Parse([StringSyntax("CompositeFormat")] string format)
	{
		throw null;
	}
}
