using System;

namespace Mosa.Platform.x86.Intrinsic
{
	public class DelegateStub
	{
		private object instance = null;
		private int methodPointer = 0;

		public DelegateStub(object instance, int methodPointer)
		{
			this.instance = instance;
			this.methodPointer = methodPointer;
		}

		public void Invoke()
		{
			if (instance == null)
			{
				Native.InvokeInstanceDelegate(instance, methodPointer);
				return;
			}
			Native.InvokeDelegate(instance, methodPointer);
		}

		public object InvokeWithReturn()
		{
			if (instance == null)
				return Native.InvokeDelegateWithReturn(instance, methodPointer);
			return Native.InvokeInstanceDelegateWithReturn(instance, methodPointer);
		}

		public IAsyncResult BeginInvoke(AsyncCallback callback, object arg)
		{ return null; }

		public void EndInvoke(AsyncCallback result)
		{}
	}
}
