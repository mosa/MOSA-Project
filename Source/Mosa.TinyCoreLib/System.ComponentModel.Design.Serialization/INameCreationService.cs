namespace System.ComponentModel.Design.Serialization;

public interface INameCreationService
{
	string CreateName(IContainer? container, Type dataType);

	bool IsValidName(string name);

	void ValidateName(string name);
}
