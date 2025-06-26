using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace System;

public class ArgumentOutOfRangeException : ArgumentException
{
	public virtual object? ActualValue { get; }

	public override string Message => ActualValue != null ? $"{base.Message} ({ActualValue})" : base.Message;

	public ArgumentOutOfRangeException()
		: base(Internal.Exceptions.ArgumentOutOfRangeException) {}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected ArgumentOutOfRangeException(SerializationInfo info, StreamingContext context) {}

	public ArgumentOutOfRangeException(string? paramName)
		: base(Internal.Exceptions.ArgumentOutOfRangeException, paramName) {}

	public ArgumentOutOfRangeException(string? message, Exception? innerException)
		: base(message, innerException) {}

	public ArgumentOutOfRangeException(string? paramName, object? actualValue, string? message)
		: base(message, paramName) => ActualValue = actualValue;

	public ArgumentOutOfRangeException(string? paramName, string? message)
		: base(message, paramName) {}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context) {}

	public static void ThrowIfEqual<T>(T value, T other, [CallerArgumentExpression("value")] string? paramName = null) where T : IEquatable<T>?
	{
		if (value.Equals(other))
			throw new ArgumentOutOfRangeException(paramName);
	}

	public static void ThrowIfGreaterThan<T>(T value, T other, [CallerArgumentExpression("value")] string? paramName = null) where T : IComparable<T>
	{
		if (value.CompareTo(other) > 0)
			throw new ArgumentOutOfRangeException(paramName);
	}

	public static void ThrowIfGreaterThanOrEqual<T>(T value, T other, [CallerArgumentExpression("value")] string? paramName = null) where T : IComparable<T>
	{
		if (value.CompareTo(other) >= 0)
			throw new ArgumentOutOfRangeException(paramName);
	}

	public static void ThrowIfLessThan<T>(T value, T other, [CallerArgumentExpression("value")] string? paramName = null) where T : IComparable<T>
	{
		if (value.CompareTo(other) < 0)
			throw new ArgumentOutOfRangeException(paramName);
	}

	public static void ThrowIfLessThanOrEqual<T>(T value, T other, [CallerArgumentExpression("value")] string? paramName = null) where T : IComparable<T>
	{
		if (value.CompareTo(other) <= 0)
			throw new ArgumentOutOfRangeException(paramName);
	}

	public static void ThrowIfNegative<T>(T value, [CallerArgumentExpression("value")] string? paramName = null) where T : INumberBase<T>
	{
		if (T.IsNegative(value))
			throw new ArgumentOutOfRangeException(paramName);
	}

	public static void ThrowIfNegativeOrZero<T>(T value, [CallerArgumentExpression("value")] string? paramName = null) where T : INumberBase<T>
	{
		if (T.IsNegative(value) || T.IsZero(value))
			throw new ArgumentOutOfRangeException(paramName);
	}

	public static void ThrowIfNotEqual<T>(T value, T other, [CallerArgumentExpression("value")] string? paramName = null) where T : IEquatable<T>?
	{
		if (!value.Equals(other))
			throw new ArgumentOutOfRangeException(paramName);
	}

	public static void ThrowIfZero<T>(T value, [CallerArgumentExpression("value")] string? paramName = null) where T : INumberBase<T>
	{
		if (T.IsZero(value))
			throw new ArgumentOutOfRangeException(paramName);
	}
}
