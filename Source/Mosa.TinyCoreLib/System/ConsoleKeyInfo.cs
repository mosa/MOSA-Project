using System.Diagnostics.CodeAnalysis;

namespace System;

public readonly struct ConsoleKeyInfo : IEquatable<ConsoleKeyInfo>
{
	private readonly int _dummyPrimitive;

	public ConsoleKey Key
	{
		get
		{
			throw null;
		}
	}

	public char KeyChar
	{
		get
		{
			throw null;
		}
	}

	public ConsoleModifiers Modifiers
	{
		get
		{
			throw null;
		}
	}

	public ConsoleKeyInfo(char keyChar, ConsoleKey key, bool shift, bool alt, bool control)
	{
		throw null;
	}

	public bool Equals(ConsoleKeyInfo obj)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(ConsoleKeyInfo a, ConsoleKeyInfo b)
	{
		throw null;
	}

	public static bool operator !=(ConsoleKeyInfo a, ConsoleKeyInfo b)
	{
		throw null;
	}
}
