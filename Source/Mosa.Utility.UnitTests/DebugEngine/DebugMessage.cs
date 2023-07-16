// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.UnitTests.DebugEngine;

public delegate void CallBack(int id, ulong value);

public class DebugMessage
{
	public int ID => UnitTest.UnitTestID;

	public UnitTest UnitTest { get; set; }

	public DebugMessage(UnitTest unittest)
	{
		UnitTest = unittest;
	}
}
