namespace System.DirectoryServices.Protocols;

public class DirectoryControl
{
	public bool IsCritical
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ServerSide
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Type
	{
		get
		{
			throw null;
		}
	}

	public DirectoryControl(string type, byte[] value, bool isCritical, bool serverSide)
	{
	}

	public virtual byte[] GetValue()
	{
		throw null;
	}
}
