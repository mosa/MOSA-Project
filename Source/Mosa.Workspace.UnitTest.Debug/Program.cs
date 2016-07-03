// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTest.Collection;
using Mosa.UnitTest.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Mosa.Workspace.UnitTest.Debug
{
	internal class Program
	{
		private static UnitTestEngine unitTestEngine = new UnitTestEngine();

		private static void Main(string[] args)
		{
			Stopwatch stopwatch = new Stopwatch();

			unitTestEngine.Initialize();

			stopwatch.Start();

			var list = new List<Thread>();

			for (int i = 0; i < 1; i++)
			{
				var thread = new Thread(LaunchTest);
				thread.Name = "#" + i.ToString();

				//thread.IsBackground = true;
				thread.Start();
				list.Add(thread);
			}

			foreach (var thread in list)
			{
				thread.Join();
			}

			stopwatch.Stop();
			Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds + " ms");

			((IDisposable)(unitTestEngine)).Dispose();

			return;
		}

		private static void LaunchTest()
		{
			Console.WriteLine("Thread Start: " + Thread.CurrentThread.Name);

			for (int i = 0; i < 1; i++)
			{
				int value = unitTestEngine.Run<int>("Mosa.UnitTest.Collection", "Int32Tests", "AddI4I4", 1, 2);
				value = unitTestEngine.Run<int>("Mosa.UnitTest.Collection", "Int32Tests", "AddI4I4", 3, 4);
				value = unitTestEngine.Run<int>("Mosa.UnitTest.Collection", "Int32Tests", "AddI4I4", 5, 6);
				value = unitTestEngine.Run<int>("Mosa.UnitTest.Collection", "Int32Tests", "AddI4I4", 7, 8);
				value = unitTestEngine.Run<int>("Mosa.UnitTest.Collection", "Int32Tests", "AddI4I4", 9, 0);

				double a1 = 7;
				double b1 = 9;

				var d1 = unitTestEngine.Run<double>("Mosa.UnitTest.Collection", "DoubleTests", "AddR8R8", a1, b1);
				var d2 = DoubleTests.AddR8R8(a1, b1);

				float a2 = 7;
				float b2 = 9;

				var d1a = unitTestEngine.Run<float>("Mosa.UnitTest.Collection", "SingleTests", "AddR4R4", a2, b2);
				var d2a = SingleTests.AddR4R4(a2, b2);

				var b1b = unitTestEngine.Run<bool>("Mosa.UnitTest.Collection", "ValueTypeTests", "TestValueTypeStaticField");
				var b2b = ValueTypeTests.TestValueTypeStaticField();

				var z = unitTestEngine.Run<bool>("Mosa.UnitTest.Collection", "LdlocaTests", "LdlocaCheckValueR8", 1d);
			}

			Console.WriteLine("Thread End: " + Thread.CurrentThread.Name);
		}
	}
}
