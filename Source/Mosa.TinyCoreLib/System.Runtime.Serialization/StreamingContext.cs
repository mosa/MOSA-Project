using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Serialization;

public readonly struct StreamingContext
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public object? Context
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public StreamingContextStates State
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public StreamingContext(StreamingContextStates state)
	{
		throw null;
	}

	[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public StreamingContext(StreamingContextStates state, object? additional)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}
}
