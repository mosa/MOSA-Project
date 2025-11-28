using System.Collections.Specialized;

namespace System.Management;

public class WqlEventQuery : EventQuery
{
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

	public string EventClassName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public StringCollection GroupByPropertyList
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan GroupWithinInterval
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string HavingCondition
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override string QueryLanguage
	{
		get
		{
			throw null;
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

	public TimeSpan WithinInterval
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public WqlEventQuery()
	{
	}

	public WqlEventQuery(string queryOrEventClassName)
	{
	}

	public WqlEventQuery(string eventClassName, string condition)
	{
	}

	public WqlEventQuery(string eventClassName, string condition, TimeSpan groupWithinInterval)
	{
	}

	public WqlEventQuery(string eventClassName, string condition, TimeSpan groupWithinInterval, string[] groupByPropertyList)
	{
	}

	public WqlEventQuery(string eventClassName, TimeSpan withinInterval)
	{
	}

	public WqlEventQuery(string eventClassName, TimeSpan withinInterval, string condition)
	{
	}

	public WqlEventQuery(string eventClassName, TimeSpan withinInterval, string condition, TimeSpan groupWithinInterval, string[] groupByPropertyList, string havingCondition)
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
