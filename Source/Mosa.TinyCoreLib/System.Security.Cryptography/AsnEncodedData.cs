namespace System.Security.Cryptography;

public class AsnEncodedData
{
	public Oid? Oid
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[] RawData
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected AsnEncodedData()
	{
	}

	public AsnEncodedData(byte[] rawData)
	{
	}

	public AsnEncodedData(ReadOnlySpan<byte> rawData)
	{
	}

	public AsnEncodedData(AsnEncodedData asnEncodedData)
	{
	}

	public AsnEncodedData(Oid? oid, byte[] rawData)
	{
	}

	public AsnEncodedData(Oid? oid, ReadOnlySpan<byte> rawData)
	{
	}

	public AsnEncodedData(string oid, byte[] rawData)
	{
	}

	public AsnEncodedData(string oid, ReadOnlySpan<byte> rawData)
	{
	}

	public virtual void CopyFrom(AsnEncodedData asnEncodedData)
	{
	}

	public virtual string Format(bool multiLine)
	{
		throw null;
	}
}
