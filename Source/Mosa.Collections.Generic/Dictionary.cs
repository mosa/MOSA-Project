// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections;

namespace Mosa.Collections.Generic
{
    // DictionaryNode<TKey, TValue>

    #region DictionaryNode<TKey, TValue>
    public class DictionaryNode<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;
        public DictionaryNode<TKey, TValue> Next = null;
        public DictionaryNode<TKey, TValue> Prev = null;

        public DictionaryNode()
        {
            // Nothing to do here...
        }

        public DictionaryNode(TKey Key, TValue Value)
        {
            this.Key = Key;
            this.Value = Value;
        }

        ~DictionaryNode()
        {
            this.Next = null;
            this.Prev = null;
        }
    }
	#endregion

	// Dictionary<TKey, TValue>

	#region Dictionary<TKey, TValue>
	public class Dictionary<TKey, TValue>: IEnumerable, IEnumerator where TKey: IComparable
    {
        protected DictionaryNode<TKey, TValue> FirstNode = null;
        protected DictionaryNode<TKey, TValue> LastNode = null;
        protected DictionaryNode<TKey, TValue> CurrentNode = null;
        protected DictionaryNode<TKey, TValue> EnumNode = null;
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
            get { return EnumNode.Key; }
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

		public Dictionary()
		{
			// Nothing to do here...
		}

		~Dictionary()
		{
			Clear();
		}

		public TValue this[TKey Key]
		{
			get
			{
				DictionaryNode<TKey, TValue> NodePointer = FindNode(Key);

				if (NodePointer == null)
				{
					throw new CollectionsDataNotFoundException("Dictionary.cs", "Dictionary<TKey, TValue>", "this[Key]", "Key", "Key cannot be found!");
				}

				return NodePointer.Value;
			}

			set
			{
				DictionaryNode<TKey, TValue> NodePointer = FindNode(Key);

				if (NodePointer == null)
				{
					throw new CollectionsDataNotFoundException("Dictionary.cs", "Dictionary<TKey, TValue>", "this[Key]", "Key", "Key cannot be found!");
				}

				NodePointer.Value = value;
			}
		}

		public void Clear()
		{
			DictionaryNode<TKey, TValue> NodePointer = FirstNode;
			DictionaryNode<TKey, TValue> BackupNode = null;

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
            get { return (Count == 0 && FirstNode == null && LastNode == null); }
        }

        public bool IsNotEmpty
        {
            get { return (Count > 0 && FirstNode != null & LastNode != null); }
        }

		public DictionaryNode<TKey, TValue> GetFirstNode
		{
			get
			{
				CurrentNode = FirstNode;

				return CurrentNode;
			}
		}

		public DictionaryNode<TKey, TValue> GetPrevNode
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

		public DictionaryNode<TKey, TValue> GetNextNode
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

		public DictionaryNode<TKey, TValue> GetLastNode
		{
			get
			{
				CurrentNode = LastNode;

				return CurrentNode;
			}
		}

		public List<TKey> KeysToList()
		{
			List<TKey> Keys = new List<TKey>();

			DictionaryNode<TKey, TValue> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				Keys.AddLast(NodePointer.Key);

				NodePointer = NodePointer.Next;
			}

			return Keys;
		}

		public TKey[] KeysToArray()
		{
			DictionaryNode<TKey, TValue> NodePointer = FirstNode;
			TKey[] Keys = new TKey[Count];
			int Counter = -1;

			while (NodePointer != null)
			{
				Counter++;

				Keys[Counter] = NodePointer.Key;

				NodePointer = NodePointer.Next;
			}

			return Keys;
		}

		public TValue[] ValuesToArray()
		{
			DictionaryNode<TKey, TValue> NodePointer = FirstNode;
			TValue[] Values = new TValue[Count];
			int Counter = -1;

			while (NodePointer != null)
			{
				Counter++;

				Values[Counter] = NodePointer.Value;

				NodePointer = NodePointer.Next;
			}

			return Values;
		}

		public DictionaryNode<TKey, TValue> FindNode(TKey Key)
		{
			DictionaryNode<TKey, TValue> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				if (NodePointer.Key.CompareTo(Key) == 0)
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

		public bool Contains(TKey Key)
        {
            return (FindNode(Key) != null);
        }

        public bool NotContains(TKey Key)
        {
            return (FindNode(Key) == null);
        }

        public DictionaryNode<TKey, TValue> FindNode(DictionaryNode<TKey, TValue> Node)
        {
            DictionaryNode<TKey, TValue> NodePointer = FirstNode;

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

        public bool Contains(DictionaryNode<TKey, TValue> Node)
        {
            return (FindNode(Node) != null);
        }

        public bool NotContains(DictionaryNode<TKey, TValue> Node)
        {
            return (FindNode(Node) == null);
        }

        public virtual DictionaryNode<TKey, TValue> Add(TKey Key, TValue Value)
        {
            return AddLast(Key, Value);
        }

        public DictionaryNode<TKey, TValue> AddFirst(TKey Key, TValue Value)
        {
            if (Contains(Key))
            {
                throw new CollectionsDataExistsException("Dictionary.cs", "Dictionary<TKey, TValue>", "AddFirst", "Key", "Given key already exists!");
            }
            else
            {
                DictionaryNode<TKey, TValue> NewNode = new DictionaryNode<TKey, TValue>(Key, Value);

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
        }

        public DictionaryNode<TKey, TValue> AddLast(TKey Key, TValue Value)
        {
            if (Contains(Key))
            {
                throw new CollectionsDataExistsException("Dictionary.cs", "Dictionary<TKey, TValue>", "AddLast", "Key", "Given key already exists!");
            }
            else
            {
                DictionaryNode<TKey, TValue> NewNode = new DictionaryNode<TKey, TValue>(Key, Value);

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
        }

        public DictionaryNode<TKey, TValue> AddBefore(DictionaryNode<TKey, TValue> Node, TKey Key, TValue Value)
        {
            if (Node == null)
            {
                throw new CollectionsDataNullException("Dictionary.cs", "Dictionary<TKey, TValue>", "AddBefore", "Node", "Node cannot be NULL!");
            }

            if (NotContains(Node))
            {
                throw new CollectionsDataNotFoundException("Dictionary.cs", "Dictionary<TKey, TValue>", "AddBefore", "Node", "Node cannot be found in this Dictionary!");
            }

            if (Contains(Key))
            {
                throw new CollectionsDataNullException("Dictionary.cs", "Dictionary<TKey, TValue>", "AddBefore", "Key", "Given key already exists!");
            }

            DictionaryNode<TKey, TValue> NewNode = new DictionaryNode<TKey, TValue>(Key, Value);

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
                DictionaryNode<TKey, TValue> PrevNode = LastNode.Prev;
                NewNode.Prev = PrevNode;
                NewNode.Next = LastNode;
                PrevNode.Next = NewNode;
                LastNode.Prev = NewNode;

                Count++;

                return NewNode;
            }

            if (Node != FirstNode && Node != LastNode && Count > 2)
            {
                DictionaryNode<TKey, TValue> PrevNode = Node.Prev;
                NewNode.Prev = PrevNode;
                NewNode.Next = Node;
                PrevNode.Next = NewNode;
                Node.Prev = NewNode;

                Count++;

                return NewNode;
            }

            return null;
        }

        public DictionaryNode<TKey, TValue> AddAfter(DictionaryNode<TKey, TValue> Node, TKey Key, TValue Value)
        {
            if (Node == null)
            {
                throw new CollectionsDataNullException("Dictionary.cs", "Dictionary<TKey, TValue>", "AddAfter", "Node", "Node cannot be NULL!");
            }

            if (NotContains(Node))
            {
                throw new CollectionsDataNullException("Dictionary.cs", "Dictionary<TKey, TValue>", "AddAfter", "Node", "Node cannot be found in this Dictionary!");
            }

            if (Contains(Key))
            {
                throw new CollectionsDataNullException("Dictionary.cs", "Dictionary<TKey, TValue>", "AddAfter", "Key", "Given key already exists!");
            }

            DictionaryNode<TKey, TValue> NewNode = new DictionaryNode<TKey, TValue>(Key, Value);

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
                DictionaryNode<TKey, TValue> NextNode = FirstNode.Next;

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
                DictionaryNode<TKey, TValue> NextNode = Node.Next;

                Node.Next = NewNode;
                NewNode.Prev = Node;
                NewNode.Next = NextNode;
                NextNode.Prev = NewNode;

                Count++;

                return NewNode;
            }

            return null;
        }

        public bool Remove(TKey Key)
        {
			DictionaryNode<TKey, TValue> NodePointer = FindNode(Key);

			if (NodePointer != null)
			{
				return Remove(NodePointer);
			}
			else
			{
				return false;
			}
        }

        public bool Remove(DictionaryNode<TKey, TValue> Node)
        {
            if (Node == null)
            {
                throw new CollectionsDataNullException("Dictionary.cs", "Dictionary<TKey, TValue>", "Delete", "Node", "Node cannot be NULL!");
            }

            if (NotContains(Node))
            {
                throw new CollectionsDataNullException("Dictionary.cs", "Dictionary<TKey, TValue>", "Delete", "Node", "Node cannot be found in this Dictionary!");
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

		public Dictionary<TKey, TValue> Clone()
		{
			Dictionary<TKey, TValue> Cloned = new Dictionary<TKey, TValue>();
			DictionaryNode<TKey, TValue> NodePointer = FirstNode;

			while (NodePointer != null)
			{
				Cloned.AddLast(NodePointer.Key, NodePointer.Value);

				NodePointer = NodePointer.Next;
			}

			return Cloned;
		}

		public void CloneFrom(Dictionary<TKey, TValue> Source)
        {
            if (Source == null)
            {
                throw new CollectionsDataNullException("Dictionary.cs", "Dictionary<TKey, TValue>", "CloneFrom", "Source", "Source cannot be NULL!");
            }

            Clear();

            DictionaryNode<TKey, TValue> NodePointer = Source.FirstNode;

            while (NodePointer != null)
            {
                Add(NodePointer.Key, NodePointer.Value);

                NodePointer = NodePointer.Next;
            }
        }

        public void CloneTo(Dictionary<TKey, TValue> Destination)
        {
            if (Destination == null)
            {
                throw new CollectionsDataNullException("Dictionary.cs", "Dictionary<TKey, TValue>", "CloneTo", "Destination", "Destination cannot be NULL!");
            }

            Destination.Clear();

            DictionaryNode<TKey, TValue> NodePointer = FirstNode;

            while (NodePointer != null)
            {
                Destination.Add(NodePointer.Key, NodePointer.Value);

                NodePointer = NodePointer.Next;
            }
        }

        public void SortWithBinarySearchTree()
        {
            BinarySearchTree<TKey> BSTree = new BinarySearchTree<TKey>();
            Dictionary<TKey, TValue> SortedDictionary = new Dictionary<TKey, TValue>();

            DictionaryNode<TKey, TValue> NodePointer = FirstNode;

            while (NodePointer != null)
            {
                BSTree.Add(NodePointer.Key);

                NodePointer = NodePointer.Next;
            }

			foreach (TKey Item in BSTree.TraverseMinToMax())
            {
                SortedDictionary.Add(Item, this[Item]);
            }

            CloneFrom(SortedDictionary);

            BSTree.DeleteAll();
            BSTree = null;

            SortedDictionary.Clear();
            SortedDictionary = null;
        }
    }
    #endregion
}
