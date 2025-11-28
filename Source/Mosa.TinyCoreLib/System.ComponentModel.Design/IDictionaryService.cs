namespace System.ComponentModel.Design;

public interface IDictionaryService
{
	object? GetKey(object? value);

	object? GetValue(object key);

	void SetValue(object key, object? value);
}
