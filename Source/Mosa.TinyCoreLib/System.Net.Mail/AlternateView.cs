using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail;

public class AlternateView : AttachmentBase
{
	public Uri? BaseUri
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public LinkedResourceCollection LinkedResources
	{
		get
		{
			throw null;
		}
	}

	public AlternateView(Stream contentStream)
		: base((string)null)
	{
	}

	public AlternateView(Stream contentStream, ContentType? contentType)
		: base((string)null)
	{
	}

	public AlternateView(Stream contentStream, string? mediaType)
		: base((string)null)
	{
	}

	public AlternateView(string fileName)
		: base((string)null)
	{
	}

	public AlternateView(string fileName, ContentType? contentType)
		: base((string)null)
	{
	}

	public AlternateView(string fileName, string? mediaType)
		: base((string)null)
	{
	}

	public static AlternateView CreateAlternateViewFromString(string content)
	{
		throw null;
	}

	public static AlternateView CreateAlternateViewFromString(string content, ContentType? contentType)
	{
		throw null;
	}

	public static AlternateView CreateAlternateViewFromString(string content, Encoding? contentEncoding, string? mediaType)
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}
}
