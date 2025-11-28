namespace System.Runtime.Serialization;

[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public class ObjectIDGenerator
{
	public virtual long GetId(object obj, out bool firstTime)
	{
		throw null;
	}

	public virtual long HasId(object obj, out bool firstTime)
	{
		throw null;
	}
}
