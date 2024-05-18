namespace System.Diagnostics.SymbolStore;

public interface ISymbolScope
{
	int EndOffset { get; }

	ISymbolMethod Method { get; }

	ISymbolScope Parent { get; }

	int StartOffset { get; }

	ISymbolScope[] GetChildren();

	ISymbolVariable[] GetLocals();

	ISymbolNamespace[] GetNamespaces();
}
