using System.ComponentModel;
using System.IO;
using System.Speech.Recognition.SrgsGrammar;

namespace System.Speech.Recognition;

public class Grammar
{
	public bool Enabled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected internal virtual bool IsStg
	{
		get
		{
			throw null;
		}
	}

	public bool Loaded
	{
		get
		{
			throw null;
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Priority
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected string ResourceName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string RuleName
	{
		get
		{
			throw null;
		}
	}

	public float Weight
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public event EventHandler<SpeechRecognizedEventArgs> SpeechRecognized
	{
		add
		{
		}
		remove
		{
		}
	}

	protected Grammar()
	{
	}

	public Grammar(Stream stream)
	{
	}

	public Grammar(Stream stream, string ruleName)
	{
	}

	public Grammar(Stream stream, string ruleName, object[] parameters)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public Grammar(Stream stream, string ruleName, Uri baseUri)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public Grammar(Stream stream, string ruleName, Uri baseUri, object[] parameters)
	{
	}

	public Grammar(GrammarBuilder builder)
	{
	}

	public Grammar(SrgsDocument srgsDocument)
	{
	}

	public Grammar(SrgsDocument srgsDocument, string ruleName)
	{
	}

	public Grammar(SrgsDocument srgsDocument, string ruleName, object[] parameters)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public Grammar(SrgsDocument srgsDocument, string ruleName, Uri baseUri)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public Grammar(SrgsDocument srgsDocument, string ruleName, Uri baseUri, object[] parameters)
	{
	}

	public Grammar(string path)
	{
	}

	public Grammar(string path, string ruleName)
	{
	}

	public Grammar(string path, string ruleName, object[] parameters)
	{
	}

	public static Grammar LoadLocalizedGrammarFromType(Type type, params object[] onInitParameters)
	{
		throw null;
	}

	protected void StgInit(object[] parameters)
	{
	}
}
