using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public class ViaHeaderValue : ICloneable
{
	public string? Comment
	{
		get
		{
			throw null;
		}
	}

	public string? ProtocolName
	{
		get
		{
			throw null;
		}
	}

	public string ProtocolVersion
	{
		get
		{
			throw null;
		}
	}

	public string ReceivedBy
	{
		get
		{
			throw null;
		}
	}

	public ViaHeaderValue(string protocolVersion, string receivedBy)
	{
	}

	public ViaHeaderValue(string protocolVersion, string receivedBy, string? protocolName)
	{
	}

	public ViaHeaderValue(string protocolVersion, string receivedBy, string? protocolName, string? comment)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static ViaHeaderValue Parse(string input)
	{
		throw null;
	}

	object ICloneable.Clone()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out ViaHeaderValue? parsedValue)
	{
		throw null;
	}
}
