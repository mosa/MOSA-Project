using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System;

public class WeakReference : ISerializable
{
	public virtual bool IsAlive
	{
		get
		{
			throw null;
		}
	}

	public virtual object? Target
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual bool TrackResurrection
	{
		get
		{
			throw null;
		}
	}

	public WeakReference(object? target)
	{
	}

	public WeakReference(object? target, bool trackResurrection)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected WeakReference(SerializationInfo info, StreamingContext context)
	{
	}

	~WeakReference()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
public sealed class WeakReference<T> : ISerializable where T : class?
{
	public WeakReference(T target)
	{
	}

	public WeakReference(T target, bool trackResurrection)
	{
	}

	~WeakReference()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public void SetTarget(T target)
	{
	}

	public bool TryGetTarget([MaybeNullWhen(false)][NotNullWhen(true)] out T target)
	{
		throw null;
	}
}
