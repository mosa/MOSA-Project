namespace System.Runtime.Remoting;

public class ObjectHandle : MarshalByRefObject
{
	public ObjectHandle(object? o)
	{
	}

	public object? Unwrap()
	{
		throw null;
	}
}
