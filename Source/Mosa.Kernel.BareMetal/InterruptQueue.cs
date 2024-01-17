// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal;

public static class InterruptQueue
{
	private const int MaxIRQs = 8196;

	private static int[] slots;
	private static int first;
	private static int last;
	private static int length;

	public static void Setup()
	{
		Debug.WriteLine("InterruptQueue:Setup()");

		slots = new int[8196];
		first = -1;
		last = -1;
		length = 0;

		for (var i = 0; i < MaxIRQs; i++)
		{
			slots[i] = -1;
		}

		Debug.WriteLine("InterruptQueue:Setup() [Exit]");
	}

	public static int GetLength() => length;

	public static void Enqueue(uint irq)
	{
		if (last == -1)
		{
			first = (int)irq;
			last = (int)irq;
			length = 1;
		}
		else
		{
			if (last == irq)
				return;

			if (slots[irq] != -1)
				return;

			slots[last] = (int)irq;

			last = (int)irq;

			length++;
		}
	}

	public static int Dequeue()
	{
		if (first == -1)
		{
			return -1;
		}
		else
		{
			var ret = first;

			first = slots[first];

			if (first == -1)
			{
				last = -1;
			}

			length--;

			return ret;
		}
	}
}
