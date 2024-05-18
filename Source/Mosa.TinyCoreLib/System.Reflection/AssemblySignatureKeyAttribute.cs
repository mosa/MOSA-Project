namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
public sealed class AssemblySignatureKeyAttribute : Attribute
{
	public string Countersignature
	{
		get
		{
			throw null;
		}
	}

	public string PublicKey
	{
		get
		{
			throw null;
		}
	}

	public AssemblySignatureKeyAttribute(string publicKey, string countersignature)
	{
	}
}
