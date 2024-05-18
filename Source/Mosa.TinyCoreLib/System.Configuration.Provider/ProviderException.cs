using System.Runtime.Serialization;

namespace System.Configuration.Provider;

public class ProviderException : Exception
{
	public ProviderException()
	{
	}

	protected ProviderException(SerializationInfo info, StreamingContext context)
	{
	}

	public ProviderException(string message)
	{
	}

	public ProviderException(string message, Exception innerException)
	{
	}
}
