// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Diagnostics;

public static class Debug
{
	public static bool AutoFlush { get; set; }

	public static int IndentLevel { get; set; }

	public static int IndentSize { get; set; } = 2;

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
		if (!condition)
			Fail(message, detailMessage);
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
		Environment.FailFast(message + "\nDetails: " + detailMessage);
	}

	[Conditional("DEBUG")]
	public static void Flush()
	{
		throw new NotImplementedException();
	}

	[Conditional("DEBUG")]
	public static void Indent()
	{
		IndentLevel++;
	}

	[Conditional("DEBUG")]
	public static void Unindent()
	{
		IndentLevel--;
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
		if (value == null)
			return;
		Console.Write("DEBUG (" + category + "): " + value);
	}

	[Conditional("DEBUG")]
	public static void Write(string message, string category)
	{
		Console.Write("DEBUG (" + category + "): " + message);
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
		if (value == null)
			return;
		Console.WriteLine("DEBUG (" + category + "): " + value);
	}

	[Conditional("DEBUG")]
	public static void WriteLine(string message, string category)
	{
		Console.WriteLine("DEBUG (" + category + "): " + message);
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
		Console.Write(message);
	}

	[Conditional("DEBUG")]
	public static void Print(string format, params object[] args)
	{
		throw new NotImplementedException();

		//Print(string.Format(format, args));
	}
}
