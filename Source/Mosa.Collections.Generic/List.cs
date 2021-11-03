// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections;

namespace Mosa.Collections.Generic
{
	// ListNode<AnyType>

	#region ListNode<AnyType>

	public class ListNode<AnyType>
	{
		public AnyType Data;
		public ListNode<AnyType> Prev = null;
		public ListNode<AnyType> Next = null;

		public ListNode()
		{
			// Nothing to do here...
		}

		public ListNode(AnyType Data)
		{
			this.Data = Data;
		}

		~ListNode()
		{
			Prev = null;
			Next = null;
		}
	}

	#endregion ListNode<AnyType>

	// List<AnyType>

	#region List<AnyType>

	public class List<AnyType> : IEnumerable, IEnumerator where AnyType : IComparable
	{
		protected ListNode<AnyType> FirstNode = null;
		protected ListNode<AnyType> LastNode = null;
		protected ListNode<AnyType> CurrentNode = null;
		protected ListNode<AnyType> EnumNode = null;
		public int Count { get; protected set; } = 0;

		public bool IsFixedSize => false;
		public bool IsReadOnly => false;
		public bool IsSynchronized => false;
		public object SyncRoot => this;

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (IEnumerator)this;
		}

		object IEnumerator.Current
		{
			get { return EnumNode.Data; }
		}

		bool IEnumerator.MoveNext()
		{
			if (EnumNode != null)
			{
				EnumNode = EnumNode.Next;
			}
			else
			{
				EnumNode = FirstNode;
			}

			return (EnumNode != null);
		}

		void IEnumerator.Reset()
		{
			EnumNode = null;
		}

		public List()
		{
			// Nothing to do here...
		}

		~List()
		{
			Clear();
		}

		public AnyType this[int Index]
		{
			get
			{
				if (Index + 1 > 0 && Index + 1 <= Count && Count > 0)
				{
					ListNode<AnyType> NodePointer = FirstNode;

					for (uint counter = 0; counter < Index; counter++)
					{
						NodePointer = NodePointer.Next;
					}

					return NodePointer.Data;
				}
				else
				{
					throw new CollectionsDataOutOfRangeException("List.cs", "List<AnyType>", "this[Index]", "Index", "Index is out of range!");
				}
			}
			set
			{
				if (Index + 1 > 0 && Index + 1 <= Count && Count > 0)
				{
					ListNode<AnyType> NodePointer = FirstNode;

					for (uint counter = 0; counter < Index; counter++)
					{
						NodePointer = NodePointer.Next;
					}

					NodePointer.Data = value;
				}
				else
				{
					throw new CollectionsDataOutOfRangeException("List.cs", "List<AnyType>", "this[Index]", "Index", "Index is out of range!");
				}
			}
		}

		public void Clear()
		{
			ListNode<AnyType> NodePointer = FirstNode;
			ListNode<AnyType> BackupPointer = null;

			while (NodePointer != null)
			{
				BackupPointer = NodePointer.Next;

				NodePointer = null;

				Count--;

				NodePointer = BackupPointer;
			}

			FirstNode = null;
			LastNode = null;
			CurrentNode = null;
			Count = 0;
		}

		public bool IsEmpty
		{
			get { return (Count == 0 && FirstNode == null && LastNode == null); }
		}

		public bool IsNotEmpty
		{
			get { return (Count > 0 && FirstNode != null && LastNode != null); }
		}

		public ListNode<AnyType> GetFirstNode
		{
			get
			{
				CurrentNode = FirstNode;

				return CurrentNode;
			}
		}

		public ListNode<AnyType> GetPrevNode
		{
			get
			{
				if (CurrentNode != null)
				{
					CurrentNode = CurrentNode.Prev;
				}

				return CurrentNode;
			}
		}

		public ListNode<AnyType> GetNextNode
		{
			get
			{
				if (CurrentNode != null)
				{
					CurrentNode = CurrentNode.Next;
				}

				return CurrentNode;
			}
		}

		public ListNode<AnyType> GetLastNode
		{
			get
			{
				CurrentNode = LastNode;

				return CurrentNode;
			}
		}

		public ListNode<AnyType> Find(AnyType Data)
		{
			return FindFirst(Data);
		}

		public ListNode<AnyType> FindFirst(AnyType Data)
		{
			ListNode<AnyType> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				if (NodePointer.Data.CompareTo(Data) == 0)
				{
					return NodePointer;
				}
				else
				{
					NodePointer = NodePointer.Next;
				}
			}

