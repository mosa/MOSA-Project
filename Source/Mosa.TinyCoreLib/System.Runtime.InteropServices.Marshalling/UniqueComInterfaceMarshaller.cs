using System.Runtime.Versioning;

namespace System.Runtime.InteropServices.Marshalling;

[UnsupportedOSPlatform("android")]
[UnsupportedOSPlatform("browser")]
[UnsupportedOSPlatform("ios")]
[UnsupportedOSPlatform("tvos")]
[CLSCompliant(false)]
[CustomMarshaller(typeof(CustomMarshallerAttribute.GenericPlaceholder), MarshalMode.Default, typeof(UniqueComInterfaceMarshaller<>))]
public static class UniqueComInterfaceMarshaller<T>
{
	public unsafe static void* ConvertToUnmanaged(T? managed)
	{
		throw null;
	}

	public unsafe static T? ConvertToManaged(void* unmanaged)
	{
		throw null;
	}

	public unsafe static void Free(void* unmanaged)
	{
	}
}
