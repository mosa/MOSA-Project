using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography;

public struct CngProperty : IEquatable<CngProperty>
{
	private object _dummy;

	private int _dummyPrimitive;

	public readonly string Name
	{
		get
		{
			throw null;
		}
	}

	public readonly CngPropertyOptions Options
	{
		get
		{
			throw null;
		}
	}

	public CngProperty(string name, byte[]? value, CngPropertyOptions options)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(CngProperty other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public byte[]? GetValue()
	{
		throw null;
	}

	public static bool operator ==(CngProperty left, CngProperty right)
	{
		throw null;
	}

	public static bool operator !=(CngProperty left, CngProperty right)
	{
		throw null;
	}
}
