namespace System.DirectoryServices.Protocols;

public class LdapDirectoryIdentifier : DirectoryIdentifier
{
	public bool Connectionless
	{
		get
		{
			throw null;
		}
	}

	public bool FullyQualifiedDnsHostName
	{
		get
		{
			throw null;
		}
	}

	public int PortNumber
	{
		get
		{
			throw null;
		}
	}

	public string[] Servers
	{
		get
		{
			throw null;
		}
	}

	public LdapDirectoryIdentifier(string server)
	{
	}

	public LdapDirectoryIdentifier(string server, bool fullyQualifiedDnsHostName, bool connectionless)
	{
	}

	public LdapDirectoryIdentifier(string server, int portNumber)
	{
	}

	public LdapDirectoryIdentifier(string server, int portNumber, bool fullyQualifiedDnsHostName, bool connectionless)
	{
	}

	public LdapDirectoryIdentifier(string[] servers, bool fullyQualifiedDnsHostName, bool connectionless)
	{
	}

	public LdapDirectoryIdentifier(string[] servers, int portNumber, bool fullyQualifiedDnsHostName, bool connectionless)
	{
	}
}
