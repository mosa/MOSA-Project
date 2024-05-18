namespace System.ComponentModel;

public interface IEditableObject
{
	void BeginEdit();

	void CancelEdit();

	void EndEdit();
}
