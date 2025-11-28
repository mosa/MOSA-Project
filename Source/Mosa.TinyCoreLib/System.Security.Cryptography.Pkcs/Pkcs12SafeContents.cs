using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography.Pkcs;

public sealed class Pkcs12SafeContents
{
	public Pkcs12ConfidentialityMode ConfidentialityMode
	{
		get
		{
			throw null;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public Pkcs12CertBag AddCertificate(X509Certificate2 certificate)
	{
		throw null;
	}

	public Pkcs12KeyBag AddKeyUnencrypted(AsymmetricAlgorithm key)
	{
		throw null;
	}

	public Pkcs12SafeContentsBag AddNestedContents(Pkcs12SafeContents safeContents)
	{
		throw null;
	}

	public void AddSafeBag(Pkcs12SafeBag safeBag)
	{
	}

	public Pkcs12SecretBag AddSecret(Oid secretType, ReadOnlyMemory<byte> secretValue)
	{
		throw null;
	}

	public Pkcs12ShroudedKeyBag AddShroudedKey(AsymmetricAlgorithm key, byte[]? passwordBytes, PbeParameters pbeParameters)
	{
		throw null;
	}

	public Pkcs12ShroudedKeyBag AddShroudedKey(AsymmetricAlgorithm key, ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters)
	{
		throw null;
	}

	public Pkcs12ShroudedKeyBag AddShroudedKey(AsymmetricAlgorithm key, ReadOnlySpan<char> password, PbeParameters pbeParameters)
	{
		throw null;
	}

	public Pkcs12ShroudedKeyBag AddShroudedKey(AsymmetricAlgorithm key, string? password, PbeParameters pbeParameters)
	{
		throw null;
	}

	public void Decrypt(byte[]? passwordBytes)
	{
	}

	public void Decrypt(ReadOnlySpan<byte> passwordBytes)
	{
	}

	public void Decrypt(ReadOnlySpan<char> password)
	{
	}

	public void Decrypt(string? password)
	{
	}

	public IEnumerable<Pkcs12SafeBag> GetBags()
	{
		throw null;
	}
}
