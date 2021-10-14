// ================================================================================================
// Copyright (c) MOSA Project. Licensed under the New BSD License.
// ================================================================================================
// AUTHOR       : TAYLAN INAN
// E-MAIL       : taylaninan@yahoo.com
// GITHUB       : www.github.com/taylaninan/
// ================================================================================================

using System;

namespace Mosa.Collections.Generic
{
    ///////////////////////////////////////////////////////////////////////////
    //
    // BINARYTREENODE<>
    //
    ///////////////////////////////////////////////////////////////////////////
    // TODO: Add deletion of a single node
    ///////////////////////////////////////////////////////////////////////////
    
    #region BINARYTREENODE...
    public class BinaryTreeNode<AnyType>
    {
        public AnyType Data;
        public uint Count = 0;
        public BinaryTreeNode<AnyType> Parent = null;
        public BinaryTreeNode<AnyType> Left = null;
        public BinaryTreeNode<AnyType> Right = null;

        public BinaryTreeNode()
        {
            // Nothing to do here...
        }

        public BinaryTreeNode(AnyType Data)
        {
            this.Data = Data;
            this.Count = 1;
        }

        ~BinaryTreeNode()
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

    ///////////////////////////////////////////////////////////////////////////
    //
    // BINARYTREE<>
    //
    ///////////////////////////////////////////////////////////////////////////
    #region BINARYTREE...
    public class BinaryTree<AnyType> where AnyType : IComparable
    {
        private BinaryTreeNode<AnyType> RootNode = null;
        private uint TotalSize = 0;
        private uint ActiveSize = 0;

        public BinaryTree()
        {
            // Nothing to do here...
        }

        ~BinaryTree()
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

        public BinaryTreeNode<AnyType> GetRootNode
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

        public BinaryTreeNode<AnyType> Add(AnyType Data)
        {
            return Add(Data, this.RootNode);
        }

        private BinaryTreeNode<AnyType> Add(AnyType Data, BinaryTreeNode<AnyType> SubTree)
        {
            if (SubTree == null)
            {
                BinaryTreeNode<AnyType> NewNode = new BinaryTreeNode<AnyType>();

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
                        BinaryTreeNode<AnyType> NewNode = new BinaryTreeNode<AnyType>();

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
                        BinaryTreeNode<AnyType> NewNode = new BinaryTreeNode<AnyType>();

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

        public BinaryTreeNode<AnyType> GetMinNode()
        {
            return GetMinNode(RootNode);
        }

        public BinaryTreeNode<AnyType> GetMinNode(BinaryTreeNode<AnyType> SubTree)
        {
            BinaryTreeNode<AnyType> Result = SubTree;

            if (Result != null)
            {
                while (Result.Left != null)
                {
                    Result = Result.Left;
                }
            }

            return Result;
        }

        public BinaryTreeNode<AnyType> GetMinNodeActive()
        {
            return GetMinNodeActive(RootNode);
        }

        public BinaryTreeNode<AnyType> GetMinNodeActive(BinaryTreeNode<AnyType> SubTree)
        {
            BinaryTreeNode<AnyType> Result = SubTree;

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

        public BinaryTreeNode<AnyType> GetMaxNode()
        {
            return GetMaxNode(RootNode);
        }

        public BinaryTreeNode<AnyType> GetMaxNode(BinaryTreeNode<AnyType> SubTree)
        {
            BinaryTreeNode<AnyType> Result = SubTree;

            if (Result != null)
            {
                while (Result.Right != null)
                {
                    Result = Result.Right;
                }
            }

            return Result;
        }

        public BinaryTreeNode<AnyType> GetMaxNodeActive()
        {
            return GetMaxNodeActive(RootNode);
        }

        public BinaryTreeNode<AnyType> GetMaxNodeActive(BinaryTreeNode<AnyType> SubTree)
        {
            BinaryTreeNode<AnyType> Result = SubTree;

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
            BinaryTreeNode<AnyType> CurrentNode = Find(Data);

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
            BinaryTreeNode<AnyType> CurrentNode = Find(Data);

            if (CurrentNode != null)
            {
                return CurrentNode.IsEmpty;
            }
            else
            {
                return false;
            }
        }

        public BinaryTreeNode<AnyType> Find(AnyType Data)
        {
            return Find(Data, RootNode);
        }

        private BinaryTreeNode<AnyType> Find(AnyType Data, BinaryTreeNode<AnyType> SubTree)
        {
            BinaryTreeNode<AnyType> ResultNode = null;

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
            BinaryTreeNode<AnyType> CurrentNode = Find(Data);

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
            BinaryTreeNode<AnyType> CurrentNode = Find(Data);

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

        private void DeleteTree(BinaryTreeNode<AnyType> SubTree)
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

        private void TraverseTreeMinToMax(List<AnyType> Sorted, BinaryTreeNode<AnyType> SubTree)
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

        private void TraverseTreeMaxToMin(List<AnyType> Sorted, BinaryTreeNode<AnyType> SubTree)
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

        private void ConvertTreeToDictionary (Dictionary<AnyType, uint> IndexAndCounts, BinaryTreeNode<AnyType> SubTree)
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
