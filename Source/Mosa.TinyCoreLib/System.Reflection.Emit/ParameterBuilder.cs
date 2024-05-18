namespace System.Reflection.Emit;

public abstract class ParameterBuilder
{
	public virtual int Attributes
	{
		get
		{
			throw null;
		}
	}

	public bool IsIn
	{
		get
		{
			throw null;
		}
	}

	public bool IsOptional
	{
		get
		{
			throw null;
		}
	}

	public bool IsOut
	{
		get
		{
			throw null;
		}
	}

	public virtual string? Name
	{
		get
		{
			throw null;
		}
	}

	public virtual int Position
	{
		get
		{
			throw null;
		}
	}

	public virtual void SetConstant(object? defaultValue)
	{
	}

	public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
	{
	}

	public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
	{
	}

	protected abstract void SetCustomAttributeCore(ConstructorInfo con, ReadOnlySpan<byte> binaryAttribute);
}
