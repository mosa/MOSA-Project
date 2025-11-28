using System.Collections.Immutable;

namespace System.Reflection.Metadata.Ecma335;

public readonly struct PermissionSetEncoder
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public BlobBuilder Builder
	{
		get
		{
			throw null;
		}
	}

	public PermissionSetEncoder(BlobBuilder builder)
	{
		throw null;
	}

	public PermissionSetEncoder AddPermission(string typeName, ImmutableArray<byte> encodedArguments)
	{
		throw null;
	}

	public PermissionSetEncoder AddPermission(string typeName, BlobBuilder encodedArguments)
	{
		throw null;
	}
}
