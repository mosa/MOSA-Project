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
			instance = methodPointer == 0 ? instance : null;
			++methodPointer;
			//Native.InvokeDelegate();
		}
	}
}
