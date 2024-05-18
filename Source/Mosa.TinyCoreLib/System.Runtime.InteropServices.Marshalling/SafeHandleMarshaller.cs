using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.InteropServices.Marshalling;

[CustomMarshaller(typeof(CustomMarshallerAttribute.GenericPlaceholder), MarshalMode.ManagedToUnmanagedIn, typeof(SafeHandleMarshaller<>.ManagedToUnmanagedIn))]
[CustomMarshaller(typeof(CustomMarshallerAttribute.GenericPlaceholder), MarshalMode.ManagedToUnmanagedRef, typeof(SafeHandleMarshaller<>.ManagedToUnmanagedRef))]
[CustomMarshaller(typeof(CustomMarshallerAttribute.GenericPlaceholder), MarshalMode.ManagedToUnmanagedOut, typeof(SafeHandleMarshaller<>.ManagedToUnmanagedOut))]
public static class SafeHandleMarshaller<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T> where T : SafeHandle
{
	public struct ManagedToUnmanagedIn
	{
		private int _dummyPrimitive;

		private T _handle;

		public void FromManaged(T handle)
		{
		}

		public IntPtr ToUnmanaged()
		{
			throw null;
		}

		public void Free()
		{
		}
	}

	public struct ManagedToUnmanagedRef
	{
		private int _dummyPrimitive;

		private T _handle;

		public ManagedToUnmanagedRef()
		{
			_dummyPrimitive = 0;
			_handle = null;
		}

		public void FromManaged(T handle)
		{
		}

		public IntPtr ToUnmanaged()
		{
			throw null;
		}

		public void FromUnmanaged(IntPtr value)
		{
		}

		public void OnInvoked()
		{
		}

		public T ToManagedFinally()
		{
			throw null;
		}

		public void Free()
		{
		}
	}

	public struct ManagedToUnmanagedOut
	{
		private int _dummyPrimitive;

		private T _newHandle;

		public ManagedToUnmanagedOut()
		{
			_dummyPrimitive = 0;
			_newHandle = null;
		}

		public void FromUnmanaged(IntPtr value)
		{
		}

		public T ToManaged()
		{
			throw null;
		}

		public void Free()
		{
		}
	}
}
