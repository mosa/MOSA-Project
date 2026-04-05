using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public struct RuntimeMethodHandle : IEquatable<RuntimeMethodHandle>, ISerializable
{
	public IntPtr Value { get; }

	internal RuntimeMethodHandle(IntPtr value) => Value = value;

	public override bool Equals(object? obj) => obj is RuntimeMethodHandle handle && Equals(handle);

	public bool Equals(RuntimeMethodHandle handle) => Value == handle.Value;

	public IntPtr GetFunctionPointer() => throw new NotImplementedException();

	public override int GetHashCode() => Value.GetHashCode();

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void GetObjectData(SerializationInfo info, StreamingContext context) => throw new NotImplementedException();

	public static RuntimeMethodHandle FromIntPtr(IntPtr value) => new(value);

	public static IntPtr ToIntPtr(RuntimeMethodHandle value) => value.Value;

	public static bool operator ==(RuntimeMethodHandle left, RuntimeMethodHandle right) => left.Equals(right);

	public static bool operator !=(RuntimeMethodHandle left, RuntimeMethodHandle right) => !left.Equals(right);
}
