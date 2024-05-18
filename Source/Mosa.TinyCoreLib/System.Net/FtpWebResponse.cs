using System.IO;

namespace System.Net;

public class FtpWebResponse : WebResponse, IDisposable
{
	public string? BannerMessage
	{
		get
		{
			throw null;
		}
	}

	public override long ContentLength
	{
		get
		{
			throw null;
		}
	}

	public string? ExitMessage
	{
		get
		{
			throw null;
		}
	}

	public override WebHeaderCollection Headers
	{
		get
		{
			throw null;
		}
	}

	public DateTime LastModified
	{
		get
		{
			throw null;
		}
	}

	public override Uri ResponseUri
	{
		get
		{
			throw null;
		}
	}

	public FtpStatusCode StatusCode
	{
		get
		{
			throw null;
		}
	}

	public string? StatusDescription
	{
		get
		{
			throw null;
		}
	}

	public override bool SupportsHeaders
	{
		get
		{
			throw null;
		}
	}

	public string? WelcomeMessage
	{
		get
		{
			throw null;
		}
	}

	internal FtpWebResponse()
	{
	}

	public override void Close()
	{
	}

	public override Stream GetResponseStream()
	{
		throw null;
	}
}
