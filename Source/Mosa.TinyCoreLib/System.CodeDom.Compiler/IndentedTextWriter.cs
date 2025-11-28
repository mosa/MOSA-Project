using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.CodeDom.Compiler;

public class IndentedTextWriter : TextWriter
{
	public const string DefaultTabString = "    ";

	public override Encoding Encoding
	{
		get
		{
			throw null;
		}
	}

	public int Indent
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TextWriter InnerWriter
	{
		get
		{
			throw null;
		}
	}

	public override string NewLine
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

	public IndentedTextWriter(TextWriter writer)
	{
	}

	public IndentedTextWriter(TextWriter writer, string tabString)
	{
	}

	public override void Close()
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

	protected virtual void OutputTabs()
	{
	}

	protected virtual Task OutputTabsAsync()
	{
		throw null;
	}

	public override void Write(bool value)
	{
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

	public override void Write(double value)
	{
	}

	public override void Write(int value)
	{
	}

	public override void Write(long value)
	{
	}

	public override void Write(object? value)
	{
	}

	public override void Write(float value)
	{
	}

	public override void Write(string? s)
	{
	}

	public override void Write([StringSyntax("CompositeFormat")] string format, object? arg0)
	{
	}

	public override void Write([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1)
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

	public override Task WriteAsync(StringBuilder? value, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public override void WriteLine()
	{
	}

	public override void WriteLine(bool value)
	{
	}

	public override void WriteLine(char value)
	{
	}

	public override void WriteLine(char[]? buffer)
	{
	}

	public override void WriteLine(char[] buffer, int index, int count)
	{
	}

	public override void WriteLine(double value)
	{
	}

	public override void WriteLine(int value)
	{
	}

	public override void WriteLine(long value)
	{
	}

	public override void WriteLine(object? value)
	{
	}

	public override void WriteLine(float value)
	{
	}

	public override void WriteLine(string? s)
	{
	}

	public override void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0)
	{
	}

	public override void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1)
	{
	}

	public override void WriteLine([StringSyntax("CompositeFormat")] string format, params object?[] arg)
	{
	}

	[CLSCompliant(false)]
	public override void WriteLine(uint value)
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

	public override Task WriteLineAsync(StringBuilder? value, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public void WriteLineNoTabs(string? s)
	{
	}

	public Task WriteLineNoTabsAsync(string? s)
	{
		throw null;
	}
}
