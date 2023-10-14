// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework;

public class Counter
{
	public readonly string Name;
	public int Count;

	public Counter(string name)
	{
		Name = name;
	}

	public Counter(string name, int count)
	{
		Name = name;
		Count = count;
	}

	public void Increment(int value = 1)
	{
		Count += value;
	}

	public void Reset()
	{
		Count = 0;
	}

	public void Set(int count)
	{
		Count = count;
	}

	public void Set(bool condition, int truevalue = 1, int falsevalue = 0)
	{
		Count = condition ? truevalue : falsevalue;
	}

	public override string ToString() => $"{Name}: {Count}";
}
