using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO;

public abstract class TextWriter : MarshalByRefObject, IAsyncDisposable, IDisposable
{
	protected char[] CoreNewLine;

	public static readonly TextWriter Null;

	public abstract Encoding Encoding { get; }

	public virtual IFormatProvider FormatProvider
	{
		get
		{
			throw null;
		}
	}

	public virtual string NewLine
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	protected TextWriter()
	{
	}

	protected TextWriter(IFormatProvider? formatProvider)
	{
	}

	public virtual void Close()
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public virtual ValueTask DisposeAsync()
	{
		throw null;
	}

	public virtual void Flush()
	{
	}

	public virtual Task FlushAsync()
	{
		throw null;
	}

	public virtual Task FlushAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public static TextWriter Synchronized(TextWriter writer)
	{
		throw null;
	}

	public virtual void Write(bool value)
	{
	}

	public virtual void Write(char value)
	{
	}

	public virtual void Write(char[]? buffer)
	{
	}

	public virtual void Write(char[] buffer, int index, int count)
	{
	}

	public virtual void Write(decimal value)
	{
	}

	public virtual void Write(double value)
	{
	}

	public virtual void Write(int value)
	{
	}

	public virtual void Write(long value)
	{
	}

	public virtual void Write(object? value)
	{
	}

	public virtual void Write(ReadOnlySpan<char> buffer)
	{
	}

	public virtual void Write(float value)
	{
	}

	public virtual void Write(string? value)
	{
	}

	public virtual void Write([StringSyntax("CompositeFormat")] string format, object? arg0)
	{
	}

	public virtual void Write([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1)
	{
	}

	public virtual void Write([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1, object? arg2)
	{
	}

	public virtual void Write([StringSyntax("CompositeFormat")] string format, params object?[] arg)
	{
	}

	public virtual void Write(StringBuilder? value)
	{
	}

	[CLSCompliant(false)]
	public virtual void Write(uint value)
	{
	}

	[CLSCompliant(false)]
	public virtual void Write(ulong value)
	{
	}

	public virtual Task WriteAsync(char value)
	{
		throw null;
	}

	public Task WriteAsync(char[]? buffer)
	{
		throw null;
	}

	public virtual Task WriteAsync(char[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual Task WriteAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual Task WriteAsync(string? value)
	{
		throw null;
	}

	public virtual Task WriteAsync(StringBuilder? value, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual void WriteLine()
	{
	}

	public virtual void WriteLine(bool value)
	{
	}

	public virtual void WriteLine(char value)
	{
	}

	public virtual void WriteLine(char[]? buffer)
	{
	}

	public virtual void WriteLine(char[] buffer, int index, int count)
	{
	}

	public virtual void WriteLine(decimal value)
	{
	}

	public virtual void WriteLine(double value)
	{
	}

	public virtual void WriteLine(int value)
	{
	}

	public virtual void WriteLine(long value)
	{
	}

	public virtual void WriteLine(object? value)
	{
	}

	public virtual void WriteLine(ReadOnlySpan<char> buffer)
	{
	}

	public virtual void WriteLine(float value)
	{
	}

	public virtual void WriteLine(string? value)
	{
	}

	public virtual void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0)
	{
	}

	public virtual void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1)
	{
	}

	public virtual void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1, object? arg2)
	{
	}

	public virtual void WriteLine([StringSyntax("CompositeFormat")] string format, params object?[] arg)
	{
	}

	public virtual void WriteLine(StringBuilder? value)
	{
	}

	[CLSCompliant(false)]
	public virtual void WriteLine(uint value)
	{
	}

	[CLSCompliant(false)]
	public virtual void WriteLine(ulong value)
	{
	}

	public virtual Task WriteLineAsync()
	{
		throw null;
	}

	public virtual Task WriteLineAsync(char value)
	{
		throw null;
	}

	public Task WriteLineAsync(char[]? buffer)
	{
		throw null;
	}

	public virtual Task WriteLineAsync(char[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual Task WriteLineAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual Task WriteLineAsync(string? value)
	{
		throw null;
	}

	public virtual Task WriteLineAsync(StringBuilder? value, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}
}
