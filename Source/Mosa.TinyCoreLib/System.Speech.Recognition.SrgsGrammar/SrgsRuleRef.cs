using System.ComponentModel;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar;

[ImmutableObject(true)]
public class SrgsRuleRef : SrgsElement
{
	public static readonly SrgsRuleRef Dictation;

	public static readonly SrgsRuleRef Garbage;

	public static readonly SrgsRuleRef MnemonicSpelling;

	public static readonly SrgsRuleRef Null;

	public static readonly SrgsRuleRef Void;

	public string Params
	{
		get
		{
			throw null;
		}
	}

	public string SemanticKey
	{
		get
		{
			throw null;
		}
	}

	public Uri Uri
	{
		get
		{
			throw null;
		}
	}

	public SrgsRuleRef(SrgsRule rule)
	{
	}

	public SrgsRuleRef(SrgsRule rule, string semanticKey)
	{
	}

	public SrgsRuleRef(SrgsRule rule, string semanticKey, string parameters)
	{
	}

	public SrgsRuleRef(Uri uri)
	{
	}

	public SrgsRuleRef(Uri uri, string rule)
	{
	}

	public SrgsRuleRef(Uri uri, string rule, string semanticKey)
	{
	}

	public SrgsRuleRef(Uri uri, string rule, string semanticKey, string parameters)
	{
	}

	internal override string DebuggerDisplayString()
	{
		throw null;
	}

	internal override void WriteSrgs(XmlWriter writer)
	{
		throw null;
	}
}
