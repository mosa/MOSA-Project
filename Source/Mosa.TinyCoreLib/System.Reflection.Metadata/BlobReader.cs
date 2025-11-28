namespace System.Reflection.Metadata;

public struct BlobReader
{
	private int _dummyPrimitive;

	public unsafe byte* CurrentPointer
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

	public unsafe byte* StartPointer
	{
		get
		{
			throw null;
		}
	}

	public unsafe BlobReader(byte* buffer, int length)
	{
		throw null;
	}

	public void Align(byte alignment)
	{
	}

	public int IndexOf(byte value)
	{
		throw null;
	}

	public BlobHandle ReadBlobHandle()
	{
		throw null;
	}

	public bool ReadBoolean()
	{
		throw null;
	}

	public byte ReadByte()
	{
		throw null;
	}

	public byte[] ReadBytes(int byteCount)
	{
		throw null;
	}

	public void ReadBytes(int byteCount, byte[] buffer, int bufferOffset)
	{
	}

	public char ReadChar()
	{
		throw null;
	}

	public int ReadCompressedInteger()
	{
		throw null;
	}

	public int ReadCompressedSignedInteger()
	{
		throw null;
	}

	public object? ReadConstant(ConstantTypeCode typeCode)
	{
		throw null;
	}

	public DateTime ReadDateTime()
	{
		throw null;
	}

	public decimal ReadDecimal()
	{
		throw null;
	}

	public double ReadDouble()
	{
		throw null;
	}

	public Guid ReadGuid()
	{
		throw null;
	}

	public short ReadInt16()
	{
		throw null;
	}

	public int ReadInt32()
	{
		throw null;
	}

	public long ReadInt64()
	{
		throw null;
	}

	public sbyte ReadSByte()
	{
		throw null;
	}

	public SerializationTypeCode ReadSerializationTypeCode()
	{
		throw null;
	}

	public string? ReadSerializedString()
	{
		throw null;
	}

	public SignatureHeader ReadSignatureHeader()
	{
		throw null;
	}

	public SignatureTypeCode ReadSignatureTypeCode()
	{
		throw null;
	}

	public float ReadSingle()
	{
		throw null;
	}

	public EntityHandle ReadTypeHandle()
	{
		throw null;
	}

	public ushort ReadUInt16()
	{
		throw null;
	}

	public uint ReadUInt32()
	{
		throw null;
	}

	public ulong ReadUInt64()
	{
		throw null;
	}

	public string ReadUTF16(int byteCount)
	{
		throw null;
	}

	public string ReadUTF8(int byteCount)
	{
		throw null;
	}

	public void Reset()
	{
	}

	public bool TryReadCompressedInteger(out int value)
	{
		throw null;
	}

	public bool TryReadCompressedSignedInteger(out int value)
	{
		throw null;
	}
}
