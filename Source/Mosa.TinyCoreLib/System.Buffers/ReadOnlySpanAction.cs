namespace System.Buffers;

public delegate void ReadOnlySpanAction<T, in TArg>(ReadOnlySpan<T> span, TArg arg);
