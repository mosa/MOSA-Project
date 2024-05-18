namespace System.ComponentModel;

public interface IIntellisenseBuilder
{
	string Name { get; }

	bool Show(string language, string value, ref string newValue);
}
