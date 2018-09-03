// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using Mosa.UnitTests;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mosa.Utility.UnitTests
{
	public class UnitTest
	{
		public string FullMethodName { get; set; }
		public MethodInfo MethodInfo { get; set; }
		public MosaUnitTestAttribute UnitTestAttribute { get; set; }
		public object[] Values { get; set; }

		public object Expected { get; set; }
		public object Result { get; set; }

		public UnitTestStatus Status { get; set; }

		public MosaMethod MosaMethod { get; set; }
		public IntPtr MosaMethodAddress { get; set; }

		public string MethodNamespaceName { get; set; }
		public string MethodTypeName { get; set; }
		public string MethodName { get; set; }

		public int UnitTestID { get; set; }
		public IList<int> SerializedUnitTest { get; set; }
		public List<byte> SerializedResult { get; set; }
	}
}
