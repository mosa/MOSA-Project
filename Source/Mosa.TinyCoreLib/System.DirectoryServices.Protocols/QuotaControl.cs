using System.Runtime.Versioning;
using System.Security.Principal;

namespace System.DirectoryServices.Protocols;

[SupportedOSPlatform("windows")]
public class QuotaControl : DirectoryControl
{
	public SecurityIdentifier QuerySid
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public QuotaControl()
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public QuotaControl(SecurityIdentifier querySid)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public override byte[] GetValue()
	{
		throw null;
	}
}
