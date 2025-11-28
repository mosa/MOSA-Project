using System.Collections;

namespace System.ComponentModel.Design;

public interface ISelectionService
{
	object? PrimarySelection { get; }

	int SelectionCount { get; }

	event EventHandler SelectionChanged;

	event EventHandler SelectionChanging;

	bool GetComponentSelected(object component);

	ICollection GetSelectedComponents();

	void SetSelectedComponents(ICollection? components);

	void SetSelectedComponents(ICollection? components, SelectionTypes selectionType);
}
