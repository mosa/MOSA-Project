using System.Buffers;
using System.Security.Principal;

namespace System.Net.Security;

public sealed class NegotiateAuthentication : IDisposable
{
	public TokenImpersonationLevel ImpersonationLevel
	{
		get
		{
			throw null;
		}
	}

	public bool IsAuthenticated
	{
		get
		{
			throw null;
		}
	}

	public bool IsEncrypted
	{
		get
		{
			throw null;
		}
	}

	public bool IsMutuallyAuthenticated
	{
		get
		{
			throw null;
		}
	}

	public bool IsServer
	{
		get
		{
			throw null;
		}
	}

	public bool IsSigned
	{
		get
		{
			throw null;
		}
	}

	public string Package
	{
		get
		{
			throw null;
		}
	}

	public ProtectionLevel ProtectionLevel
	{
		get
		{
			throw null;
		}
	}

	public IIdentity RemoteIdentity
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
	}

	public NegotiateAuthentication(NegotiateAuthenticationClientOptions clientOptions)
	{
	}

	public NegotiateAuthentication(NegotiateAuthenticationServerOptions serverOptions)
	{
	}

	public void Dispose()
	{
	}

	public byte[]? GetOutgoingBlob(ReadOnlySpan<byte> incomingBlob, out NegotiateAuthenticationStatusCode statusCode)
	{
		throw null;
	}

	public string? GetOutgoingBlob(string? incomingBlob, out NegotiateAuthenticationStatusCode statusCode)
	{
		throw null;
	}

	public NegotiateAuthenticationStatusCode Unwrap(ReadOnlySpan<byte> input, IBufferWriter<byte> outputWriter, out bool wasEncrypted)
	{
		throw null;
	}

	public NegotiateAuthenticationStatusCode UnwrapInPlace(Span<byte> input, out int unwrappedOffset, out int unwrappedLength, out bool wasEncrypted)
	{
		throw null;
	}

	public NegotiateAuthenticationStatusCode Wrap(ReadOnlySpan<byte> input, IBufferWriter<byte> outputWriter, bool requestEncryption, out bool isEncrypted)
	{
		throw null;
	}
}
