// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Diagnostics
{
	public static class Debug
	{
		private static bool autoFlush = false;

		public static bool AutoFlush
		{
			get { return autoFlush; }
			set { autoFlush = value; }
		}

		private static int indentLevel = 0;

		public static int IndentLevel
		{
			get { return indentLevel; }
			set { indentLevel = value; }
		}

		private static int indentSize = 2;

		public static int IndentSize
		{
			get { return indentSize; }
			set { indentSize = value; }
		}

		//public static TraceListenerCollection Listeners
		//{
		//	get { return null; }
		//}

		[Conditional("DEBUG")]
		public static void Assert(bool condition)
		{
			Assert(condition, null, null);
		}

		[Conditional("DEBUG")]
		public static void Assert(bool condition, string message)
		{
			Assert(condition, message, null);
		}

		[Conditional("DEBUG")]
		public static void Assert(bool condition, string message, string detailMessage)
		{
			throw new NotImplementedException(); //Plug
		}

		[Conditional("DEBUG")]
		public static void Assert(bool condition, string message, string detailMessageFormat, params object[] args)
		{
			//Assert(condition,message, string.Format(detailMessageFormat, args));
			throw new NotImplementedException();
		}

		[Conditional("DEBUG")]
		public static void Close()
		{
			throw new NotImplementedException();
		}

		[Conditional("DEBUG")]
		public static void Fail(string message)
		{
			Fail(message, null);
		}

		[Conditional("DEBUG")]
		public static void Fail(string message, string detailMessage)
		{
			throw new NotImplementedException(); //Plug
		}

		[Conditional("DEBUG")]
		public static void Flush()
		{
			throw new NotImplementedException();
		}

		[Conditional("DEBUG")]
		public static void Indent()
		{
			indentLevel++;
		}

		[Conditional("DEBUG")]
		public static void Unindent()
		{
			indentLevel--;
		}

		[Conditional("DEBUG")]
		public static void Write(object value)
		{
			Write(value, "");
		}

		[Conditional("DEBUG")]
		public static void Write(string message)
		{
			Write(message, "");
		}

		[Conditional("DEBUG")]
		public static void Write(object value, string category)
		{
			throw new NotImplementedException(); //Plug
		}

		[Conditional("DEBUG")]
		public static void Write(string message, string category)
		{
			throw new NotImplementedException(); //Plug
		}

		[Conditional("DEBUG")]
		public static void WriteIf(bool condition, object value)
		{
			if (condition)
				Write(value, null);
		}

		[Conditional("DEBUG")]
		public static void WriteIf(bool condition, string message)
		{
			if (condition)
				Write(message, null);
		}

		[Conditional("DEBUG")]
		public static void WriteIf(bool condition, object value, string category)
		{
			if (condition)
				Write(value, category);
		}

		[Conditional("DEBUG")]
		public static void WriteIf(bool condition, string message, string category)
		{
			if (condition)
				Write(message, category);
		}

		[Conditional("DEBUG")]
		public static void WriteLine(object value)
		{
			WriteLine(value, null);
		}

		[Conditional("DEBUG")]
		public static void WriteLine(string message)
		{
			WriteLine(message, (string)null);
		}

		[Conditional("DEBUG")]
		public static void WriteLine(string format, params object[] args)
		{
			throw new NotImplementedException();

			//WriteLine(string.Format(format, args));
		}

		[Conditional("DEBUG")]
		public static void WriteLine(object value, string category)
		{
			throw new NotImplementedException(); //Plug
		}

		[Conditional("DEBUG")]
		public static void WriteLine(string message, string category)
		{
			throw new NotImplementedException(); //Plug
		}

		[Conditional("DEBUG")]
		public static void WriteLineIf(bool condition, string message)
		{
			WriteLineIf(condition, message);
		}

		[Conditional("DEBUG")]
		public static void WriteLineIf(bool condition, object value, string category)
		{
			if (condition)
				WriteLine(value, category);
		}

		[Conditional("DEBUG")]
		public static void WriteLineIf(bool condition, string message, string category)
		{
			if (condition)
				WriteLine(message, category);
		}

		[Conditional("DEBUG")]
		public static void Print(string message)
		{
			throw new NotImplementedException(); //Plug
		}

		[Conditional("DEBUG")]
		public static void Print(string format, params Object[] args)
		{
			throw new NotImplementedException();

			//Print(string.Format(format, args));
		}
	}
}
