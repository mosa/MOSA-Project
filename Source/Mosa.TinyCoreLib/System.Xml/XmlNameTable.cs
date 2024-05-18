namespace System.Xml;

public abstract class XmlNameTable
{
	public abstract string Add(char[] array, int offset, int length);

	public abstract string Add(string array);

	public abstract string? Get(char[] array, int offset, int length);

	public abstract string? Get(string array);
}
