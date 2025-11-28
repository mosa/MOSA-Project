using System.Runtime.Versioning;

namespace System.Runtime.InteropServices.ObjectiveC;

[SupportedOSPlatform("macos")]
[CLSCompliant(false)]
public static class ObjectiveCMarshal
{
	public unsafe delegate delegate* unmanaged<IntPtr, void> UnhandledExceptionPropagationHandler(Exception exception, RuntimeMethodHandle lastMethod, out IntPtr context);

	public enum MessageSendFunction
	{
		MsgSend,
		MsgSendFpret,
		MsgSendStret,
		MsgSendSuper,
		MsgSendSuperStret
	}

	public unsafe static void Initialize(delegate* unmanaged<void> beginEndCallback, delegate* unmanaged<IntPtr, int> isReferencedCallback, delegate* unmanaged<IntPtr, void> trackedObjectEnteredFinalization, UnhandledExceptionPropagationHandler unhandledExceptionPropagationHandler)
	{
		throw null;
	}

	public static GCHandle CreateReferenceTrackingHandle(object obj, out Span<IntPtr> taggedMemory)
	{
		throw null;
	}

	public static void SetMessageSendCallback(MessageSendFunction msgSendFunction, IntPtr func)
	{
		throw null;
	}

	public static void SetMessageSendPendingException(Exception? exception)
	{
		throw null;
	}
}
