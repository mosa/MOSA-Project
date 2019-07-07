// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.Extension;
using Mosa.Runtime.Extension;
using System;

namespace Mosa.Kernel.BareMetal
{
	public /*readonly*/ struct PageNodeDoubleListEntry
	{
		private readonly IntPtr Ptr;

		public IntPtr Next
		{
			get { return Ptr.LoadPointer(0); }
			set { Ptr.StorePointer(0, value); }
		}

		public IntPtr Previous
		{
			get { return Ptr.LoadPointer(IntPtr.Size); }
			set { Ptr.StorePointer((uint)IntPtr.Size, value); }
		}

		public bool IsNextPage { get { return Next.IsNull(); } }
		public bool IsPreviousPage { get { return Next.IsNull(); } }

		public PageNodeDoubleListEntry(IntPtr page)
		{
			Ptr = page;
		}

		public void ClearPage()
		{
			Page.ZeroOutPage(Ptr);
		}

		public void InsertPageBefore(IntPtr page)
		{
			var insertPage = new PageNodeDoubleListEntry(page);
			insertPage.Next = Ptr;
			insertPage.Previous = Previous;

			Previous = insertPage.Ptr;
		}

		public void InsertPageAfter(IntPtr page)
		{
			var insertPage = new PageNodeDoubleListEntry(page);
			insertPage.Next = Next;
			insertPage.Previous = Ptr;

			Next = insertPage.Ptr;
		}

		public IntPtr RemovePage()
		{
			var previous = new PageNodeDoubleListEntry(Previous);
			previous.Next = Next;

			var next = new PageNodeDoubleListEntry(Next);
			next.Previous = Previous;

			return Ptr;
		}
	}
}
