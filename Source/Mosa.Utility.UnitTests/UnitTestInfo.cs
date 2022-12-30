// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Reflection;
using Mosa.UnitTests;

namespace Mosa.Utility.UnitTests
{
	public class UnitTestInfo
	{
		public string FullMethodName { get; set; }
		public MethodInfo MethodInfo { get; set; }
		public MosaUnitTestAttribute UnitTestAttribute { get; set; }
		public object[] Values { get; set; }

		public object Expected { get; set; }
		public bool Skip { get; set; }
	}
}
