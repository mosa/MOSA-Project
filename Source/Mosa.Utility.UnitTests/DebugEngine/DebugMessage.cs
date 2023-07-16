// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.UnitTests.DebugEngine;

public delegate void CallBack(int id, ulong value);

public class DebugMessage
{
	public int ID { get; internal set; }

	public IList<byte> CommandData { get; }

	public ulong ResponseData { get; internal set; }

	public object Other { get; set; }

	public DebugMessage(int id, IList<int> data)
	{
		ID = id;
		CommandData = new List<byte>(data.Count * 4);

		foreach (var i in data)
		{
			CommandData.Add((byte)(i & 0xFF));
			CommandData.Add((byte)((i >> 8) & 0xFF));
			CommandData.Add((byte)((i >> 16) & 0xFF));
			CommandData.Add((byte)((i >> 24) & 0xFF));
		}
	}

	public DebugMessage(int id, IList<int> data, object other)
		: this(id, data)
	{
		Other = other;
	}
}
