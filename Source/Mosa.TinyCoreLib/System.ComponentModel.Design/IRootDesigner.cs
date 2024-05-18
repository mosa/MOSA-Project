namespace System.ComponentModel.Design;

public interface IRootDesigner : IDesigner, IDisposable
{
	ViewTechnology[] SupportedTechnologies { get; }

	object GetView(ViewTechnology technology);
}
