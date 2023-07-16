// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Reflection;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.UnitTests;

namespace Mosa.Utility.UnitTests;

public class UnitTest
{
	private readonly UnitTestInfo UnitTestInfo;
	private readonly LinkerMethodInfo LinkerMethodInfo;

	public string FullMethodName => UnitTestInfo.FullMethodName;

	public MethodInfo MethodInfo => UnitTestInfo.MethodInfo;

	public MosaUnitTestAttribute UnitTestAttribute => UnitTestInfo.UnitTestAttribute;

	public object[] Values => UnitTestInfo.Values;

	public object Expected => UnitTestInfo.Expected;

	public MosaMethod MosaMethod => LinkerMethodInfo.MosaMethod;

	public IntPtr MosaMethodAddress => LinkerMethodInfo.MosaMethodAddress;

	public string MethodNamespaceName => LinkerMethodInfo.MethodNamespaceName;

	public string MethodTypeName => LinkerMethodInfo.MethodTypeName;

	public string MethodName => LinkerMethodInfo.MethodName;

	public object Result { get; set; }

	public UnitTestStatus Status { get; set; }

	public int UnitTestID { get; set; }

	public List<int> SerializedUnitTest { get; set; }

	public ulong SerializedResult { get; set; }

	public UnitTest(UnitTestInfo unitTestInfo, LinkerMethodInfo linkerMethodInfo)
	{
		UnitTestInfo = unitTestInfo;
		LinkerMethodInfo = linkerMethodInfo;

		Status = unitTestInfo.Skip ? UnitTestStatus.Skipped : UnitTestStatus.Pending;
	}
}
