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
		private DiscoveredUnitTest UnitTestDiscovery;

		public string FullMethodName { get { return UnitTestDiscovery.FullMethodName; } }
		public MethodInfo MethodInfo { get { return UnitTestDiscovery.MethodInfo; } }
		public MosaUnitTestAttribute UnitTestAttribute { get { return UnitTestDiscovery.UnitTestAttribute; } }
		public object[] Values { get { return UnitTestDiscovery.Values; } }

		public object Expected { get { return UnitTestDiscovery.Expected; } }

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

		public UnitTest(DiscoveredUnitTest unitTestDiscovery)
		{
			UnitTestDiscovery = unitTestDiscovery;

			Status = unitTestDiscovery.Skip ? UnitTestStatus.Skipped : UnitTestStatus.Pending;
		}
	}
}
