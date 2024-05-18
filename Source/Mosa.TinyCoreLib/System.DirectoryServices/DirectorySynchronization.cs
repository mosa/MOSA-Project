using System.ComponentModel;

namespace System.DirectoryServices;

public class DirectorySynchronization
{
	[DefaultValue(DirectorySynchronizationOptions.None)]
	public DirectorySynchronizationOptions Option
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DirectorySynchronization()
	{
	}

	public DirectorySynchronization(byte[]? cookie)
	{
	}

	public DirectorySynchronization(DirectorySynchronization? sync)
	{
	}

	public DirectorySynchronization(DirectorySynchronizationOptions option)
	{
	}

	public DirectorySynchronization(DirectorySynchronizationOptions option, byte[]? cookie)
	{
	}

	public DirectorySynchronization Copy()
	{
		throw null;
	}

	public byte[] GetDirectorySynchronizationCookie()
	{
		throw null;
	}

	public void ResetDirectorySynchronizationCookie()
	{
	}

	public void ResetDirectorySynchronizationCookie(byte[]? cookie)
	{
	}
}
