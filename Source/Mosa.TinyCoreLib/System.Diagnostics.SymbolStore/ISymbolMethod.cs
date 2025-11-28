namespace System.Diagnostics.SymbolStore;

public interface ISymbolMethod
{
	ISymbolScope RootScope { get; }

	int SequencePointCount { get; }

	SymbolToken Token { get; }

	ISymbolNamespace GetNamespace();

	int GetOffset(ISymbolDocument document, int line, int column);

	ISymbolVariable[] GetParameters();

	int[] GetRanges(ISymbolDocument document, int line, int column);

	ISymbolScope GetScope(int offset);

	void GetSequencePoints(int[]? offsets, ISymbolDocument[]? documents, int[]? lines, int[]? columns, int[]? endLines, int[]? endColumns);

	bool GetSourceStartEnd(ISymbolDocument[]? docs, int[]? lines, int[]? columns);
}
