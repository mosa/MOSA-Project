namespace System.Management;

public class RelatedObjectQuery : WqlObjectQuery
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

	public string RelatedClass
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string RelatedQualifier
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string RelatedRole
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

	public RelatedObjectQuery()
	{
	}

	public RelatedObjectQuery(bool isSchemaQuery, string sourceObject, string relatedClass, string relationshipClass, string relatedQualifier, string relationshipQualifier, string relatedRole, string thisRole)
	{
	}

	public RelatedObjectQuery(string queryOrSourceObject)
	{
	}

	public RelatedObjectQuery(string sourceObject, string relatedClass)
	{
	}

	public RelatedObjectQuery(string sourceObject, string relatedClass, string relationshipClass, string relatedQualifier, string relationshipQualifier, string relatedRole, string thisRole, bool classDefinitionsOnly)
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
