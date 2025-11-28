using System.Collections;
using System.Xml;

namespace System.Configuration;

public abstract class ConfigurationElement
{
	public Configuration CurrentConfiguration
	{
		get
		{
			throw null;
		}
	}

	public ElementInformation ElementInformation
	{
		get
		{
			throw null;
		}
	}

	protected virtual ConfigurationElementProperty ElementProperty
	{
		get
		{
			throw null;
		}
	}

	protected ContextInformation EvaluationContext
	{
		get
		{
			throw null;
		}
	}

	protected bool HasContext
	{
		get
		{
			throw null;
		}
	}

	protected object this[ConfigurationProperty prop]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected object this[string propertyName]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ConfigurationLockCollection LockAllAttributesExcept
	{
		get
		{
			throw null;
		}
	}

	public ConfigurationLockCollection LockAllElementsExcept
	{
		get
		{
			throw null;
		}
	}

	public ConfigurationLockCollection LockAttributes
	{
		get
		{
			throw null;
		}
	}

	public ConfigurationLockCollection LockElements
	{
		get
		{
			throw null;
		}
	}

	public bool LockItem
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected virtual ConfigurationPropertyCollection Properties
	{
		get
		{
			throw null;
		}
	}

	protected virtual void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
	{
	}

	public override bool Equals(object compareTo)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	protected virtual string GetTransformedAssemblyString(string assemblyName)
	{
		throw null;
	}

	protected virtual string GetTransformedTypeString(string typeName)
	{
		throw null;
	}

	protected virtual void Init()
	{
	}

	protected virtual void InitializeDefault()
	{
	}

	protected virtual bool IsModified()
	{
		throw null;
	}

	public virtual bool IsReadOnly()
	{
		throw null;
	}

	protected virtual void ListErrors(IList errorList)
	{
	}

	protected virtual bool OnDeserializeUnrecognizedAttribute(string name, string value)
	{
		throw null;
	}

	protected virtual bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
	{
		throw null;
	}

	protected virtual object OnRequiredPropertyNotFound(string name)
	{
		throw null;
	}

	protected virtual void PostDeserialize()
	{
	}

	protected virtual void PreSerialize(XmlWriter writer)
	{
	}

	protected virtual void Reset(ConfigurationElement parentElement)
	{
	}

	protected virtual void ResetModified()
	{
	}

	protected virtual bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
	{
		throw null;
	}

	protected virtual bool SerializeToXmlElement(XmlWriter writer, string elementName)
	{
		throw null;
	}

	protected void SetPropertyValue(ConfigurationProperty prop, object value, bool ignoreLocks)
	{
	}

	protected virtual void SetReadOnly()
	{
	}

	protected virtual void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
	{
	}
}
