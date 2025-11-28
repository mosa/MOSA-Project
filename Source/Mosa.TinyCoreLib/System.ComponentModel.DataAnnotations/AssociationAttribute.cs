using System.Collections.Generic;

namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
[Obsolete("AssociationAttribute has been deprecated and is not supported.")]
public sealed class AssociationAttribute : Attribute
{
	public bool IsForeignKey
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public string OtherKey
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<string> OtherKeyMembers
	{
		get
		{
			throw null;
		}
	}

	public string ThisKey
	{
		get
		{
			throw null;
		}
	}

	public IEnumerable<string> ThisKeyMembers
	{
		get
		{
			throw null;
		}
	}

	public AssociationAttribute(string name, string thisKey, string otherKey)
	{
	}
}
