namespace System.Security.Policy;

[Obsolete("This type is obsolete. See https://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
public sealed class PermissionRequestEvidence : EvidenceBase
{
	public PermissionSet DeniedPermissions
	{
		get
		{
			throw null;
		}
	}

	public PermissionSet OptionalPermissions
	{
		get
		{
			throw null;
		}
	}

	public PermissionSet RequestedPermissions
	{
		get
		{
			throw null;
		}
	}

	public PermissionRequestEvidence(PermissionSet request, PermissionSet optional, PermissionSet denied)
	{
	}

	public PermissionRequestEvidence Copy()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
