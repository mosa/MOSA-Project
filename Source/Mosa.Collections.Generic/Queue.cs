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
	#endregion

	// Queue<AnyType>

	#region Queue<AnyType>
	public class Queue<AnyType> : IEnumerable, IEnumerator where AnyType : IComparable
    {
        protected QueueNode<AnyType> FirstNode = null;
        protected QueueNode<AnyType> LastNode = null;
        protected QueueNode<AnyType> CurrentNode = null;
        protected uint Size = 0;
        protected QueueNode<AnyType> EnumNode = null;

        public Queue()
        {
            // Nothing to do here...
        }

        ~Queue()
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

        public void DeleteAll()
        {
            QueueNode<AnyType> NodePointer = FirstNode;
            QueueNode<AnyType> BackupNode = null;

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
            QueueNode<AnyType> NodePointer = FirstNode;
            QueueNode<AnyType> BackupNode = null;

            while (NodePointer != null)
            {
                BackupNode = NodePointer.Next;

                if (NodePointer.Data.CompareTo(Data) == 0)
                {
                    Delete(NodePointer);
                }

                Size--;

                NodePointer = BackupNode;
            }
        }

        public bool Delete(QueueNode<AnyType> Node)
        {
            if (Node == null)
            {
                throw new CollectionsDataNullException("Queue.cs", "Queue<AnyType>", "Delete", "Node", "Node cannot be NULL!");
            }

            if (NotContains(Node))
            {
                throw new CollectionsDataNotFoundException("Queue.cs", "Queue<AnyType>", "Delete", "Node", "Node cannot be found in this Queue!");
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

        public QueueNode<AnyType> Enqueue(AnyType Data)
        {
            QueueNode<AnyType> NewNode = new QueueNode<AnyType>(Data);

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

        public AnyType Dequeue()
        {
            if (FirstNode == null && LastNode == null && Size == 0)
            {
                throw new CollectionsDataNotFoundException("Queue.cs", "Queue<AnyType>", "Dequeue", "", "Queue is empty! No data exists in Queue!");
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
                QueueNode<AnyType> BackupNode = FirstNode;
                AnyType Result = FirstNode.Data;

                FirstNode = FirstNode.Next;
                FirstNode.Prev = null;

                BackupNode = null;

                Size--;

                return Result;
            }

            throw new CollectionsUnknownErrorException("Queue.cs", "Queue<AnyType>", "Dequeue", "", "A Queue has either the size of 0, 1 or more than 1. This unknown exception should not execute!");
        }

        public AnyType Peek()
        {
            if (FirstNode == null && LastNode == null && Size == 0)
            {
                throw new CollectionsDataNotFoundException("Queue.cs", "Queue<AnyType>", "Peek", "", "Queue is empty! No data exists in Queue!");
            }

            if (FirstNode != null && LastNode != null && Size > 0)
            {
                return FirstNode.Data;
            }

            throw new CollectionsUnknownErrorException("Queue.cs", "Queue<AnyType>", "Peek", "", "A Queue has either the size of 0 or more than 0. This unknown exception should not execute!");
        }

        public void CloneFrom(Queue<AnyType> Source)
        {
            if (Source == null)
            {
                throw new CollectionsDataNullException("Queue.cs", "Queue<AnyType>", "CloneFrom", "Source", "Source cannot be NULL!");
            }

            DeleteAll();

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

            Destination.DeleteAll();

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
    }
    #endregion
}
