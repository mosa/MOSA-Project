using System.Runtime.Serialization;

namespace System.Reflection;

public sealed class Missing : ISerializable
{
	public static readonly Missing Value;

	internal Missing()
	{
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
