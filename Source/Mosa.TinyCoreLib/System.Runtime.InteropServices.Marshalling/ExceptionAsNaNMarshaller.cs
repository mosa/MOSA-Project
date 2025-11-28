namespace System.Runtime.InteropServices.Marshalling;

[CustomMarshaller(typeof(Exception), MarshalMode.UnmanagedToManagedOut, typeof(ExceptionAsNaNMarshaller<>))]
public static class ExceptionAsNaNMarshaller<T> where T : struct
{
	public static T ConvertToUnmanaged(Exception e)
	{
		throw null;
	}
}
