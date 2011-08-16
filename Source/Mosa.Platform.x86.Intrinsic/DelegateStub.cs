using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			Native.InvokeDelegate(instance, methodPointer);
		}

		public object InvokeWithReturn()
		{
			return Native.InvokeDelegateWithReturn(instance, methodPointer);
		}

		public IAsyncResult BeginInvoke(AsyncCallback callback, object arg)
		{ return null; }

		public void EndInvoke(AsyncCallback result)
		{}
	}
}
