/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Runtime.InteropServices;

namespace System
{
	/// <summary>
	/// Implementation of the "System.Delegate" class.
	/// </summary>
	public class Delegate
	{
		protected object instance = null;
		protected uint methodPointer = 0;

		internal Delegate()
		{
		}

		internal Delegate(object instance, uint methodPointer)
		{
			this.instance = instance;
			this.methodPointer = methodPointer;
		}

		//public void Invoke()
		//{
		//    if (instance == null)
		//    {
		//        InvokeDelegate(instance, methodPointer);
		//    }
		//    else
		//    {
		//        InvokeInstanceDelegate(instance, methodPointer);
		//    }
		//}

		//public object InvokeWithReturn()
		//{
		//    if (instance == null)
		//    {
		//        return InvokeDelegateWithReturn(instance, methodPointer);
		//    }
		//    else
		//    {
		//        return InvokeInstanceDelegateWithReturn(instance, methodPointer);
		//    }
		//}

		//public IAsyncResult BeginInvoke(AsyncCallback callback, object arg)
		//{
		//    return null;
		//}

		//public void EndInvoke(AsyncCallback result)
		//{ }

		//[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.InvokeDelegate, System.Platform.x86")]
		//public extern static void InvokeDelegate(object obj, uint ptr);

		//[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.InvokeInstanceDelegate, Mosa.Platform.x86")]
		//public extern static void InvokeInstanceDelegate(object obj, uint ptr);

		//[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.InvokeDelegateWithReturn, Mosa.Platform.x86")]
		//public extern static object InvokeDelegateWithReturn(object obj, uint ptr);

		//[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.InvokeInstanceDelegateWithReturn, Mosa.Platform.x86")]
		//public extern static object InvokeInstanceDelegateWithReturn(object obj, uint ptr);

		//[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetMethodLookupTable, Mosa.Platform.x86")]
		//public extern static uint GetMethodLookupTable(uint ptr);

	}
}
