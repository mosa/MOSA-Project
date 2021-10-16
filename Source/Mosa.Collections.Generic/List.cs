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
    #endregion

    // List<AnyType>

    #region List<AnyType>
    public class List<AnyType> : IEnumerable, IEnumerator where AnyType : IComparable
    {
        protected ListNode<AnyType> FirstNode = null;
        protected ListNode<AnyType> LastNode = null;
        protected ListNode<AnyType> CurrentNode = null;
        protected uint Size = 0;
        protected ListNode<AnyType> EnumNode = null;

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
            DeleteAll();
        }

        public void DeleteAll()
        {
            ListNode<AnyType> NodePointer = FirstNode;
            ListNode<AnyType> BackupPointer = null;

            while (NodePointer != null)
            {
                BackupPointer = NodePointer.Next;

                NodePointer = null;

                Size--;

                NodePointer = BackupPointer;
            }

            FirstNode = null;
            LastNode = null;
            CurrentNode = null;
            Size = 0;
        }

        public uint GetSize
        {
            get { return Size; }
        }

        public bool IsEmpty
        {
            get { return (Size == 0 && FirstNode == null && LastNode == null); }
        }

        public bool IsNotEmpty
        {
            get { return (Size > 0 && FirstNode != null && LastNode != null); }
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
                NewNode.Prev = null;
                NewNode.Next = FirstNode;
                FirstNode.Prev = NewNode;

                LastNode = FirstNode;
                FirstNode = NewNode;

                Size++;

                return NewNode;
            }

            if (FirstNode != null && LastNode != null && FirstNode != LastNode && Size > 1)
            {
                NewNode.Prev = null;
                NewNode.Next = FirstNode;
                FirstNode.Prev = NewNode;

                FirstNode = NewNode;

                Size++;

                return NewNode;
            }

            return null;
        }

        public ListNode<AnyType> AddLast(AnyType Data)
        {
            ListNode<AnyType> NewNode = new ListNode<AnyType>(Data);

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

            if (Node == FirstNode && Node == LastNode && Size == 1)
            {
                NewNode.Prev = null;
                NewNode.Next = FirstNode;
                FirstNode.Prev = NewNode;

                FirstNode = NewNode;

                Size++;

                return NewNode;
            }

            if (Node == FirstNode && Node != LastNode && Size > 1)
            {
                NewNode.Prev = null;
                NewNode.Next = FirstNode;
                FirstNode.Prev = NewNode;

                FirstNode = NewNode;

                Size++;

                return NewNode;
            }

            if (Node != FirstNode && Node == LastNode && Size > 1)
            {
                ListNode<AnyType> PrevNode = LastNode.Prev;

                NewNode.Prev = PrevNode;
                NewNode.Next = LastNode;
                PrevNode.Next = NewNode;
                LastNode.Prev = NewNode;

                Size++;

                return NewNode;
            }

            if (Node != FirstNode && Node != LastNode && Size > 2)
            {
                ListNode<AnyType> PrevNode = Node.Prev;

                NewNode.Prev = PrevNode;
                NewNode.Next = Node;
                PrevNode.Next = NewNode;
                Node.Prev = NewNode;

                Size++;

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

            if (Node == FirstNode && Node == LastNode && Size == 1)
            {
                FirstNode.Next = NewNode;
                NewNode.Prev = FirstNode;
                NewNode.Next = null;

                LastNode = NewNode;

                Size++;

                return NewNode;
            }

            if (Node == FirstNode && Node != LastNode && Size > 1)
            {
                ListNode<AnyType> NextNode = FirstNode.Next;

                FirstNode.Next = NewNode;
                NewNode.Prev = FirstNode;
                NewNode.Next = NextNode;
                NextNode.Prev = NewNode;

                Size++;

                return NewNode;
            }

            if (Node != FirstNode && Node == LastNode && Size > 1)
            {
                LastNode.Next = NewNode;
                NewNode.Prev = LastNode;
                NewNode.Next = null;

                LastNode = NewNode;

                Size++;

                return NewNode;
            }

            if (Node != FirstNode && Node != LastNode && Size > 2)
            {
                ListNode<AnyType> NextNode = Node.Next;

                Node.Next = NewNode;
                NewNode.Prev = Node;
                NewNode.Next = NextNode;
                NextNode.Prev = NewNode;

                Size++;

                return NewNode;
            }

            return null;
        }

        public bool Delete(ListNode<AnyType> Node)
        {
            if (Node == null)
            {
                throw new CollectionsDataNullException("List.cs", "List<AnyType>", "Delete", "Node", "Node cannot be NULL!");
            }

            if (NotContains(Node))
            {
                throw new CollectionsDataNotFoundException("List.cs", "List<AnyType>", "Delete", "Node", "Node cannot be found in this List!");
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

        public bool DeleteFirst(AnyType Data)
        {
            ListNode<AnyType> NodePointer = FirstNode;
            bool IsNodeFound = false;

            while (NodePointer != null && IsNodeFound == false)
            {
                if (NodePointer.Data.CompareTo(Data) == 0)
                {
                    Delete(NodePointer);

                    IsNodeFound = true;
                }
                else
                {
                    NodePointer = NodePointer.Next;
                }
            }

            return IsNodeFound;
        }

        public bool DeleteLast(AnyType Data)
        {
            ListNode<AnyType> NodePointer = LastNode;
            bool IsNodeFound = false;

            while (NodePointer != null && IsNodeFound == false)
            {
                if (NodePointer.Data.CompareTo(Data) == 0)
                {
                    Delete(NodePointer);

                    IsNodeFound = true;
                }
                else
                {
                    NodePointer = NodePointer.Prev;
                }
            }

            return IsNodeFound;
        }

        public bool DeleteAll(AnyType Data)
        {
            ListNode<AnyType> NodePointer = FirstNode;
            ListNode<AnyType> NextNode = null;
            bool IsNodeFound = false;

            while (NodePointer != null)
            {
                NextNode = NodePointer.Next;

                if (NodePointer.Data.CompareTo(Data) == 0)
                {
                    Delete(NodePointer);

                    IsNodeFound = true;
                }

                NodePointer = NextNode;
            }

            return IsNodeFound;
        }

        public void CloneFrom(List<AnyType> Source)
        {
            if (Source == null)
            {
                throw new CollectionsDataNullException("List.cs", "List<AnyType>", "CloneFrom", "Source", "Source cannot be NULL!");
            }

            DeleteAll();

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

            Destination.DeleteAll();

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

        public void SortWithBinaryTree()
        {
            BinaryTree<AnyType> BTree = new BinaryTree<AnyType>();
            ListNode<AnyType> NodePointer = FirstNode;

            while (NodePointer != null)
            {
                BTree.Add(NodePointer.Data);

                NodePointer = NodePointer.Next;
            }

            CloneFrom(BTree.TraverseMinToMax());

            BTree.DeleteAll();
            BTree = null;
        }
    }
    #endregion
}
