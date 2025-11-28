namespace System.ComponentModel.Design.Serialization;

public interface IDesignerLoaderHost2 : IDesignerHost, IServiceContainer, IServiceProvider, IDesignerLoaderHost
{
	bool CanReloadWithErrors { get; set; }

	bool IgnoreErrorsDuringReload { get; set; }
}
