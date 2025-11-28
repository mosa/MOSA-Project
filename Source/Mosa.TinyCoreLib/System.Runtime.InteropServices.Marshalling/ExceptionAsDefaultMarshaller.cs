namespace System.Runtime.InteropServices.Marshalling;

[CustomMarshaller(typeof(Exception), MarshalMode.UnmanagedToManagedOut, typeof(ExceptionAsDefaultMarshaller<>))]
public static class ExceptionAsDefaultMarshaller<T> where T : struct
{
	public static T ConvertToUnmanaged(Exception e)
	{
		throw null;
	}
}
