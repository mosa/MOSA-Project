using System.Collections;
using System.Runtime.Serialization;

namespace System.Configuration;

public class SettingsAttributeDictionary : Hashtable
{
	public SettingsAttributeDictionary()
	{
	}

	public SettingsAttributeDictionary(SettingsAttributeDictionary attributes)
	{
	}

	protected SettingsAttributeDictionary(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
