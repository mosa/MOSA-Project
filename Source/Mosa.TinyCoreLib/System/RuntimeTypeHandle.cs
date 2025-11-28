using System.ComponentModel;
using System.Runtime.Serialization;

namespace System;

public struct RuntimeTypeHandle : IEquatable<RuntimeTypeHandle>, ISerializable
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

	public bool Equals(RuntimeTypeHandle handle)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public ModuleHandle GetModuleHandle()
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public static RuntimeTypeHandle FromIntPtr(IntPtr value)
	{
		throw null;
	}

	public static IntPtr ToIntPtr(RuntimeTypeHandle value)
	{
		throw null;
	}

	public static bool operator ==(object? left, RuntimeTypeHandle right)
	{
		throw null;
	}

	public static bool operator ==(RuntimeTypeHandle left, object? right)
	{
		throw null;
	}

	public static bool operator !=(object? left, RuntimeTypeHandle right)
	{
		throw null;
	}

	public static bool operator !=(RuntimeTypeHandle left, object? right)
	{
		throw null;
	}
}
