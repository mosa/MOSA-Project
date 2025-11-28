namespace System.Reflection.Metadata.Ecma335;

public readonly struct InstructionEncoder
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public BlobBuilder CodeBuilder
	{
		get
		{
			throw null;
		}
	}

	public ControlFlowBuilder? ControlFlowBuilder
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

	public InstructionEncoder(BlobBuilder codeBuilder, ControlFlowBuilder? controlFlowBuilder = null)
	{
		throw null;
	}

	public void Branch(ILOpCode code, LabelHandle label)
	{
	}

	public void Call(EntityHandle methodHandle)
	{
	}

	public void Call(MemberReferenceHandle methodHandle)
	{
	}

	public void Call(MethodDefinitionHandle methodHandle)
	{
	}

	public void Call(MethodSpecificationHandle methodHandle)
	{
	}

	public void CallIndirect(StandaloneSignatureHandle signature)
	{
	}

	public LabelHandle DefineLabel()
	{
		throw null;
	}

	public void LoadArgument(int argumentIndex)
	{
	}

	public void LoadArgumentAddress(int argumentIndex)
	{
	}

	public void LoadConstantI4(int value)
	{
	}

	public void LoadConstantI8(long value)
	{
	}

	public void LoadConstantR4(float value)
	{
	}

	public void LoadConstantR8(double value)
	{
	}

	public void LoadLocal(int slotIndex)
	{
	}

	public void LoadLocalAddress(int slotIndex)
	{
	}

	public void LoadString(UserStringHandle handle)
	{
	}

	public void MarkLabel(LabelHandle label)
	{
	}

	public void OpCode(ILOpCode code)
	{
	}

	public void StoreArgument(int argumentIndex)
	{
	}

	public void StoreLocal(int slotIndex)
	{
	}

	public SwitchInstructionEncoder Switch(int branchCount)
	{
		throw null;
	}

	public void Token(int token)
	{
	}

	public void Token(EntityHandle handle)
	{
	}
}
