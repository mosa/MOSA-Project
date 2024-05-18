using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Security;

[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public class PermissionSet : ICollection, IEnumerable, IDeserializationCallback, ISecurityEncodable, IStackWalk
{
	public virtual int Count
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	public virtual object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	public PermissionSet(PermissionState state)
	{
	}

	public PermissionSet(PermissionSet? permSet)
	{
	}

	public IPermission? AddPermission(IPermission? perm)
	{
		throw null;
	}

	protected virtual IPermission? AddPermissionImpl(IPermission? perm)
	{
		throw null;
	}

	public void Assert()
	{
	}

	public bool ContainsNonCodeAccessPermissions()
	{
		throw null;
	}

	[Obsolete]
	public static byte[] ConvertPermissionSet(string inFormat, byte[] inData, string outFormat)
	{
		throw null;
	}

	public virtual PermissionSet Copy()
	{
		throw null;
	}

	public virtual void CopyTo(Array array, int index)
	{
	}

	public void Demand()
	{
	}

	[Obsolete]
	public void Deny()
	{
	}

	public override bool Equals(object? o)
	{
		throw null;
	}

	public virtual void FromXml(SecurityElement et)
	{
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	protected virtual IEnumerator GetEnumeratorImpl()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public IPermission? GetPermission(Type? permClass)
	{
		throw null;
	}

	protected virtual IPermission? GetPermissionImpl(Type? permClass)
	{
		throw null;
	}

	public PermissionSet? Intersect(PermissionSet? other)
	{
		throw null;
	}

	public bool IsEmpty()
	{
		throw null;
	}

	public bool IsSubsetOf(PermissionSet? target)
	{
		throw null;
	}

	public bool IsUnrestricted()
	{
		throw null;
	}

	public void PermitOnly()
	{
	}

	public IPermission? RemovePermission(Type? permClass)
	{
		throw null;
	}

	protected virtual IPermission? RemovePermissionImpl(Type? permClass)
	{
		throw null;
	}

	public static void RevertAssert()
	{
	}

	public IPermission? SetPermission(IPermission? perm)
	{
		throw null;
	}

	protected virtual IPermission? SetPermissionImpl(IPermission? perm)
	{
		throw null;
	}

	void IDeserializationCallback.OnDeserialization(object? sender)
	{
	}

	public override string ToString()
	{
		throw null;
	}

	public virtual SecurityElement? ToXml()
	{
		throw null;
	}

	public PermissionSet? Union(PermissionSet? other)
	{
		throw null;
	}
}
