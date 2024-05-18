namespace System.ComponentModel.Design;

public interface IReferenceService
{
	IComponent? GetComponent(object reference);

	string? GetName(object reference);

	object? GetReference(string name);

	object[] GetReferences();

	object[] GetReferences(Type baseType);
}
