/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Collections;

namespace Mosa.ClassLib
{
    public class RedBlackTree<K, T> where K : IComparable
    {
        protected enum Color { Red = 1, Black = 0 };

        protected class RedBlackTreeNode<NK, NT> where NK : IComparable
        {
            public Color color;
            public NT data;
            public NK key;

            public RedBlackTreeNode<NK, NT> parent, left, right;

            public RedBlackTreeNode(NK key, NT data, Color color)
            {
                this.key = key;
                this.color = color;
                this.data = data;
            }

            //public override string ToString()   // for debugging
            //{
            //    return key.ToString() + " - " + data.ToString() + " : " + (color == Color.Red ? "Red" : "Black");
            //}
        }

        protected RedBlackTreeNode<K, T> root;
        protected uint size = 0;

        public uint Size { get { return size; } }

        public void Clear()
        {
            root = null;
            size = 0;
        }

        protected bool Contains(K key)
        {
            RedBlackTreeNode<K, T> cur = root;

            while (cur != null) {
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

        protected RedBlackTreeNode<K, T> Find(K key)
        {
            RedBlackTreeNode<K, T> cur = root;

            while (cur != null) {
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

        protected void Insert(RedBlackTreeNode<K, T> parent, RedBlackTreeNode<K, T> newnode)
        {
            int cmp = newnode.key.CompareTo(parent.key);

            if (cmp < 0) /* less than */ {
                if (parent.left == null) {
                    newnode.parent = parent;
                    parent.left = newnode;
                }
                else
                    Insert(parent.left, newnode);
            }
            else
                if (parent.right == null) {
                    newnode.parent = parent;
                    parent.right = newnode;
                }
                else
                    Insert(parent.right, newnode);
        }

        protected RedBlackTreeNode<K, T> GetGrandparent(RedBlackTreeNode<K, T> n)
        {
            if ((n != null) && (n.parent != null))
                return n.parent.parent;
            else
                return null;
        }

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

        protected bool IsLeaf(RedBlackTreeNode<K, T> n)
        {
            return ((n.left == null) && (n.right == null));
        }

        protected void ReplaceNode(RedBlackTreeNode<K, T> n, RedBlackTreeNode<K, T> c)
        {
            if (n.parent.left == n)
                n.parent.left = c;
            else
                n.parent.right = c;

            c.parent = n.parent;
        }

        protected void InsertCase1(RedBlackTreeNode<K, T> n)
        {
            if (n.parent == null)
                n.color = Color.Black;
            else
                InsertCase2(n);
        }

        protected void InsertCase2(RedBlackTreeNode<K, T> n)
        {
            if (n.parent.color == Color.Black)
                return;
            else
                InsertCase3(n);
        }

        protected void InsertCase3(RedBlackTreeNode<K, T> n)
        {
            RedBlackTreeNode<K, T> u = GetUncle(n);
            RedBlackTreeNode<K, T> g;

            if ((u != null) && (u.color == Color.Red)) {
                n.parent.color = Color.Black;
                u.color = Color.Black;
                g = GetGrandparent(n);
                g.color = Color.Red;
                InsertCase1(g);
            }
            else {
                InsertCase4(n);
            }
        }

        protected void InsertCase4(RedBlackTreeNode<K, T> n)
        {
            RedBlackTreeNode<K, T> g = GetGrandparent(n);

            if ((n == n.parent.right) && (n.parent == g.left)) {
                RotateLeft(n.parent);
                n = n.left;
            }
            else if ((n == n.parent.left) && (n.parent == g.right)) {
                RotateRight(n.parent);
                n = n.right;
            }
            InsertCase5(n);
        }

        protected void InsertCase5(RedBlackTreeNode<K, T> n)
        {
            RedBlackTreeNode<K, T> g = GetGrandparent(n);

            n.parent.color = Color.Black;
            g.color = Color.Red;
            if ((n == n.parent.left) && (n.parent == g.left)) {
                RotateRight(g);
            }
            else {
                RotateLeft(g);
            }
        }

        protected RedBlackTreeNode<K, T> GetSibling(RedBlackTreeNode<K, T> n)
        {
            if (n == n.parent.left)
                return n.parent.right;
            else
                return n.parent.left;
        }

        protected void DeleteOneChild(RedBlackTreeNode<K, T> n)
        {
            RedBlackTreeNode<K, T> child = IsLeaf(n.right) ? n.left : n.right;

            ReplaceNode(n, child);
            if (n.color == Color.Black) {
                if (child.color == Color.Red)
                    child.color = Color.Black;
                else
                    DeleteCase1(child);
            }
        }

        protected void DeleteCase1(RedBlackTreeNode<K, T> n)
        {
            if (n.parent != null)
                DeleteCase2(n);
        }

        protected void DeleteCase2(RedBlackTreeNode<K, T> n)
        {
            RedBlackTreeNode<K, T> s = GetSibling(n);

            if (s.color == Color.Red) {
                n.parent.color = Color.Red;
                s.color = Color.Black;
                if (n == n.parent.left)
                    RotateLeft(n.parent);
                else
                    RotateRight(n.parent);
            }
            DeleteCase3(n);
        }

        protected void DeleteCase3(RedBlackTreeNode<K, T> n)
        {
            RedBlackTreeNode<K, T> s = GetSibling(n);

            if ((n.parent.color == Color.Black) &&
                (s.color == Color.Black) &&
                (s.left.color == Color.Black) &&
                (s.right.color == Color.Black)) {
                s.color = Color.Red;
                DeleteCase1(n.parent);
            }
            else
                DeleteCase4(n);
        }

        protected void DeleteCase4(RedBlackTreeNode<K, T> n)
        {
            RedBlackTreeNode<K, T> s = GetSibling(n);

            if ((n.parent.color == Color.Red) &&
                (s.color == Color.Black) &&
                (s.left.color == Color.Black) &&
                (s.right.color == Color.Black)) {
                s.color = Color.Red;
                n.parent.color = Color.Black;
            }
            else
                DeleteCase5(n);
        }

        protected void DeleteCase5(RedBlackTreeNode<K, T> n)
        {
            RedBlackTreeNode<K, T> s = GetSibling(n);

            if ((n == n.parent.left) &&
                (s.color == Color.Black) &&
                (s.left.color == Color.Red) &&
                (s.right.color == Color.Black)) {
                s.color = Color.Red;
                s.left.color = Color.Black;
                RotateRight(s);
            }
            else if ((n == n.parent.right) &&
                       (s.color == Color.Black) &&
                       (s.right.color == Color.Red) &&
                       (s.left.color == Color.Black)) {
                s.color = Color.Red;
                s.right.color = Color.Black;
                RotateLeft(s);
            }
            DeleteCase6(n);
        }

        protected void DeleteCase6(RedBlackTreeNode<K, T> n)
        {
            RedBlackTreeNode<K, T> s = GetSibling(n);

            s.color = n.parent.color;
            n.parent.color = Color.Black;
            if (n == n.parent.left) {
                s.right.color = Color.Black;
                RotateLeft(n.parent);
            }
            else {
                s.left.color = Color.Black;
                RotateRight(n.parent);
            }
        }

        protected RedBlackTreeNode<K, T> FindMin(RedBlackTreeNode<K, T> s)
        {
            if (s == null)
                return null;

            RedBlackTreeNode<K, T> cur = s;

            while (cur.left != null)
                cur = cur.left;

            return cur;
        }

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
