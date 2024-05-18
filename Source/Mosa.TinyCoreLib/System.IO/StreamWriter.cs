using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO;

public class StreamWriter : TextWriter
{
	public new static readonly StreamWriter Null;

	public virtual bool AutoFlush
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual Stream BaseStream
	{
		get
		{
			throw null;
		}
	}

	public override Encoding Encoding
	{
		get
		{
			throw null;
		}
	}

	public StreamWriter(Stream stream)
	{
	}

	public StreamWriter(Stream stream, Encoding encoding)
	{
	}

	public StreamWriter(Stream stream, Encoding encoding, int bufferSize)
	{
	}

	public StreamWriter(Stream stream, Encoding? encoding = null, int bufferSize = -1, bool leaveOpen = false)
	{
	}

	public StreamWriter(string path)
	{
	}

	public StreamWriter(string path, bool append)
	{
	}

	public StreamWriter(string path, bool append, Encoding encoding)
	{
	}

	public StreamWriter(string path, bool append, Encoding encoding, int bufferSize)
	{
	}

	public StreamWriter(string path, FileStreamOptions options)
	{
	}

	public StreamWriter(string path, Encoding encoding, FileStreamOptions options)
	{
	}

	public override void Close()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override ValueTask DisposeAsync()
	{
		throw null;
	}

	public override void Flush()
	{
	}

	public override Task FlushAsync()
	{
		throw null;
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public override void Write(char value)
	{
	}

	public override void Write(char[]? buffer)
	{
	}

	public override void Write(char[] buffer, int index, int count)
	{
	}

	public override void Write(ReadOnlySpan<char> buffer)
	{
	}

	public override void Write(string? value)
	{
	}

	public override void Write([StringSyntax("CompositeFormat")] string format, object? arg0)
	{
	}

	public override void Write([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1)
	{
	}

	public override void Write([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1, object? arg2)
	{
	}

	public override void Write([StringSyntax("CompositeFormat")] string format, params object?[] arg)
	{
	}

	public override Task WriteAsync(char value)
	{
		throw null;
	}

	public override Task WriteAsync(char[] buffer, int index, int count)
	{
		throw null;
	}

	public override Task WriteAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public override Task WriteAsync(string? value)
	{
		throw null;
	}

	public override void WriteLine(ReadOnlySpan<char> buffer)
	{
	}

	public override void WriteLine(string? value)
	{
	}

	public override void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0)
	{
	}

	public override void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1)
	{
	}

	public override void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1, object? arg2)
	{
	}

	public override void WriteLine([StringSyntax("CompositeFormat")] string format, params object?[] arg)
	{
	}

	public override Task WriteLineAsync()
	{
		throw null;
	}

	public override Task WriteLineAsync(char value)
	{
		throw null;
	}

	public override Task WriteLineAsync(char[] buffer, int index, int count)
	{
		throw null;
	}

	public override Task WriteLineAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public override Task WriteLineAsync(string? value)
	{
		throw null;
	}
}
