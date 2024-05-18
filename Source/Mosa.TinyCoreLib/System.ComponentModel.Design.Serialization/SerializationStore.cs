using System.Collections;
using System.IO;

namespace System.ComponentModel.Design.Serialization;

public abstract class SerializationStore : IDisposable
{
	public abstract ICollection Errors { get; }

	public abstract void Close();

	protected virtual void Dispose(bool disposing)
	{
	}

	public abstract void Save(Stream stream);

	void IDisposable.Dispose()
	{
	}
}
