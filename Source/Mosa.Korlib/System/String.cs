// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Stack = System.Collections.Stack;

namespace System;

/// <summary>
/// Implementation of the "System.String" class
/// </summary>
public sealed class String : IEnumerable, IEnumerable<char>, IEquatable<String>, IComparable, IComparable<String>
{
	/// <summary>
	/// Length
	/// </summary>
	internal int length;

	private readonly char start_char;

	public int Length { get { return length; } }

	public static string Empty = "";

	internal unsafe char* first_char
	{
		get
		{
			fixed (char* c = &start_char)
			{
				return c;
			}
		}
	}


	internal unsafe ref char GetRawStringData() => ref *first_char;

	[IndexerName("Chars")]
	public unsafe char this[int index]
	{
		get
		{
			if (index < 0 || index >= length)
				throw new IndexOutOfRangeException();

			return first_char[index];
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern String(ReadOnlySpan<char> value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern String(char c, int count);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern String(char[] value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern String(char[] value, int startIndex, int length);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern unsafe String(sbyte* value, int startIndex, int length);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern unsafe String(sbyte* value);

	//[MethodImpl(MethodImplOptions.InternalCall)]
	//public unsafe extern String(sbyte* value, int startIndex, int length, Encoding enc);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public unsafe extern String(char* value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public unsafe extern String(char* value, int startIndex, int length);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern string InternalAllocateString(int length);

	private static unsafe string Ctor(ReadOnlySpan<char> value)
	{
		int len = value.Length;

		if (len == 0)
			return Empty;

		// TODO: Actually move memory instead of copying it
		string result = InternalAllocateString(len);
		char* chars = result.first_char;

		var pointer = MemoryMarshal.GetReference(value);
		byte* ptr = (byte*)Unsafe.As<char, byte>(ref pointer);

		for (int i = 0; i < len; i++)
		{
			*chars = (char)ptr[i];
			chars++;
		}

		return result;
	}

	private static unsafe string Ctor(char c, int count)
	{
		string result = InternalAllocateString(count);
		char* chars = result.first_char;

		while (count > 0)
		{
			*chars = c;
			count--;
			chars++;
		}

		return result;
	}

	private static string Ctor(char[] value)
	{
		return Ctor(value, 0, value.Length);
	}

	private static unsafe string Ctor(char[] value, int startIndex, int length)
	{
		if (length == 0)
			return Empty;

		string result = InternalAllocateString(length);
		char* chars = result.first_char;

		for (int index = startIndex; index < startIndex + length; index++)
		{
			*chars = value[index];
			chars++;
		}

		return result;
	}

	private static unsafe string Ctor(string source, int startIndex, int length)
	{
		if (source.Length == 0)
			return Empty;

		string result = InternalAllocateString(length);
		char* chars = result.first_char;

		for (int index = startIndex; index < startIndex + length; index++)
		{
			*chars = source[index];
			chars++;
		}

		return result;
	}

	private static unsafe string Ctor(sbyte* value, int startIndex, int length)
	{
		if (length == 0)
			return Empty;

		string result = InternalAllocateString(length);
		char* chars = result.first_char;

		value += startIndex;

		for (int index = 0; index < length; index++)
		{
			*chars = (char)*value;
			chars++;
			value++;
		}

		return result;
	}

	private static unsafe string Ctor(sbyte* value)
	{
		int length = 0;
		sbyte* at = value;

		while (*at != 0)
		{
			length++;
			at++;
		}

		if (length == 0)
			return Empty;

		string result = InternalAllocateString(length);
		char* chars = result.first_char;

		for (int index = 0; index < length; index++)
		{
			*chars = (char)*value;
			chars++;
			value++;
		}

		return result;
	}

	private static unsafe string Ctor(char* value, int startIndex, int length)
	{
		if (length == 0)
			return InternalAllocateString(0);

		string result = InternalAllocateString(length);
		char* chars = result.first_char;

		value += startIndex;

		for (int index = 0; index < length; index++)
		{
			*chars = *value;
			chars++;
			value++;
		}

		return result;
	}

	private static unsafe string Ctor(char* value)
	{
		int length = 0;
		char* at = value;

		while (*at != 0)
		{
			length++;
		}

		if (length == 0)
			return Empty;

		string result = InternalAllocateString(length);
		char* chars = result.first_char;

		for (int index = 0; index < length; index++)
		{
			*chars = *value;
			chars++;
			value++;
		}

		return result;
	}

	public string[] Split(char c)
	{
		var str = this;
		var ls = new List<string>();

		int indx;

		while ((indx = str.IndexOf(c)) != -1)
		{
			ls.Add(str.Substring(0, indx));
			str = str.Substring(indx + 1);
		}

		if (str.Length > 0)
			ls.Add(str);

		return ls.ToArray();
	}

	public string[] Split(string s)
	{
		var str = this;
		var ls = new List<string>();

		int indx;

		while ((indx = str.IndexOf(s)) != -1)
		{
			ls.Add(str.Substring(0, indx));
			str = str.Substring(indx + s.length);
		}

		if (str.Length > 0)
			ls.Add(str);

		return ls.ToArray();
	}

	public string Replace(string s1, string s2)
	{
		if (IsNullOrEmpty(s1) || !Contains(s1))
			return this;

		var str = this;

		string start;

		while (str.IndexOf(s1) > -1)
		{
			var index = str.IndexOf(s1);

			// Get start
			start = str.Substring(0, index);

			// Get end
			str = str.Substring(index + s1.Length);

			// Replace occurence
			str = $"{start}{s2}{str}";
		}

		return str;
	}

	public string Insert(int startIndex, string str)
	{
		if (startIndex == 0) return str + this;
		return this.Substring(0, startIndex) + str + this.Substring(startIndex);
	}

	public string Remove(int startIndex, int count)
	{
		return this.Substring(0, startIndex) + this.Substring(startIndex + count);
	}

	public static string Format(string format, params object[] args)
	{
		return format;
	}

	public bool Equals(string s)
	{
		return Equals(this, s);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is string))
			return false;

		return Equals(this, (string)obj);
	}

	public static bool operator ==(string a, string b)
	{
		return Equals(a, b);
	}

	public static bool operator !=(string a, string b)
	{
		return !Equals(a, b);
	}

	public static unsafe implicit operator ReadOnlySpan<char>(string s)
	{
		if (s == null) return ReadOnlySpan<char>.Empty;
		return new ReadOnlySpan<char>(s.first_char, s.length);
	}

	public static implicit operator string(Span<char> span)
	{
		return span.ToString();
	}

	public static unsafe bool Equals(string a, string b)
	{
		if (a == null || b == null) { return false; }

		if (a.length != b.length) { return false; }

		char* pa = a.first_char;
		char* pb = b.first_char;

		for (int i = 0; i < a.Length; ++i)
			if (pa[i] != pb[i]) { return false; }

		return true;
	}

	// Favor Invariant Culture & Ignore Case
	public int CompareTo(object obj)
	{
		if (ReferenceEquals(this, obj)) { return 0; }
		if (obj == null) { return 1; }
		if (!(obj is string)) { throw new ArgumentException("Argument Type Must Be String", "value"); }

		return CompareTo((string)obj);
	}

	// Favor Invariant Culture & Ignore Case
	public int CompareTo (string obj)
	{
		return Compare(this, obj);
	}

	// Favor Invariant Culture & Ignore Case
	public int Compare (string left, string right)
	{
		left = left.ToLower();
		right = right.ToLower();

		if (Equals(left, right)) { return 0; }

		for (int counter = 0; counter < Math.Min(left.Length, right.Length); counter++)
		{
			if (left[counter] < right[counter]) { return -1; }
			if (left[counter] > right[counter]) { return 1; }
		}

		if (left.Length < right.Length) { return -1; }
		if (left.Length > right.Length) { return 1; }

		return 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return base.GetHashCode();

		//ulong seed = Marvin.DefaultSeed;

		//// Multiplication below will not overflow since going from positive Int32 to UInt32.
		//return Marvin.ComputeHash32(ref Unsafe.As<char, byte>(ref _firstChar), (uint)_stringLength * 2 /* in bytes, not chars */, (uint)seed, (uint)(seed >> 32));
	}

	public override string ToString()
	{
		return this;
	}

	public List<string> Split(char delimiter, string text)
	{
		var ret = new List<string>();
		int startPos = 0;
		string temp = Empty;

		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] == delimiter)
			{
				temp = text.Substring(startPos, text.IndexOf(delimiter) - startPos);

				startPos = i + 1;
			}
		}
		if (temp != Empty)
		{
			ret.Add(temp);
		}

