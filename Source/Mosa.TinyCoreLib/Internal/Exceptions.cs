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
		[DoesNotReturn] public static void ThrowMultiDimensionalArrayException(string name) => throw new ArgumentException("The array has more than one dimension.", name);
		[DoesNotReturn] public static void ThrowArrayNotZeroBasedIndexingException(string name) => throw new ArgumentException("The array doesn't use zero-based indexing.", name);
		[DoesNotReturn] public static void ThrowArrayTypeMismatchException(string name) => throw new ArgumentException("The array's element type doesn't match the collection's element type.", name);
	}

	public static class LinkedList
	{
		public static class Enumerator
		{
			[DoesNotReturn] public static void ThrowListModifiedException() => throw new InvalidOperationException("The list was modified after the enumerator was created.");
		}

		[DoesNotReturn] public static void ThrowNodeNotInListException() => throw new InvalidOperationException("The node is not in the list.");
		[DoesNotReturn] public static void ThrowNewNodeInAnotherListException() => throw new InvalidOperationException("The new node is already in another list.");
		[DoesNotReturn] public static void ThrowListTooBigException(string name) => throw new ArgumentException("The list is too big for the destination array.", name);
		[DoesNotReturn] public static void ThrowListEmptyException(string name) => throw new ArgumentException("The list is empty.", name);
	}

	public static class Stack
	{
		public static class Enumerator
		{
			[DoesNotReturn] public static void ThrowStackModifiedException() => throw new InvalidOperationException("The stack was modified after the enumerator was created.");
		}

		[DoesNotReturn] public static void ThrowStackTooBigException(string name) => throw new ArgumentException("The stack is too big for the destination array.", name);
		[DoesNotReturn] public static void ThrowStackEmptyException() => throw new InvalidOperationException("The stack is empty.");
	}

	public static class Queue
	{
		public static class Enumerator
		{
			[DoesNotReturn] public static void ThrowQueueModifiedException() => throw new InvalidOperationException("The queue was modified after the enumerator was created.");
		}

		[DoesNotReturn] public static void ThrowQueueTooBigException(string name) => throw new ArgumentException("The queue is too big for the destination array.", name);
	}

	public static class List
	{
		public static class Enumerator
		{
			[DoesNotReturn] public static void ThrowListModifiedException() => throw new InvalidOperationException("The list was modified after the enumerator was created.");
		}

		[DoesNotReturn] public static void ThrowValueIncorrectTypeException(string name) => throw new ArgumentException("The value's type is different from the list's element type.", name);
		[DoesNotReturn] public static void ThrowIndexOutOfBoundsException(string name) => throw new ArgumentException("The index is greater or equal to the list's element count.", name);
		[DoesNotReturn] public static void ThrowListTooBigException(string name) => throw new ArgumentException("The list is too big for the destination array.", name);
	}

	public static class EqualityComparer
	{
		[DoesNotReturn] public static void ThrowInvalidTypeException(string name) => throw new ArgumentException("The parameter is of a type that cannot be cast to the compared type.", name);
	}
}
