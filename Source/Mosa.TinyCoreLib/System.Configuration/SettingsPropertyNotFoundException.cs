using System.Runtime.Serialization;

namespace System.Configuration;

public class SettingsPropertyNotFoundException : Exception
{
	public SettingsPropertyNotFoundException()
	{
	}

	protected SettingsPropertyNotFoundException(SerializationInfo info, StreamingContext context)
	{
	}

	public SettingsPropertyNotFoundException(string message)
	{
	}

	public SettingsPropertyNotFoundException(string message, Exception innerException)
	{
	}
}
