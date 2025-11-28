namespace System.Diagnostics.SymbolStore;

public interface ISymbolBinder1
{
	ISymbolReader? GetReader(IntPtr importer, string filename, string searchPath);
}
