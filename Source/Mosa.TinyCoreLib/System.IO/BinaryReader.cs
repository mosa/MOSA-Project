using System.Text;

namespace System.IO;

public class BinaryReader : IDisposable
{
	public virtual Stream BaseStream
	{
		get
		{
			throw null;
		}
	}

	public BinaryReader(Stream input)
	{
	}

	public BinaryReader(Stream input, Encoding encoding)
	{
	}

	public BinaryReader(Stream input, Encoding encoding, bool leaveOpen)
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

	protected virtual void FillBuffer(int numBytes)
	{
	}

	public virtual int PeekChar()
	{
		throw null;
	}

	public virtual int Read()
	{
		throw null;
	}

	public virtual int Read(byte[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual int Read(char[] buffer, int index, int count)
	{
		throw null;
	}

	public virtual int Read(Span<byte> buffer)
	{
		throw null;
	}

	public virtual int Read(Span<char> buffer)
	{
		throw null;
	}

	public int Read7BitEncodedInt()
	{
		throw null;
	}

	public long Read7BitEncodedInt64()
	{
		throw null;
	}

	public virtual bool ReadBoolean()
	{
		throw null;
	}

	public virtual byte ReadByte()
	{
		throw null;
	}

	public virtual byte[] ReadBytes(int count)
	{
		throw null;
	}

	public virtual char ReadChar()
	{
		throw null;
	}

	public virtual char[] ReadChars(int count)
	{
		throw null;
	}

	public virtual decimal ReadDecimal()
	{
		throw null;
	}

	public virtual double ReadDouble()
	{
		throw null;
	}

	public virtual Half ReadHalf()
	{
		throw null;
	}

	public virtual short ReadInt16()
	{
		throw null;
	}

	public virtual int ReadInt32()
	{
		throw null;
	}

	public virtual long ReadInt64()
	{
		throw null;
	}

	[CLSCompliant(false)]
	public virtual sbyte ReadSByte()
	{
		throw null;
	}

	public virtual float ReadSingle()
	{
		throw null;
	}

	public virtual string ReadString()
	{
		throw null;
	}

	[CLSCompliant(false)]
	public virtual ushort ReadUInt16()
	{
		throw null;
	}

	[CLSCompliant(false)]
	public virtual uint ReadUInt32()
	{
		throw null;
	}

	[CLSCompliant(false)]
	public virtual ulong ReadUInt64()
	{
		throw null;
	}
}
