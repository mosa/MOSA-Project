// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Collections.Generic
{
    // BinarySearchTreeNode<AnyType>
    
    #region BinarySearchTreeNode<AnyType>
    public class BinarySearchTreeNode<AnyType>
    {
        public AnyType Data;
        public uint Count = 0;
        public BinarySearchTreeNode<AnyType> Parent = null;
        public BinarySearchTreeNode<AnyType> Left = null;
        public BinarySearchTreeNode<AnyType> Right = null;

        public BinarySearchTreeNode()
        {
            // Nothing to do here...
        }

        public BinarySearchTreeNode(AnyType Data)
        {
            this.Data = Data;
            this.Count = 1;
        }

        ~BinarySearchTreeNode()
        {
            this.Count = 0;
            this.Parent = null;
            this.Left = null;
            this.Right = null;
        }

        public bool IsEmpty
        {
            get { return (this.Count == 0); }
        }

        public bool IsNotEmpty
        {
            get { return (this.Count > 0); }
        }

        public void AddOne()
        {
            this.Count++;
        }

        public void DeleteOne()
        {
            if (this.Count > 0) { this.Count--; }
        }

        public void DeleteAll()
        {
            this.Count = 0;
        }
    }
	#endregion

	// BinarySearchTree<AnyType>
	// TODO: Add deletion of a single node

	#region BinarySearchTree<AnyType>
	public class BinarySearchTree<AnyType> where AnyType : IComparable
    {
        private BinarySearchTreeNode<AnyType> RootNode = null;
        private uint TotalSize = 0;
        private uint ActiveSize = 0;

        public BinarySearchTree()
        {
            // Nothing to do here...
        }

        ~BinarySearchTree()
        {
            DeleteAll();
        }

        public uint GetTotalSize
        {
            get { return this.TotalSize; }
        }

        public uint GetActiveSize
        {
            get { return this.ActiveSize; }
        }

        public BinarySearchTreeNode<AnyType> GetRootNode
        {
            get { return this.RootNode; }
        }

        public bool IsEmpty
        {
            get { return this.ActiveSize == 0; }
        }

        public bool IsNotEmpty
        {
            get { return this.ActiveSize > 0; }
        }

        public BinarySearchTreeNode<AnyType> Add(AnyType Data)
        {
            return Add(Data, this.RootNode);
        }

        private BinarySearchTreeNode<AnyType> Add(AnyType Data, BinarySearchTreeNode<AnyType> SubTree)
        {
            if (SubTree == null)
            {
                BinarySearchTreeNode<AnyType> NewNode = new BinarySearchTreeNode<AnyType>();

                NewNode.Data = Data;
                NewNode.Count = 1;
                NewNode.Parent = null;
                NewNode.Left = null;
                NewNode.Right = null;

                this.RootNode = NewNode;

                this.ActiveSize++;
                this.TotalSize++;

                return NewNode;
            }
            else
            {
                if (Data.CompareTo(SubTree.Data) == 0)
                {
                    if (SubTree.IsEmpty)
                    {
                        SubTree.AddOne();

                        this.ActiveSize++;
                    }
                    else
                    {
                        SubTree.AddOne();
                    }

                    return SubTree;
                }

                if (Data.CompareTo(SubTree.Data) < 0)
                {
                    if (SubTree.Left != null)
                    {
                        return Add(Data, SubTree.Left);
                    }
                    else
                    {
                        BinarySearchTreeNode<AnyType> NewNode = new BinarySearchTreeNode<AnyType>();

                        NewNode.Data = Data;
                        NewNode.Count = 1;
                        NewNode.Parent = SubTree;
                        NewNode.Left = null;
                        NewNode.Right = null;

                        SubTree.Left = NewNode;

                        this.ActiveSize++;
                        this.TotalSize++;

                        return NewNode;
                    }
                }

                if (Data.CompareTo(SubTree.Data) > 0)
                {
                    if (SubTree.Right != null)
                    {
                        return Add(Data, SubTree.Right);
                    }
                    else
                    {
                        BinarySearchTreeNode<AnyType> NewNode = new BinarySearchTreeNode<AnyType>();

                        NewNode.Data = Data;
                        NewNode.Count = 1;
                        NewNode.Parent = SubTree;
                        NewNode.Left = null;
                        NewNode.Right = null;

                        SubTree.Right = NewNode;

                        this.ActiveSize++;
                        this.TotalSize++;

                        return NewNode;
                    }
                }

                return null;
            }
        }

        public BinarySearchTreeNode<AnyType> GetMinNode()
        {
            return GetMinNode(RootNode);
        }

        public BinarySearchTreeNode<AnyType> GetMinNode(BinarySearchTreeNode<AnyType> SubTree)
        {
            BinarySearchTreeNode<AnyType> Result = SubTree;

            if (Result != null)
            {
                while (Result.Left != null)
                {
                    Result = Result.Left;
                }
            }

            return Result;
        }

        public BinarySearchTreeNode<AnyType> GetMinNodeActive()
        {
            return GetMinNodeActive(RootNode);
        }

        public BinarySearchTreeNode<AnyType> GetMinNodeActive(BinarySearchTreeNode<AnyType> SubTree)
        {
            BinarySearchTreeNode<AnyType> Result = SubTree;

            if (Result != null)
            {
                while (Result.Left != null)
                {
                    Result = Result.Left;
                }

                while (Result.IsEmpty && Result.Parent != null && Result != SubTree)
                {
                    Result = Result.Parent;
                }

                if (Result.IsEmpty && Result.Parent == null && Result == SubTree)
                {
                    Result = GetMinNodeActive(Result.Right);
                }
            }

            return Result;
        }

        public BinarySearchTreeNode<AnyType> GetMaxNode()
        {
            return GetMaxNode(RootNode);
        }

        public BinarySearchTreeNode<AnyType> GetMaxNode(BinarySearchTreeNode<AnyType> SubTree)
        {
            BinarySearchTreeNode<AnyType> Result = SubTree;

            if (Result != null)
            {
                while (Result.Right != null)
                {
                    Result = Result.Right;
                }
            }

            return Result;
        }

        public BinarySearchTreeNode<AnyType> GetMaxNodeActive()
        {
            return GetMaxNodeActive(RootNode);
        }

        public BinarySearchTreeNode<AnyType> GetMaxNodeActive(BinarySearchTreeNode<AnyType> SubTree)
        {
            BinarySearchTreeNode<AnyType> Result = SubTree;

            if (Result != null)
            {
                while (Result.Right != null)
                {
                    Result = Result.Right;
                }

                while (Result.IsEmpty && Result.Parent != null && Result != SubTree)
                {
                    Result = Result.Parent;
                }

                while (Result.IsEmpty && Result.Parent == null && Result == SubTree)
                {
                    Result = GetMaxNodeActive(Result.Left);
                }
            }

            return Result;
        }

        public bool Contains(AnyType Data)
        {
            return (Find(Data) != null);
        }

        public bool ContainsActive(AnyType Data)
        {
            BinarySearchTreeNode<AnyType> CurrentNode = Find(Data);

            if (CurrentNode != null)
            {
                return CurrentNode.IsNotEmpty;
            }
            else
            {
                return false;
            }
        }

        public bool ContainsPassive(AnyType Data)
        {
            BinarySearchTreeNode<AnyType> CurrentNode = Find(Data);

            if (CurrentNode != null)
            {
                return CurrentNode.IsEmpty;
            }
            else
            {
                return false;
            }
        }

        public BinarySearchTreeNode<AnyType> Find(AnyType Data)
        {
            return Find(Data, RootNode);
        }

        private BinarySearchTreeNode<AnyType> Find(AnyType Data, BinarySearchTreeNode<AnyType> SubTree)
        {
            BinarySearchTreeNode<AnyType> ResultNode = null;

            if (SubTree != null)
            {
                if (Data.CompareTo(SubTree.Data) == 0 && ResultNode == null)
                {
                    ResultNode = SubTree;
                }

                if (Data.CompareTo(SubTree.Data) < 0 && SubTree.Left != null && ResultNode == null)
                {
                    ResultNode = Find(Data, SubTree.Left);
                }

                if (Data.CompareTo(SubTree.Data) > 0 && SubTree.Right != null && ResultNode == null)
                {
                    ResultNode = Find(Data, SubTree.Right);
                }
            }

            return ResultNode;
        }

        public void DeleteOne(AnyType Data)
        {
            BinarySearchTreeNode<AnyType> CurrentNode = Find(Data);

            if (CurrentNode != null)
            {
                CurrentNode.DeleteOne();

                if (CurrentNode.IsEmpty)
                {
                    this.ActiveSize--;
                }
            }
        }

        public void DeleteAll(AnyType Data)
        {
            BinarySearchTreeNode<AnyType> CurrentNode = Find(Data);

            if (CurrentNode != null)
            {
                CurrentNode.DeleteAll();

                this.ActiveSize--;
            }
        }

        public void DeleteAll()
        {
            DeleteTree(RootNode);

            RootNode = null;
            TotalSize = 0;
            ActiveSize = 0;
        }

        private void DeleteTree(BinarySearchTreeNode<AnyType> SubTree)
        {
            if (SubTree != null)
            {
                if (SubTree.Parent != null)
                {
                    if (SubTree.Parent.Left == SubTree) { SubTree.Parent.Left = null; }
                    if (SubTree.Parent.Right == SubTree) { SubTree.Parent.Right = null; }
                }

                if (SubTree.Left != null) { DeleteTree(SubTree.Left); }
                if (SubTree.Right != null) { DeleteTree(SubTree.Right); }

                if (SubTree.IsNotEmpty)
                {
                    SubTree.DeleteAll();
                    this.ActiveSize--;
                }

                SubTree = null;

                this.TotalSize--;
            }
        }

        public List<AnyType> TraverseMinToMax()
        {
            List<AnyType> Sorted = new List<AnyType>();

            TraverseTreeMinToMax(Sorted, RootNode);

            return Sorted;
        }

        private void TraverseTreeMinToMax(List<AnyType> Sorted, BinarySearchTreeNode<AnyType> SubTree)
        {
            if (SubTree != null)
            {
                TraverseTreeMinToMax(Sorted, SubTree.Left);

                for (uint counter = 0; counter < SubTree.Count; counter++)
                {
                    Sorted.Add(SubTree.Data);
                }

                TraverseTreeMinToMax(Sorted, SubTree.Right);
            }
        }

        public List<AnyType> TraverseMaxToMin()
        {
            List<AnyType> Sorted = new List<AnyType>();

            TraverseTreeMaxToMin(Sorted, RootNode);

            return Sorted;
        }

        private void TraverseTreeMaxToMin(List<AnyType> Sorted, BinarySearchTreeNode<AnyType> SubTree)
        {
            if (SubTree != null)
            {
                TraverseTreeMaxToMin(Sorted, SubTree.Right);

                for (uint counter = 0; counter < SubTree.Count; counter++)
                {
                    Sorted.Add(SubTree.Data);
                }

                TraverseTreeMaxToMin(Sorted, SubTree.Left);
            }
        }

        public Dictionary<AnyType, uint> ConvertToDictionary()
        {
            Dictionary<AnyType, uint> IndexAndCounts = new Dictionary<AnyType, uint>();

            ConvertTreeToDictionary(IndexAndCounts, RootNode);

            return IndexAndCounts;
        }

        private void ConvertTreeToDictionary (Dictionary<AnyType, uint> IndexAndCounts, BinarySearchTreeNode<AnyType> SubTree)
        {
            if (SubTree != null)
            {
                ConvertTreeToDictionary(IndexAndCounts, SubTree.Left);

                IndexAndCounts.Add(SubTree.Data, SubTree.Count);

                ConvertTreeToDictionary(IndexAndCounts, SubTree.Right);
            }
        }
    }
    #endregion
}
