namespace System.Management;

public class RelationshipQuery : WqlObjectQuery
{
	public bool ClassDefinitionsOnly
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

	public string RelationshipClass
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string RelationshipQualifier
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string SourceObject
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string ThisRole
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public RelationshipQuery()
	{
	}

	public RelationshipQuery(bool isSchemaQuery, string sourceObject, string relationshipClass, string relationshipQualifier, string thisRole)
	{
	}

	public RelationshipQuery(string queryOrSourceObject)
	{
	}

	public RelationshipQuery(string sourceObject, string relationshipClass)
	{
	}

	public RelationshipQuery(string sourceObject, string relationshipClass, string relationshipQualifier, string thisRole, bool classDefinitionsOnly)
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
