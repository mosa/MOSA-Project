using System.Runtime.Serialization;

namespace System.DirectoryServices.ActiveDirectory;

public class ForestTrustCollisionException : ActiveDirectoryOperationException, ISerializable
{
	public ForestTrustRelationshipCollisionCollection? Collisions
	{
		get
		{
			throw null;
		}
	}

	public ForestTrustCollisionException()
	{
	}

	protected ForestTrustCollisionException(SerializationInfo info, StreamingContext context)
	{
	}

	public ForestTrustCollisionException(string? message)
	{
	}

	public ForestTrustCollisionException(string? message, Exception? inner)
	{
	}

	public ForestTrustCollisionException(string? message, Exception? inner, ForestTrustRelationshipCollisionCollection? collisions)
	{
	}

	public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
