using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace System.Security.Policy;

public sealed class HashMembershipCondition : IDeserializationCallback, ISerializable, ISecurityEncodable, ISecurityPolicyEncodable, IMembershipCondition
{
	public HashAlgorithm HashAlgorithm
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[] HashValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public HashMembershipCondition(HashAlgorithm hashAlg, byte[] value)
	{
	}

	public bool Check(Evidence evidence)
	{
		throw null;
	}

	public IMembershipCondition Copy()
	{
		throw null;
	}

	public override bool Equals(object o)
	{
		throw null;
	}

	public void FromXml(SecurityElement e)
	{
	}

	public void FromXml(SecurityElement e, PolicyLevel level)
	{
	}

	public override int GetHashCode()
	{
		throw null;
	}

	void IDeserializationCallback.OnDeserialization(object sender)
	{
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public override string ToString()
	{
		throw null;
	}

	public SecurityElement ToXml()
	{
		throw null;
	}

	public SecurityElement ToXml(PolicyLevel level)
	{
		throw null;
	}
}
