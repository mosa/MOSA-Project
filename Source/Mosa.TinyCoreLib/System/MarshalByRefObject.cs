namespace System;

public abstract class MarshalByRefObject
{
	[Obsolete("This Remoting API is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0010", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public object GetLifetimeService()
	{
		throw null;
	}

	[Obsolete("This Remoting API is not supported and throws PlatformNotSupportedException.", DiagnosticId = "SYSLIB0010", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public virtual object InitializeLifetimeService()
	{
		throw null;
	}

	protected MarshalByRefObject MemberwiseClone(bool cloneIdentity)
	{
		throw null;
	}
}
