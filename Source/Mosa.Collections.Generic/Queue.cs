// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections;

namespace Mosa.Collections.Generic
{
	// QueueNode<AnyType>

	#region QueueNode<AnyType>

	public class QueueNode<AnyType>
	{
		public AnyType Data;
		public QueueNode<AnyType> Next = null;
		public QueueNode<AnyType> Prev = null;

		public QueueNode()
		{
			// Nothing to do here...
		}

		public QueueNode(AnyType Data)
		{
			this.Data = Data;
		}

		~QueueNode()
		{
			Next = null;
			Prev = null;
		}
	}

	#endregion QueueNode<AnyType>

	// Queue<AnyType>

	#region Queue<AnyType>

	public class Queue<AnyType> : IEnumerable, IEnumerator where AnyType : IComparable
	{
		protected QueueNode<AnyType> FirstNode = null;
		protected QueueNode<AnyType> LastNode = null;
		protected QueueNode<AnyType> CurrentNode = null;
		protected QueueNode<AnyType> EnumNode = null;
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

		public Queue()
		{
			// Nothing to do here...
		}

		~Queue()
		{
			Clear();
		}

		public void Clear()
		{
			QueueNode<AnyType> NodePointer = FirstNode;
			QueueNode<AnyType> BackupNode = null;

			while (NodePointer != null && Count > 0)
			{
				BackupNode = NodePointer.Next;

				NodePointer = null;

				Count--;

				NodePointer = BackupNode;
			}

			FirstNode = null;
			LastNode = null;
			CurrentNode = null;
			Count = 0;
		}

		public bool IsEmpty
		{
			get { return (FirstNode == null && LastNode == null && Count == 0); }
		}

		public bool IsNotEmpty
		{
			get { return (FirstNode != null && LastNode != null && Count > 0); }
		}

		public QueueNode<AnyType> GetFirstNode
		{
			get
			{
				CurrentNode = FirstNode;

				return CurrentNode;
			}
		}

		public QueueNode<AnyType> GetPrevNode
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

		public QueueNode<AnyType> GetNextNode
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

		public QueueNode<AnyType> GetLastNode
		{
			get
			{
				CurrentNode = LastNode;

				return CurrentNode;
			}
		}

		public QueueNode<AnyType> Find(AnyType Data)
		{
			return FindFirst(Data);
		}

