using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Security.Authentication.ExtendedProtection;

public class ExtendedProtectionPolicy : ISerializable
{
	public ChannelBinding? CustomChannelBinding
	{
		get
		{
			throw null;
		}
	}

	public ServiceNameCollection? CustomServiceNames
	{
		get
		{
			throw null;
		}
	}

	public static bool OSSupportsExtendedProtection
	{
		get
		{
			throw null;
		}
	}

	public PolicyEnforcement PolicyEnforcement
	{
		get
		{
			throw null;
		}
	}

	public ProtectionScenario ProtectionScenario
	{
		get
		{
			throw null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	protected ExtendedProtectionPolicy(SerializationInfo info, StreamingContext context)
	{
	}

	public ExtendedProtectionPolicy(PolicyEnforcement policyEnforcement)
	{
	}

	public ExtendedProtectionPolicy(PolicyEnforcement policyEnforcement, ChannelBinding customChannelBinding)
	{
	}

	public ExtendedProtectionPolicy(PolicyEnforcement policyEnforcement, ProtectionScenario protectionScenario, ICollection? customServiceNames)
	{
	}

	public ExtendedProtectionPolicy(PolicyEnforcement policyEnforcement, ProtectionScenario protectionScenario, ServiceNameCollection? customServiceNames)
	{
	}

	void ISerializable.GetObjectData(SerializationInfo? info, StreamingContext context)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
