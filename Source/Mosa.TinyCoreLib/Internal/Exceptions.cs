using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Internal;

internal static class Exceptions
{
    public const string Exception = "An exception was thrown.";
    public const string PlatformNotSupportedException = "The platform is not supported.";
    public const string InvalidOperationException = "The requested operation cannot be performed.";
    public const string NotSupportedException = "The operation is not supported.";
    public const string NotImplementedException = "The operation is not implemented.";
    public const string RankException = "Attempted to operate on an array with an incorrect number of dimensions.";
    public const string ArgumentException = "The value does not fall within the expected range.";
    public const string ArithmeticException = "An arithmetic, casting, or conversion error occured.";
    public const string SystemException = "A system exception was thrown.";
    public const string OverflowException = "A checked arithmetic, casting, or conversion operation resulted in an overflow.";
    public const string FormatException = "An invalid format argument was passed, or a composite format string is malformed.";
    public const string IndexOutOfRangeException = "The index was outside the bounds of the array.";
    public const string ArgumentNullException = "The argument cannot be null.";
    public const string ArgumentOutOfRangeException = "The argument is out of range.";
    public const string ObjectDisposedException = "The object was used after being disposed.";
    public const string TypeLoadException = "A failure has occured while loading a type.";

	public static class Generic
	{
		[DoesNotReturn] public static void ParameterOutOfRange(string name) => throw new ArgumentOutOfRangeException(name);
	}
}
