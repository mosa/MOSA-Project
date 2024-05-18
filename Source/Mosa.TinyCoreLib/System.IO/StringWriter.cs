using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO;

public class StringWriter : TextWriter
{
	public override Encoding Encoding
	{
		get
		{
			throw null;
		}
	}

	public StringWriter()
	{
	}

	public StringWriter(IFormatProvider? formatProvider)
	{
	}

	public StringWriter(StringBuilder sb)
	{
	}

	public StringWriter(StringBuilder sb, IFormatProvider? formatProvider)
	{
	}

	public override void Close()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override Task FlushAsync()
	{
		throw null;
	}

	public virtual StringBuilder GetStringBuilder()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public override void Write(char value)
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

	public override void Write(StringBuilder? value)
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

	public override Task WriteAsync(StringBuilder? value, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public override void WriteLine(ReadOnlySpan<char> buffer)
	{
	}

	public override void WriteLine(StringBuilder? value)
	{
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

	public override Task WriteLineAsync(StringBuilder? value, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}
}
