using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Threading.RateLimiting;

public abstract class RateLimitLease : IDisposable
{
	public abstract bool IsAcquired { get; }

	public abstract IEnumerable<string> MetadataNames { get; }

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public virtual IEnumerable<KeyValuePair<string, object?>> GetAllMetadata()
	{
		throw null;
	}

	public abstract bool TryGetMetadata(string metadataName, out object? metadata);

	public bool TryGetMetadata<T>(MetadataName<T> metadataName, [MaybeNull] out T metadata)
	{
		throw null;
	}
}
