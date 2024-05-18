using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.ServiceModel.Syndication;

public class InlineCategoriesDocument : CategoriesDocument
{
	public Collection<SyndicationCategory> Categories
	{
		get
		{
			throw null;
		}
	}

	public bool IsFixed
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Scheme
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public InlineCategoriesDocument()
	{
	}

	public InlineCategoriesDocument(IEnumerable<SyndicationCategory> categories)
	{
	}

	public InlineCategoriesDocument(IEnumerable<SyndicationCategory> categories, bool isFixed, string scheme)
	{
	}

	protected internal virtual SyndicationCategory CreateCategory()
	{
		throw null;
	}
}
