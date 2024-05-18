using System.Text;
using System.Threading.Tasks;

namespace System.IO;

public class BinaryWriter : IAsyncDisposable, IDisposable
{
	public static readonly BinaryWriter Null;

	protected Stream OutStream;

	public virtual Stream BaseStream
	{
		get
		{
			throw null;
		}
	}

	protected BinaryWriter()
	{
	}

	public BinaryWriter(Stream output)
	{
	}

	public BinaryWriter(Stream output, Encoding encoding)
	{
	}

	public BinaryWriter(Stream output, Encoding encoding, bool leaveOpen)
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

	public virtual long Seek(int offset, SeekOrigin origin)
	{
		throw null;
	}

	public virtual void Write(bool value)
	{
	}

	public virtual void Write(byte value)
	{
	}

	public virtual void Write(byte[] buffer)
	{
	}

	public virtual void Write(byte[] buffer, int index, int count)
	{
	}

	public virtual void Write(char ch)
	{
	}

	public virtual void Write(char[] chars)
	{
	}

	public virtual void Write(char[] chars, int index, int count)
	{
	}

	public virtual void Write(decimal value)
	{
	}

	public virtual void Write(double value)
	{
	}

	public virtual void Write(Half value)
	{
	}

	public virtual void Write(short value)
	{
	}

	public virtual void Write(int value)
	{
	}

	public virtual void Write(long value)
	{
	}

	public virtual void Write(ReadOnlySpan<byte> buffer)
	{
	}

	public virtual void Write(ReadOnlySpan<char> chars)
	{
	}

	[CLSCompliant(false)]
	public virtual void Write(sbyte value)
	{
	}

	public virtual void Write(float value)
	{
	}

	public virtual void Write(string value)
	{
	}

	[CLSCompliant(false)]
	public virtual void Write(ushort value)
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

	public void Write7BitEncodedInt(int value)
	{
	}

	public void Write7BitEncodedInt64(long value)
	{
	}
}
