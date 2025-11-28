using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail;

public class LinkedResource : AttachmentBase
{
	public Uri? ContentLink
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public LinkedResource(Stream contentStream)
		: base((string)null)
	{
	}

	public LinkedResource(Stream contentStream, ContentType? contentType)
		: base((string)null)
	{
	}

	public LinkedResource(Stream contentStream, string? mediaType)
		: base((string)null)
	{
	}

	public LinkedResource(string fileName)
		: base((string)null)
	{
	}

	public LinkedResource(string fileName, ContentType? contentType)
		: base((string)null)
	{
	}

	public LinkedResource(string fileName, string? mediaType)
		: base((string)null)
	{
	}

	public static LinkedResource CreateLinkedResourceFromString(string content)
	{
		throw null;
	}

	public static LinkedResource CreateLinkedResourceFromString(string content, ContentType? contentType)
	{
		throw null;
	}

	public static LinkedResource CreateLinkedResourceFromString(string content, Encoding? contentEncoding, string? mediaType)
	{
		throw null;
	}
}
