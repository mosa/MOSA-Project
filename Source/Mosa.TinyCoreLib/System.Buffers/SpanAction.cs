namespace System.Buffers;

public delegate void SpanAction<T, in TArg>(Span<T> span, TArg arg);
