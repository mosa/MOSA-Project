using System.IO;
using System.Threading.Tasks;

namespace System.Net.Security;

public abstract class AuthenticatedStream : Stream
{
	protected Stream InnerStream
	{
		get
		{
			throw null;
		}
	}

	public abstract bool IsAuthenticated { get; }

	public abstract bool IsEncrypted { get; }

	public abstract bool IsMutuallyAuthenticated { get; }

	public abstract bool IsServer { get; }

	public abstract bool IsSigned { get; }

	public bool LeaveInnerStreamOpen
	{
		get
		{
			throw null;
		}
	}

	protected AuthenticatedStream(Stream innerStream, bool leaveInnerStreamOpen)
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override ValueTask DisposeAsync()
	{
		throw null;
	}
}
