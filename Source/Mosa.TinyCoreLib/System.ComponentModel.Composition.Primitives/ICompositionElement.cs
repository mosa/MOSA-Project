namespace System.ComponentModel.Composition.Primitives;

public interface ICompositionElement
{
	string DisplayName { get; }

	ICompositionElement? Origin { get; }
}
