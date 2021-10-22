// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections;

namespace Mosa.Collections.Generic
{
    // StackNode<AnyType>

    #region StackNode<AnyType>
    public class StackNode<AnyType>
    {
        public AnyType Data;
        public StackNode<AnyType> Next = null;
        public StackNode<AnyType> Prev = null;

        public StackNode()
        {
            // Nothing to do here...
        }

        public StackNode(AnyType Data)
        {
            this.Data = Data;
        }

        ~StackNode()
        {
            Next = null;
            Prev = null;
        }
    }
    #endregion

    // Stack<AnyType>

    #region Stack<AnyType>
    public class Stack<AnyType> : IEnumerable, IEnumerator where AnyType : IComparable
    {
        protected StackNode<AnyType> FirstNode = null;
        protected StackNode<AnyType> LastNode = null;
        protected StackNode<AnyType> CurrentNode = null;
        protected StackNode<AnyType> EnumNode = null;
		public uint Count { get; protected set; } = 0;

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
                EnumNode = EnumNode.Prev;
            }
            else
            {
                EnumNode = LastNode;
            }

            return (EnumNode != null);
        }

        void IEnumerator.Reset()
        {
            EnumNode = null;
        }

		public Stack()
		{
			// Nothing to do here...
		}

		~Stack()
		{
			Clear();
		}

		public void Clear()
		{
			StackNode<AnyType> NodePointer = FirstNode;
			StackNode<AnyType> BackupNode = null;

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

        public StackNode<AnyType> GetFirstNode
        {
            get
            {
                CurrentNode = FirstNode;

                return CurrentNode;
            }
        }

        public StackNode<AnyType> GetPrevNode
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

        public StackNode<AnyType> GetNextNode
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

        public StackNode<AnyType> GetLastNode
        {
            get
            {
                CurrentNode = LastNode;

                return CurrentNode;
            }
        }

		public StackNode<AnyType> Find(AnyType Data)
		{
			return FindLast(Data);
		}

        public StackNode<AnyType> FindFirst(AnyType Data)
        {
            StackNode<AnyType> NodePointer = FirstNode;

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

        public StackNode<AnyType> FindLast(AnyType Data)
        {
            StackNode<AnyType> NodePointer = LastNode;

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

		public StackNode<AnyType> Find(StackNode<AnyType> Node)
		{
			return FindLast(Node);
		}

        public StackNode<AnyType> FindFirst(StackNode<AnyType> Node)
        {
            StackNode<AnyType> NodePointer = FirstNode;

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

        public StackNode<AnyType> FindLast(StackNode<AnyType> Node)
        {
            StackNode<AnyType> NodePointer = LastNode;

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

        public bool Contains(StackNode<AnyType> Node)
        {
            return (FindFirst(Node) != null);
        }

        public bool NotContains(StackNode<AnyType> Node)
        {
            return (FindFirst(Node) == null);
        }

        public void RemoveAll(AnyType Data)
        {
            StackNode<AnyType> NodePointer = FirstNode;
            StackNode<AnyType> BackupNode = null;

            while (NodePointer != null)
            {
                BackupNode = NodePointer.Next;

                if (NodePointer.Data.CompareTo(Data) == 0)
                {
                    RemoveNode(NodePointer);

					Count--;
				}

				NodePointer = BackupNode;
            }
        }
		public bool Remove(AnyType Data)
		{
			return RemoveLast(Data);
		}

		public bool RemoveFirst(AnyType Data)
		{
			StackNode<AnyType> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				if (NodePointer.Data.CompareTo(Data) == 0)
				{
					RemoveNode(NodePointer);

					Count--;

					return true;
				}

				NodePointer = NodePointer.Next;
			}

			return false;
		}

		public bool RemoveLast(AnyType Data)
		{
			StackNode<AnyType> NodePointer = LastNode;

			while (NodePointer != null)
			{
				if (NodePointer.Data.CompareTo(Data) == 0)
				{
					RemoveNode(NodePointer);

					Count--;

					return true;
				}

				NodePointer = NodePointer.Prev;
			}

			return false;
		}

		public bool RemoveNode(StackNode<AnyType> Node)
        {
            if (Node == null)
            {
                throw new CollectionsDataNullException("Stack.cs", "Stack<AnyType>", "Delete", "Node", "Node cannot be NULL!");
            }

            if (NotContains(Node))
            {
                throw new CollectionsDataNotFoundException("Stack.cs", "Stack<AnyType>", "Delete", "Node", "Node cannot be found in this Stack!");
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

        public StackNode<AnyType> Push(AnyType Data)
        {
            StackNode<AnyType> NewNode = new StackNode<AnyType>(Data);

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

        public AnyType Pop()
        {
            if (FirstNode == null && LastNode == null && Count == 0)
            {
                throw new CollectionsDataNotFoundException("Stack.cs", "Stack<AnyType>", "Pop", "", "Stack is empty! No data exists in Stack!");
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
                AnyType Result = LastNode.Data;

                StackNode<AnyType> BackupNode = LastNode;

                LastNode = BackupNode.Prev;
                LastNode.Next = null;

                BackupNode = null;

                Count--;

                return Result;
            }

            throw new CollectionsUnknownErrorException("Stack.cs", "Stack<AnyType>", "Pop", "", "A Stack has either the size of 0, 1 or more than 1. This unknown exception should not execute!");
        }

		public bool TryPop (out AnyType Item)
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
				Item = LastNode.Data;

				StackNode<AnyType> BackupNode = LastNode;

				LastNode = BackupNode.Prev;
				LastNode.Next = null;

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
                throw new CollectionsDataNotFoundException("Stack.cs", "Stack<AnyType>", "Pop", "", "Stack is empty! No data exists in Stack!");
            }

            if (FirstNode != null && LastNode != null && FirstNode == LastNode && Count == 1)
            {
                return LastNode.Data;
            }

            if (FirstNode != null && LastNode != null && FirstNode != LastNode && Count > 1)
            {
                return LastNode.Data;
            }

            throw new CollectionsUnknownErrorException("Stack.cs", "Stack<AnyType>", "Peek", "", "A Stack has either the size of 0, 1 or more than 1. This unknown exception should not execute!");
        }

		public bool TryPeek(out AnyType Item)
		{
			Item = default(AnyType);

			if (FirstNode == null && LastNode == null && Count == 0)
			{
				return false;
			}

			if (FirstNode != null && LastNode != null && FirstNode == LastNode && Count == 1)
			{
				Item = LastNode.Data;

				return true;
			}

			if (FirstNode != null && LastNode != null && FirstNode != LastNode && Count > 1)
			{
				Item = LastNode.Data;

				return true;
			}

			return false;
		}

		public Stack<AnyType> Clone()
		{
			Stack<AnyType> Cloned = new Stack<AnyType>();
			StackNode<AnyType> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				Cloned.Push(NodePointer.Data);

				NodePointer = NodePointer.Next;
			}

			return Cloned;
		}

		public void CloneFrom(Stack<AnyType> Source)
        {
            if (Source == null)
            {
                throw new CollectionsDataNullException("Stack.cs", "Stack<AnyType>", "CloneFrom", "Source", "Source cannot be NULL!");
            }

            Clear();

            StackNode<AnyType> NodePointer = Source.FirstNode;

            while (NodePointer != null)
            {
                Push(NodePointer.Data);

                NodePointer = NodePointer.Next;
            }
        }

        public void CloneTo(Stack<AnyType> Destination)
        {
            if (Destination == null)
            {
                throw new CollectionsDataNullException("Stack.cs", "Stack<AnyType>", "CloneTo", "Destination", "Destination cannot be NULL!");
            }

            Destination.Clear();

            StackNode<AnyType> NodePointer = FirstNode;

            while (NodePointer != null)
            {
                Destination.Push(NodePointer.Data);

                NodePointer = NodePointer.Next;
            }
        }

        public void AppendFrom(Stack<AnyType> Source)
        {
            if (Source == null)
            {
                throw new CollectionsDataNullException("Stack.cs", "Stack<AnyType>", "AppendFrom", "Source", "Source cannot be NULL!");
            }

            StackNode<AnyType> NodePointer = Source.FirstNode;

            while (NodePointer != null)
            {
                Push(NodePointer.Data);

                NodePointer = NodePointer.Next;
            }
        }

        public void AppendTo(Stack<AnyType> Destination)
        {
            if (Destination == null)
            {
                throw new CollectionsDataNullException("Stack.cs", "Stack<AnyType>", "AppendTo", "Destination", "Destination cannot be NULL!");
            }

            StackNode<AnyType> NodePointer = FirstNode;

            while (NodePointer != null)
            {
                Destination.Push(NodePointer.Data);

                NodePointer = NodePointer.Next;
            }
        }

		public Stack<AnyType> Reverse()
		{
			Stack<AnyType> Reversed = new Stack<AnyType>();
			StackNode<AnyType> NodePointer = LastNode;

			while (NodePointer != null)
			{
				Reversed.Push(NodePointer.Data);

				NodePointer = NodePointer.Prev;
			}

			return Reversed;
		}

		public AnyType[] ToArray()
		{
			AnyType[] ConvertedArray = new AnyType[Count];
			StackNode<AnyType> NodePointer = FirstNode;
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
			StackNode<AnyType> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				BSTree.Add(NodePointer.Data);

				NodePointer = NodePointer.Next;
			}

			Clear();

			foreach (AnyType ListItem in BSTree.TraverseMinToMax())
			{
				Push(ListItem);
			}

			BSTree.Clear();
			BSTree = null;
		}
	}
	#endregion
}
