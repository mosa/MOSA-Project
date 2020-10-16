// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTests;
using System.Reflection;

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
