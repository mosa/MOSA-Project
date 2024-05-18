using System.IO;
using System.Runtime.Versioning;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Security;

public class SslStream : AuthenticatedStream
{
	public override bool CanRead
	{
		get
		{
			throw null;
		}
	}

	public override bool CanSeek
	{
		get
		{
			throw null;
		}
	}

	public override bool CanTimeout
	{
		get
		{
			throw null;
		}
	}

	public override bool CanWrite
	{
		get
		{
			throw null;
		}
	}

	public virtual bool CheckCertRevocationStatus
	{
		get
		{
			throw null;
		}
	}

	public virtual CipherAlgorithmType CipherAlgorithm
	{
		get
		{
			throw null;
		}
	}

	public virtual int CipherStrength
	{
		get
		{
			throw null;
		}
	}

	public virtual HashAlgorithmType HashAlgorithm
	{
		get
		{
			throw null;
		}
	}

	public virtual int HashStrength
	{
		get
		{
			throw null;
		}
	}

	public override bool IsAuthenticated
	{
		get
		{
			throw null;
		}
	}

	public override bool IsEncrypted
	{
		get
		{
			throw null;
		}
	}

	public override bool IsMutuallyAuthenticated
	{
		get
		{
			throw null;
		}
	}

	public override bool IsServer
	{
		get
		{
			throw null;
		}
	}

	public override bool IsSigned
	{
		get
		{
			throw null;
		}
	}

	public virtual ExchangeAlgorithmType KeyExchangeAlgorithm
	{
		get
		{
			throw null;
		}
	}

	public virtual int KeyExchangeStrength
	{
		get
		{
			throw null;
		}
	}

	public override long Length
	{
		get
		{
			throw null;
		}
	}

	public virtual X509Certificate? LocalCertificate
	{
		get
		{
			throw null;
		}
	}

	public SslApplicationProtocol NegotiatedApplicationProtocol
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	public virtual TlsCipherSuite NegotiatedCipherSuite
	{
		get
		{
			throw null;
		}
	}

	public override long Position
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override int ReadTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual X509Certificate? RemoteCertificate
	{
		get
		{
			throw null;
		}
	}

	public virtual SslProtocols SslProtocol
	{
		get
		{
			throw null;
		}
	}

	public string TargetHostName
	{
		get
		{
			throw null;
		}
	}

	public TransportContext TransportContext
	{
		get
		{
			throw null;
		}
	}

	public override int WriteTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SslStream(Stream innerStream)
		: base(null, leaveInnerStreamOpen: false)
	{
	}

	public SslStream(Stream innerStream, bool leaveInnerStreamOpen)
		: base(null, leaveInnerStreamOpen: false)
	{
	}

	public SslStream(Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback? userCertificateValidationCallback)
		: base(null, leaveInnerStreamOpen: false)
	{
	}

	public SslStream(Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback? userCertificateValidationCallback, LocalCertificateSelectionCallback? userCertificateSelectionCallback)
		: base(null, leaveInnerStreamOpen: false)
	{
	}

	public SslStream(Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback? userCertificateValidationCallback, LocalCertificateSelectionCallback? userCertificateSelectionCallback, EncryptionPolicy encryptionPolicy)
		: base(null, leaveInnerStreamOpen: false)
	{
	}

	public void AuthenticateAsClient(SslClientAuthenticationOptions sslClientAuthenticationOptions)
	{
	}

	public virtual void AuthenticateAsClient(string targetHost)
	{
	}

	public virtual void AuthenticateAsClient(string targetHost, X509CertificateCollection? clientCertificates, bool checkCertificateRevocation)
	{
	}

	public virtual void AuthenticateAsClient(string targetHost, X509CertificateCollection? clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
	{
	}

	public Task AuthenticateAsClientAsync(SslClientAuthenticationOptions sslClientAuthenticationOptions, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual Task AuthenticateAsClientAsync(string targetHost)
	{
		throw null;
	}

	public virtual Task AuthenticateAsClientAsync(string targetHost, X509CertificateCollection? clientCertificates, bool checkCertificateRevocation)
	{
		throw null;
	}

	public virtual Task AuthenticateAsClientAsync(string targetHost, X509CertificateCollection? clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
	{
		throw null;
	}

	public void AuthenticateAsServer(SslServerAuthenticationOptions sslServerAuthenticationOptions)
	{
	}

	public virtual void AuthenticateAsServer(X509Certificate serverCertificate)
	{
	}

	public virtual void AuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, bool checkCertificateRevocation)
	{
	}

	public virtual void AuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
	{
	}

	public Task AuthenticateAsServerAsync(ServerOptionsSelectionCallback optionsCallback, object? state, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public Task AuthenticateAsServerAsync(SslServerAuthenticationOptions sslServerAuthenticationOptions, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public virtual Task AuthenticateAsServerAsync(X509Certificate serverCertificate)
	{
		throw null;
	}

	public virtual Task AuthenticateAsServerAsync(X509Certificate serverCertificate, bool clientCertificateRequired, bool checkCertificateRevocation)
	{
		throw null;
	}

	public virtual Task AuthenticateAsServerAsync(X509Certificate serverCertificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
	{
		throw null;
	}

	public virtual IAsyncResult BeginAuthenticateAsClient(string targetHost, AsyncCallback? asyncCallback, object? asyncState)
	{
		throw null;
	}

	public virtual IAsyncResult BeginAuthenticateAsClient(string targetHost, X509CertificateCollection? clientCertificates, bool checkCertificateRevocation, AsyncCallback? asyncCallback, object? asyncState)
	{
		throw null;
	}

	public virtual IAsyncResult BeginAuthenticateAsClient(string targetHost, X509CertificateCollection? clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation, AsyncCallback? asyncCallback, object? asyncState)
	{
		throw null;
	}

	public virtual IAsyncResult BeginAuthenticateAsServer(X509Certificate serverCertificate, AsyncCallback? asyncCallback, object? asyncState)
	{
		throw null;
	}

	public virtual IAsyncResult BeginAuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, bool checkCertificateRevocation, AsyncCallback? asyncCallback, object? asyncState)
	{
		throw null;
	}

	public virtual IAsyncResult BeginAuthenticateAsServer(X509Certificate serverCertificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation, AsyncCallback? asyncCallback, object? asyncState)
	{
		throw null;
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback? asyncCallback, object? asyncState)
	{
		throw null;
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? asyncCallback, object? asyncState)
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override ValueTask DisposeAsync()
	{
		throw null;
	}

	public virtual void EndAuthenticateAsClient(IAsyncResult asyncResult)
	{
	}

	public virtual void EndAuthenticateAsServer(IAsyncResult asyncResult)
	{
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		throw null;
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
	}

	~SslStream()
	{
	}

	public override void Flush()
	{
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
	{
		throw null;
	}

	[SupportedOSPlatform("freebsd")]
	[SupportedOSPlatform("linux")]
	[SupportedOSPlatform("windows")]
	public virtual Task NegotiateClientCertificateAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw null;
	}

	public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		throw null;
	}

	public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public override int ReadByte()
	{
		throw null;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw null;
	}

	public override void SetLength(long value)
	{
	}

	public virtual Task ShutdownAsync()
	{
		throw null;
	}

	public void Write(byte[] buffer)
	{
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
	}

	public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		throw null;
	}

	public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}
}
