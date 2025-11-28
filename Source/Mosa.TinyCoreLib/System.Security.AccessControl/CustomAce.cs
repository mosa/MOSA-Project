namespace System.Security.AccessControl;

public sealed class CustomAce : GenericAce
{
	public static readonly int MaxOpaqueLength;

	public override int BinaryLength
	{
		get
		{
			throw null;
		}
	}

	public int OpaqueLength
	{
		get
		{
			throw null;
		}
	}

	public CustomAce(AceType type, AceFlags flags, byte[]? opaque)
	{
	}

	public override void GetBinaryForm(byte[] binaryForm, int offset)
	{
	}

	public byte[]? GetOpaque()
	{
		throw null;
	}

	public void SetOpaque(byte[]? opaque)
	{
	}
}
