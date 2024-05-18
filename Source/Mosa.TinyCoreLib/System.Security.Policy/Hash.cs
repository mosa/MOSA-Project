using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace System.Security.Policy;

public sealed class Hash : EvidenceBase, ISerializable
{
	public byte[] MD5
	{
		get
		{
			throw null;
		}
	}

	public byte[] SHA1
	{
		get
		{
			throw null;
		}
	}

	public byte[] SHA256
	{
		get
		{
			throw null;
		}
	}

	public Hash(Assembly assembly)
	{
	}

	public static Hash CreateMD5(byte[] md5)
	{
		throw null;
	}

	public static Hash CreateSHA1(byte[] sha1)
	{
		throw null;
	}

	public static Hash CreateSHA256(byte[] sha256)
	{
		throw null;
	}

	public byte[] GenerateHash(HashAlgorithm hashAlg)
	{
		throw null;
	}

	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
