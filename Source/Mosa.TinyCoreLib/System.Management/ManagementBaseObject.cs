using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Management;

[ToolboxItem(false)]
public class ManagementBaseObject : Component, ICloneable, ISerializable
{
	public virtual ManagementPath ClassPath
	{
		get
		{
			throw null;
		}
	}

	public object this[string propertyName]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual PropertyDataCollection Properties
	{
		get
		{
			throw null;
		}
	}

	public virtual QualifierDataCollection Qualifiers
	{
		get
		{
			throw null;
		}
	}

	public virtual PropertyDataCollection SystemProperties
	{
		get
		{
			throw null;
		}
	}

	private protected ManagementBaseObject()
	{
	}

	protected ManagementBaseObject(SerializationInfo info, StreamingContext context)
	{
	}

	public virtual object Clone()
	{
		throw null;
	}

	public bool CompareTo(ManagementBaseObject otherObject, ComparisonSettings settings)
	{
		throw null;
	}

	public new void Dispose()
	{
	}

	public override bool Equals(object obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public object GetPropertyQualifierValue(string propertyName, string qualifierName)
	{
		throw null;
	}

	public object GetPropertyValue(string propertyName)
	{
		throw null;
	}

	public object GetQualifierValue(string qualifierName)
	{
		throw null;
	}

	public string GetText(TextFormat format)
	{
		throw null;
	}

	public static explicit operator IntPtr(ManagementBaseObject managementObject)
	{
		throw null;
	}

	public void SetPropertyQualifierValue(string propertyName, string qualifierName, object qualifierValue)
	{
	}

	public void SetPropertyValue(string propertyName, object propertyValue)
	{
	}

	public void SetQualifierValue(string qualifierName, object qualifierValue)
	{
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
