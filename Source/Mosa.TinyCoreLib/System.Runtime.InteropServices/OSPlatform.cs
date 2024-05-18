using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.InteropServices;

public readonly struct OSPlatform : IEquatable<OSPlatform>
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public static OSPlatform FreeBSD
	{
		get
		{
			throw null;
		}
	}

	public static OSPlatform Linux
	{
		get
		{
			throw null;
		}
	}

	public static OSPlatform OSX
	{
		get
		{
			throw null;
		}
	}

	public static OSPlatform Windows
	{
		get
		{
			throw null;
		}
	}

	public static OSPlatform Create(string osPlatform)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(OSPlatform other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(OSPlatform left, OSPlatform right)
	{
		throw null;
	}

	public static bool operator !=(OSPlatform left, OSPlatform right)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
