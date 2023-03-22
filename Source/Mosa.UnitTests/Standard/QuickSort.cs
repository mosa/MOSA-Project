// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Standard;

public static class QuickSort
{
	public static int Partition(int[] numbers, int left, int right)
	{
		var pivot = numbers[left];
		while (true)
		{
			while (numbers[left] < pivot)
				left++;

			while (numbers[right] > pivot)
				right--;

			if (left < right)
			{
				var temp = numbers[right];
				numbers[right] = numbers[left];
				numbers[left] = temp;
			}
			else
			{
				return right;
			}
		}
	}

	public static void SortQuick(int[] arr, int left, int right)
	{
		if (left < right)
		{
			var pivot = Partition(arr, left, right);

			if (pivot > 1)
				SortQuick(arr, left, pivot - 1);

			if (pivot + 1 < right)
				SortQuick(arr, pivot + 1, right);
		}
	}
}
