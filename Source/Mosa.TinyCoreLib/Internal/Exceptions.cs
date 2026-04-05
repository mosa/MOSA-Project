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
		[DoesNotReturn] public static void Overflow() => throw new OverflowException();
		[DoesNotReturn] public static void NotSupported() => throw new NotSupportedException();
		[DoesNotReturn] public static void InvalidOperation() => throw new InvalidOperationException();
		[DoesNotReturn] public static void ObjectDisposed(string name) => throw new ObjectDisposedException(name);
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

	public static class Color
	{
		[DoesNotReturn] public static void ThrowComponentOutOfRangeException(string name) => throw new ArgumentException("The color component value is out of range.", name);
	}

	public static class Span
	{
		[DoesNotReturn] public static void ThrowReferenceException() => throw new ArgumentException("The span's child type is a reference type or contains reference types.");
		[DoesNotReturn] public static void ThrowTypeMismatchException() => throw new ArrayTypeMismatchException("The span's child type is a reference type, and the array is not of the chosen type.");
		[DoesNotReturn] public static void ThrowDestinationShorterException() => throw new ArgumentException("The destination span is shorter than the source span.");
	}

	public static class Stream
	{
		[DoesNotReturn] public static void ThrowClosedStreamException() => throw new ObjectDisposedException(null);
	}

	public static class MemoryStream
	{
		[DoesNotReturn] public static void ThrowBufferTooSmallException(string name) => throw new ArgumentException("The buffer is too small for the specified count.", name);
		[DoesNotReturn] public static void ThrowBufferNotVisibleException() => throw new UnauthorizedAccessException("The MemoryStream wasn't created with a publicly visible buffer.");
		[DoesNotReturn] public static void ThrowInvalidSeekOriginException(string name) => throw new ArgumentException("The seek origin value is invalid.", name);
		[DoesNotReturn] public static void ThrowSeekingBeforeBeginningException() => throw new IOException("The offset caused the position to become negative.");
	}

	public static class CollectionBase
	{
		[DoesNotReturn] public static void ThrowValueNotFoundException() => throw new ArgumentException("The value wasn't found inside the collection.");
	}
}
