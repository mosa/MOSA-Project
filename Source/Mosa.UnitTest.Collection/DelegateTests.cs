// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTest.Collection
{
	public static class DelegateTests
	{
		private static int status;

		#region DelegateVoid

		private delegate void DelegateVoid();

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

		private static void DelegateVoidTarget1()
		{
			status = 1;
		}

		private static void DelegateVoidTarget2()
		{
			status += 2;
		}

		#endregion DelegateVoid

		#region DelegateParameters

		private delegate void DelegateParameters(int a, int b);

		public static int CallDelegateParameters(int a, int b)
		{
			status = 9999;
			DelegateParameters d = DelegateParametersTarget;
			d(a, b);
			return status;
		}

		private static void DelegateParametersTarget(int a, int b)
		{
			status = a * 10000 + b;
		}

		#endregion DelegateParameters

		#region DelegateReturn

		private delegate int DelegateReturn();

		public static int CallDelegateReturn(int a)
		{
			status = a;
			DelegateReturn d = DelegateReturnTarget;
			return d();
		}

		private static int DelegateReturnTarget()
		{
			return status + 3;
		}

		#endregion DelegateReturn

		#region DelegateParametersReturn

		private delegate int DelegateParametersReturn(int a, int b);

		public static int CallDelegateParametersReturn(int a, int b)
		{
			DelegateParametersReturn d = DelegateParametersReturnTarget;
			int result = d(a, b);
			return result;
		}

		private static int DelegateParametersReturnTarget(int a, int b)
		{
			return a * 10000 + b;
		}

		#endregion DelegateParametersReturn

		#region DelegateBox

		private delegate object DelegateBox(int p);

		public static int CallDelegateBox(int p)
		{
			DelegateBox d = DelegateBoxTarget;
			d(p);
			return p;
		}

		private static object DelegateBoxTarget(int p)
		{
			return p;
		}

		#endregion DelegateBox

		#region DelegateGenericReturn

		private delegate T DelegateGenericReturn<T>();

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

		private struct A
		{
			public int Value;

			public A(int x)
			{
				Value = x;
			}
		}

		private static A DelegateReturnStructATarget()
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

		private struct B
		{
			public int Value1;
			public int Value2;

			public B(int x)
			{
				Value1 = x;
				Value2 = x * 2;
			}
		}

		private static B DelegateReturnStructBTarget()
		{
			B b = new B(status);
			return b;
		}

		#endregion DelegateGenericReturn

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

		#endregion DelegateGenericTarget (disabled)

		#region DelegateGenericTargetReturn (disabled)

		//delegate T DelegateGenericTargetReturn<T>(T p);

		//public static bool CallDelegateGenericTargetReturn()
		//{
		//    DelegateGenericTargetReturn<int> d = DelegateGenericTargetReturnTarget<int>;
		//    return d(50) == 50;
		//}

		//private static T DelegateGenericTargetReturnTarget<T>(T p)
		//{
		//    return p;
		//}

		#endregion DelegateGenericTargetReturn (disabled)

		#region InstanceDelegate

		private class AA
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

		#endregion InstanceDelegate

		private class Worker
		{
			public int Value = 10;

			public int SumPlusValue(int a, int b)
			{
				return a + b + Value;
			}

			public int SumValue(int a)
			{
				return a + Value;
			}
		}

		private delegate int SumPlusValue(int a, int b);

		private delegate int SumValue(int a);

		public static int TestInstanceDelegate1(int a)
		{
			Worker w = new Worker();
			w.Value = 25;

			SumValue executeSum = w.SumValue;

			int sum = executeSum(a);

			return sum;
		}

		public static int TestInstanceDelegate2(int a, int b)
		{
			Worker w = new Worker();
			w.Value = 25;

			SumPlusValue executeSum = w.SumPlusValue;

			int sum = executeSum(a, b);

			return sum;
		}
	}
}
