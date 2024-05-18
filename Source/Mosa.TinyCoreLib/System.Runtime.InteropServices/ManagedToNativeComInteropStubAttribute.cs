namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class ManagedToNativeComInteropStubAttribute : Attribute
{
	public Type ClassType
	{
		get
		{
			throw null;
		}
	}

	public string MethodName
	{
		get
		{
			throw null;
		}
	}

	public ManagedToNativeComInteropStubAttribute(Type classType, string methodName)
	{
	}
}
