// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Collections.Generic
{
	// BinarySearchTreeNode<AnyType>

	#region BinarySearchTreeNode<AnyType>

	public class BinarySearchTreeNode<AnyType>
	{
		public BinarySearchTreeNode<AnyType> Parent = null;
		public BinarySearchTreeNode<AnyType> Left = null;
		public BinarySearchTreeNode<AnyType> Right = null;
		public AnyType Data;
		public uint CollisionCount { get; protected set; } = 0;

		public BinarySearchTreeNode()
		{
			// Nothing to do here...
		}

		public BinarySearchTreeNode(AnyType Data)
		{
			this.Data = Data;
			this.CollisionCount = 1;
			this.Parent = null;
			this.Left = null;
			this.Right = null;
		}

		public BinarySearchTreeNode(AnyType Data, BinarySearchTreeNode<AnyType> Parent)
		{
			this.Data = Data;
			this.CollisionCount = 1;
			this.Parent = Parent;
			this.Left = null;
			this.Right = null;
		}

		~BinarySearchTreeNode()
		{
			this.CollisionCount = 0;
			this.Parent = null;
			this.Left = null;
			this.Right = null;
		}

		public bool IsEmpty
		{
			get { return (this.CollisionCount == 0); }
		}

		public bool IsNotEmpty
		{
			get { return (this.CollisionCount > 0); }
		}

		public void InsertOne()
		{
			this.CollisionCount++;
		}

		public void RemoveOne()
		{
			if (this.CollisionCount > 0) { this.CollisionCount--; }
		}

		public void RemoveAll()
		{
			this.CollisionCount = 0;
		}
	}

	#endregion BinarySearchTreeNode<AnyType>

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
			Clear();
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
				BinarySearchTreeNode<AnyType> NewNode = new BinarySearchTreeNode<AnyType>(Data, SubTree);

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
						SubTree.InsertOne();

						this.ActiveSize++;
					}
					else
					{
						SubTree.InsertOne();
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
						BinarySearchTreeNode<AnyType> NewNode = new BinarySearchTreeNode<AnyType>(Data, SubTree);

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
						BinarySearchTreeNode<AnyType> NewNode = new BinarySearchTreeNode<AnyType>(Data, SubTree);

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

		public void Remove(AnyType Data)
		{
			BinarySearchTreeNode<AnyType> CurrentNode = Find(Data);

			if (CurrentNode != null)
			{
				CurrentNode.RemoveOne();

				if (CurrentNode.IsEmpty)
				{
					this.ActiveSize--;
				}
			}
		}

		public void RemoveAll(AnyType Data)
		{
			BinarySearchTreeNode<AnyType> CurrentNode = Find(Data);

			if (CurrentNode != null)
			{
				CurrentNode.RemoveAll();

				this.ActiveSize--;
			}
		}

		public void Clear()
		{
			RemoveTree(RootNode);

			RootNode = null;
			TotalSize = 0;
			ActiveSize = 0;
		}

		private void RemoveTree(BinarySearchTreeNode<AnyType> SubTree)
		{
			if (SubTree != null)
			{
				if (SubTree.Parent != null)
				{
					if (SubTree.Parent.Left == SubTree) { SubTree.Parent.Left = null; }
					if (SubTree.Parent.Right == SubTree) { SubTree.Parent.Right = null; }
				}

				if (SubTree.Left != null) { RemoveTree(SubTree.Left); }
				if (SubTree.Right != null) { RemoveTree(SubTree.Right); }

				if (SubTree.IsNotEmpty)
				{
					SubTree.RemoveAll();
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

				for (uint counter = 0; counter < SubTree.CollisionCount; counter++)
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

				for (uint counter = 0; counter < SubTree.CollisionCount; counter++)
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

		private void ConvertTreeToDictionary(Dictionary<AnyType, uint> IndexAndCounts, BinarySearchTreeNode<AnyType> SubTree)
		{
			if (SubTree != null)
			{
				ConvertTreeToDictionary(IndexAndCounts, SubTree.Left);

				IndexAndCounts.Add(SubTree.Data, SubTree.CollisionCount);

				ConvertTreeToDictionary(IndexAndCounts, SubTree.Right);
			}
		}
	}

	#endregion BinarySearchTree<AnyType>
}
