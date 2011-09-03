/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Kai Patrick Reisert <kpreisert@googlemail.com> 
 */

using System;

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
            status = 0;
            DelegateParameters d = DelegateParametersTarget;
            d(a, b);
            return status;
        }

        public static void DelegateParametersTarget(int a, int b)
        {
            status = a * 10 + b;
        }

        #endregion

        #region DelegateReturn

        delegate int DelegateReturn();

        public static bool CallDelegateReturn()
        {
            status = 3;
            DelegateReturn d = DelegateReturnTarget;
            int result = d();
            return result == 3;
        }

        public static int DelegateReturnTarget()
        {
            return status;
        }

        #endregion

        #region DelegateParametersReturn

        delegate int DelegateParametersReturn(int p1, int p2);

        public static bool CallDelegateParametersReturn(int p1, int p2)
        {
            DelegateParametersReturn d = DelegateParametersReturnTarget;
            int result = d(p1, p2);
            return result == p1 * 10 + p2;
        }

        public static int DelegateParametersReturnTarget(int p1, int p2)
        {
            return p1 * 10 + p2;
        }

        #endregion

        #region DelegateBox

        delegate object DelegateBox(int p);

        public static bool CallDelegateBox(int p)
        {
            DelegateBox d = DelegateBoxTarget;
            int result = (int)d(p);
            return result == p;
        }

        public static object DelegateBoxTarget(int p)
        {
            return p;
        }

        #endregion

        #region DelegateGenericReturn

        delegate T DelegateGenericReturn<T>();

        public static bool CallDelegateGenericReturn()
        {
            status = 5;
            DelegateGenericReturn<int> d = DelegateReturnTarget;
            return d() == 5;
        }

        public static bool CallDelegateGenericReturnStructA()
        {
            status = 5;
            DelegateGenericReturn<A> d = DelegateReturnStructATarget;
            A res = d();
            return res.Value == 5;
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
    }
}