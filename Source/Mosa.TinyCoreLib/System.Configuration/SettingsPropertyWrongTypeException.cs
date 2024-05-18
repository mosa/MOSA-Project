using System.Runtime.Serialization;

namespace System.Configuration;

public class SettingsPropertyWrongTypeException : Exception
{
	public SettingsPropertyWrongTypeException()
	{
	}

	protected SettingsPropertyWrongTypeException(SerializationInfo info, StreamingContext context)
	{
	}

	public SettingsPropertyWrongTypeException(string message)
	{
	}

	public SettingsPropertyWrongTypeException(string message, Exception innerException)
	{
	}
}
