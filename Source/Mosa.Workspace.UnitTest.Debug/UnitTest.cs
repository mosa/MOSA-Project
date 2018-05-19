// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTest.Collection;
using System.Reflection;

namespace Mosa.Workspace.UnitTest.Debug
{
	internal static partial class UnitTests
	{
		public class UnitTest
		{
			public string FullMethodName { get; set; }
			public MethodInfo Method { get; set; }
			public MosaUnitTestAttribute UnitTestAttribute { get; set; }
			public object[] ParameterValues { get; set; }
			public object Expected { get; set; }
			public object Result { get; set; }
			public bool Skipped { get; set; }
			public bool Passed { get; set; }
		}
	}
}