using System.Runtime.Serialization;

namespace System.Management;

public class ManagementObject : ManagementBaseObject, ICloneable
{
	public override ManagementPath ClassPath
	{
		get
		{
			throw null;
		}
	}

	public ObjectGetOptions Options
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual ManagementPath Path
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ManagementScope Scope
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ManagementObject()
	{
	}

	public ManagementObject(ManagementPath path)
	{
	}

	public ManagementObject(ManagementPath path, ObjectGetOptions options)
	{
	}

	public ManagementObject(ManagementScope scope, ManagementPath path, ObjectGetOptions options)
	{
	}

	protected ManagementObject(SerializationInfo info, StreamingContext context)
	{
	}

	public ManagementObject(string path)
	{
	}

	public ManagementObject(string path, ObjectGetOptions options)
	{
	}

	public ManagementObject(string scopeString, string pathString, ObjectGetOptions options)
	{
	}

	public override object Clone()
	{
		throw null;
	}

	public void CopyTo(ManagementOperationObserver watcher, ManagementPath path)
	{
	}

	public void CopyTo(ManagementOperationObserver watcher, ManagementPath path, PutOptions options)
	{
	}

	public void CopyTo(ManagementOperationObserver watcher, string path)
	{
	}

	public void CopyTo(ManagementOperationObserver watcher, string path, PutOptions options)
	{
	}

	public ManagementPath CopyTo(ManagementPath path)
	{
		throw null;
	}

	public ManagementPath CopyTo(ManagementPath path, PutOptions options)
	{
		throw null;
	}

	public ManagementPath CopyTo(string path)
	{
		throw null;
	}

	public ManagementPath CopyTo(string path, PutOptions options)
	{
		throw null;
	}

	public void Delete()
	{
	}

	public void Delete(DeleteOptions options)
	{
	}

	public void Delete(ManagementOperationObserver watcher)
	{
	}

	public void Delete(ManagementOperationObserver watcher, DeleteOptions options)
	{
	}

	public new void Dispose()
	{
	}

	public void Get()
	{
	}

	public void Get(ManagementOperationObserver watcher)
	{
	}

	public ManagementBaseObject GetMethodParameters(string methodName)
	{
		throw null;
	}

	protected override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public ManagementObjectCollection GetRelated()
	{
		throw null;
	}

	public void GetRelated(ManagementOperationObserver watcher)
	{
	}

	public void GetRelated(ManagementOperationObserver watcher, string relatedClass)
	{
	}

	public void GetRelated(ManagementOperationObserver watcher, string relatedClass, string relationshipClass, string relationshipQualifier, string relatedQualifier, string relatedRole, string thisRole, bool classDefinitionsOnly, EnumerationOptions options)
	{
	}

	public ManagementObjectCollection GetRelated(string relatedClass)
	{
		throw null;
	}

	public ManagementObjectCollection GetRelated(string relatedClass, string relationshipClass, string relationshipQualifier, string relatedQualifier, string relatedRole, string thisRole, bool classDefinitionsOnly, EnumerationOptions options)
	{
		throw null;
	}

	public ManagementObjectCollection GetRelationships()
	{
		throw null;
	}

	public void GetRelationships(ManagementOperationObserver watcher)
	{
	}

	public void GetRelationships(ManagementOperationObserver watcher, string relationshipClass)
	{
	}

	public void GetRelationships(ManagementOperationObserver watcher, string relationshipClass, string relationshipQualifier, string thisRole, bool classDefinitionsOnly, EnumerationOptions options)
	{
	}

	public ManagementObjectCollection GetRelationships(string relationshipClass)
	{
		throw null;
	}

	public ManagementObjectCollection GetRelationships(string relationshipClass, string relationshipQualifier, string thisRole, bool classDefinitionsOnly, EnumerationOptions options)
	{
		throw null;
	}

	public void InvokeMethod(ManagementOperationObserver watcher, string methodName, ManagementBaseObject inParameters, InvokeMethodOptions options)
	{
	}

	public void InvokeMethod(ManagementOperationObserver watcher, string methodName, object[] args)
	{
	}

	public ManagementBaseObject InvokeMethod(string methodName, ManagementBaseObject inParameters, InvokeMethodOptions options)
	{
		throw null;
	}

	public object InvokeMethod(string methodName, object[] args)
	{
		throw null;
	}

	public ManagementPath Put()
	{
		throw null;
	}

	public void Put(ManagementOperationObserver watcher)
	{
	}

	public void Put(ManagementOperationObserver watcher, PutOptions options)
	{
	}

	public ManagementPath Put(PutOptions options)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