		return ret;
	}

	public unsafe string ToUpper()
	{
		string result = InternalAllocateString(length);
		char* chars = result.first_char;
		char* self = first_char;

		for (int i = 0; i < length; i++)
		{
			if (self[i] >= 97 && self[i] <= 122)
				*chars = (char)(self[i] - 32);
			else
				*chars = self[i];
			chars++;
		}

		return result;
	}

	public unsafe string ToLower()
	{
		string result = InternalAllocateString(length);
		char* chars = result.first_char;
		char* self = first_char;

		for (int i = 0; i < length; i++)
		{
			if (self[i] >= 65 && self[i] <= 90)
				*chars = (char)(self[i] + 32);
			else
				*chars = self[i];
			chars++;
		}

		return result;
	}

	// TODO: Seems some compiler bugs prevent the original algorithms from working...
	public unsafe static string Concat(string a, string b)
	{
		a ??= Empty;
		b ??= Empty;

		string result = InternalAllocateString(a.length + b.length);
		char* chars = result.first_char;

		char* aPtr = a.first_char;
		char* bPtr = b.first_char;
		for (int i = 0; i < a.length; i++)
		{
			*chars = aPtr[i];
			chars++;
		}
		for (int i = 0; i < b.length; i++)
		{
			*chars = bPtr[i];
			chars++;
		}

		return result;
	}

	public unsafe static string Concat(string a, string b, string c)
	{
		a ??= Empty;
		b ??= Empty;
		c ??= Empty;

		string result = InternalAllocateString(a.length + b.length + c.length);
		char* chars = result.first_char;

		char* aPtr = a.first_char;
		char* bPtr = b.first_char;
		char* cPtr = c.first_char;
		for (int i = 0; i < a.length; i++)
		{
			*chars = aPtr[i];
			chars++;
		}
		for (int i = 0; i < b.length; i++)
		{
			*chars = bPtr[i];
			chars++;
		}
		for (int i = 0; i < c.length; i++)
		{
			*chars = cPtr[i];
			chars++;
		}

		return result;
	}

	public unsafe static string Concat(string a, string b, string c, string d)
	{
		a ??= Empty;
		b ??= Empty;
		c ??= Empty;
		d ??= Empty;

		string result = InternalAllocateString(a.length + b.length + c.length + d.length);
		char* chars = result.first_char;

		char* aPtr = a.first_char;
		char* bPtr = b.first_char;
		char* cPtr = c.first_char;
		char* dPtr = d.first_char;
		for (int i = 0; i < a.length; i++)
		{
			*chars = aPtr[i];
			chars++;
		}
		for (int i = 0; i < b.length; i++)
		{
			*chars = bPtr[i];
			chars++;
		}
		for (int i = 0; i < c.length; i++)
		{
			*chars = cPtr[i];
			chars++;
		}
		for (int i = 0; i < d.length; i++)
		{
			*chars = dPtr[i];
			chars++;
		}

		return result;
	}

	public static string Concat(object a)
	{
		return a.ToString();
	}

	public static string Concat(object a, object b)
	{
		return Concat(a.ToString(), b.ToString());
	}

	public static string Concat(object a, object b, object c)
	{
		return Concat(a.ToString(), b.ToString(), c.ToString());
	}

	public static string Concat(object a, object b, object c, object d)
	{
		return Concat(a.ToString(), b.ToString(), c.ToString(), d.ToString());
	}

	public static string Concat(params object[] args)
	{
		if (args.Length == 0)
			return Empty;

		string result = args[0].ToString();

		for (int i = 1; i < args.Length; ++i)
			result = Concat(result, args[i].ToString());

		return result;
	}

	public static string Concat(string[] objects)
	{
		if (objects.Length == 0)
			return Empty;

		string result = objects[0].ToString();

		for (int i = 1; i < objects.Length; ++i)
			result = Concat(result, objects[i].ToString());

		return result;
	}

	public unsafe string Substring(int startIndex)
	{
		if (startIndex == 0)
			return Empty;

		// FIXME: Following line does not compile correctly
		if (startIndex < 0 || startIndex > length)
			throw new ArgumentOutOfRangeException("startIndex");

		if (startIndex < 0)
			throw new ArgumentOutOfRangeException("startIndex");

		if (startIndex > length)
			throw new ArgumentOutOfRangeException("startIndex");

		int newlen = length - startIndex;
		string result = InternalAllocateString(newlen);

		char* chars = result.first_char;

		for (int index = 0; index < newlen; index++)
			*chars++ = this[startIndex + index];

		return result;
	}

	public unsafe string Substring(int startIndex, int length)
	{
		if (length < 0)
			throw new ArgumentOutOfRangeException("length", "< 0");

		if (startIndex < 0 || startIndex > this.length)
			throw new ArgumentOutOfRangeException("startIndex");

		string result = InternalAllocateString(length);

		char* chars = result.first_char;

		for (int index = 0; index < length; index++)
			*chars++ = this[startIndex + index];

		return result;
	}

	public unsafe bool StartsWith(string value)
	{
		if (value == null)
			throw new ArgumentNullException(nameof(value));

		if (value.length > length)
			return false;

		var thisChar = first_char;
		var cmpChar = value.first_char;
		for (var i = 0; i < value.length; i++)
		{
			if (*thisChar != *cmpChar)
				return false;
			thisChar++;
			cmpChar++;
		}
		return true;
	}

	public unsafe bool EndsWith(string value)
	{
		if (value == null)
			throw new ArgumentNullException(nameof(value));

		if (value.length > length)
			return false;

		var thisChar = first_char + (length - value.length);
		var cmpChar = value.first_char;
		for (var i = 0; i < value.length; i++)
		{
			if (*thisChar != *cmpChar)
				return false;
			thisChar++;
			cmpChar++;
		}
		return true;
	}

	public char[] ToCharArray()
	{
		char[] array = new char[Length];

		for (int i = 0; i < Length; i++)
			array[i] = this[i];

		return array;
	}

	public static bool IsNullOrEmpty(string value)
	{
		return (value == null) || (value.Length == 0);
	}

	public static bool IsNullOrWhiteSpace(string value)
	{
		if (value == null) return true;

		for (var i = 0; i < value.Length; i++)
		{
			if (!char.IsWhiteSpace(value[i]))
				return false;
		}

		return true;
	}

	public bool Contains(string value)
	{
		return IndexOf(value) > -1;
	}

	public bool Contains(char value)
	{
		return IndexOf(value) > -1;
	}

	public int IndexOf(string value)
	{
		if (length == 0)
			return -1;

		return IndexOfImpl(value, 0, length);
	}

	public int IndexOf(char value)
	{
		if (length == 0)
			return -1;

		return IndexOfImpl(value, 0, length);
	}

	public int IndexOf(char value, int startIndex)
	{
		return IndexOf(value, startIndex, length - startIndex);
	}

	public int IndexOf(char value, int startIndex, int count)
	{
		if (startIndex < 0)
			throw new ArgumentOutOfRangeException("startIndex", "< 0");
		if (count < 0)
			throw new ArgumentOutOfRangeException("count", "< 0");
		if (startIndex > length - count)
			throw new ArgumentOutOfRangeException("startIndex + count > this.length");

		if ((startIndex == 0 && length == 0) || (startIndex == length) || (count == 0))
			return -1;

		return IndexOfImpl(value, startIndex, count);
	}

	public int IndexOfAny(char[] anyOf)
	{
		if (anyOf == null)
			throw new ArgumentNullException("anyOf");
		if (length == 0)
			return -1;

		return IndexOfAnyImpl(anyOf, 0, length);
	}

	public int IndexOfAny(char[] anyOf, int startIndex)
	{
		if (anyOf == null)
			throw new ArgumentNullException("anyOf");
		if (startIndex < 0 || startIndex > length)
			throw new ArgumentOutOfRangeException("startIndex");

		return IndexOfAnyImpl(anyOf, startIndex, length - startIndex);
	}

	public int IndexOfAny(char[] anyOf, int startIndex, int count)
	{
		if (anyOf == null)
			throw new ArgumentNullException("anyOf");
		if (startIndex < 0)
			throw new ArgumentOutOfRangeException("startIndex", "< 0");
		if (count < 0)
			throw new ArgumentOutOfRangeException("count", "< 0");
		if (startIndex > length - count)
			throw new ArgumentOutOfRangeException("startIndex + count > this.length");

		return IndexOfAnyImpl(anyOf, startIndex, count);
	}

	public int LastIndexOfAny(char[] anyOf)
	{
		if (anyOf == null)
			throw new ArgumentNullException("anyOf");

		return InternalLastIndexOfAny(anyOf, length - 1, length);
	}

	public int LastIndexOfAny(char[] anyOf, int startIndex)
	{
		if (anyOf == null)
			throw new ArgumentNullException("anyOf");

		if (startIndex < 0 || startIndex >= length)
			throw new ArgumentOutOfRangeException();

		if (length == 0)
			return -1;

		return IndexOfAnyImpl(anyOf, startIndex, startIndex + 1);
	}

	public int LastIndexOfAny(char[] anyOf, int startIndex, int count)
	{
		if (anyOf == null)
			throw new ArgumentNullException("anyOf");
		if ((startIndex < 0) || (startIndex >= Length))
			throw new ArgumentOutOfRangeException("startIndex", "< 0 || > this.Length");
		if ((count < 0) || (count > Length))
			throw new ArgumentOutOfRangeException("count", "< 0 || > this.Length");
		if (startIndex - count + 1 < 0)
			throw new ArgumentOutOfRangeException("startIndex - count + 1 < 0");

		if (length == 0)
			return -1;

		return InternalLastIndexOfAny(anyOf, startIndex, count);
	}

	public int LastIndexOf(char value)
	{
		if (length == 0)
			return -1;

		return LastIndexOfImpl(value, length - 1, length);
	}

	public int LastIndexOf(char value, int startIndex)
	{
		return LastIndexOf(value, startIndex, startIndex + 1);
	}

	public int LastIndexOf(char value, int startIndex, int count)
	{
		if (startIndex == 0 && length == 0)
			return -1;
		if ((startIndex < 0) || (startIndex >= Length))
			throw new ArgumentOutOfRangeException("startIndex", "< 0 || >= this.Length");
		if ((count < 0) || (count > Length))
			throw new ArgumentOutOfRangeException("count", "< 0 || > this.Length");
		if (startIndex - count + 1 < 0)
			throw new ArgumentOutOfRangeException("startIndex - count + 1 < 0");

		return LastIndexOfImpl(value, startIndex, count);
	}

	private int IndexOfImpl(char value, int startIndex, int count)
	{
		for (int i = startIndex; i < count; i++)
			if (this[i] == value)
				return i;

		return -1;
	}

	private int IndexOfImpl(string value, int startIndex, int count)
	{
		for (int i = startIndex; i < count; i++)
		{
			bool found = true;
			for (int n = 0; n < value.length; n++)
			{
				if (this[i + n] != value[n])
				{
					found = false;
					break;
				}
			}
			if (found)
				return i;
		}

		return -1;
	}

	private int IndexOfAnyImpl(char[] anyOf, int startIndex, int count)
	{
		for (int i = 0; i < count; i++)
		for (int loop = 0; loop != anyOf.Length; loop++)
			if (this[startIndex + i] == anyOf[loop])
				return startIndex + i;

		return -1;
	}

	private int LastIndexOfImpl(char value, int startIndex, int count)
	{
		for (int i = 0; i < count; i++)
			if (this[startIndex + i] == value)
				return startIndex + i;

		return -1;
	}

	private int InternalLastIndexOfAny(char[] anyOf, int startIndex, int count)
	{
		for (int i = count - 1; i >= 0; i--)
		for (int loop = 0; loop != anyOf.Length; loop++)
			if (this[startIndex + i] == anyOf[loop])
				return startIndex + i;

		return -1;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new CharEnumerator(this);
	}

	IEnumerator<char> IEnumerable<char>.GetEnumerator()
	{
		return new CharEnumerator(this);
	}

	private const int TrimHead = 0;
	private const int TrimTail = 1;
	private const int TrimBoth = 2;

	/// <summary>
	/// Removes a set of characters from the end of this string.
	/// </summary>
	/// <param name="trimChars">Characters to remove.</param>
	/// <returns>Trimmed string.</returns>
	public string TrimEnd(char[] trimChars)
	{
		if (null == trimChars || trimChars.Length == 0)
		{
			return TrimHelper(TrimTail);
		}
		return TrimHelper(trimChars, TrimTail);
	}

	public string TrimEnd()
	{
		return TrimHelper(TrimTail);
	}

	/// <summary>
	/// Trims the whitespace from both ends of the string.
	/// Whitespace is defined by Char.IsWhiteSpace.
	/// </summary>
	/// <returns>Trimmed string.</returns>
	public string Trim()
	{
		return TrimHelper(TrimBoth);
	}

	private string TrimHelper(int trimType)
	{
		//end will point to the first non-trimmed character on the right
		//start will point to the first non-trimmed character on the Left
		int end = Length - 1;
		int start = 0;

		//Trim specified characters.
		if (trimType != TrimTail)
		{
			for (start = 0; start < Length; start++)
			{
				if (!char.IsWhiteSpace(this[start])) break;
			}
		}

		if (trimType != TrimHead)
		{
			for (end = Length - 1; end >= start; end--)
			{
				if (!char.IsWhiteSpace(this[end])) break;
			}
		}

		return CreateTrimmedString(start, end);
	}

	private string TrimHelper(char[] trimChars, int trimType)
	{
		//end will point to the first non-trimmed character on the right
		//start will point to the first non-trimmed character on the Left
		int end = Length - 1;
		int start = 0;

		//Trim specified characters.
		if (trimType != TrimTail)
		{
			for (start = 0; start < Length; start++)
			{
				int i = 0;
				char ch = this[start];
				for (i = 0; i < trimChars.Length; i++)
				{
					if (trimChars[i] == ch) break;
				}
				if (i == trimChars.Length)
				{ // the character is not white space
					break;
				}
			}
		}

		if (trimType != TrimHead)
		{
			for (end = Length - 1; end >= start; end--)
			{
				int i = 0;
				char ch = this[end];
				for (i = 0; i < trimChars.Length; i++)
				{
					if (trimChars[i] == ch) break;
				}
				if (i == trimChars.Length)
				{ // the character is not white space
					break;
				}
			}
		}

		return CreateTrimmedString(start, end);
	}

	private string CreateTrimmedString(int start, int end)
	{
		int len = end - start + 1;
		if (len == Length)
		{
			// Don't allocate a new string as the trimmed string has not changed.
			return this;
		}
		else
		{
			if (len == 0)
			{
				return string.Empty;
			}
			return string.Ctor(this, start, len);
		}
	}
}
