// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using Mosa.UnitTest.Collection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mosa.Workspace.UnitTest.Debug
{
	public class UnitTest
	{
		public string FullMethodName { get; set; }
		public MethodInfo MethodInfo { get; set; }
		public MosaUnitTestAttribute UnitTestAttribute { get; set; }
		public object[] Values { get; set; }

		public object Expected { get; set; }
		public object Result { get; set; }
		public bool Skipped { get; set; }
		public bool Passed { get; set; }

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
