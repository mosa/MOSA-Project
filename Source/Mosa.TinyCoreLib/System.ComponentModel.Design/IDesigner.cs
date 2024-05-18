namespace System.ComponentModel.Design;

public interface IDesigner : IDisposable
{
	IComponent Component { get; }

	DesignerVerbCollection? Verbs { get; }

	void DoDefaultAction();

	void Initialize(IComponent component);
}
