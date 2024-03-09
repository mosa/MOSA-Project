// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Text;

public class StringBuilder
{
	private char[] characters;
	private int length;

	public StringBuilder() => characters = new char[byte.MaxValue];

	public StringBuilder(int capacity) => characters = new char[capacity];

	public StringBuilder Clear()
	{
		length = 0;
		return this;
	}

	public StringBuilder Append(char c)
	{
		EnsureCapacity();

		characters[length++] = c;
		return this;
	}

	public StringBuilder Append(char c, int count)
	{
		EnsureCapacity(count);

		for (var i = 0; i < count; i++)
			characters[length++] = c;

		return this;
	}

	public StringBuilder Append(string s)
	{
		EnsureCapacity(s.length);

		foreach (var c in s)
			characters[length++] = c;

		return this;
	}

	public StringBuilder AppendLine(string s)
	{
		EnsureCapacity(s.length);

		foreach (var c in s)
			characters[length++] = c;

		characters[length++] = '\n';
		return this;
	}

	public char[] ToCharArray() => characters;

	public override string ToString() => new string(characters, 0, length);

	private void EnsureCapacity(int capacity = 0)
	{
		var newLength = length + capacity;
		if (newLength < characters.Length)
			return;

		var chars = new char[newLength + 100];
		for (var i = 0; i < length; i++)
			chars[i] = characters[i];

		characters = chars;
	}
}
