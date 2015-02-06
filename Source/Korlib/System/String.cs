/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Runtime.CompilerServices;

namespace System
{
	/// <summary>
	/// Implementation of the "System.String" class
	/// </summary>
	public class String
	{
		/*
		 * Michael "grover" Froehlich, 2010/05/17:
		 *
		 * Strings are unfortunately very special in their behavior and memory layout. The AOT compiler
		 * expects strings to have a very specific layout in memory to be able to compile instructions,
		 * such as ldstr ahead of time. This is only true as long as we don't have a real loader, app domains
		 * and actual string interning, which would be used in the AOT too. Until that time has come,
		 * the memory layout of String must match:
		 *
		 * Object:
		 *      - Method Table Ptr
		 *      - Sync Block
		 * String:
		 *      - Length
		 *      - Variable length UTF-16 character buffer (should we switch to UTF-8 instead?)
		 *
		 * This layout is used to generated strings in the AOT'd image in CILTransformationStage, which transforms
		 * CIL instructions to IR and replaces ldstr by a load from the data section, which contains the string laid
		 * out along the above lines.
		 *
		 */

		/// <summary>
		/// Length
		/// </summary>
		internal int length;

		private char start_char;

		public int Length
		{
			get
			{
				return this.length;
			}
		}

		public static string Empty = "";

		internal unsafe char* first_char
		{
			get
			{
				char* result;

				fixed (char* c = &start_char)
				{
					result = c;
				}

				return result;
			}
		}

