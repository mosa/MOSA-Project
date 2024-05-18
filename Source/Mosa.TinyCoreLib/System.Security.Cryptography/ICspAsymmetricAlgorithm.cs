namespace System.Security.Cryptography;

public interface ICspAsymmetricAlgorithm
{
	CspKeyContainerInfo CspKeyContainerInfo { get; }

	byte[] ExportCspBlob(bool includePrivateParameters);

	void ImportCspBlob(byte[] rawData);
}
