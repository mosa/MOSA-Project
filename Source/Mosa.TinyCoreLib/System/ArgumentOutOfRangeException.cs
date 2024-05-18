using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace System;

public class ArgumentOutOfRangeException : ArgumentException
{
	public virtual object? ActualValue
	{
		get
		{
			throw null;
		}
	}

	public override string Message
	{
		get
		{
			throw null;
		}
	}

	public ArgumentOutOfRangeException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected ArgumentOutOfRangeException(SerializationInfo info, StreamingContext context)
	{
	}

	public ArgumentOutOfRangeException(string? paramName)
	{
	}

	public ArgumentOutOfRangeException(string? message, Exception? innerException)
	{
	}

	public ArgumentOutOfRangeException(string? paramName, object? actualValue, string? message)
	{
	}

	public ArgumentOutOfRangeException(string? paramName, string? message)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public static void ThrowIfEqual<T>(T value, T other, [CallerArgumentExpression("value")] string? paramName = null) where T : IEquatable<T>?
	{
		throw null;
	}

	public static void ThrowIfGreaterThan<T>(T value, T other, [CallerArgumentExpression("value")] string? paramName = null) where T : IComparable<T>
	{
		throw null;
	}

	public static void ThrowIfGreaterThanOrEqual<T>(T value, T other, [CallerArgumentExpression("value")] string? paramName = null) where T : IComparable<T>
	{
		throw null;
	}

	public static void ThrowIfLessThan<T>(T value, T other, [CallerArgumentExpression("value")] string? paramName = null) where T : IComparable<T>
	{
		throw null;
	}

	public static void ThrowIfLessThanOrEqual<T>(T value, T other, [CallerArgumentExpression("value")] string? paramName = null) where T : IComparable<T>
	{
		throw null;
	}

	public static void ThrowIfNegative<T>(T value, [CallerArgumentExpression("value")] string? paramName = null) where T : INumberBase<T>
	{
		throw null;
	}

	public static void ThrowIfNegativeOrZero<T>(T value, [CallerArgumentExpression("value")] string? paramName = null) where T : INumberBase<T>
	{
		throw null;
	}

	public static void ThrowIfNotEqual<T>(T value, T other, [CallerArgumentExpression("value")] string? paramName = null) where T : IEquatable<T>?
	{
		throw null;
	}

	public static void ThrowIfZero<T>(T value, [CallerArgumentExpression("value")] string? paramName = null) where T : INumberBase<T>
	{
		throw null;
	}
}
