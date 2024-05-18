namespace System.Runtime.InteropServices.Marshalling;

[CustomMarshaller(typeof(Exception), MarshalMode.UnmanagedToManagedOut, typeof(ExceptionAsVoidMarshaller))]
public static class ExceptionAsVoidMarshaller
{
	public static void ConvertToUnmanaged(Exception e)
	{
	}
}
