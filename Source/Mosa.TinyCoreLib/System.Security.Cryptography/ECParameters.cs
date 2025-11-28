namespace System.Security.Cryptography;

public struct ECParameters
{
	public ECCurve Curve;

	public byte[]? D;

	public ECPoint Q;

	public void Validate()
	{
	}
}
