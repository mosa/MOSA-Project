namespace System.Configuration;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public sealed class ConfigurationCollectionAttribute : Attribute
{
	public string AddItemName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string ClearItemsName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ConfigurationElementCollectionType CollectionType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Type ItemType
	{
		get
		{
			throw null;
		}
	}

	public string RemoveItemName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ConfigurationCollectionAttribute(Type itemType)
	{
	}
}
