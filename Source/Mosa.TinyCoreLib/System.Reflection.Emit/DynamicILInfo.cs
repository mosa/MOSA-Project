namespace System.Reflection.Emit;

public sealed class DynamicILInfo
{
	public DynamicMethod DynamicMethod
	{
		get
		{
			throw null;
		}
	}

	internal DynamicILInfo()
	{
	}

	public int GetTokenFor(byte[] signature)
	{
		throw null;
	}

	public int GetTokenFor(DynamicMethod method)
	{
		throw null;
	}

	public int GetTokenFor(RuntimeFieldHandle field)
	{
		throw null;
	}

	public int GetTokenFor(RuntimeFieldHandle field, RuntimeTypeHandle contextType)
	{
		throw null;
	}

	public int GetTokenFor(RuntimeMethodHandle method)
	{
		throw null;
	}

	public int GetTokenFor(RuntimeMethodHandle method, RuntimeTypeHandle contextType)
	{
		throw null;
	}

	public int GetTokenFor(RuntimeTypeHandle type)
	{
		throw null;
	}

	public int GetTokenFor(string literal)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe void SetCode(byte* code, int codeSize, int maxStackSize)
	{
	}

	public void SetCode(byte[]? code, int maxStackSize)
	{
	}

	[CLSCompliant(false)]
	public unsafe void SetExceptions(byte* exceptions, int exceptionsSize)
	{
	}

	public void SetExceptions(byte[]? exceptions)
	{
	}

	[CLSCompliant(false)]
	public unsafe void SetLocalSignature(byte* localSignature, int signatureSize)
	{
	}

	public void SetLocalSignature(byte[]? localSignature)
	{
	}
}
