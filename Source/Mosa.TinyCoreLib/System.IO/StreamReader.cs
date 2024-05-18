using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO;

public class StreamReader : TextReader
{
	public new static readonly StreamReader Null;

	public virtual Stream BaseStream
	{
		get
		{
			throw null;
		}
	}

	public virtual Encoding CurrentEncoding
	{
		get
		{
			throw null;
		}
	}

	public bool EndOfStream
	{
		get
		{
			throw null;
		}
	}

	public StreamReader(Stream stream)
	{
	}

	public StreamReader(Stream stream, bool detectEncodingFromByteOrderMarks)
	{
	}

	public StreamReader(Stream stream, Encoding encoding)
	{
	}

	public StreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
	{
	}

	public StreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize)
	{
	}

	public StreamReader(Stream stream, Encoding? encoding = null, bool detectEncodingFromByteOrderMarks = true, int bufferSize = -1, bool leaveOpen = false)
	{
	}

	public StreamReader(string path)
	{
	}

	public StreamReader(string path, bool detectEncodingFromByteOrderMarks)
	{
	}

	public StreamReader(string path, FileStreamOptions options)
	{
	}

	public StreamReader(string path, Encoding encoding)
	{
	}

	public StreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
	{
	}

	public StreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize)
	{
	}

	public StreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, FileStreamOptions options)
	{
	}

	public override void Close()
	{
	}

	public void DiscardBufferedData()
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

	public override int ReadBlock(char[] buffer, int index, int count)
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
