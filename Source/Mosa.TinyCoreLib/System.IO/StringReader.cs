using System.Threading;
using System.Threading.Tasks;

namespace System.IO;

public class StringReader : TextReader
{
	public StringReader(string s)
	{
	}

	public override void Close()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override int Peek()
	{
		throw null;
	}

	public override int Read()
	{
		throw null;
	}

	public override int Read(char[] buffer, int index, int count)
	{
		throw null;
	}

	public override int Read(Span<char> buffer)
	{
		throw null;
	}

	public override Task<int> ReadAsync(char[] buffer, int index, int count)
	{
		throw null;
	}

	public override ValueTask<int> ReadAsync(Memory<char> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public override int ReadBlock(Span<char> buffer)
	{
		throw null;
	}

	public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
	{
		throw null;
	}

	public override ValueTask<int> ReadBlockAsync(Memory<char> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public override string? ReadLine()
	{
		throw null;
	}

	public override Task<string?> ReadLineAsync()
	{
		throw null;
	}

	public override ValueTask<string?> ReadLineAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	public override string ReadToEnd()
	{
		throw null;
	}

	public override Task<string> ReadToEndAsync()
	{
		throw null;
	}

	public override Task<string> ReadToEndAsync(CancellationToken cancellationToken)
	{
		throw null;
	}
}
