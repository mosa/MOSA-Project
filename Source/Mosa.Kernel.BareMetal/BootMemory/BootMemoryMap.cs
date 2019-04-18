// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Kernel.BareMetal.BootMemory
{
	public static class BootMemoryMap
	{
		private static BootMemoryMapTable Map;

		public static void Initialize()
		{
			var entry = Platform.GetMemoryMapLocation();

			Map = new BootMemoryMapTable(entry)
			{
				Count = 0
			};
		}

		public static BootMemoryMapEntry SetMemoryMap(IntPtr address, ulong size, BootMemoryMapType type)
		{
			var entry = Map.GetBootMemoryMapEntry(Map.Count);

			entry.Address = address;
			entry.Size = size;
			entry.Type = type;
			entry.Valid = true;

			Map.Count += 1;

			return entry;
		}

		public static void Compact()
		{
			// Merge address ranges
			while (FindOverlapAndMerge(BootMemoryMapType.Reserved)) ;
			while (FindOverlapAndMerge(BootMemoryMapType.Available)) ;

			// Trim available ranges
			while (FindOverlapAndTrimAvailable()) ;
		}

		private static bool FindOverlapAndMerge(BootMemoryMapType type)
		{
			var count = Map.Count;

			// outer loop
			for (uint outer = 0; outer < count; outer++)
			{
				var outermap = Map.GetBootMemoryMapEntry(outer);

				if (!outermap.Valid || outermap.Type != type)
					continue;

				// inner loop
				for (uint inner = 0; inner < count; inner++)
				{
					if (inner == outer)
						continue;

					var innermap = Map.GetBootMemoryMapEntry(inner);

					if (!innermap.Valid || innermap.Type != type)
						continue;

					var innerStartInt = (ulong)innermap.Address.ToInt64();
					var innerEndInt = innerStartInt + innermap.Size - 1;

					var outerStartInt = (ulong)outermap.Address.ToInt64();
					var outerEndInt = outerStartInt + outermap.Size - 1;

					if (innerStartInt > outerEndInt || innerEndInt < outerStartInt)
						continue; // no overlap

					// create new map & de-activate old maps

					outermap.Valid = false;
					innermap.Valid = false;

					var newStartInt = innerStartInt < outerStartInt ? innerStartInt : outerStartInt;
					var newEndInt = innerEndInt > outerEndInt ? innerEndInt : outerEndInt;

					var newStart = new IntPtr((long)newStartInt);
					var newSize = newEndInt - newStartInt + 1;

					SetMemoryMap(newStart, newSize, type);

					return true;
				}
			}

			return false;
		}

		private static bool FindOverlapAndTrimAvailable()
		{
			var count = Map.Count;

			// outer loop
			for (uint outer = 0; outer < count; outer++)
			{
				var outermap = Map.GetBootMemoryMapEntry(outer);

				if (!outermap.Valid || outermap.Type != BootMemoryMapType.Available)
					continue;

				// inner loop
				for (uint inner = 0; inner < count; inner++)
				{
					if (inner == outer)
						continue;

					var innermap = Map.GetBootMemoryMapEntry(inner);

					if (!innermap.Valid || innermap.Type != BootMemoryMapType.Available)
						continue;

					var availableStart = (ulong)innermap.Address.ToInt64();
					var availableEnd = availableStart + innermap.Size - 1;

					var reservedStart = (ulong)outermap.Address.ToInt64();
					var reservedEnd = reservedStart + outermap.Size - 1;

					if (availableStart > reservedEnd || availableEnd < reservedStart)
						continue;   // no overlap

					// de-activate old map && create new map(s)

					innermap.Valid = false;

					// 4 variations

					// 1. Available completely contained
					if (availableStart >= reservedStart && availableEnd <= reservedEnd)
						return true;

					// 2. Overlapped on left
					if (availableStart > reservedStart && availableStart < reservedEnd)
					{
						var newStart = reservedEnd + 1;
						var newEnd = availableEnd;
						var newSize = newEnd - newStart + 1;

						SetMemoryMap(new IntPtr((long)newStart), newSize, BootMemoryMapType.Available);

						return true;
					}

					// 3. Overlapped on right
					if (availableEnd > reservedStart && availableEnd < reservedEnd)
					{
						var newStart = availableStart;
						var newEnd = reservedStart - 1;
						var newSize = newEnd - newStart + 1;

						SetMemoryMap(new IntPtr((long)newStart), newSize, BootMemoryMapType.Available);

						return true;
					}

					// 4. Overlapped in middle
					var newLeftStart = availableStart;
					var newLeftEnd = reservedStart - 1;
					var newLeftSize = newLeftEnd - newLeftStart + 1;

					var newRightStart = reservedEnd + 1;
					var newRightEnd = availableEnd;
					var newRightSize = newRightEnd - newRightStart + 1;

					SetMemoryMap(new IntPtr((long)newLeftStart), newLeftSize, BootMemoryMapType.Available);
					SetMemoryMap(new IntPtr((long)newRightStart), newRightSize, BootMemoryMapType.Available);

					return true;
				}
			}

			return false;
		}
	}
}
