using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public struct RuntimeMethodHandle : IEquatable<RuntimeMethodHandle>, ISerializable
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

	public bool Equals(RuntimeMethodHandle handle)
	{
		throw null;
	}

	public IntPtr GetFunctionPointer()
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

	public static RuntimeMethodHandle FromIntPtr(IntPtr value)
	{
		throw null;
	}

	public static IntPtr ToIntPtr(RuntimeMethodHandle value)
	{
		throw null;
	}

	public static bool operator ==(RuntimeMethodHandle left, RuntimeMethodHandle right)
	{
		throw null;
	}

	public static bool operator !=(RuntimeMethodHandle left, RuntimeMethodHandle right)
	{
		throw null;
	}
}
