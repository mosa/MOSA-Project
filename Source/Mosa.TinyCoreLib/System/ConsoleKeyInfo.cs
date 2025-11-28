using System.Diagnostics.CodeAnalysis;

namespace System;

public readonly struct ConsoleKeyInfo : IEquatable<ConsoleKeyInfo>
{
	public ConsoleKey Key { get; }

	public char KeyChar { get; }

	public ConsoleModifiers Modifiers { get; }

	public ConsoleKeyInfo(char keyChar, ConsoleKey key, bool shift, bool alt, bool control)
	{
		var modifiers = ConsoleModifiers.None;
		if (shift) modifiers |= ConsoleModifiers.Shift;
		if (alt) modifiers |= ConsoleModifiers.Alt;
		if (control) modifiers |= ConsoleModifiers.Control;

		KeyChar = keyChar;
		Key = key;
		Modifiers = modifiers;
	}

	public bool Equals(ConsoleKeyInfo obj) => KeyChar == obj.KeyChar && Key == obj.Key && Modifiers == obj.Modifiers;

	public override bool Equals([NotNullWhen(true)] object? value) => value is ConsoleKeyInfo key && Equals(key);

	public override int GetHashCode() => HashCode.Combine(KeyChar, Key, Modifiers);

	public static bool operator ==(ConsoleKeyInfo a, ConsoleKeyInfo b) => a.Equals(b);

	public static bool operator !=(ConsoleKeyInfo a, ConsoleKeyInfo b) => !a.Equals(b);
}
