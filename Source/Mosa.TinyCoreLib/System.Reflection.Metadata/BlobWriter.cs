using System.Collections.Immutable;
using System.IO;

namespace System.Reflection.Metadata;

public struct BlobWriter
{
	private object _dummy;

	private int _dummyPrimitive;

	public Blob Blob
	{
		get
		{
			throw null;
		}
	}

	public int Length
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
		set
		{
		}
	}

	public int RemainingBytes
	{
		get
		{
			throw null;
		}
	}

	public BlobWriter(byte[] buffer)
	{
		throw null;
	}

	public BlobWriter(byte[] buffer, int start, int count)
	{
		throw null;
	}

	public BlobWriter(int size)
	{
		throw null;
	}

	public BlobWriter(Blob blob)
	{
		throw null;
	}

	public void Align(int alignment)
	{
	}

	public void Clear()
	{
	}

	public bool ContentEquals(BlobWriter other)
	{
		throw null;
	}

	public void PadTo(int offset)
	{
	}

	public byte[] ToArray()
	{
		throw null;
	}

	public byte[] ToArray(int start, int byteCount)
	{
		throw null;
	}

	public ImmutableArray<byte> ToImmutableArray()
	{
		throw null;
	}

	public ImmutableArray<byte> ToImmutableArray(int start, int byteCount)
	{
		throw null;
	}

	public void WriteBoolean(bool value)
	{
	}

	public void WriteByte(byte value)
	{
	}

	public unsafe void WriteBytes(byte* buffer, int byteCount)
	{
	}

	public void WriteBytes(byte value, int byteCount)
	{
	}

	public void WriteBytes(byte[] buffer)
	{
	}

	public void WriteBytes(byte[] buffer, int start, int byteCount)
	{
	}

	public void WriteBytes(ImmutableArray<byte> buffer)
	{
	}

	public void WriteBytes(ImmutableArray<byte> buffer, int start, int byteCount)
	{
	}

	public int WriteBytes(Stream source, int byteCount)
	{
		throw null;
	}

	public void WriteBytes(BlobBuilder source)
	{
	}

	public void WriteCompressedInteger(int value)
	{
	}

	public void WriteCompressedSignedInteger(int value)
	{
	}

	public void WriteConstant(object? value)
	{
	}

	public void WriteDateTime(DateTime value)
	{
	}

	public void WriteDecimal(decimal value)
	{
	}

	public void WriteDouble(double value)
	{
	}

	public void WriteGuid(Guid value)
	{
	}

	public void WriteInt16(short value)
	{
	}

	public void WriteInt16BE(short value)
	{
	}

	public void WriteInt32(int value)
	{
	}

	public void WriteInt32BE(int value)
	{
	}

	public void WriteInt64(long value)
	{
	}

	public void WriteReference(int reference, bool isSmall)
	{
	}

	public void WriteSByte(sbyte value)
	{
	}

	public void WriteSerializedString(string? str)
	{
	}

	public void WriteSingle(float value)
	{
	}

	public void WriteUInt16(ushort value)
	{
	}

	public void WriteUInt16BE(ushort value)
	{
	}

	public void WriteUInt32(uint value)
	{
	}

	public void WriteUInt32BE(uint value)
	{
	}

	public void WriteUInt64(ulong value)
	{
	}

	public void WriteUserString(string value)
	{
	}

	public void WriteUTF16(char[] value)
	{
	}

	public void WriteUTF16(string value)
	{
	}

	public void WriteUTF8(string value, bool allowUnpairedSurrogates)
	{
	}
}
