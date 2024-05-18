namespace System.Configuration;

public sealed class SettingElementCollection : ConfigurationElementCollection
{
	public override ConfigurationElementCollectionType CollectionType
	{
		get
		{
			throw null;
		}
	}

	protected override string ElementName
	{
		get
		{
			throw null;
		}
	}

	public void Add(SettingElement element)
	{
	}

	public void Clear()
	{
	}

	protected override ConfigurationElement CreateNewElement()
	{
		throw null;
	}

	public SettingElement Get(string elementKey)
	{
		throw null;
	}

	protected override object GetElementKey(ConfigurationElement element)
	{
		throw null;
	}

	public void Remove(SettingElement element)
	{
	}
}
