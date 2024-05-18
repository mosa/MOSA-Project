using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail;

public class Attachment : AttachmentBase
{
	public ContentDisposition? ContentDisposition
	{
		get
		{
			throw null;
		}
	}

	public string? Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Encoding? NameEncoding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Attachment(Stream contentStream, ContentType contentType)
		: base((string)null)
	{
	}

	public Attachment(Stream contentStream, string? name)
		: base((string)null)
	{
	}

	public Attachment(Stream contentStream, string? name, string? mediaType)
		: base((string)null)
	{
	}

	public Attachment(string fileName)
		: base((string)null)
	{
	}

	public Attachment(string fileName, ContentType contentType)
		: base((string)null)
	{
	}

	public Attachment(string fileName, string? mediaType)
		: base((string)null)
	{
	}

	public static Attachment CreateAttachmentFromString(string content, ContentType contentType)
	{
		throw null;
	}

	public static Attachment CreateAttachmentFromString(string content, string? name)
	{
		throw null;
	}

	public static Attachment CreateAttachmentFromString(string content, string? name, Encoding? contentEncoding, string? mediaType)
	{
		throw null;
	}
}
