// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.UnitTests.DebugEngine;

public delegate void CallBack(DebugMessage response);

public class DebugMessage
{
	public int ID { get; internal set; }

	public byte Code { get; }

	public IList<byte> CommandData { get; }

	public List<byte> ResponseData { get; internal set; }

	public CallBack CallBack { get; set; }

	public object Other { get; set; }

	public DebugMessage(byte code, IList<byte> data)
	{
		Code = code;
		CommandData = data;
	}

	public DebugMessage(byte code, byte[] data)
	{
		Code = code;
		CommandData = new List<byte>(data.Length);

		foreach (var b in data)
		{
			CommandData.Add(b);
		}
	}

	public DebugMessage(byte code, byte[] data, int length)
	{
		Code = code;
		CommandData = new List<byte>(data.Length);

		for (var i = 0; i < length; i++)
		{
			CommandData.Add(data[i]);
		}
	}

	public DebugMessage(byte code, IList<int> data)
	{
		Code = code;
		CommandData = new List<byte>(data.Count * 4);

		foreach (var i in data)
		{
			CommandData.Add((byte)(i & 0xFF));
			CommandData.Add((byte)((i >> 8) & 0xFF));
			CommandData.Add((byte)((i >> 16) & 0xFF));
			CommandData.Add((byte)((i >> 24) & 0xFF));
		}
	}

	public DebugMessage(byte code, IList<int> data, object other)
		: this(code, data)
	{
		Other = other;
	}

	public DebugMessage(byte code, IList<int> data, CallBack callback)
		: this(code, data)
	{
		CallBack = callback;
	}

	public DebugMessage(byte code, IList<byte> data, CallBack callback)
		: this(code, data)
	{
		CallBack = callback;
	}

	public DebugMessage(byte code, int[] data, CallBack callback)
		: this(code, data)
	{
		CallBack = callback;
	}

	public int GetInt32(int index)
	{
		return (ResponseData[index + 3] << 24) | (ResponseData[index + 2] << 16) | (ResponseData[index + 1] << 8) | ResponseData[index];
	}

	public uint GetUInt32(int index)
	{
		return (uint)GetInt32(index);
	}

	private static byte[] CreateByteArray(List<byte> data)
	{
		var array = new byte[data.Count];
		data.CopyTo(array);
		return array;
	}
}
