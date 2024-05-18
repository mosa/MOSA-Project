using System.Collections.Specialized;

namespace System.Management;

public class SelectQuery : WqlObjectQuery
{
	public string ClassName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Condition
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsSchemaQuery
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override string QueryString
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public StringCollection SelectedProperties
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SelectQuery()
	{
	}

	public SelectQuery(bool isSchemaQuery, string condition)
	{
	}

	public SelectQuery(string queryOrClassName)
	{
	}

	public SelectQuery(string className, string condition)
	{
	}

	public SelectQuery(string className, string condition, string[] selectedProperties)
	{
	}

	protected internal void BuildQuery()
	{
	}

	public override object Clone()
	{
		throw null;
	}

	protected internal override void ParseQuery(string query)
	{
	}
}
