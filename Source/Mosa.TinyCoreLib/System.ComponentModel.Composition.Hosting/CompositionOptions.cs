namespace System.ComponentModel.Composition.Hosting;

[Flags]
public enum CompositionOptions
{
	Default = 0,
	DisableSilentRejection = 1,
	IsThreadSafe = 2,
	ExportCompositionService = 4
}