			return NodePointer;
		}

		public ListNode<AnyType> FindLast(AnyType Data)
		{
			ListNode<AnyType> NodePointer = LastNode;

			while (NodePointer != null)
			{
				if (NodePointer.Data.CompareTo(Data) == 0)
				{
					return NodePointer;
				}
				else
				{
					NodePointer = NodePointer.Prev;
				}
			}

			return NodePointer;
		}

		public bool Contains(AnyType Data)
		{
			return (FindFirst(Data) != null);
		}

		public bool NotContains(AnyType Data)
		{
			return (FindFirst(Data) == null);
		}

		public int IndexOf(AnyType Data)
		{
			int Index = -1;

			ListNode<AnyType> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				Index++;

				if (NodePointer.Data.CompareTo(Data) == 0)
				{
					return Index;
				}
				else
				{
					NodePointer = NodePointer.Next;
				}
			}

			return -1;
		}

		public int LastIndexOf(AnyType Data)
		{
			int Index = Count;

			ListNode<AnyType> NodePointer = LastNode;

			while (NodePointer != null)
			{
				Index--;

				if (NodePointer.Data.CompareTo(Data) == 0)
				{
					return Index;
				}
				else
				{
					NodePointer = NodePointer.Prev;
				}
			}

			return -1;
		}

		public ListNode<AnyType> Find(ListNode<AnyType> Node)
		{
			return FindFirst(Node);
		}

		public ListNode<AnyType> FindFirst(ListNode<AnyType> Node)
		{
			ListNode<AnyType> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				if (NodePointer == Node)
				{
					return NodePointer;
				}
				else
				{
					NodePointer = NodePointer.Next;
				}
			}

			return NodePointer;
		}

		public ListNode<AnyType> FindLast(ListNode<AnyType> Node)
		{
			ListNode<AnyType> NodePointer = LastNode;

			while (NodePointer != null)
			{
				if (NodePointer == Node)
				{
					return NodePointer;
				}
				else
				{
					NodePointer = NodePointer.Prev;
				}
			}

			return NodePointer;
		}

		public bool Contains(ListNode<AnyType> Node)
		{
			return (FindFirst(Node) != null);
		}

		public bool NotContains(ListNode<AnyType> Node)
		{
			return (FindFirst(Node) == null);
		}

		public virtual ListNode<AnyType> Add(AnyType Data)
		{
			return AddLast(Data);
		}

		public ListNode<AnyType> AddFirst(AnyType Data)
		{
			ListNode<AnyType> NewNode = new ListNode<AnyType>(Data);

			if (FirstNode == null && LastNode == null && Count == 0)
			{
				NewNode.Prev = null;
				NewNode.Next = null;

				FirstNode = NewNode;
				LastNode = NewNode;

				Count++;

				return NewNode;
			}

			if (FirstNode != null && LastNode != null && FirstNode == LastNode && Count == 1)
			{
				NewNode.Prev = null;
				NewNode.Next = FirstNode;
				FirstNode.Prev = NewNode;

				LastNode = FirstNode;
				FirstNode = NewNode;

				Count++;

				return NewNode;
			}

			if (FirstNode != null && LastNode != null && FirstNode != LastNode && Count > 1)
			{
				NewNode.Prev = null;
				NewNode.Next = FirstNode;
				FirstNode.Prev = NewNode;

				FirstNode = NewNode;

				Count++;

				return NewNode;
			}

			return null;
		}

		public ListNode<AnyType> AddLast(AnyType Data)
		{
			ListNode<AnyType> NewNode = new ListNode<AnyType>(Data);

			if (FirstNode == null && LastNode == null && Count == 0)
			{
				NewNode.Prev = null;
				NewNode.Next = null;

				FirstNode = NewNode;
				LastNode = NewNode;

				Count++;

				return NewNode;
			}

			if (FirstNode != null && LastNode != null && FirstNode == LastNode && Count == 1)
			{
				NewNode.Prev = LastNode;
				NewNode.Next = null;

				LastNode.Next = NewNode;
				LastNode = NewNode;

				Count++;

				return NewNode;
			}

			if (FirstNode != null && LastNode != null && FirstNode != LastNode && Count > 1)
			{
				NewNode.Prev = LastNode;
				NewNode.Next = null;

				LastNode.Next = NewNode;
				LastNode = NewNode;

				Count++;

				return NewNode;
			}

			return null;
		}

		public ListNode<AnyType> AddBefore(ListNode<AnyType> Node, AnyType Data)
		{
			if (Node == null)
			{
				throw new CollectionsDataNullException("List.cs", "List<AnyType>", "AddBefore", "Node", "Node cannot be NULL!");
			}

			if (NotContains(Node))
			{
				throw new CollectionsDataNotFoundException("List.cs", "List<AnyType>", "AddBefore", "Node", "Node cannot be found in this List!");
			}

			ListNode<AnyType> NewNode = new ListNode<AnyType>(Data);

			if (Node == FirstNode && Node == LastNode && Count == 1)
			{
				NewNode.Prev = null;
				NewNode.Next = FirstNode;
				FirstNode.Prev = NewNode;

				FirstNode = NewNode;

				Count++;

				return NewNode;
			}

			if (Node == FirstNode && Node != LastNode && Count > 1)
			{
				NewNode.Prev = null;
				NewNode.Next = FirstNode;
				FirstNode.Prev = NewNode;

				FirstNode = NewNode;

				Count++;

				return NewNode;
			}

			if (Node != FirstNode && Node == LastNode && Count > 1)
			{
				ListNode<AnyType> PrevNode = LastNode.Prev;

				NewNode.Prev = PrevNode;
				NewNode.Next = LastNode;
				PrevNode.Next = NewNode;
				LastNode.Prev = NewNode;

				Count++;

				return NewNode;
			}

			if (Node != FirstNode && Node != LastNode && Count > 2)
			{
				ListNode<AnyType> PrevNode = Node.Prev;

				NewNode.Prev = PrevNode;
				NewNode.Next = Node;
				PrevNode.Next = NewNode;
				Node.Prev = NewNode;

				Count++;

				return NewNode;
			}

			return null;
		}

		public ListNode<AnyType> AddAfter(ListNode<AnyType> Node, AnyType Data)
		{
			if (Node == null)
			{
				throw new CollectionsDataNullException("List.cs", "List<AnyType>", "AddAfter", "Node", "Node cannot be NULL!");
			}

			if (NotContains(Node))
			{
				throw new CollectionsDataNotFoundException("List.cs", "List<AnyType>", "AddAfter", "Node", "Node cannot be found in this List!");
			}

			ListNode<AnyType> NewNode = new ListNode<AnyType>(Data);

			if (Node == FirstNode && Node == LastNode && Count == 1)
			{
				FirstNode.Next = NewNode;
				NewNode.Prev = FirstNode;
				NewNode.Next = null;

				LastNode = NewNode;

				Count++;

				return NewNode;
			}

			if (Node == FirstNode && Node != LastNode && Count > 1)
			{
				ListNode<AnyType> NextNode = FirstNode.Next;

				FirstNode.Next = NewNode;
				NewNode.Prev = FirstNode;
				NewNode.Next = NextNode;
				NextNode.Prev = NewNode;

				Count++;

				return NewNode;
			}

			if (Node != FirstNode && Node == LastNode && Count > 1)
			{
				LastNode.Next = NewNode;
				NewNode.Prev = LastNode;
				NewNode.Next = null;

				LastNode = NewNode;

				Count++;

				return NewNode;
			}

			if (Node != FirstNode && Node != LastNode && Count > 2)
			{
				ListNode<AnyType> NextNode = Node.Next;

				Node.Next = NewNode;
				NewNode.Prev = Node;
				NewNode.Next = NextNode;
				NextNode.Prev = NewNode;

				Count++;

				return NewNode;
			}

			return null;
		}

		public bool RemoveAt(int Index)
		{
			ListNode<AnyType> NodePointer = FirstNode;
			int Counter = -1;

			while (NodePointer != null)
			{
				Counter++;

				if (Counter == Index)
				{
					return Remove(NodePointer);
				}
				else
				{
					NodePointer = NodePointer.Next;
				}
			}

			return false;
		}

		public bool Remove(ListNode<AnyType> Node)
		{
			if (Node == null)
			{
				throw new CollectionsDataNullException("List.cs", "List<AnyType>", "Delete", "Node", "Node cannot be NULL!");
			}

			if (NotContains(Node))
			{
				throw new CollectionsDataNotFoundException("List.cs", "List<AnyType>", "Delete", "Node", "Node cannot be found in this List!");
			}

			if (Node.Prev == null && Node.Next == null && Count == 1)
			{
				FirstNode = null;
				LastNode = null;
				Node = null;

				Count--;

				return true;
			}

			if (Node.Prev == null && Node.Next != null && Count > 1)
			{
				FirstNode = Node.Next;
				FirstNode.Prev = null;
				Node = null;

				Count--;

				return true;
			}

			if (Node.Prev != null && Node.Next == null && Count > 1)
			{
				LastNode = Node.Prev;
				LastNode.Next = null;
				Node = null;

				Count--;

				return true;
			}

			if (Node.Prev != null && Node.Next != null && Count > 2)
			{
				Node.Prev.Next = Node.Next;
				Node.Next.Prev = Node.Prev;
				Node = null;

				Count--;

				return true;
			}

			return false;
		}

		public bool RemoveFirst(AnyType Data)
		{
			ListNode<AnyType> NodePointer = FirstNode;
			bool IsNodeFound = false;

			while (NodePointer != null && IsNodeFound == false)
			{
				if (NodePointer.Data.CompareTo(Data) == 0)
				{
					Remove(NodePointer);

					IsNodeFound = true;
				}
				else
				{
					NodePointer = NodePointer.Next;
				}
			}

			return IsNodeFound;
		}

		public bool RemoveLast(AnyType Data)
		{
			ListNode<AnyType> NodePointer = LastNode;
			bool IsNodeFound = false;

			while (NodePointer != null && IsNodeFound == false)
			{
				if (NodePointer.Data.CompareTo(Data) == 0)
				{
					Remove(NodePointer);

					IsNodeFound = true;
				}
				else
				{
					NodePointer = NodePointer.Prev;
				}
			}

			return IsNodeFound;
		}

		public bool RemoveAll(AnyType Data)
		{
			ListNode<AnyType> NodePointer = FirstNode;
			ListNode<AnyType> NextNode = null;
			bool IsNodeFound = false;

			while (NodePointer != null)
			{
				NextNode = NodePointer.Next;

				if (NodePointer.Data.CompareTo(Data) == 0)
				{
					Remove(NodePointer);

					IsNodeFound = true;
				}

				NodePointer = NextNode;
			}

			return IsNodeFound;
		}

		public List<AnyType> Reverse()
		{
			ListNode<AnyType> NodePointer = LastNode;
			List<AnyType> Reversed = new List<AnyType>();

			while (NodePointer != null)
			{
				Reversed.Add(NodePointer.Data);

				NodePointer = NodePointer.Prev;
			}

			return Reversed;
		}

		public List<AnyType> Clone()
		{
			List<AnyType> Cloned = new List<AnyType>();
			ListNode<AnyType> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				Cloned.Add(NodePointer.Data);

				NodePointer = NodePointer.Next;
			}

			return Cloned;
		}

		public void CloneFrom(List<AnyType> Source)
		{
			if (Source == null)
			{
				throw new CollectionsDataNullException("List.cs", "List<AnyType>", "CloneFrom", "Source", "Source cannot be NULL!");
			}

			Clear();

			ListNode<AnyType> NodePointer = Source.FirstNode;

			while (NodePointer != null)
			{
				AddLast(NodePointer.Data);

				NodePointer = NodePointer.Next;
			}
		}

		public void CloneTo(List<AnyType> Destination)
		{
			if (Destination == null)
			{
				throw new CollectionsDataNullException("List.cs", "List<AnyType>", "CloneTo", "Destination", "Destination cannot be NULL!");
			}

			Destination.Clear();

			ListNode<AnyType> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				Destination.AddLast(NodePointer.Data);

				NodePointer = NodePointer.Next;
			}
		}

		public void AppendFrom(List<AnyType> Source)
		{
			if (Source == null)
			{
				throw new CollectionsDataNullException("List.cs", "List<AnyType>", "AppendFrom", "Source", "Source cannot be NULL!");
			}

			ListNode<AnyType> NodePointer = Source.FirstNode;

			while (NodePointer != null)
			{
				AddLast(NodePointer.Data);

				NodePointer = NodePointer.Next;
			}
		}

		public void AppendTo(List<AnyType> Destination)
		{
			if (Destination == null)
			{
				throw new CollectionsDataNullException("List.cs", "List<AnyType>", "AppendTo", "Destination", "Destination cannot be NULL!");
			}

			ListNode<AnyType> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				Destination.AddLast(NodePointer.Data);

				NodePointer = NodePointer.Next;
			}
		}

		public AnyType[] ToArray()
		{
			ListNode<AnyType> NodePointer = FirstNode;
			AnyType[] ConvertedArray = new AnyType[Count];
			int Counter = -1;

			while (NodePointer != null)
			{
				Counter++;

				ConvertedArray[Counter] = NodePointer.Data;

				NodePointer = NodePointer.Next;
			}

			return ConvertedArray;
		}

		public void SortWithBinarySearchTree()
		{
			BinarySearchTree<AnyType> BTree = new BinarySearchTree<AnyType>();
			ListNode<AnyType> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				BTree.Add(NodePointer.Data);

				NodePointer = NodePointer.Next;
			}

			CloneFrom(BTree.TraverseMinToMax());

			BTree.Clear();
			BTree = null;
		}
	}

	#endregion List<AnyType>
}
