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
		private readonly UnitTestInfo UnitTestInfo;
		private readonly LinkerMethodInfo LinkerMethodInfo;

		public string FullMethodName { get { return UnitTestInfo.FullMethodName; } }
		public MethodInfo MethodInfo { get { return UnitTestInfo.MethodInfo; } }
		public MosaUnitTestAttribute UnitTestAttribute { get { return UnitTestInfo.UnitTestAttribute; } }
		public object[] Values { get { return UnitTestInfo.Values; } }
		public object Expected { get { return UnitTestInfo.Expected; } }

		public MosaMethod MosaMethod { get { return LinkerMethodInfo.MosaMethod; } }
		public IntPtr MosaMethodAddress { get { return LinkerMethodInfo.MosaMethodAddress; } }
		public string MethodNamespaceName { get { return LinkerMethodInfo.MethodNamespaceName; } }
		public string MethodTypeName { get { return LinkerMethodInfo.MethodTypeName; } }
		public string MethodName { get { return LinkerMethodInfo.MethodName; } }

		public object Result { get; set; }

		public UnitTestStatus Status { get; set; }

		public int UnitTestID { get; set; }
		public IList<int> SerializedUnitTest { get; set; }
		public List<byte> SerializedResult { get; set; }

		public UnitTest(UnitTestInfo unitTestInfo, LinkerMethodInfo linkerMethodInfo)
		{
			UnitTestInfo = unitTestInfo;
			LinkerMethodInfo = linkerMethodInfo;

			Status = unitTestInfo.Skip ? UnitTestStatus.Skipped : UnitTestStatus.Pending;
		}
	}
}
