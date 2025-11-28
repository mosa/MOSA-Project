namespace System.Security.Policy;

public class CodeConnectAccess
{
	public static readonly string AnyScheme;

	public static readonly int DefaultPort;

	public static readonly int OriginPort;

	public static readonly string OriginScheme;

	public int Port
	{
		get
		{
			throw null;
		}
	}

	public string Scheme
	{
		get
		{
			throw null;
		}
	}

	public CodeConnectAccess(string allowScheme, int allowPort)
	{
	}

	public static CodeConnectAccess CreateAnySchemeAccess(int allowPort)
	{
		throw null;
	}

	public static CodeConnectAccess CreateOriginSchemeAccess(int allowPort)
	{
		throw null;
	}

	public override bool Equals(object o)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}
}
