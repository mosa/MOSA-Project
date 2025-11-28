using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.Design;

namespace System.DirectoryServices;

[TypeConverter(typeof(DirectoryEntryConverter))]
public class DirectoryEntry : Component
{
	[DefaultValue(AuthenticationTypes.Secure)]
	public AuthenticationTypes AuthenticationType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DirectoryEntries Children
	{
		get
		{
			throw null;
		}
	}

	public Guid Guid
	{
		get
		{
			throw null;
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public string NativeGuid
	{
		get
		{
			throw null;
		}
	}

	public object NativeObject
	{
		get
		{
			throw null;
		}
	}

	public ActiveDirectorySecurity ObjectSecurity
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DirectoryEntryConfiguration? Options
	{
		get
		{
			throw null;
		}
	}

	public DirectoryEntry Parent
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue(null)]
	public string? Password
	{
		set
		{
		}
	}

	[DefaultValue("")]
	public string Path
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public PropertyCollection Properties
	{
		get
		{
			throw null;
		}
	}

	public string SchemaClassName
	{
		get
		{
			throw null;
		}
	}

	public DirectoryEntry SchemaEntry
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue(true)]
	public bool UsePropertyCache
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(null)]
	public string? Username
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DirectoryEntry()
	{
	}

	public DirectoryEntry(object adsObject)
	{
	}

	public DirectoryEntry(string? path)
	{
	}

	public DirectoryEntry(string? path, string? username, string? password)
	{
	}

	public DirectoryEntry(string? path, string? username, string? password, AuthenticationTypes authenticationType)
	{
	}

	public void Close()
	{
	}

	public void CommitChanges()
	{
	}

	public DirectoryEntry CopyTo(DirectoryEntry newParent)
	{
		throw null;
	}

	public DirectoryEntry CopyTo(DirectoryEntry newParent, string? newName)
	{
		throw null;
	}

	public void DeleteTree()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public static bool Exists(string path)
	{
		throw null;
	}

	public object? Invoke(string methodName, params object?[]? args)
	{
		throw null;
	}

	public object? InvokeGet(string propertyName)
	{
		throw null;
	}

	public void InvokeSet(string propertyName, params object?[]? args)
	{
	}

	public void MoveTo(DirectoryEntry newParent)
	{
	}

	public void MoveTo(DirectoryEntry newParent, string? newName)
	{
	}

	public void RefreshCache()
	{
	}

	public void RefreshCache(string[] propertyNames)
	{
	}

	public void Rename(string? newName)
	{
	}
}
