namespace System.Runtime.Serialization;

[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public class SurrogateSelector : ISurrogateSelector
{
	public virtual void AddSurrogate(Type type, StreamingContext context, ISerializationSurrogate surrogate)
	{
	}

	public virtual void ChainSelector(ISurrogateSelector selector)
	{
	}

	public virtual ISurrogateSelector? GetNextSelector()
	{
		throw null;
	}

	public virtual ISerializationSurrogate? GetSurrogate(Type type, StreamingContext context, out ISurrogateSelector selector)
	{
		throw null;
	}

	public virtual void RemoveSurrogate(Type type, StreamingContext context)
	{
	}
}
