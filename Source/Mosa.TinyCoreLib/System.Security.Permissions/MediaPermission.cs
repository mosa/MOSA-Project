namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class MediaPermission : CodeAccessPermission, IUnrestrictedPermission
{
	public MediaPermissionAudio Audio
	{
		get
		{
			throw null;
		}
	}

	public MediaPermissionImage Image
	{
		get
		{
			throw null;
		}
	}

	public MediaPermissionVideo Video
	{
		get
		{
			throw null;
		}
	}

	public MediaPermission()
	{
	}

	public MediaPermission(MediaPermissionAudio permissionAudio)
	{
	}

	public MediaPermission(MediaPermissionAudio permissionAudio, MediaPermissionVideo permissionVideo, MediaPermissionImage permissionImage)
	{
	}

	public MediaPermission(MediaPermissionImage permissionImage)
	{
	}

	public MediaPermission(MediaPermissionVideo permissionVideo)
	{
	}

	public MediaPermission(PermissionState state)
	{
	}

	public override IPermission Copy()
	{
		throw null;
	}

	public override void FromXml(SecurityElement securityElement)
	{
	}

	public override IPermission Intersect(IPermission target)
	{
		throw null;
	}

	public override bool IsSubsetOf(IPermission target)
	{
		throw null;
	}

	public bool IsUnrestricted()
	{
		throw null;
	}

	public override SecurityElement ToXml()
	{
		throw null;
	}

	public override IPermission Union(IPermission target)
	{
		throw null;
	}
}
