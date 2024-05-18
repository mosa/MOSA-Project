namespace System.ComponentModel.Design;

public interface IHelpService
{
	void AddContextAttribute(string name, string value, HelpKeywordType keywordType);

	void ClearContextAttributes();

	IHelpService CreateLocalContext(HelpContextType contextType);

	void RemoveContextAttribute(string name, string value);

	void RemoveLocalContext(IHelpService localContext);

	void ShowHelpFromKeyword(string helpKeyword);

	void ShowHelpFromUrl(string helpUrl);
}
