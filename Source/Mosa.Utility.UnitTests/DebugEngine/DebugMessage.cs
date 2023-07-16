// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.UnitTests.DebugEngine;

public delegate void CallBack(int id, ulong value);

public class DebugMessage
{
	public int ID => UnitTest.UnitTestID;

	public ulong ResponseData { get; internal set; }

	public UnitTest UnitTest { get; set; }

	public DebugMessage(UnitTest unittest)
	{
		UnitTest = unittest;

		//CommandData = new List<byte>(unittest.SerializedUnitTest.Count * 4);

		//foreach (var i in unittest.SerializedUnitTest)
		//{
		//	CommandData.Add((byte)(i & 0xFF));
		//	CommandData.Add((byte)((i >> 8) & 0xFF));
		//	CommandData.Add((byte)((i >> 16) & 0xFF));
		//	CommandData.Add((byte)((i >> 24) & 0xFF));
		//}
	}
}
