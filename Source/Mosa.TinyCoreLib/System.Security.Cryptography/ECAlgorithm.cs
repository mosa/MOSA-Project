namespace System.Security.Cryptography;

public abstract class ECAlgorithm : AsymmetricAlgorithm
{
	public virtual byte[] ExportECPrivateKey()
	{
		throw null;
	}

	public string ExportECPrivateKeyPem()
	{
		throw null;
	}

	public virtual ECParameters ExportExplicitParameters(bool includePrivateParameters)
	{
		throw null;
	}

	public virtual ECParameters ExportParameters(bool includePrivateParameters)
	{
		throw null;
	}

	public virtual void GenerateKey(ECCurve curve)
	{
	}

	public virtual void ImportECPrivateKey(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public override void ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public override void ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public override void ImportFromEncryptedPem(ReadOnlySpan<char> input, ReadOnlySpan<byte> passwordBytes)
	{
	}

	public override void ImportFromEncryptedPem(ReadOnlySpan<char> input, ReadOnlySpan<char> password)
	{
	}

	public override void ImportFromPem(ReadOnlySpan<char> input)
	{
	}

	public virtual void ImportParameters(ECParameters parameters)
	{
	}

	public override void ImportPkcs8PrivateKey(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public override void ImportSubjectPublicKeyInfo(ReadOnlySpan<byte> source, out int bytesRead)
	{
		throw null;
	}

	public virtual bool TryExportECPrivateKey(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public bool TryExportECPrivateKeyPem(Span<char> destination, out int charsWritten)
	{
		throw null;
	}

	public override bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public override bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public override bool TryExportPkcs8PrivateKey(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public override bool TryExportSubjectPublicKeyInfo(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
