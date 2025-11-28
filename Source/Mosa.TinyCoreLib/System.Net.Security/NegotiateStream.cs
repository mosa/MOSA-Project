using System.IO;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Security;

public class NegotiateStream : AuthenticatedStream
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

	public virtual TokenImpersonationLevel ImpersonationLevel
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

	public override long Length
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

	public virtual IIdentity RemoteIdentity
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

	public NegotiateStream(Stream innerStream)
		: base(null, leaveInnerStreamOpen: false)
	{
	}

	public NegotiateStream(Stream innerStream, bool leaveInnerStreamOpen)
		: base(null, leaveInnerStreamOpen: false)
	{
	}

	public virtual void AuthenticateAsClient()
	{
	}

	public virtual void AuthenticateAsClient(NetworkCredential credential, ChannelBinding? binding, string targetName)
	{
	}

	public virtual void AuthenticateAsClient(NetworkCredential credential, ChannelBinding? binding, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel)
	{
	}

	public virtual void AuthenticateAsClient(NetworkCredential credential, string targetName)
	{
	}

	public virtual void AuthenticateAsClient(NetworkCredential credential, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel)
	{
	}

	public virtual Task AuthenticateAsClientAsync()
	{
		throw null;
	}

	public virtual Task AuthenticateAsClientAsync(NetworkCredential credential, ChannelBinding? binding, string targetName)
	{
		throw null;
	}

	public virtual Task AuthenticateAsClientAsync(NetworkCredential credential, ChannelBinding? binding, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel)
	{
		throw null;
	}

	public virtual Task AuthenticateAsClientAsync(NetworkCredential credential, string targetName)
	{
		throw null;
	}

	public virtual Task AuthenticateAsClientAsync(NetworkCredential credential, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel)
	{
		throw null;
	}

	public virtual void AuthenticateAsServer()
	{
	}

	public virtual void AuthenticateAsServer(NetworkCredential credential, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel)
	{
	}

	public virtual void AuthenticateAsServer(NetworkCredential credential, ExtendedProtectionPolicy? policy, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel)
	{
	}

	public virtual void AuthenticateAsServer(ExtendedProtectionPolicy? policy)
	{
	}

	public virtual Task AuthenticateAsServerAsync()
	{
		throw null;
	}

	public virtual Task AuthenticateAsServerAsync(NetworkCredential credential, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel)
	{
		throw null;
	}

	public virtual Task AuthenticateAsServerAsync(NetworkCredential credential, ExtendedProtectionPolicy? policy, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel)
	{
		throw null;
	}

	public virtual Task AuthenticateAsServerAsync(ExtendedProtectionPolicy? policy)
	{
		throw null;
	}

	public virtual IAsyncResult BeginAuthenticateAsClient(AsyncCallback? asyncCallback, object? asyncState)
	{
		throw null;
	}

	public virtual IAsyncResult BeginAuthenticateAsClient(NetworkCredential credential, ChannelBinding? binding, string targetName, AsyncCallback? asyncCallback, object? asyncState)
	{
		throw null;
	}

	public virtual IAsyncResult BeginAuthenticateAsClient(NetworkCredential credential, ChannelBinding? binding, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel, AsyncCallback? asyncCallback, object? asyncState)
	{
		throw null;
	}

	public virtual IAsyncResult BeginAuthenticateAsClient(NetworkCredential credential, string targetName, AsyncCallback? asyncCallback, object? asyncState)
	{
		throw null;
	}

	public virtual IAsyncResult BeginAuthenticateAsClient(NetworkCredential credential, string targetName, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel allowedImpersonationLevel, AsyncCallback? asyncCallback, object? asyncState)
	{
		throw null;
	}

	public virtual IAsyncResult BeginAuthenticateAsServer(AsyncCallback? asyncCallback, object? asyncState)
	{
		throw null;
	}

	public virtual IAsyncResult BeginAuthenticateAsServer(NetworkCredential credential, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel, AsyncCallback? asyncCallback, object? asyncState)
	{
		throw null;
	}

	public virtual IAsyncResult BeginAuthenticateAsServer(NetworkCredential credential, ExtendedProtectionPolicy? policy, ProtectionLevel requiredProtectionLevel, TokenImpersonationLevel requiredImpersonationLevel, AsyncCallback? asyncCallback, object? asyncState)
	{
		throw null;
	}

	public virtual IAsyncResult BeginAuthenticateAsServer(ExtendedProtectionPolicy? policy, AsyncCallback? asyncCallback, object? asyncState)
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

	public override void Flush()
	{
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
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

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw null;
	}

	public override void SetLength(long value)
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
