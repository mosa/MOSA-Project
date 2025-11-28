using System.Threading;
using System.Threading.Tasks;

namespace System.IO;

public abstract class TextReader : MarshalByRefObject, IDisposable
{
	public static readonly TextReader Null;

	public virtual void Close()
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public virtual int Peek()
	{
		throw null;
	}

	public virtual int Read()
	{
		throw null;
	}

	public virtual int Read(char[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual int Read(Span<char> buffer)
	{
		throw null;
	}

	public virtual Task<int> ReadAsync(char[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual ValueTask<int> ReadAsync(Memory<char> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual int ReadBlock(char[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual int ReadBlock(Span<char> buffer)
	{
		throw null;
	}

	public virtual Task<int> ReadBlockAsync(char[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual ValueTask<int> ReadBlockAsync(Memory<char> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual string? ReadLine()
	{
		throw null;
	}

	public virtual Task<string?> ReadLineAsync()
	{
		throw null;
	}

	public virtual ValueTask<string?> ReadLineAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public virtual string ReadToEnd()
	{
		throw null;
	}

	public virtual Task<string> ReadToEndAsync()
	{
		throw null;
	}

	public virtual Task<string> ReadToEndAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public static TextReader Synchronized(TextReader reader)
	{
		throw null;
	}
}
