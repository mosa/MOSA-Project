using System.IO;

namespace System.Net.Sockets;

public class SendPacketsElement
{
	public byte[]? Buffer
	{
		get
		{
			throw null;
		}
	}

	public int Count
	{
		get
		{
			throw null;
		}
	}

	public bool EndOfPacket
	{
		get
		{
			throw null;
		}
	}

	public string? FilePath
	{
		get
		{
			throw null;
		}
	}

	public FileStream? FileStream
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyMemory<byte>? MemoryBuffer
	{
		get
		{
			throw null;
		}
	}

	public int Offset
	{
		get
		{
			throw null;
		}
	}

	public long OffsetLong
	{
		get
		{
			throw null;
		}
	}

	public SendPacketsElement(byte[] buffer)
	{
	}

	public SendPacketsElement(byte[] buffer, int offset, int count)
	{
	}

	public SendPacketsElement(byte[] buffer, int offset, int count, bool endOfPacket)
	{
	}

	public SendPacketsElement(FileStream fileStream)
	{
	}

	public SendPacketsElement(FileStream fileStream, long offset, int count)
	{
	}

	public SendPacketsElement(FileStream fileStream, long offset, int count, bool endOfPacket)
	{
	}

	public SendPacketsElement(ReadOnlyMemory<byte> buffer)
	{
	}

	public SendPacketsElement(ReadOnlyMemory<byte> buffer, bool endOfPacket)
	{
	}

	public SendPacketsElement(string filepath)
	{
	}

	public SendPacketsElement(string filepath, int offset, int count)
	{
	}

	public SendPacketsElement(string filepath, int offset, int count, bool endOfPacket)
	{
	}

	public SendPacketsElement(string filepath, long offset, int count)
	{
	}

	public SendPacketsElement(string filepath, long offset, int count, bool endOfPacket)
	{
	}
}
