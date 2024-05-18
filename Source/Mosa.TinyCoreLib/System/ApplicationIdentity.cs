using System.Runtime.Serialization;

namespace System;

public sealed class ApplicationIdentity : ISerializable
{
	public string CodeBase
	{
		get
		{
			throw null;
		}
	}

	public string FullName
	{
		get
		{
			throw null;
		}
	}

	public ApplicationIdentity(string applicationIdentityFullName)
	{
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
