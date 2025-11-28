namespace System.Runtime.InteropServices.Marshalling;

[CustomMarshaller(typeof(Exception), MarshalMode.UnmanagedToManagedOut, typeof(ExceptionAsHResultMarshaller<>))]
public static class ExceptionAsHResultMarshaller<T> where T : struct
{
	public static T ConvertToUnmanaged(Exception e)
	{
		throw null;
	}
}
