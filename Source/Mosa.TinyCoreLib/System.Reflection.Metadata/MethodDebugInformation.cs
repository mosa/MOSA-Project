namespace System.Reflection.Metadata;

public readonly struct MethodDebugInformation
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public DocumentHandle Document
	{
		get
		{
			throw null;
		}
	}

	public StandaloneSignatureHandle LocalSignature
	{
		get
		{
			throw null;
		}
	}

	public BlobHandle SequencePointsBlob
	{
		get
		{
			throw null;
		}
	}

	public SequencePointCollection GetSequencePoints()
	{
		throw null;
	}

	public MethodDefinitionHandle GetStateMachineKickoffMethod()
	{
		throw null;
	}
}
