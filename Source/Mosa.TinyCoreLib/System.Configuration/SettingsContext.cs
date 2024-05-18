using System.Collections;
using System.Runtime.Serialization;

namespace System.Configuration;

public class SettingsContext : Hashtable
{
	public SettingsContext()
	{
	}

	protected SettingsContext(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
