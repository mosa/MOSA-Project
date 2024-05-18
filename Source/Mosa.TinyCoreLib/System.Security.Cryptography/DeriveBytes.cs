namespace System.Security.Cryptography;

public abstract class DeriveBytes : IDisposable
{
	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public abstract byte[] GetBytes(int cb);

	public abstract void Reset();
}
