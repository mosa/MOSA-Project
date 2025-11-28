namespace System.ComponentModel.Design;

public interface IMenuCommandService
{
	DesignerVerbCollection Verbs { get; }

	void AddCommand(MenuCommand command);

	void AddVerb(DesignerVerb verb);

	MenuCommand? FindCommand(CommandID commandID);

	bool GlobalInvoke(CommandID commandID);

	void RemoveCommand(MenuCommand command);

	void RemoveVerb(DesignerVerb verb);

	void ShowContextMenu(CommandID menuID, int x, int y);
}
