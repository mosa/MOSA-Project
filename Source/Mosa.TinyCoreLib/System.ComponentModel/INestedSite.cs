namespace System.ComponentModel;

public interface INestedSite : ISite, IServiceProvider
{
	string? FullName { get; }
}
