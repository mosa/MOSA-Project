using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail;

public class MailMessage : IDisposable
{
	public AlternateViewCollection AlternateViews
	{
		get
		{
			throw null;
		}
	}

	public AttachmentCollection Attachments
	{
		get
		{
			throw null;
		}
	}

	public MailAddressCollection Bcc
	{
		get
		{
			throw null;
		}
	}

	public string Body
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

	public Encoding? BodyEncoding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TransferEncoding BodyTransferEncoding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public MailAddressCollection CC
	{
		get
		{
			throw null;
		}
	}

	public DeliveryNotificationOptions DeliveryNotificationOptions
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public MailAddress? From
	{
		get
		{
			throw null;
		}
		[param: DisallowNull]
		set
		{
		}
	}

	public NameValueCollection Headers
	{
		get
		{
			throw null;
		}
	}

	public Encoding? HeadersEncoding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsBodyHtml
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public MailPriority Priority
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Obsolete("ReplyTo has been deprecated. Use ReplyToList instead, which can accept multiple addresses.")]
	public MailAddress? ReplyTo
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public MailAddressCollection ReplyToList
	{
		get
		{
			throw null;
		}
	}

	public MailAddress? Sender
	{
		get
		{
			throw null;
		}
		[param: DisallowNull]
		set
		{
		}
	}

	public string Subject
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

	public Encoding? SubjectEncoding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public MailAddressCollection To
	{
		get
		{
			throw null;
		}
	}

	public MailMessage()
	{
	}

	public MailMessage(MailAddress from, MailAddress to)
	{
	}

	public MailMessage(string from, string to)
	{
	}

	public MailMessage(string from, string to, string? subject, string? body)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}
}
