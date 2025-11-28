namespace System.Runtime.Serialization;

[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class SafeSerializationEventArgs : EventArgs
{
	public StreamingContext StreamingContext
	{
		get
		{
			throw null;
		}
	}

	internal SafeSerializationEventArgs()
	{
	}

	public void AddSerializedState(ISafeSerializationData serializedState)
	{
	}
}
