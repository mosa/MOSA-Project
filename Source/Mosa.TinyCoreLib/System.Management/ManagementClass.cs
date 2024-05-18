using System.CodeDom;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace System.Management;

public class ManagementClass : ManagementObject
{
	public StringCollection Derivation
	{
		get
		{
			throw null;
		}
	}

	public MethodDataCollection Methods
	{
		get
		{
			throw null;
		}
	}

	public override ManagementPath Path
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ManagementClass()
	{
	}

	public ManagementClass(ManagementPath path)
	{
	}

	public ManagementClass(ManagementPath path, ObjectGetOptions options)
	{
	}

	public ManagementClass(ManagementScope scope, ManagementPath path, ObjectGetOptions options)
	{
	}

	protected ManagementClass(SerializationInfo info, StreamingContext context)
	{
	}

	public ManagementClass(string path)
	{
	}

	public ManagementClass(string path, ObjectGetOptions options)
	{
	}

	public ManagementClass(string scope, string path, ObjectGetOptions options)
	{
	}

	public override object Clone()
	{
		throw null;
	}

	public ManagementObject CreateInstance()
	{
		throw null;
	}

	public ManagementClass Derive(string newClassName)
	{
		throw null;
	}

	public ManagementObjectCollection GetInstances()
	{
		throw null;
	}

	public ManagementObjectCollection GetInstances(EnumerationOptions options)
	{
		throw null;
	}

	public void GetInstances(ManagementOperationObserver watcher)
	{
	}

	public void GetInstances(ManagementOperationObserver watcher, EnumerationOptions options)
	{
	}

	protected override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public ManagementObjectCollection GetRelatedClasses()
	{
		throw null;
	}

	public void GetRelatedClasses(ManagementOperationObserver watcher)
	{
	}

	public void GetRelatedClasses(ManagementOperationObserver watcher, string relatedClass)
	{
	}

	public void GetRelatedClasses(ManagementOperationObserver watcher, string relatedClass, string relationshipClass, string relationshipQualifier, string relatedQualifier, string relatedRole, string thisRole, EnumerationOptions options)
	{
	}

	public ManagementObjectCollection GetRelatedClasses(string relatedClass)
	{
		throw null;
	}

	public ManagementObjectCollection GetRelatedClasses(string relatedClass, string relationshipClass, string relationshipQualifier, string relatedQualifier, string relatedRole, string thisRole, EnumerationOptions options)
	{
		throw null;
	}

	public ManagementObjectCollection GetRelationshipClasses()
	{
		throw null;
	}

	public void GetRelationshipClasses(ManagementOperationObserver watcher)
	{
	}

	public void GetRelationshipClasses(ManagementOperationObserver watcher, string relationshipClass)
	{
	}

	public void GetRelationshipClasses(ManagementOperationObserver watcher, string relationshipClass, string relationshipQualifier, string thisRole, EnumerationOptions options)
	{
	}

	public ManagementObjectCollection GetRelationshipClasses(string relationshipClass)
	{
		throw null;
	}

	public ManagementObjectCollection GetRelationshipClasses(string relationshipClass, string relationshipQualifier, string thisRole, EnumerationOptions options)
	{
		throw null;
	}

	public CodeTypeDeclaration GetStronglyTypedClassCode(bool includeSystemClassInClassDef, bool systemPropertyClass)
	{
		throw null;
	}

	public bool GetStronglyTypedClassCode(CodeLanguage lang, string filePath, string classNamespace)
	{
		throw null;
	}

	public ManagementObjectCollection GetSubclasses()
	{
		throw null;
	}

	public ManagementObjectCollection GetSubclasses(EnumerationOptions options)
	{
		throw null;
	}

	public void GetSubclasses(ManagementOperationObserver watcher)
	{
	}

	public void GetSubclasses(ManagementOperationObserver watcher, EnumerationOptions options)
	{
	}
}
