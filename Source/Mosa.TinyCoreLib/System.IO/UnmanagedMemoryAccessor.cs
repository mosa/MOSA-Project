using System.Runtime.InteropServices;

namespace System.IO;

public class UnmanagedMemoryAccessor : IDisposable
{
	public bool CanRead
	{
		get
		{
			throw null;
		}
	}

	public bool CanWrite
	{
		get
		{
			throw null;
		}
	}

	public long Capacity
	{
		get
		{
			throw null;
		}
	}

	protected bool IsOpen
	{
		get
		{
			throw null;
		}
	}

	protected UnmanagedMemoryAccessor()
	{
	}

	public UnmanagedMemoryAccessor(SafeBuffer buffer, long offset, long capacity)
	{
	}

	public UnmanagedMemoryAccessor(SafeBuffer buffer, long offset, long capacity, FileAccess access)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	protected void Initialize(SafeBuffer buffer, long offset, long capacity, FileAccess access)
	{
	}

	public int ReadArray<T>(long position, T[] array, int offset, int count) where T : struct
	{
		throw null;
	}

	public bool ReadBoolean(long position)
	{
		throw null;
	}

	public byte ReadByte(long position)
	{
		throw null;
	}

	public char ReadChar(long position)
	{
		throw null;
	}

	public decimal ReadDecimal(long position)
	{
		throw null;
	}

	public double ReadDouble(long position)
	{
		throw null;
	}

	public short ReadInt16(long position)
	{
		throw null;
	}

	public int ReadInt32(long position)
	{
		throw null;
	}

	public long ReadInt64(long position)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public sbyte ReadSByte(long position)
	{
		throw null;
	}

	public float ReadSingle(long position)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public ushort ReadUInt16(long position)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public uint ReadUInt32(long position)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public ulong ReadUInt64(long position)
	{
		throw null;
	}

	public void Read<T>(long position, out T structure) where T : struct
	{
		throw null;
	}

	public void Write(long position, bool value)
	{
	}

	public void Write(long position, byte value)
	{
	}

	public void Write(long position, char value)
	{
	}

	public void Write(long position, decimal value)
	{
	}

	public void Write(long position, double value)
	{
	}

	public void Write(long position, short value)
	{
	}

	public void Write(long position, int value)
	{
	}

	public void Write(long position, long value)
	{
	}

	[CLSCompliant(false)]
	public void Write(long position, sbyte value)
	{
	}

	public void Write(long position, float value)
	{
	}

	[CLSCompliant(false)]
	public void Write(long position, ushort value)
	{
	}

	[CLSCompliant(false)]
	public void Write(long position, uint value)
	{
	}

	[CLSCompliant(false)]
	public void Write(long position, ulong value)
	{
	}

	public void WriteArray<T>(long position, T[] array, int offset, int count) where T : struct
	{
	}

	public void Write<T>(long position, ref T structure) where T : struct
	{
	}
}
