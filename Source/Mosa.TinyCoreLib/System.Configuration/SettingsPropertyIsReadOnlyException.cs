using System.Runtime.Serialization;

namespace System.Configuration;

public class SettingsPropertyIsReadOnlyException : Exception
{
	public SettingsPropertyIsReadOnlyException()
	{
	}

	protected SettingsPropertyIsReadOnlyException(SerializationInfo info, StreamingContext context)
	{
	}

	public SettingsPropertyIsReadOnlyException(string message)
	{
	}

	public SettingsPropertyIsReadOnlyException(string message, Exception innerException)
	{
	}
}
