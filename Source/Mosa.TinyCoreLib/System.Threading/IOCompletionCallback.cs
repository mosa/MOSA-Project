namespace System.Threading;

[CLSCompliant(false)]
public unsafe delegate void IOCompletionCallback(uint errorCode, uint numBytes, NativeOverlapped* pOVERLAP);
