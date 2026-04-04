using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public struct RuntimeFieldHandle : IEquatable<RuntimeFieldHandle>, ISerializable
{
	public IntPtr Value { get; }

	internal RuntimeFieldHandle(IntPtr value) => Value = value;

	public override bool Equals(object? obj) => obj is RuntimeFieldHandle handle && Equals(handle);

	public bool Equals(RuntimeFieldHandle handle) => Value == handle.Value;

	public override int GetHashCode() => Value.GetHashCode();

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void GetObjectData(SerializationInfo info, StreamingContext context) => throw new NotImplementedException();

	public static RuntimeFieldHandle FromIntPtr(IntPtr value) => new(value);

	public static IntPtr ToIntPtr(RuntimeFieldHandle value) => value.Value;

	public static bool operator ==(RuntimeFieldHandle left, RuntimeFieldHandle right) => left.Equals(right);

	public static bool operator !=(RuntimeFieldHandle left, RuntimeFieldHandle right) => !left.Equals(right);
}
