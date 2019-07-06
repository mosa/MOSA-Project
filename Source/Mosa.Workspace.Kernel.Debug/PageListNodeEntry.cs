// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.Extension;
using System;

namespace Mosa.Kernel.BareMetal
{
	public /*readonly*/ struct PageListNodeEntry
	{
		private readonly IntPtr Entry;

		public static int Entries = (int)(Page.Size / IntPtr.Size);

		public IntPtr Next { get { return Entry.LoadPointer(0); } }

		public PageListNodeEntry(IntPtr entry)
		{
			Entry = entry;
		}

		public void Clear()
		{
			Page.ZeroOutPage(Entry);
		}

		public void Add(IntPtr entry, uint count)
		{
			ulong page = (ulong)entry.ToInt64() >> (int)Page.Shift;

			if (count >= (uint)Page.Mask)
			{
				// TODO: too many to add to any entry
			}

			Add(page, count);
		}

		private void Add(ulong page, uint count)
		{
			for (uint i = 1; i < Entries; ++i)
			{
				var value = (ulong)Entry.LoadPointer(i).ToInt64();

				ulong entryPage = value >> (int)Page.Shift;
				uint entryCount = (uint)(value & ~Page.Mask);   // always going to be 32bit

				if (entryPage == 0 || entryCount == 0)
					continue;

				// 1. new entry is immediately after
				if (entryPage + entryCount == page)
				{
					uint newEntryCount = entryCount + count;

					if (newEntryCount > (uint)Page.Mask)
					{
						// TODO: problem
					}

					Entry.StorePointer(i, new IntPtr((long)(entryPage << (int)Page.Shift) | newEntryCount));

					// TODO: Check if this entry can be merged with previous one

					return;
				}

				// 2. new entry is immediately before
				else if (page + count == entryPage)
				{
					uint newEntryCount = entryCount + count;

					if (newEntryCount > (uint)Page.Mask)
					{
						// problem
					}

					Entry.StorePointer(i, new IntPtr((long)(page << (int)Page.Shift) | newEntryCount));

					// TODO: Check if this entry can be merged with next one

					return;
				}

				// 3. new entry not immediate at all
				else continue;
			}
		}
	}
}
