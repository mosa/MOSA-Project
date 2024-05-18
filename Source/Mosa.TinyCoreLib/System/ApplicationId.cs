using System.Diagnostics.CodeAnalysis;

namespace System;

public sealed class ApplicationId
{
	public string? Culture
	{
		get
		{
			throw null;
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public string? ProcessorArchitecture
	{
		get
		{
			throw null;
		}
	}

	public byte[] PublicKeyToken
	{
		get
		{
			throw null;
		}
	}

	public Version Version
	{
		get
		{
			throw null;
		}
	}

	public ApplicationId(byte[] publicKeyToken, string name, Version version, string? processorArchitecture, string? culture)
	{
	}

	public ApplicationId Copy()
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? o)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
