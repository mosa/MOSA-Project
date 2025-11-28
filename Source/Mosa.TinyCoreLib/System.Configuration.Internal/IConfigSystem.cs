namespace System.Configuration.Internal;

public interface IConfigSystem
{
	IInternalConfigHost Host { get; }

	IInternalConfigRoot Root { get; }

	void Init(Type typeConfigHost, params object[] hostInitParams);
}
