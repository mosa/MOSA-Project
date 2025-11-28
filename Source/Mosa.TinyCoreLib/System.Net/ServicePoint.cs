using System.Security.Cryptography.X509Certificates;

namespace System.Net;

public class ServicePoint
{
	public Uri Address
	{
		get
		{
			throw null;
		}
	}

	public BindIPEndPoint? BindIPEndPointDelegate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public X509Certificate? Certificate
	{
		get
		{
			throw null;
		}
	}

	public X509Certificate? ClientCertificate
	{
		get
		{
			throw null;
		}
	}

	public int ConnectionLeaseTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int ConnectionLimit
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string ConnectionName
	{
		get
		{
			throw null;
		}
	}

	public int CurrentConnections
	{
		get
		{
			throw null;
		}
	}

	public bool Expect100Continue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DateTime IdleSince
	{
		get
		{
			throw null;
		}
	}

	public int MaxIdleTime
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual Version ProtocolVersion
	{
		get
		{
			throw null;
		}
	}

	public int ReceiveBufferSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool SupportsPipelining
	{
		get
		{
			throw null;
		}
	}

	public bool UseNagleAlgorithm
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal ServicePoint()
	{
	}

	public bool CloseConnectionGroup(string connectionGroupName)
	{
		throw null;
	}

	public void SetTcpKeepAlive(bool enabled, int keepAliveTime, int keepAliveInterval)
	{
	}
}
