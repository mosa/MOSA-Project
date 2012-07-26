/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai Patrick Reisert <kpreisert@googlemail.com> 
 */


namespace Mosa.Test.Collection
{
	public static class DelegateTests
	{
		private static int status;

		#region DelegateVoid

		delegate void DelegateVoid();

		public static bool DefineDelegate()
		{
			DelegateVoid d = DelegateVoidTarget1;
			return d != null;
		}

		public static int CallDelegateVoid1()
		{
			status = 0;
			DelegateVoid d = DelegateVoidTarget1;
			d();
			return status;
		}

		public static int CallDelegateVoid2()
		{
			status = 0;
			DelegateVoid d = DelegateVoidTarget2;
			d();
			return status;
		}

		public static int ReassignDelegateVoid()
		{
			status = 0;
			DelegateVoid d = DelegateVoidTarget1;
			d();
			d = DelegateVoidTarget2;
			d();
			return status;
		}

		public static void DelegateVoidTarget1()
		{
			status = 1;
		}

		public static void DelegateVoidTarget2()
		{
			status += 2;
		}

		#endregion

		#region DelegateParameters

		delegate void DelegateParameters(int a, int b);

		public static int CallDelegateParameters(int a, int b)
		{
			status = 9999;
			DelegateParameters d = DelegateParametersTarget;
			d(a, b);
			return status;
		}

		public static void DelegateParametersTarget(int a, int b)
		{
			status = a * 10000 + b;
		}

		#endregion

		#region DelegateReturn

		delegate int DelegateReturn();

		public static int CallDelegateReturn(int a)
		{
			status = a;
			DelegateReturn d = DelegateReturnTarget;
			return d();
		}

		public static int DelegateReturnTarget()
		{
			return status + 3;
		}

		#endregion

		#region DelegateParametersReturn

		delegate int DelegateParametersReturn(int a, int b);

		public static int CallDelegateParametersReturn(int a, int b)
		{
			DelegateParametersReturn d = DelegateParametersReturnTarget;
			int result = d(a, b);
			return result;
		}

		public static int DelegateParametersReturnTarget(int a, int b)
		{
			return a * 10000 + b;
		}

		#endregion

		#region DelegateBox

		delegate object DelegateBox(int p);

		public static int CallDelegateBox(int p)
		{
			DelegateBox d = DelegateBoxTarget;
			d(p);
			return p;
		}

		public static object DelegateBoxTarget(int p)
		{
			return p;
		}

		#endregion

		#region DelegateGenericReturn

		delegate T DelegateGenericReturn<T>();

		public static int CallDelegateGenericReturn(int a)
		{
			status = a;
			DelegateGenericReturn<int> d = DelegateReturnTarget;
			return d();
		}

		public static int CallDelegateGenericReturnStructA(int a)
		{
			status = a;
			DelegateGenericReturn<A> d = DelegateReturnStructATarget;
			A res = d();
			return res.Value;
		}

		public struct A
		{
			public int Value;

			public A(int x)
			{
				Value = x;
			}
		}

		public static A DelegateReturnStructATarget()
		{
			A a = new A(status);
			return a;
		}

		public static bool CallDelegateGenericReturnStructB()
		{
			status = 5;
			DelegateGenericReturn<B> d = DelegateReturnStructBTarget;
			B res = d();
			return res.Value1 == 5 && res.Value2 == 10;
		}

		public struct B
		{
			public int Value1;
			public int Value2;

			public B(int x)
			{
				Value1 = x;
				Value2 = x * 2;
			}
		}

		public static B DelegateReturnStructBTarget()
		{
			B b = new B(status);
			return b;
		}

		#endregion

		#region DelegateGenericTarget (disabled)

		//delegate void DelegateGenericTarget(int p);

		//public static bool CallDelegateGenericTarget()
		//{
		//    DelegateGenericTarget d = DelegateGenericTargetTarget<int>;
		//    d(1);
		//    return true;
		//}

		//public static void DelegateGenericTargetTarget<T>(T p)
		//{
		//}

		#endregion

		#region DelegateGenericTargetReturn (disabled)

		//delegate T DelegateGenericTargetReturn<T>(T p);

		//public static bool CallDelegateGenericTargetReturn()
		//{
		//    DelegateGenericTargetReturn<int> d = DelegateGenericTargetReturnTarget<int>;
		//    return d(50) == 50;
		//}

		//public static T DelegateGenericTargetReturnTarget<T>(T p)
		//{
		//    return p;
		//}

		#endregion

		#region InstanceDelegate

		public class AA
		{
			public int Status;
			public static int StatusStatic;

			public void CallDelegate()
			{
				Status = 123;
				DelegateVoid d = Target;
				d();
			}

			public void Target()
			{
				Status = 456;
			}

			public void CallDelegateStatic()
			{
				StatusStatic = 1230;
				DelegateVoid d = TargetStatic;
				d();
			}

			public void TargetStatic()
			{
				StatusStatic = 4560;
			}
		}

		public static int CallInstanceDelegate()
		{
			AA aa = new AA();
			aa.CallDelegate();
			return aa.Status;
		}

		public static int CallInstanceDelegateStatic()
		{
			AA aa = new AA();
			aa.CallDelegateStatic();
			return AA.StatusStatic;
		}

		#endregion
	}
}