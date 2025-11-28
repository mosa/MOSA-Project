namespace System.Configuration;

[ConfigurationCollection(typeof(SchemeSettingElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap, AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
public sealed class SchemeSettingElementCollection : ConfigurationElementCollection
{
	public override ConfigurationElementCollectionType CollectionType
	{
		get
		{
			throw null;
		}
	}

	public SchemeSettingElement this[int index]
	{
		get
		{
			throw null;
		}
	}

	public new SchemeSettingElement this[string name]
	{
		get
		{
			throw null;
		}
	}

	protected override ConfigurationElement CreateNewElement()
	{
		throw null;
	}

	protected override object GetElementKey(ConfigurationElement element)
	{
		throw null;
	}

	public int IndexOf(SchemeSettingElement element)
	{
		throw null;
	}
}
