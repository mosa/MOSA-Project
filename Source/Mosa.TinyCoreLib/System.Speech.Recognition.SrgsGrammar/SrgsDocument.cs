using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar;

public class SrgsDocument
{
	public Collection<string> AssemblyReferences
	{
		get
		{
			throw null;
		}
	}

	public Collection<string> CodeBehind
	{
		get
		{
			throw null;
		}
	}

	public CultureInfo Culture
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool Debug
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Collection<string> ImportNamespaces
	{
		get
		{
			throw null;
		}
	}

	public string Language
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SrgsGrammarMode Mode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Namespace
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SrgsPhoneticAlphabet PhoneticAlphabet
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SrgsRule Root
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SrgsRulesCollection Rules
	{
		get
		{
			throw null;
		}
	}

	public string Script
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Uri XmlBase
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SrgsDocument()
	{
	}

	public SrgsDocument(GrammarBuilder builder)
	{
	}

	public SrgsDocument(SrgsRule grammarRootRule)
	{
	}

	public SrgsDocument(string path)
	{
	}

	public SrgsDocument(XmlReader srgsGrammar)
	{
	}

	public void WriteSrgs(XmlWriter srgsGrammar)
	{
	}
}
