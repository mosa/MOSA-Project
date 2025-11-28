namespace System.Security.Cryptography;

public class FromBase64Transform : IDisposable, ICryptoTransform
{
	public virtual bool CanReuseTransform
	{
		get
		{
			throw null;
		}
	}

	public bool CanTransformMultipleBlocks
	{
		get
		{
			throw null;
		}
	}

	public int InputBlockSize
	{
		get
		{
			throw null;
		}
	}

	public int OutputBlockSize
	{
		get
		{
			throw null;
		}
	}

	public FromBase64Transform()
	{
	}

	public FromBase64Transform(FromBase64TransformMode whitespaces)
	{
	}

	public void Clear()
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~FromBase64Transform()
	{
	}

	public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
	{
		throw null;
	}

	public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
	{
		throw null;
	}
}
