using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Security;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public class HostProtectionException : SystemException
{
	public HostProtectionResource DemandedResources
	{
		get
		{
			throw null;
		}
	}

	public HostProtectionResource ProtectedResources
	{
		get
		{
			throw null;
		}
	}

	public HostProtectionException()
	{
	}

	protected HostProtectionException(SerializationInfo info, StreamingContext context)
	{
	}

	public HostProtectionException(string message)
	{
	}

	public HostProtectionException(string message, Exception e)
	{
	}

	public HostProtectionException(string message, HostProtectionResource protectedResources, HostProtectionResource demandedResources)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
