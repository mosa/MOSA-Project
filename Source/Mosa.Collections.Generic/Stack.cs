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
        protected uint Size = 0;
        protected StackNode<AnyType> EnumNode = null;

        public Stack()
        {
            // Nothing to do here...
        }

        ~Stack()
        {
            DeleteAll();
        }

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

        public uint GetSize
        {
            get { return Size; }
        }

        public bool IsEmpty
        {
            get { return (FirstNode == null && LastNode == null && Size == 0); }
        }

        public bool IsNotEmpty
        {
            get { return (FirstNode != null && LastNode != null && Size > 0); }
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

        public StackNode<AnyType> FindFirst(AnyType Data)
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

        public StackNode<AnyType> FindLast(AnyType Data)
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

        public bool Contains(AnyType Data)
        {
            return (FindFirst(Data) != null);
        }

        public bool NotContains(AnyType Data)
        {
            return (FindFirst(Data) == null);
        }

        public StackNode<AnyType> FindFirst(StackNode<AnyType> Node)
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

        public StackNode<AnyType> FindLast(StackNode<AnyType> Node)
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

        public bool Contains(StackNode<AnyType> Node)
        {
            return (FindFirst(Node) != null);
        }

        public bool NotContains(StackNode<AnyType> Node)
        {
            return (FindFirst(Node) == null);
        }

        public void DeleteAll()
        {
            StackNode<AnyType> NodePointer = FirstNode;
            StackNode<AnyType> BackupNode = null;

            while (NodePointer != null && Size > 0)
            {
                BackupNode = NodePointer.Next;

                NodePointer = null;

                Size--;

                NodePointer = BackupNode;
            }

            FirstNode = null;
            LastNode = null;
            CurrentNode = null;
            Size = 0;
        }

        public void DeleteAll(AnyType Data)
        {
            StackNode<AnyType> NodePointer = FirstNode;
            StackNode<AnyType> BackupNode = null;

            while (NodePointer != null)
            {
                BackupNode = NodePointer.Next;

                if (NodePointer.Data.CompareTo(Data) == 0)
                {
                    Delete(NodePointer);

					Size--;
				}

				NodePointer = BackupNode;
            }
        }

		public bool DeleteFirst(AnyType Data)
		{
			StackNode<AnyType> NodePointer = LastNode;

			while (NodePointer != null)
			{
				if (NodePointer.Data.CompareTo(Data) == 0)
				{
					Delete(NodePointer);

					Size--;

					return true;
				}

				NodePointer = NodePointer.Prev;
			}

			return false;
		}

		public bool DeleteLast(AnyType Data)
		{
			StackNode<AnyType> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				if (NodePointer.Data.CompareTo(Data) == 0)
				{
					Delete(NodePointer);

					Size--;

					return true;
				}

				NodePointer = NodePointer.Next;
			}

			return false;
		}

		public bool Delete(StackNode<AnyType> Node)
        {
            if (Node == null)
            {
                throw new CollectionsDataNullException("Stack.cs", "Stack<AnyType>", "Delete", "Node", "Node cannot be NULL!");
            }

            if (NotContains(Node))
            {
                throw new CollectionsDataNotFoundException("Stack.cs", "Stack<AnyType>", "Delete", "Node", "Node cannot be found in this Stack!");
            }

            if (Node.Prev == null && Node.Next == null && Size == 1)
            {
                FirstNode = null;
                LastNode = null;

                Node = null;

                Size--;

                return true;
            }

            if (Node.Prev == null && Node.Next != null && Size > 1)
            {
                FirstNode = Node.Next;
                FirstNode.Prev = null;

                Node = null;

                Size--;

                return true;
            }

            if (Node.Prev != null && Node.Next == null && Size > 1)
            {
                LastNode = Node.Prev;
                LastNode.Next = null;

                Node = null;

                Size--;

                return true;
            }

            if (Node.Prev != null && Node.Next != null && Size > 2)
            {
                Node.Prev.Next = Node.Next;
                Node.Next.Prev = Node.Prev;

                Node = null;

                Size--;

                return true;
            }

            return false;
        }

        public StackNode<AnyType> Push(AnyType Data)
        {
            StackNode<AnyType> NewNode = new StackNode<AnyType>(Data);

            if (FirstNode == null && LastNode == null && Size == 0)
            {
                NewNode.Prev = null;
                NewNode.Next = null;

                FirstNode = NewNode;
                LastNode = NewNode;

                Size++;

                return NewNode;
            }

            if (FirstNode != null && LastNode != null && FirstNode == LastNode && Size == 1)
            {
                NewNode.Prev = LastNode;
                NewNode.Next = null;

                LastNode.Next = NewNode;
                LastNode = NewNode;

                Size++;

                return NewNode;
            }

            if (FirstNode != null && LastNode != null && FirstNode != LastNode && Size > 1)
            {
                NewNode.Prev = LastNode;
                NewNode.Next = null;

                LastNode.Next = NewNode;
                LastNode = NewNode;

                Size++;

                return NewNode;
            }

            return null;
        }

        public AnyType Pop()
        {
            if (FirstNode == null && LastNode == null && Size == 0)
            {
                throw new CollectionsDataNotFoundException("Stack.cs", "Stack<AnyType>", "Pop", "", "Stack is empty! No data exists in Stack!");
            }

            if (FirstNode != null && LastNode != null && FirstNode == LastNode && Size == 1)
            {
                AnyType Result = FirstNode.Data;

                FirstNode = null;
                LastNode = null;

                Size--;

                return Result;
            }

            if (FirstNode != null && LastNode != null && FirstNode != LastNode && Size > 1)
            {
                AnyType Result = LastNode.Data;

                StackNode<AnyType> BackupNode = LastNode;

                LastNode = BackupNode.Prev;
                LastNode.Next = null;

                BackupNode = null;

                Size--;

                return Result;
            }

            throw new CollectionsUnknownErrorException("Stack.cs", "Stack<AnyType>", "Pop", "", "A Stack has either the size of 0, 1 or more than 1. This unknown exception should not execute!");
        }

        public AnyType Peek()
        {
            if (FirstNode == null && LastNode == null && Size == 0)
            {
                throw new CollectionsDataNotFoundException("Stack.cs", "Stack<AnyType>", "Pop", "", "Stack is empty! No data exists in Stack!");

            }

            if (FirstNode != null && LastNode != null && FirstNode == LastNode && Size == 1)
            {
                return LastNode.Data;
            }

            if (FirstNode != null && LastNode != null && FirstNode != LastNode && Size > 1)
            {
                return LastNode.Data;
            }

            throw new CollectionsUnknownErrorException("Stack.cs", "Stack<AnyType>", "Peek", "", "A Stack has either the size of 0, 1 or more than 1. This unknown exception should not execute!");
        }

        public void CloneFrom(Stack<AnyType> Source)
        {
            if (Source == null)
            {
                throw new CollectionsDataNullException("Stack.cs", "Stack<AnyType>", "CloneFrom", "Source", "Source cannot be NULL!");
            }

            DeleteAll();

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

            Destination.DeleteAll();

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
    }
    #endregion
}
