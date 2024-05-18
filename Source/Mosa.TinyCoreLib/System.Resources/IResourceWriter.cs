namespace System.Resources;

public interface IResourceWriter : IDisposable
{
	void AddResource(string name, byte[]? value);

	void AddResource(string name, object? value);

	void AddResource(string name, string? value);

	void Close();

	void Generate();
}
