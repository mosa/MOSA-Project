namespace System.Diagnostics.SymbolStore;

public interface ISymbolNamespace
{
	string Name { get; }

	ISymbolNamespace[] GetNamespaces();

	ISymbolVariable[] GetVariables();
}
