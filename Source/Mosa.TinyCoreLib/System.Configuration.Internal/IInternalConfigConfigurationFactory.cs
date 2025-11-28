namespace System.Configuration.Internal;

public interface IInternalConfigConfigurationFactory
{
	Configuration Create(Type typeConfigHost, params object[] hostInitConfigurationParams);

	string NormalizeLocationSubPath(string subPath, IConfigErrorInfo errorInfo);
}
