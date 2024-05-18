using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public struct RuntimeFieldHandle : IEquatable<RuntimeFieldHandle>, ISerializable
{
	private object _dummy;

	private int _dummyPrimitive;

	public IntPtr Value
	{
		get
		{
			throw null;
		}
	}

	public override bool Equals(object? obj)
	{
		throw null;
	}

	public bool Equals(RuntimeFieldHandle handle)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public static RuntimeFieldHandle FromIntPtr(IntPtr value)
	{
		throw null;
	}

	public static IntPtr ToIntPtr(RuntimeFieldHandle value)
	{
		throw null;
	}

	public static bool operator ==(RuntimeFieldHandle left, RuntimeFieldHandle right)
	{
		throw null;
	}

	public static bool operator !=(RuntimeFieldHandle left, RuntimeFieldHandle right)
	{
		throw null;
	}
}
