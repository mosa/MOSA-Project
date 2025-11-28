namespace System.Reflection.Metadata.Ecma335;

public readonly struct MethodBodyStreamEncoder
{
	public readonly struct MethodBody
	{
		private readonly object _dummy;

		private readonly int _dummyPrimitive;

		public ExceptionRegionEncoder ExceptionRegions
		{
			get
			{
				throw null;
			}
		}

		public Blob Instructions
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
		}
	}

	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public BlobBuilder Builder
	{
		get
		{
			throw null;
		}
	}

	public MethodBodyStreamEncoder(BlobBuilder builder)
	{
		throw null;
	}

	public MethodBody AddMethodBody(int codeSize, int maxStack, int exceptionRegionCount, bool hasSmallExceptionRegions, StandaloneSignatureHandle localVariablesSignature, MethodBodyAttributes attributes)
	{
		throw null;
	}

	public MethodBody AddMethodBody(int codeSize, int maxStack = 8, int exceptionRegionCount = 0, bool hasSmallExceptionRegions = true, StandaloneSignatureHandle localVariablesSignature = default(StandaloneSignatureHandle), MethodBodyAttributes attributes = MethodBodyAttributes.InitLocals, bool hasDynamicStackAllocation = false)
	{
		throw null;
	}

	public int AddMethodBody(InstructionEncoder instructionEncoder, int maxStack, StandaloneSignatureHandle localVariablesSignature, MethodBodyAttributes attributes)
	{
		throw null;
	}

	public int AddMethodBody(InstructionEncoder instructionEncoder, int maxStack = 8, StandaloneSignatureHandle localVariablesSignature = default(StandaloneSignatureHandle), MethodBodyAttributes attributes = MethodBodyAttributes.InitLocals, bool hasDynamicStackAllocation = false)
	{
		throw null;
	}
}
