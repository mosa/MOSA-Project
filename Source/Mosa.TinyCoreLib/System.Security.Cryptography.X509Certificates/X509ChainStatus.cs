using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography.X509Certificates;

public struct X509ChainStatus
{
	private object _dummy;

	private int _dummyPrimitive;

	public X509ChainStatusFlags Status
	{
		readonly get
		{
			throw null;
		}
		set
		{
		}
	}

	public string StatusInformation
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}
}
