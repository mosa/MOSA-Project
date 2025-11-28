using System.Collections;

namespace System.Security.Permissions;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class KeyContainerPermissionAccessEntryCollection : ICollection, IEnumerable
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public bool IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	public KeyContainerPermissionAccessEntry this[int index]
	{
		get
		{
			throw null;
		}
	}

	public object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	public int Add(KeyContainerPermissionAccessEntry accessEntry)
	{
		throw null;
	}

	public void Clear()
	{
	}

	public void CopyTo(Array array, int index)
	{
	}

	public void CopyTo(KeyContainerPermissionAccessEntry[] array, int index)
	{
	}

	public KeyContainerPermissionAccessEntryEnumerator GetEnumerator()
	{
		throw null;
	}

	public int IndexOf(KeyContainerPermissionAccessEntry accessEntry)
	{
		throw null;
	}

	public void Remove(KeyContainerPermissionAccessEntry accessEntry)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