		public QueueNode<AnyType> FindFirst(AnyType Data)
		{
			QueueNode<AnyType> NodePointer = FirstNode;

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

		public QueueNode<AnyType> FindLast(AnyType Data)
		{
			QueueNode<AnyType> NodePointer = LastNode;

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

		public QueueNode<AnyType> Find(QueueNode<AnyType> Node)
		{
			return FindFirst(Node);
		}

		public QueueNode<AnyType> FindFirst(QueueNode<AnyType> Node)
		{
			QueueNode<AnyType> NodePointer = FirstNode;

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

		public QueueNode<AnyType> FindLast(QueueNode<AnyType> Node)
		{
			QueueNode<AnyType> NodePointer = LastNode;

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

		public bool Contains(QueueNode<AnyType> Node)
		{
			return (FindFirst(Node) != null);
		}

		public bool NotContains(QueueNode<AnyType> Node)
		{
			return (FindFirst(Node) == null);
		}

		public void RemoveAll(AnyType Data)
		{
			QueueNode<AnyType> NodePointer = FirstNode;
			QueueNode<AnyType> BackupNode = null;

			while (NodePointer != null)
			{
				BackupNode = NodePointer.Next;

				if (NodePointer.Data.CompareTo(Data) == 0)
				{
					Remove(NodePointer);

					Count--;
				}

				NodePointer = BackupNode;
			}
		}

		public bool Remove(AnyType Data)
		{
			return RemoveFirst(Data);
		}

		public bool RemoveFirst(AnyType Data)
		{
			QueueNode<AnyType> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				if (NodePointer.Data.CompareTo(Data) == 0)
				{
					Remove(NodePointer);

					Count--;

					return true;
				}

				NodePointer = NodePointer.Next;
			}

			return false;
		}

		public bool RemoveLast(AnyType Data)
		{
			QueueNode<AnyType> NodePointer = LastNode;

			while (NodePointer != null)
			{
				if (NodePointer.Data.CompareTo(Data) == 0)
				{
					Remove(NodePointer);

					Count--;

					return true;
				}

				NodePointer = NodePointer.Prev;
			}

			return false;
		}

		public bool Remove(QueueNode<AnyType> Node)
		{
			if (Node == null)
			{
				throw new CollectionsDataNullException("Queue.cs", "Queue<AnyType>", "Delete", "Node", "Node cannot be NULL!");
			}

			if (NotContains(Node))
			{
				throw new CollectionsDataNotFoundException("Queue.cs", "Queue<AnyType>", "Delete", "Node", "Node cannot be found in this Queue!");
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

		public QueueNode<AnyType> Enqueue(AnyType Data)
		{
			QueueNode<AnyType> NewNode = new QueueNode<AnyType>(Data);

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

		public AnyType Dequeue()
		{
			if (FirstNode == null && LastNode == null && Count == 0)
			{
				throw new CollectionsDataNotFoundException("Queue.cs", "Queue<AnyType>", "Dequeue", "", "Queue is empty! No data exists in Queue!");
			}

			if (FirstNode != null && LastNode != null && FirstNode == LastNode && Count == 1)
			{
				AnyType Result = FirstNode.Data;

				FirstNode = null;
				LastNode = null;

				Count--;

				return Result;
			}

			if (FirstNode != null && LastNode != null && FirstNode != LastNode && Count > 1)
			{
				QueueNode<AnyType> BackupNode = FirstNode;
				AnyType Result = FirstNode.Data;

				FirstNode = FirstNode.Next;
				FirstNode.Prev = null;

				BackupNode = null;

				Count--;

				return Result;
			}

			throw new CollectionsUnknownErrorException("Queue.cs", "Queue<AnyType>", "Dequeue", "", "A Queue has either the size of 0, 1 or more than 1. This unknown exception should not execute!");
		}

		public bool TryDequeue(out AnyType Item)
		{
			Item = default(AnyType);

			if (FirstNode == null && LastNode == null && Count == 0)
			{
				return false;
			}

			if (FirstNode != null && LastNode != null && FirstNode == LastNode && Count == 1)
			{
				Item = FirstNode.Data;

				FirstNode = null;
				LastNode = null;

				Count--;

				return true;
			}

			if (FirstNode != null && LastNode != null && FirstNode != LastNode && Count > 1)
			{
				QueueNode<AnyType> BackupNode = FirstNode;
				Item = FirstNode.Data;

				FirstNode = FirstNode.Next;
				FirstNode.Prev = null;

				BackupNode = null;

				Count--;

				return true;
			}

			return false;
		}

		public AnyType Peek()
		{
			if (FirstNode == null && LastNode == null && Count == 0)
			{
				throw new CollectionsDataNotFoundException("Queue.cs", "Queue<AnyType>", "Peek", "", "Queue is empty! No data exists in Queue!");
			}

			if (FirstNode != null && LastNode != null && Count > 0)
			{
				return FirstNode.Data;
			}

			throw new CollectionsUnknownErrorException("Queue.cs", "Queue<AnyType>", "Peek", "", "A Queue has either the size of 0 or more than 0. This unknown exception should not execute!");
		}

		public bool TryPeek(out AnyType Item)
		{
			Item = default(AnyType);

			if (FirstNode == null && LastNode == null && Count == 0)
			{
				return false;
			}

			if (FirstNode != null && LastNode != null && Count > 0)
			{
				Item = FirstNode.Data;
				return true;
			}

			return false;
		}

		public Queue<AnyType> Clone()
		{
			Queue<AnyType> Cloned = new Queue<AnyType>();
			QueueNode<AnyType> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				Cloned.Enqueue(NodePointer.Data);

				NodePointer = NodePointer.Next;
			}

			return Cloned;
		}

		public void CloneFrom(Queue<AnyType> Source)
		{
			if (Source == null)
			{
				throw new CollectionsDataNullException("Queue.cs", "Queue<AnyType>", "CloneFrom", "Source", "Source cannot be NULL!");
			}

			Clear();

			QueueNode<AnyType> NodePointer = Source.FirstNode;

			while (NodePointer != null)
			{
				Enqueue(NodePointer.Data);

				NodePointer = NodePointer.Next;
			}
		}

		public void CloneTo(Queue<AnyType> Destination)
		{
			if (Destination == null)
			{
				throw new CollectionsDataNullException("Queue.cs", "Queue<AnyType>", "CloneTo", "Destination", "Destination cannot be NULL!");
			}

			Destination.Clear();

			QueueNode<AnyType> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				Destination.Enqueue(NodePointer.Data);

				NodePointer = NodePointer.Next;
			}
		}

		public void AppendFrom(Queue<AnyType> Source)
		{
			if (Source == null)
			{
				throw new CollectionsDataNullException("Queue.cs", "Queue<AnyType>", "AppendFrom", "Source", "Source cannot be NULL!");
			}

			QueueNode<AnyType> NodePointer = Source.FirstNode;

			while (NodePointer != null)
			{
				Enqueue(NodePointer.Data);

				NodePointer = NodePointer.Next;
			}
		}

		public void AppendTo(Queue<AnyType> Destination)
		{
			if (Destination == null)
			{
				throw new CollectionsDataNullException("Queue.cs", "Queue<AnyType>", "AppendTo", "Destination", "Destination cannot be NULL!");
			}

			QueueNode<AnyType> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				Destination.Enqueue(NodePointer.Data);

				NodePointer = NodePointer.Next;
			}
		}

		public Queue<AnyType> Reverse()
		{
			Queue<AnyType> Reversed = new Queue<AnyType>();
			QueueNode<AnyType> NodePointer = LastNode;

			while (NodePointer != null)
			{
				Reversed.Enqueue(NodePointer.Data);

				NodePointer = NodePointer.Prev;
			}

			return Reversed;
		}

		public AnyType[] ToArray()
		{
			AnyType[] ConvertedArray = new AnyType[Count];
			QueueNode<AnyType> NodePointer = FirstNode;
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
			BinarySearchTree<AnyType> BSTree = new BinarySearchTree<AnyType>();
			QueueNode<AnyType> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				BSTree.Add(NodePointer.Data);

				NodePointer = NodePointer.Next;
			}

			Clear();

			foreach (AnyType ListItem in BSTree.TraverseMinToMax())
			{
				Enqueue(ListItem);
			}

			BSTree.Clear();
			BSTree = null;
		}
	}

	#endregion Queue<AnyType>
}