		[IndexerName("Chars")]
		public unsafe char this[int index]
		{
			get
			{
				//if (index < 0 || index >= length)
				//	return (char)0; // throw new IndexOutOfRangeException();

				char result = (char)0;
				fixed (char* c = &start_char)
				{
					result = c[index];
				}

				return result;
			}
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public extern String(char c, int count);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public extern String(char[] value);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public extern String(char[] value, int startIndex, int length);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public extern unsafe String(sbyte* value, int startIndex, int length);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public extern unsafe String(sbyte* value);

		////[MethodImplAttribute(MethodImplOptions.InternalCall)]
		////public unsafe extern String(sbyte* value, int startIndex, int length, Encoding enc);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public unsafe extern String(char* value);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public unsafe extern String(char* value, int startIndex, int length);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		internal static extern string InternalAllocateString(int length);

		private static unsafe string CreateString(char c, int count)
		{
			String result = InternalAllocateString(count);
			char* chars = result.first_char;

			while (count > 0)
			{
				*chars = c;
				count--;
				chars++;
			}

			return result;
		}

		private static string CreateString(char[] value)
		{
			return CreateString(value, 0, value.Length);
		}

		private static unsafe string CreateString(char[] value, int startIndex, int length)
		{
			if (length == 0)
				return string.Empty;

			String result = InternalAllocateString(length);
			char* chars = result.first_char;

			for (int index = startIndex; index < startIndex + length; index++)
			{
				*chars = value[index];
				chars++;
			}

			return result;
		}

		private static unsafe string CreateString(sbyte* value, int startIndex, int length)
		{
			if (length == 0)
				return string.Empty;

			String result = InternalAllocateString(length);
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

		private static unsafe string CreateString(sbyte* value)
		{
			int length = 0;
			sbyte* at = value;

			while (*at != 0)
			{
				length++;
			}

			if (length == 0)
				return string.Empty;

			String result = InternalAllocateString(length);
			char* chars = result.first_char;

			for (int index = 0; index < length; index++)
			{
				*chars = (char)*value;
				chars++;
				value++;
			}

			return result;
		}

		private static unsafe string CreateString(char* value, int startIndex, int length)
		{
			if (length == 0)
				return InternalAllocateString(0);

			String result = InternalAllocateString(length);
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

		private static unsafe string CreateString(char* value)
		{
			int length = 0;
			char* at = value;

			while (*at != 0)
			{
				length++;
			}

			if (length == 0)
				return string.Empty;

			String result = InternalAllocateString(length);
			char* chars = result.first_char;

			for (int index = 0; index < length; index++)
			{
				*chars = *value;
				chars++;
				value++;
			}

			return result;
		}

		public bool Equals(String i)
		{
			return Equals(this, i);
		}

		public override bool Equals(object o)
		{
			if (!(o is String))
				return false;

			String other = (String)o;
			return other == this;
		}

		public static bool operator ==(String a, String b)
		{
			return Equals(a, b);
		}

		public static bool operator !=(String a, String b)
		{
			return !Equals(a, b);
		}

		public static unsafe bool Equals(String a, String b)
		{
			if (a == null || b == null)
				return false;

			if (a.length != b.length)
				return false;

			char* pa = a.first_char;
			char* pb = b.first_char;

			for (int i = 0; i < a.Length; ++i)
				if (pa[i] != pb[i])
					return false;

			return true;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return this;
		}

		public unsafe string ToUpper()
		{
			String result = InternalAllocateString(this.length);
			char* chars = result.first_char;
			char* self = this.first_char;

			for (int i = 0; i < this.length; i++)
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
			String result = InternalAllocateString(this.length);
			char* chars = result.first_char;
			char* self = this.first_char;

			for (int i = 0; i < this.length; i++)
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
		public unsafe static string Concat(String a, String b)
		{
			String result = InternalAllocateString(a.length + b.length);
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

		public unsafe static string Concat(String a, String b, String c)
		{
			String result = InternalAllocateString(a.length + b.length + c.length);
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

		public unsafe static string Concat(String a, String b, String c, String d)
		{
			String result = InternalAllocateString(a.length + b.length + c.length + d.length);
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

		public static string Concat(Object a)
		{
			return a.ToString();
		}

		public static string Concat(Object a, Object b)
		{
			return Concat(a.ToString(), b.ToString());
		}

		public static string Concat(Object a, Object b, Object c)
		{
			return Concat(a.ToString(), b.ToString(), c.ToString());
		}

		public static string Concat(Object a, Object b, Object c, Object d)
		{
			return Concat(a.ToString(), b.ToString(), c.ToString(), d.ToString());
		}

		public static string Concat(params object[] args)
		{
			if (args.Length == 0)
				return string.Empty;

			string result = args[0].ToString();

			for (int i = 1; i < args.Length; ++i)
				result = Concat(result, args[i].ToString());

			return result;
		}

		public static string Concat(string[] objects)
		{
			if (objects.Length == 0)
				return string.Empty;

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
			if (startIndex < 0 || startIndex > this.length)
				throw new System.ArgumentOutOfRangeException("startIndex");

			if (startIndex < 0)
				throw new System.ArgumentOutOfRangeException("startIndex");

			if (startIndex > this.length)
				throw new System.ArgumentOutOfRangeException("startIndex");

			int newlen = this.length - startIndex;
			String result = InternalAllocateString(newlen);

			char* chars = result.first_char;

			for (int index = 0; index < newlen; index++)
				*chars++ = this[startIndex + index];

			return result;
		}

		public unsafe string Substring(int startIndex, int length)
		{
			if (length < 0)
				throw new System.ArgumentOutOfRangeException("length", "< 0");

			if (startIndex < 0 || startIndex > this.length)
				throw new System.ArgumentOutOfRangeException("startIndex");

			String result = InternalAllocateString(length);

			char* chars = result.first_char;

			for (int index = 0; index < length; index++)
				*chars++ = this[startIndex + index];

			return result;
		}

		public static bool IsNullOrEmpty(string value)
		{
			return (value == null) || (value.Length == 0);
		}

		public int IndexOf(string value)
		{
			if (this.length == 0)
				return -1;

			return IndexOfImpl(value, 0, this.length);
		}

		public int IndexOf(char value)
		{
			if (this.length == 0)
				return -1;

			return IndexOfImpl(value, 0, this.length);
		}

		public int IndexOf(char value, int startIndex)
		{
			return IndexOf(value, startIndex, this.length - startIndex);
		}

		public int IndexOf(char value, int startIndex, int count)
		{
			if (startIndex < 0)
				throw new System.ArgumentOutOfRangeException("startIndex", "< 0");
			if (count < 0)
				throw new System.ArgumentOutOfRangeException("count", "< 0");
			if (startIndex > this.length - count)
				throw new System.ArgumentOutOfRangeException("startIndex + count > this.length");

			if ((startIndex == 0 && this.length == 0) || (startIndex == this.length) || (count == 0))
				return -1;

			return IndexOfImpl(value, startIndex, count);
		}

		public int IndexOfAny(char[] anyOf)
		{
			if (anyOf == null)
				throw new System.ArgumentNullException("anyOf");
			if (this.length == 0)
				return -1;

			return IndexOfAnyImpl(anyOf, 0, this.length);
		}

		public int IndexOfAny(char[] anyOf, int startIndex)
		{
			if (anyOf == null)
				throw new System.ArgumentNullException("anyOf");
			if (startIndex < 0 || startIndex > this.length)
				throw new System.ArgumentOutOfRangeException("startIndex");

			return IndexOfAnyImpl(anyOf, startIndex, this.length - startIndex);
		}

		public int IndexOfAny(char[] anyOf, int startIndex, int count)
		{
			if (anyOf == null)
				throw new System.ArgumentNullException("anyOf");
			if (startIndex < 0)
				throw new System.ArgumentOutOfRangeException("startIndex", "< 0");
			if (count < 0)
				throw new System.ArgumentOutOfRangeException("count", "< 0");
			if (startIndex > this.length - count)
				throw new System.ArgumentOutOfRangeException("startIndex + count > this.length");

			return IndexOfAnyImpl(anyOf, startIndex, count);
		}

		public int LastIndexOfAny(char[] anyOf)
		{
			if (anyOf == null)
				throw new System.ArgumentNullException("anyOf");

			return InternalLastIndexOfAny(anyOf, this.length - 1, this.length);
		}

		public int LastIndexOfAny(char[] anyOf, int startIndex)
		{
			if (anyOf == null)
				throw new System.ArgumentNullException("anyOf");

			if (startIndex < 0 || startIndex >= this.length)
				throw new System.ArgumentOutOfRangeException();

			if (this.length == 0)
				return -1;

			return IndexOfAnyImpl(anyOf, startIndex, startIndex + 1);
		}

		public int LastIndexOfAny(char[] anyOf, int startIndex, int count)
		{
			if (anyOf == null)
				throw new System.ArgumentNullException("anyOf");
			if ((startIndex < 0) || (startIndex >= this.Length))
				throw new System.ArgumentOutOfRangeException("startIndex", "< 0 || > this.Length");
			if ((count < 0) || (count > this.Length))
				throw new System.ArgumentOutOfRangeException("count", "< 0 || > this.Length");
			if (startIndex - count + 1 < 0)
				throw new System.ArgumentOutOfRangeException("startIndex - count + 1 < 0");

			if (this.length == 0)
				return -1;

			return InternalLastIndexOfAny(anyOf, startIndex, count);
		}

		public int LastIndexOf(char value)
		{
			if (this.length == 0)
				return -1;

			return LastIndexOfImpl(value, this.length - 1, this.length);
		}

		public int LastIndexOf(char value, int startIndex)
		{
			return LastIndexOf(value, startIndex, startIndex + 1);
		}

		public int LastIndexOf(char value, int startIndex, int count)
		{
			if (startIndex == 0 && this.length == 0)
				return -1;
			if ((startIndex < 0) || (startIndex >= this.Length))
				throw new System.ArgumentOutOfRangeException("startIndex", "< 0 || >= this.Length");
			if ((count < 0) || (count > this.Length))
				throw new System.ArgumentOutOfRangeException("count", "< 0 || > this.Length");
			if (startIndex - count + 1 < 0)
				throw new System.ArgumentOutOfRangeException("startIndex - count + 1 < 0");

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
	}
}