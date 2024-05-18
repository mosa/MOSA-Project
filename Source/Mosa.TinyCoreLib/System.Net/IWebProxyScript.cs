namespace System.Net;

public interface IWebProxyScript
{
	void Close();

	bool Load(Uri scriptLocation, string script, Type helperType);

	string Run(string url, string host);
}
