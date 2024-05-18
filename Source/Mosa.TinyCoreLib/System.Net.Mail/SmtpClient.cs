using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Mail;

[UnsupportedOSPlatform("browser")]
public class SmtpClient : IDisposable
{
	public X509CertificateCollection ClientCertificates
	{
		get
		{
			throw null;
		}
	}

	public ICredentialsByHost? Credentials
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SmtpDeliveryFormat DeliveryFormat
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SmtpDeliveryMethod DeliveryMethod
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool EnableSsl
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Host
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

	public string? PickupDirectoryLocation
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Port
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ServicePoint ServicePoint
	{
		get
		{
			throw null;
		}
	}

	public string? TargetName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Timeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool UseDefaultCredentials
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public event SendCompletedEventHandler? SendCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	public SmtpClient()
	{
	}

	public SmtpClient(string? host)
	{
	}

	public SmtpClient(string? host, int port)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	protected void OnSendCompleted(AsyncCompletedEventArgs e)
	{
	}

	public void Send(MailMessage message)
	{
	}

	public void Send(string from, string recipients, string? subject, string? body)
	{
	}

	public void SendAsync(MailMessage message, object? userToken)
	{
	}

	public void SendAsync(string from, string recipients, string? subject, string? body, object? userToken)
	{
	}

	public void SendAsyncCancel()
	{
	}

	public Task SendMailAsync(MailMessage message)
	{
		throw null;
	}

	public Task SendMailAsync(MailMessage message, CancellationToken cancellationToken)
	{
		throw null;
	}

	public Task SendMailAsync(string from, string recipients, string? subject, string? body)
	{
		throw null;
	}

	public Task SendMailAsync(string from, string recipients, string? subject, string? body, CancellationToken cancellationToken)
	{
		throw null;
	}
}
