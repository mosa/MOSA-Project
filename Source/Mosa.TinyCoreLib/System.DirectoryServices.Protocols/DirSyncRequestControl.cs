namespace System.DirectoryServices.Protocols;

public class DirSyncRequestControl : DirectoryControl
{
	public int AttributeCount
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[] Cookie
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DirectorySynchronizationOptions Option
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DirSyncRequestControl()
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public DirSyncRequestControl(byte[] cookie)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public DirSyncRequestControl(byte[] cookie, DirectorySynchronizationOptions option)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public DirSyncRequestControl(byte[] cookie, DirectorySynchronizationOptions option, int attributeCount)
		: base(null, null, isCritical: false, serverSide: false)
	{
	}

	public override byte[] GetValue()
	{
		throw null;
	}
}
