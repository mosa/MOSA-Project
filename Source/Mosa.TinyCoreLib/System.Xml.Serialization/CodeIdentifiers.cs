namespace System.Xml.Serialization;

public class CodeIdentifiers
{
	public bool UseCamelCasing
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeIdentifiers()
	{
	}

	public CodeIdentifiers(bool caseSensitive)
	{
	}

	public void Add(string identifier, object? value)
	{
	}

	public void AddReserved(string identifier)
	{
	}

	public string AddUnique(string identifier, object? value)
	{
		throw null;
	}

	public void Clear()
	{
	}

	public bool IsInUse(string identifier)
	{
		throw null;
	}

	public string MakeRightCase(string identifier)
	{
		throw null;
	}

	public string MakeUnique(string identifier)
	{
		throw null;
	}

	public void Remove(string identifier)
	{
	}

	public void RemoveReserved(string identifier)
	{
	}

	public object ToArray(Type type)
	{
		throw null;
	}
}
