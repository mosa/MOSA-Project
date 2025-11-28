namespace System.Security.Cryptography;

public class ToBase64Transform : IDisposable, ICryptoTransform
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

	public void Clear()
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~ToBase64Transform()
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
