// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.ClassLib
{
	/// <summary>
	/// Implements a Red Black Tree list
	/// </summary>
	/// <typeparam name="K"></typeparam>
	/// <typeparam name="T"></typeparam>
	public class RedBlackTree<K, T> where K : System.IComparable
	{
		/// <summary>
		///
		/// </summary>
		protected enum Color
		{
			/// <summary>
			///
			/// </summary>
			Red = 1,

			/// <summary>
			///
			/// </summary>
			Black = 0
		};

		/// <summary>
		///
		/// </summary>
		/// <typeparam name="X"></typeparam>
		/// <typeparam name="Y"></typeparam>
		protected class RedBlackTreeNode<X, Y> where X : System.IComparable
		{
			/// <summary>
			///
			/// </summary>
			public Color color;

			/// <summary>
			///
			/// </summary>
			public Y data;

			/// <summary>
			///
			/// </summary>
			public X key;

			/// <summary>
			///
			/// </summary>
			public RedBlackTreeNode<X, Y> parent;

			/// <summary>
			///
			/// </summary>
			public RedBlackTreeNode<X, Y> left;

			/// <summary>
			///
			/// </summary>
			public RedBlackTreeNode<X, Y> right;

			/// <summary>
			/// Initializes a new instance
			/// </summary>
			/// <param name="key">The key.</param>
			/// <param name="data">The data.</param>
			/// <param name="color">The color.</param>
			public RedBlackTreeNode(X key, Y data, Color color)
			{
				this.key = key;
				this.color = color;
				this.data = data;
			}
		}

		/// <summary>
		///
		/// </summary>
		protected RedBlackTreeNode<K, T> root;

		/// <summary>
		///
		/// </summary>
		protected uint size = 0;

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <value>The size.</value>
		public uint Size { get { return size; } }

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			root = null;
			size = 0;
		}

		/// <summary>
		/// Determines whether [contains] [the specified key].
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>
		/// 	<c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.
		/// </returns>
		protected bool Contains(K key)
		{
			RedBlackTreeNode<K, T> cur = root;

			while (cur != null)
			{
				int cmp = key.CompareTo(cur.key);

				//int cmp = (cur.key == key) ? 0 : (cur.key < key ? -1, 1);

				if (cmp == 0)
					return true;

				if (cmp < 0) /* less than */
					cur = cur.left;
				else
					cur = cur.right;
			}

			return false;
		}

		/// <summary>
		/// Inserts the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="data">The data.</param>
		public void Insert(K key, T data)
		{
			RedBlackTreeNode<K, T> newnode = new RedBlackTree<K, T>.RedBlackTreeNode<K, T>(key, data, Color.Red);

			if (root == null)
				root = newnode;
			else
				Insert(root, newnode);  // inserts into the binary tree

			InsertCase1(newnode);

			size++;
		}

		/// <summary>
		/// Deletes the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		public void Delete(K key)
		{
			RedBlackTreeNode<K, T> deletenode = Find(key);

			if (deletenode == null)
				return; // node was not found; exception in the future?

			if (IsLeaf(deletenode))
				root = null;    // was the only node in the tree
			else
				DeleteOneChild(deletenode);

			size--;
		}

		/// <summary>
		/// Finds the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		protected RedBlackTreeNode<K, T> Find(K key)
		{
			RedBlackTreeNode<K, T> cur = root;

			while (cur != null)
			{
				int cmp = key.CompareTo(cur.key);

				if (cmp == 0)
					return cur;

				if (cmp < 0) /* less than */
					cur = cur.left;
				else
					cur = cur.right;
			}

			return null;
		}

		/// <summary>
		/// Inserts the specified parent.
		/// </summary>
		/// <param name="parent">The parent.</param>
		/// <param name="newnode">The newnode.</param>
		protected void Insert(RedBlackTreeNode<K, T> parent, RedBlackTreeNode<K, T> newnode)
		{
			int cmp = newnode.key.CompareTo(parent.key);

			if (cmp < 0) /* less than */
			{
				if (parent.left == null)
				{
					newnode.parent = parent;
					parent.left = newnode;
				}
				else
					Insert(parent.left, newnode);
			}
			else
				if (parent.right == null)
			{
				newnode.parent = parent;
				parent.right = newnode;
			}
			else
				Insert(parent.right, newnode);
		}

		/// <summary>
		/// Gets the grandparent.
		/// </summary>
		/// <param name="n">The n.</param>
		/// <returns></returns>
		protected RedBlackTreeNode<K, T> GetGrandparent(RedBlackTreeNode<K, T> n)
		{
			if ((n != null) && (n.parent != null))
				return n.parent.parent;
			else
				return null;
		}

		/// <summary>
		/// Gets the uncle.
		/// </summary>
		/// <param name="n">The n.</param>
		/// <returns></returns>
		protected RedBlackTreeNode<K, T> GetUncle(RedBlackTreeNode<K, T> n)
		{
			RedBlackTreeNode<K, T> g = GetGrandparent(n);
			if (g == null)
				return null;
			if (n.parent == g.left)
				return g.right;
			else
				return g.left;
		}

		/// <summary>
		/// Rotates the right.
		/// </summary>
		/// <param name="q">The q.</param>
		protected void RotateRight(RedBlackTreeNode<K, T> q)
		{
			RedBlackTreeNode<K, T> r = q.parent;
			RedBlackTreeNode<K, T> p = q.left;
			RedBlackTreeNode<K, T> b = p.right;

			if (r == null)
				root = p;
			else
				if (r.left == q)
				r.left = p;
			else
				r.right = p;

			p.parent = r;
			p.right = q;
			q.left = b;
			q.parent = p;
			if (b != null)
				b.parent = q;
		}

		/// <summary>
		/// Rotates the left.
		/// </summary>
		/// <param name="p">The p.</param>
		protected void RotateLeft(RedBlackTreeNode<K, T> p)
		{
			RedBlackTreeNode<K, T> r = p.parent;
			RedBlackTreeNode<K, T> q = p.right;
			RedBlackTreeNode<K, T> b = q.left;

			if (r == null)
				root = q;
			else
				if (r.left == p)
				r.left = q;
			else
				r.right = q;

			q.parent = r;
			q.left = p;
			p.right = b;
			p.parent = q;
			if (b != null)
				b.parent = p;
		}

		/// <summary>
		/// Determines whether the specified n is leaf.
		/// </summary>
		/// <param name="n">The n.</param>
		/// <returns>
		/// 	<c>true</c> if the specified n is leaf; otherwise, <c>false</c>.
		/// </returns>
		protected bool IsLeaf(RedBlackTreeNode<K, T> n)
		{
			return ((n.left == null) && (n.right == null));
		}

		/// <summary>
		/// Replaces the node.
		/// </summary>
		/// <param name="n">The n.</param>
		/// <param name="c">The c.</param>
		protected void ReplaceNode(RedBlackTreeNode<K, T> n, RedBlackTreeNode<K, T> c)
		{
			if (n.parent.left == n)
				n.parent.left = c;
			else
				n.parent.right = c;

			c.parent = n.parent;
		}

		/// <summary>
		/// Inserts the case1.
		/// </summary>
		/// <param name="n">The n.</param>
		protected void InsertCase1(RedBlackTreeNode<K, T> n)
		{
			if (n.parent == null)
				n.color = Color.Black;
			else
				InsertCase2(n);
		}

		/// <summary>
		/// Inserts the case2.
		/// </summary>
		/// <param name="n">The n.</param>
		protected void InsertCase2(RedBlackTreeNode<K, T> n)
		{
			if (n.parent.color == Color.Black)
				return;
			else
				InsertCase3(n);
		}

		/// <summary>
		/// Inserts the case3.
		/// </summary>
		/// <param name="n">The n.</param>
		protected void InsertCase3(RedBlackTreeNode<K, T> n)
		{
			RedBlackTreeNode<K, T> u = GetUncle(n);
			RedBlackTreeNode<K, T> g;

			if ((u != null) && (u.color == Color.Red))
			{
				n.parent.color = Color.Black;
				u.color = Color.Black;
				g = GetGrandparent(n);
				g.color = Color.Red;
				InsertCase1(g);
			}
			else
			{
				InsertCase4(n);
			}
		}

		/// <summary>
		/// Inserts the case4.
		/// </summary>
		/// <param name="n">The n.</param>
		protected void InsertCase4(RedBlackTreeNode<K, T> n)
		{
			RedBlackTreeNode<K, T> g = GetGrandparent(n);

			if ((n == n.parent.right) && (n.parent == g.left))
			{
				RotateLeft(n.parent);
				n = n.left;
			}
			else if ((n == n.parent.left) && (n.parent == g.right))
			{
				RotateRight(n.parent);
				n = n.right;
			}
			InsertCase5(n);
		}

		/// <summary>
		/// Inserts the case5.
		/// </summary>
		/// <param name="n">The n.</param>
		protected void InsertCase5(RedBlackTreeNode<K, T> n)
		{
			RedBlackTreeNode<K, T> g = GetGrandparent(n);

			n.parent.color = Color.Black;
			g.color = Color.Red;
			if ((n == n.parent.left) && (n.parent == g.left))
			{
				RotateRight(g);
			}
			else
			{
				RotateLeft(g);
			}
		}

		/// <summary>
		/// Gets the sibling.
		/// </summary>
		/// <param name="n">The n.</param>
		/// <returns></returns>
		protected RedBlackTreeNode<K, T> GetSibling(RedBlackTreeNode<K, T> n)
		{
			if (n == n.parent.left)
				return n.parent.right;
			else
				return n.parent.left;
		}

		/// <summary>
		/// Deletes the one child.
		/// </summary>
		/// <param name="n">The n.</param>
		protected void DeleteOneChild(RedBlackTreeNode<K, T> n)
		{
			RedBlackTreeNode<K, T> child = IsLeaf(n.right) ? n.left : n.right;

			ReplaceNode(n, child);
			if (n.color == Color.Black)
			{
				if (child.color == Color.Red)
					child.color = Color.Black;
				else
					DeleteCase1(child);
			}
		}

		/// <summary>
		/// Deletes the case1.
		/// </summary>
		/// <param name="n">The n.</param>
		protected void DeleteCase1(RedBlackTreeNode<K, T> n)
		{
			if (n.parent != null)
				DeleteCase2(n);
		}

		/// <summary>
		/// Deletes the case2.
		/// </summary>
		/// <param name="n">The n.</param>
		protected void DeleteCase2(RedBlackTreeNode<K, T> n)
		{
			RedBlackTreeNode<K, T> s = GetSibling(n);

			if (s.color == Color.Red)
			{
				n.parent.color = Color.Red;
				s.color = Color.Black;
				if (n == n.parent.left)
					RotateLeft(n.parent);
				else
					RotateRight(n.parent);
			}
			DeleteCase3(n);
		}

		/// <summary>
		/// Deletes the case3.
		/// </summary>
		/// <param name="n">The n.</param>
		protected void DeleteCase3(RedBlackTreeNode<K, T> n)
		{
			RedBlackTreeNode<K, T> s = GetSibling(n);

			if ((n.parent.color == Color.Black) &&
				(s.color == Color.Black) &&
				(s.left.color == Color.Black) &&
				(s.right.color == Color.Black))
			{
				s.color = Color.Red;
				DeleteCase1(n.parent);
			}
			else
				DeleteCase4(n);
		}

		/// <summary>
		/// Deletes the case4.
		/// </summary>
		/// <param name="n">The n.</param>
		protected void DeleteCase4(RedBlackTreeNode<K, T> n)
		{
			RedBlackTreeNode<K, T> s = GetSibling(n);

			if ((n.parent.color == Color.Red) &&
				(s.color == Color.Black) &&
				(s.left.color == Color.Black) &&
				(s.right.color == Color.Black))
			{
				s.color = Color.Red;
				n.parent.color = Color.Black;
			}
			else
				DeleteCase5(n);
		}

		/// <summary>
		/// Deletes the case5.
		/// </summary>
		/// <param name="n">The n.</param>
		protected void DeleteCase5(RedBlackTreeNode<K, T> n)
		{
			RedBlackTreeNode<K, T> s = GetSibling(n);

			if ((n == n.parent.left) &&
				(s.color == Color.Black) &&
				(s.left.color == Color.Red) &&
				(s.right.color == Color.Black))
			{
				s.color = Color.Red;
				s.left.color = Color.Black;
				RotateRight(s);
			}
			else if ((n == n.parent.right) &&
					   (s.color == Color.Black) &&
					   (s.right.color == Color.Red) &&
					   (s.left.color == Color.Black))
			{
				s.color = Color.Red;
				s.right.color = Color.Black;
				RotateLeft(s);
			}
			DeleteCase6(n);
		}

		/// <summary>
		/// Deletes the case6.
		/// </summary>
		/// <param name="n">The n.</param>
		protected void DeleteCase6(RedBlackTreeNode<K, T> n)
		{
			RedBlackTreeNode<K, T> s = GetSibling(n);

			s.color = n.parent.color;
			n.parent.color = Color.Black;
			if (n == n.parent.left)
			{
				s.right.color = Color.Black;
				RotateLeft(n.parent);
			}
			else
			{
				s.left.color = Color.Black;
				RotateRight(n.parent);
			}
		}

		/// <summary>
		/// Finds the min.
		/// </summary>
		/// <param name="s">The s.</param>
		/// <returns></returns>
		protected RedBlackTreeNode<K, T> FindMin(RedBlackTreeNode<K, T> s)
		{
			if (s == null)
				return null;

			RedBlackTreeNode<K, T> cur = s;

			while (cur.left != null)
				cur = cur.left;

			return cur;
		}

		/// <summary>
		/// Finds the max.
		/// </summary>
		/// <param name="s">The s.</param>
		/// <returns></returns>
		protected RedBlackTreeNode<K, T> FindMax(RedBlackTreeNode<K, T> s)
		{
			if (s == null)
				return null;

			RedBlackTreeNode<K, T> cur = s;

			while (cur.right != null)
				cur = cur.right;

			return cur;
		}
	}
}
