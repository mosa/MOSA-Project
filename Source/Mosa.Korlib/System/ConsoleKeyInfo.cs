// Copyright (c) MOSA Project. Licensed under the New BSD License.

// ReSharper disable once CheckNamespace
namespace System;

public readonly struct ConsoleKeyInfo : IEquatable<ConsoleKeyInfo>
{
    public ConsoleKeyInfo(char keyChar, ConsoleKey key, bool shift, bool alt, bool control)
    {
        this.KeyChar = keyChar;
        this.Key = key;

        ConsoleModifiers modifiers = ConsoleModifiers.None;

        if (shift) modifiers |= ConsoleModifiers.Shift;
        if (alt) modifiers |= ConsoleModifiers.Alt;
        if (control) modifiers |= ConsoleModifiers.Control;

        this.Modifiers = modifiers;
    }

    public readonly char KeyChar;
    public readonly ConsoleKey Key;
    public readonly ConsoleModifiers Modifiers;

    public bool Equals(ConsoleKeyInfo other)
    {
	    return this.KeyChar == other.KeyChar && this.Key == other.Key && this.Modifiers == other.Modifiers;
    }

    public override bool Equals(object obj)
    {
	    return obj is ConsoleKeyInfo other && Equals(other);
    }

    public override int GetHashCode()
    {
	    return this.KeyChar;
    }

    public static bool operator ==(ConsoleKeyInfo left, ConsoleKeyInfo right)
    {
	    return left.Equals(right);
    }

    public static bool operator !=(ConsoleKeyInfo left, ConsoleKeyInfo right)
    {
	    return !left.Equals(right);
    }
}
