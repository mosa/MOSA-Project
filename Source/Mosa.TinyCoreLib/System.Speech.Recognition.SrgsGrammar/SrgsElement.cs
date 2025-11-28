using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar;

public abstract class SrgsElement : MarshalByRefObject
{
	internal abstract string DebuggerDisplayString();

	internal abstract void WriteSrgs(XmlWriter writer);
}
