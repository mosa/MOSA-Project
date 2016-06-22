// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTest.Engine;
using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class TestFixture : BaseTestFixture, IClassFixture<UnitTestFixture2>
	{
		protected T Run<T>(string ns, string type, string method, params object[] parameters)
		{
			// Get test system
			var unitTestEngine = UnitTestFixture2.UnitTestEngine;

			// execute test
			return unitTestEngine.Run<T>(ns, type, method, parameters);
		}

		protected T Run<T>(string fullMethodName, params object[] parameters)
		{
			int first = fullMethodName.LastIndexOf(".");
			int second = fullMethodName.LastIndexOf(".", first - 1);

			string ns = fullMethodName.Substring(0, second);
			string type = fullMethodName.Substring(second + 1, first - second - 1);
			string method = fullMethodName.Substring(first + 1);

			return Run<T>(ns, type, method, parameters);
		}
	}
}
