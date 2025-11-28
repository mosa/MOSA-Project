namespace System.Diagnostics.SymbolStore;

public interface ISymbolBinder
{
	[Obsolete("ISymbolBinder.GetReader has been deprecated because it is not 64-bit compatible. Use ISymbolBinder1.GetReader instead. ISymbolBinder1.GetReader accepts the importer interface pointer as an IntPtr instead of an Int32, and thus works on both 32-bit and 64-bit architectures.")]
	ISymbolReader? GetReader(int importer, string filename, string searchPath);
}
