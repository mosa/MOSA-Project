using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Mime;

namespace System.Net.Mail;

public abstract class AttachmentBase : IDisposable
{
	public string ContentId
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

	public Stream ContentStream
	{
		get
		{
			throw null;
		}
	}

	public ContentType ContentType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TransferEncoding TransferEncoding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected AttachmentBase(Stream contentStream)
	{
	}

	protected AttachmentBase(Stream contentStream, ContentType? contentType)
	{
	}

	protected AttachmentBase(Stream contentStream, string? mediaType)
	{
	}

	protected AttachmentBase(string fileName)
	{
	}

	protected AttachmentBase(string fileName, ContentType? contentType)
	{
	}

	protected AttachmentBase(string fileName, string? mediaType)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}
}
