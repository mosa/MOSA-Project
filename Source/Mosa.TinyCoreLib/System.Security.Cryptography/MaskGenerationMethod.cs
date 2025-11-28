namespace System.Security.Cryptography;

public abstract class MaskGenerationMethod
{
	public abstract byte[] GenerateMask(byte[] rgbSeed, int cbReturn);
}
