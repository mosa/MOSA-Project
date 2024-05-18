using System.Runtime.Versioning;

namespace System.Threading;

public class Barrier : IDisposable
{
	public long CurrentPhaseNumber
	{
		get
		{
			throw null;
		}
	}

	public int ParticipantCount
	{
		get
		{
			throw null;
		}
	}

	public int ParticipantsRemaining
	{
		get
		{
			throw null;
		}
	}

	public Barrier(int participantCount)
	{
	}

	public Barrier(int participantCount, Action<Barrier>? postPhaseAction)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public long AddParticipant()
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public long AddParticipants(int participantCount)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public void RemoveParticipant()
	{
	}

	public void RemoveParticipants(int participantCount)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public void SignalAndWait()
	{
	}

	[UnsupportedOSPlatform("browser")]
	public bool SignalAndWait(int millisecondsTimeout)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public bool SignalAndWait(int millisecondsTimeout, CancellationToken cancellationToken)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public void SignalAndWait(CancellationToken cancellationToken)
	{
	}

	[UnsupportedOSPlatform("browser")]
	public bool SignalAndWait(TimeSpan timeout)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public bool SignalAndWait(TimeSpan timeout, CancellationToken cancellationToken)
	{
		throw null;
	}
}
