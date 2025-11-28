namespace System.Security;

public sealed class SecureString : IDisposable
{
	public int Length
	{
		get
		{
			throw null;
		}
	}

	public SecureString()
	{
	}

	[CLSCompliant(false)]
	public unsafe SecureString(char* value, int length)
	{
	}

	public void AppendChar(char c)
	{
	}

	public void Clear()
	{
	}

	public SecureString Copy()
	{
		throw null;
	}

	public void Dispose()
	{
	}

	public void InsertAt(int index, char c)
	{
	}

	public bool IsReadOnly()
	{
		throw null;
	}

	public void MakeReadOnly()
	{
	}

	public void RemoveAt(int index)
	{
	}

	public void SetAt(int index, char c)
	{
	}
}
