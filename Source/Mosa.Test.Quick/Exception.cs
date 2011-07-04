/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;

namespace Mosa.Test.Quick.Tests
{

	public class ExceptionTest
	{
		public static void Test1()
		{
			int a = 10;
			int b = 13;
			try
			{
				int i = 12;
				a = a + i;
			}
			finally { }
			b = 7;
		}

		//public static void Test2()
		//{
		//    int a = 10;
		//    int b = 13;
		//    try
		//    {
		//        int i = 12;
		//        a = a + i;
		//    }
		//    finally
		//    {
		//        b = 7;
		//    }
		//}

		//public static void Test3()
		//{
		//    int a = 10;
		//    int b = 13;
		//    try
		//    {
		//        try
		//        {
		//            int i = 12;
		//            a = a + i;
		//        }
		//        finally
		//        {
		//            b = 5;
		//        }
		//    }
		//    finally
		//    {
		//        b = 7;
		//    }
		//}

	}

}
